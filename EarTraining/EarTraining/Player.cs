using System.Globalization;
using NAudio.Wave;
using System.Threading;
using EarTraining.Classes;
using EarTraining.Properties;
using System.IO;
using NLog;
using System.Collections.Generic;
using System.Linq;
using TSampleType = System.Single;
using TLongSampleType = System.Double;
using SoundTouch;
using System;

namespace EarTraining
{
    public class Player
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IEnumerable<Chord> _chordProgression;
        SoundTouch<TSampleType, TLongSampleType> _soundTouch;
        SoundTouchSharp _s;
        public const int BufferSamples = 5 * 2048; // floats, not bytes

        private const int BuffSize = 2048;
        private const int BusyQueuedBuffersThreshold = 3;

        public Player()
        {
            _soundTouch = new SoundTouch<TSampleType, TLongSampleType>();
            _s = new SoundTouchSharp();
        }

        public Player(IEnumerable<Chord> chordProgression)
        {
            _chordProgression = chordProgression;
            _soundTouch = new SoundTouch<TSampleType, TLongSampleType>();
            _s = new SoundTouchSharp();
        }

        public void PlayChords(int numStrums, bool changeTempo, float tempoChange)
        {
            if (_chordProgression != null && _chordProgression.Count() > 0)
            {
                PlayChords(_chordProgression, numStrums, changeTempo, tempoChange);
            }
        }

        public void PlayChords(IEnumerable<Chord> chordProgression, int numStrums, bool changeTempo, float tempoChange)
        {
            if (!changeTempo)
            {
                foreach (var chord in chordProgression)
                {
                    logger.Debug($"Playing chord {chord.Name}...");
                    PlayAudioResource(Resources.ResourceManager.GetStream(chord.GetAudioResourceName(numStrums), CultureInfo.InvariantCulture));
                }
            }
            else
            {
                _s.CreateInstance();
                var tempoChordProgression = new List<WavOutFile>();

                var inputProviders = new List<AdvancedBufferedWaveProvider>();

                logger.Debug($"Tempo change set to {tempoChange}...");
                foreach (var chord in chordProgression)
                {
                    WaveStream waveStream = new WaveFileReader(Resources.ResourceManager.GetStream(chord.GetAudioResourceName(numStrums),
                             CultureInfo.InvariantCulture));

                    if (waveStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
                    {
                        waveStream = WaveFormatConversionStream.CreatePcmStream(waveStream);
                        waveStream = new BlockAlignReductionStream(waveStream);
                    }
                    if (waveStream.WaveFormat.BitsPerSample != 16)
                    {
                        var wavFormat = new WaveFormat(waveStream.WaveFormat.SampleRate, 16, waveStream.WaveFormat.Channels);
                        waveStream = new WaveFormatConversionStream(wavFormat, waveStream);
                    }

                    var waveChannel = new WaveChannel32(waveStream);

                    WaveFormat format = waveChannel.WaveFormat;
                    var inputProvider = new AdvancedBufferedWaveProvider(format);
                    inputProvider.MaxQueuedBuffers = 100;

                    _s.SetSampleRate(format.SampleRate);
                    _s.SetChannels(format.Channels);

                    _s.SetTempoChange(0);
                    _s.SetPitchSemiTones(0);
                    _s.SetRateChange(0);

                    _s.SetTempo(tempoChange);

                    // Apply default SoundTouch settings
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_USE_QUICKSEEK, 0);

                    var profile = new TimeStretchProfile();
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_USE_AA_FILTER, profile.UseAAFilter ? 1 : 0);
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_AA_FILTER_LENGTH, profile.AAFilterLength);
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_OVERLAP_MS, profile.Overlap);
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_SEQUENCE_MS, profile.Sequence);
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_SEEKWINDOW_MS, profile.SeekWindow);

                    int bufferSecondLength = format.SampleRate * format.Channels;
                    byte[] inputBuffer = new byte[BufferSamples * sizeof(float)];
                    byte[] soundTouchOutBuffer = new byte[BufferSamples * sizeof(float)];

                    ByteAndFloatsConverter convertInputBuffer = new ByteAndFloatsConverter { Bytes = inputBuffer };
                    ByteAndFloatsConverter convertOutputBuffer = new ByteAndFloatsConverter { Bytes = soundTouchOutBuffer };
                    uint outBufferSizeFloats = (uint)convertOutputBuffer.Bytes.Length / (uint)(sizeof(float) * format.Channels);

                    int bytesRead;
                    int floatsRead;
                    uint samplesProcessed = 0;
                    int bufferIndex = 0;
                    TimeSpan actualEndMarker = TimeSpan.Zero;

                    while (waveChannel.Position < waveChannel.Length)
                    {
                        bytesRead = waveChannel.Read(convertInputBuffer.Bytes, 0, convertInputBuffer.Bytes.Length);

                        floatsRead = bytesRead / ((sizeof(float)) * format.Channels);

                        if (waveChannel.CurrentTime > waveChannel.TotalTime)
                        { 
                            _s.Clear();
                            _s.Flush();
                            waveChannel.Flush();


                            while (samplesProcessed != 0)
                            {
                                samplesProcessed = _s.ReceiveSamples(convertOutputBuffer.Floats, outBufferSizeFloats);

                                if (samplesProcessed > 0)
                                {
                                    TimeSpan currentBufferTime = waveChannel.CurrentTime;
                                    inputProvider.AddSamples(convertOutputBuffer.Bytes, 0, (int)samplesProcessed * sizeof(float) * format.Channels, currentBufferTime);
                                }
                            }
                        }

                        // ***                    Put samples in SoundTouch                   ***
                        _s.PutSamples(convertInputBuffer.Floats, (uint)floatsRead);

                        // Receive samples from SoundTouch
                        do
                        {
                            // ***                Receive samples back from SoundTouch            ***
                            // *** This is where Time Stretching and Pitch Changing are actually done *********
                            samplesProcessed = _s.ReceiveSamples(convertOutputBuffer.Floats, outBufferSizeFloats);
                            // **********************************************************************

                            if (samplesProcessed > 0)
                            {
                                TimeSpan currentBufferTime = waveChannel.CurrentTime;
                                // ** Play samples that came out of SoundTouch by adding them to AdvancedBufferedWaveProvider - the buffered player 
                                inputProvider.AddSamples(convertOutputBuffer.Bytes, 0, (int)samplesProcessed * sizeof(float) * format.Channels, currentBufferTime);
                                bufferIndex++;
                            }
                        } while (samplesProcessed != 0);
                    }

                    inputProviders.Add(inputProvider);

                }

                _s.Dispose();

                foreach (var input in inputProviders)
                {
                    PlayAudioResource(input);
                }
            }
        }

        private void PlayAudioResource(AdvancedBufferedWaveProvider input)
        {
            if (input != null)
            {

                using (var audioOutput = new DirectSoundOut(125))
                {
                    audioOutput.Init(input);
                    audioOutput.Play();

                  //while (audioOutput.PlaybackState != PlaybackState.Stopped)
                  while(input.GetQueueCount() > 0)
                  {
                       Thread.Sleep(20);
                  }

                  audioOutput.Stop();
                }
            }
            else
            {
                logger.Debug("    * input was null - check Resources!");
            }
        }


        private void PlayAudioResource(UnmanagedMemoryStream soundFile)
        {
            if (soundFile != null)
            {
                using (var wfr = new WaveFileReader(soundFile))
                {
                    using (WaveChannel32 wc = new WaveChannel32(wfr) { PadWithZeroes = false })
                    {
                        using (var audioOutput = new DirectSoundOut())
                        {
                            audioOutput.Init(wc);
                            audioOutput.Play();

                            while (audioOutput.PlaybackState != PlaybackState.Stopped)
                            {
                                Thread.Sleep(20);
                            }

                            audioOutput.Stop();
                        }
                    }
                }
            }
            else
            {
                logger.Debug("    * soundFile was null - check Resources!");
            }
        }
    }
}
