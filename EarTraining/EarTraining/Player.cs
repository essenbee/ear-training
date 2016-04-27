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
        private const int Latency = 125;

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

        public void PlayChords(int numStrums = 4)
        {
            if (_chordProgression != null && _chordProgression.Count() > 0)
            {
                PlayChords(_chordProgression, numStrums, 1.0f);
            }
        }

        public void PlayChords(int numStrums, float tempoMultiplier)
        {
            if (_chordProgression != null && _chordProgression.Count() > 0)
            {
                PlayChords(_chordProgression, numStrums, tempoMultiplier);
            }
        }

        public void PlayChords(IEnumerable<Chord> chordProgression, int numStrums, float tempoMultiplier)
        {
            _s.CreateInstance();
            var inputProviders = new List<AdvancedBufferedWaveProvider>();

            foreach (var chord in chordProgression)
            {
                var waveChannel = GetWaveChannel(Resources.ResourceManager.GetStream(chord.GetAudioResourceName(numStrums),
                         CultureInfo.InvariantCulture));
                var format = waveChannel.WaveFormat;
                var inputProvider = new AdvancedBufferedWaveProvider(format);
                inputProvider.MaxQueuedBuffers = 100;

                SetupSoundTouch(format);

                // Here we set the tempo chnages we want to apply...
                _s.SetTempo(chord.NormalTempoDelta * tempoMultiplier);

                var bufferSecondLength = format.SampleRate * format.Channels;
                var inputBuffer = new byte[BufferSamples * sizeof(float)];
                var soundTouchOutBuffer = new byte[BufferSamples * sizeof(float)];
                var convertInputBuffer = new ByteAndFloatsConverter { Bytes = inputBuffer };
                var convertOutputBuffer = new ByteAndFloatsConverter { Bytes = soundTouchOutBuffer };
                var outBufferSizeFloats = (uint)convertOutputBuffer.Bytes.Length / (uint)(sizeof(float) * format.Channels);
                uint samplesProcessed = 0;
                int bufferIndex = 0;
                var actualEndMarker = TimeSpan.Zero;

                while (waveChannel.Position < waveChannel.Length)
                {
                    int bytesRead = waveChannel.Read(convertInputBuffer.Bytes, 0, convertInputBuffer.Bytes.Length);
                    int floatsRead = bytesRead / ((sizeof(float)) * format.Channels);

                    if (waveChannel.CurrentTime >= waveChannel.TotalTime)
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

                    // Put samples into SoundTouch for processing...
                    _s.PutSamples(convertInputBuffer.Floats, (uint)floatsRead);

                    do
                    {
                        // Receive samples back from SoundTouch...
                        // This is where Time Stretching and Pitch Changing are actually done...
                        samplesProcessed = _s.ReceiveSamples(convertOutputBuffer.Floats, outBufferSizeFloats);

                        if (samplesProcessed > 0)
                        {
                            TimeSpan currentBufferTime = waveChannel.CurrentTime;
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
                PlayUsingNAudio(input);
            }

        }

        private void PlayUsingNAudio(AdvancedBufferedWaveProvider input)
        {
            if (input != null)
            {
                using (var audioOutput = new DirectSoundOut(Latency))
                {
                    audioOutput.Init(input);
                    audioOutput.Play();

                    // Sleep current thread whilst the audio plays in another thread...
                    while (input.GetQueueCount() > 0) { Thread.Sleep(10); }
                    // Playback is finished!
                    audioOutput.Stop();
                }
            }
            else
            {
                logger.Debug("Player.PlayUsingNAudio(): input buffer was null - check Resources!");
            }
        }

        private WaveChannel32 GetWaveChannel(UnmanagedMemoryStream stream)
        {
            WaveStream waveStream = new WaveFileReader(stream);

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
            return waveChannel;
        }

        private void SetupSoundTouch(WaveFormat format)
        {
            _s.SetSampleRate(format.SampleRate);
            _s.SetChannels(format.Channels);
            _s.SetTempoChange(0);
            _s.SetPitchSemiTones(0);
            _s.SetRateChange(0);

            // Apply default SoundTouch settings
            _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_USE_QUICKSEEK, 0);

            var profile = new TimeStretchProfile();
            _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_USE_AA_FILTER, profile.UseAAFilter ? 1 : 0);
            _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_AA_FILTER_LENGTH, profile.AAFilterLength);
            _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_OVERLAP_MS, profile.Overlap);
            _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_SEQUENCE_MS, profile.Sequence);
            _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_SEEKWINDOW_MS, profile.SeekWindow);
        }
    }
}
