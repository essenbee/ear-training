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

namespace EarTraining
{
    public class Player
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IEnumerable<Chord> _chordProgression;
        SoundTouch<TSampleType, TLongSampleType> _soundTouch;
        SoundTouchSharp _s;

        private const int BuffSize = 2048;

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
                logger.Debug($"Tempo change set to {tempoChange}...");
                foreach (var chord in chordProgression)
                {
                    var inFile = new WavInFile(Resources.ResourceManager.GetStream(chord.GetAudioResourceName(numStrums),
                             CultureInfo.InvariantCulture));
                    var bits = inFile.GetNumBits();
                    var sampleRate = inFile.GetSampleRate();
                    var channels = inFile.GetNumChannels();
                    var outFile = new WavOutFile(new MemoryStream(), sampleRate, bits, channels);

                    _soundTouch.SetSampleRate(sampleRate);
                    _soundTouch.SetChannels(channels);

                    _soundTouch.SetTempoChange(tempoChange);
                    _soundTouch.SetPitchSemiTones(0.0f);
                    _soundTouch.SetRateChange(0.0f);
                    _soundTouch.SetSetting(SettingId.UseQuickseek, 1);
                    _soundTouch.SetSetting(SettingId.UseAntiAliasFilter, 0);

                    _s.SetSampleRate(sampleRate);
                    _s.SetChannels(channels);

                    _s.SetTempoChange(tempoChange);
                    _s.SetPitchSemiTones(0.0f);
                    _s.SetRateChange(0.0f);
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_USE_QUICKSEEK, 0);
                    _s.SetSetting(SoundTouchSharp.SoundTouchSettings.SETTING_USE_AA_FILTER, 0);

                    int nSamples;
                    uint sSamples;
                    var sampleBuffer = new TSampleType[BuffSize];
                    var sBuffer = new float[BuffSize];
                    var buffSizeSamples = BuffSize / channels;
                    var sBuffSizeSamples = (uint)(BuffSize / channels);

                    // Process samples read from the input file
                    while (!inFile.Eof())
                    {
                        // Read a chunk of samples from the input file
                        //var num = inFile.Read(sampleBuffer, BuffSize);

                        var num = inFile.Read(sBuffer, BuffSize);

                        nSamples = num / inFile.GetNumChannels();
                        sSamples = (uint)(num / inFile.GetNumChannels());

                        // Feed the samples into SoundTouch processor
                        _soundTouch.PutSamples(sampleBuffer, nSamples);
                        _s.PutSamples(sBuffer, sSamples);

                        // Read ready samples from SoundTouch processor & write them output file.
                        // NOTES:
                        // - 'receiveSamples' doesn't necessarily return any samples at all
                        //   during some rounds!
                        // - On the other hand, during some round 'receiveSamples' may have more
                        //   ready samples than would fit into 'sampleBuffer', and for this reason 
                        //   the 'receiveSamples' call is iterated for as many times as it
                        //   outputs samples.
                        do
                        {
                            nSamples = _soundTouch.ReceiveSamples(sampleBuffer, buffSizeSamples);
                            sSamples = _s.ReceiveSamples(sBuffer, sBuffSizeSamples);

                            outFile.Write(sBuffer, nSamples * channels);
                            //outFile.Write(sampleBuffer, nSamples * channels);
                            //} while (nSamples != 0);
                        } while (sSamples != 0);
                    }

                    // Now the input file is processed, yet 'flush' few last samples that are
                    // hiding in the SoundTouch's internal processing pipeline.
                    _soundTouch.Flush();
                    _s.Flush();

                    do
                    {
                        nSamples = _soundTouch.ReceiveSamples(sampleBuffer, buffSizeSamples);
                        sSamples = _s.ReceiveSamples(sBuffer, sBuffSizeSamples);

                        outFile.Write(sBuffer, nSamples * channels);
                        //outFile.Write(sampleBuffer, nSamples * channels);
                        //} while (nSamples != 0);
                    } while (sSamples != 0);

                    tempoChordProgression.Add(outFile);
                    //inFile?.Dispose();
                }

                foreach (var outFile in tempoChordProgression)
                {
                    PlayAudioResource(outFile);
                }
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

        private void PlayAudioResource(WavOutFile outFile)
        {
            if (outFile != null)
            {
                using (outFile)
                {
                    using (var wfr = new WaveFileReader(outFile.GetStream()))
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
            }
            else
            {
                logger.Debug("    * outFile was null - check Resources!");
            }
        }
    }
}
