using System.Globalization;
using NAudio.Wave;
using System.Threading;
using EarTraining.Classes;
using EarTraining.Properties;
using System.IO;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EarTraining
{
    public class Player
    {
        // Private static variables
        // ========================
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Private variables
        // =================
        private IEnumerable<Chord> _chordProgression;
        private SoundTouchSharp _s;
        private EqualizerEffect _eqEffect;
        public const int BufferSamples = 5 * 2048; // floats, not bytes

        // Constants
        // =========
        private const int Latency = 125;
        // Preset Equalizer Settings
        private const float DefaultLoDriveFactor = 75.0f;
        private const float DefaultLoGainFactor = 0.0f;
        private const float DefaultMedDriveFactor = 40.0f;
        private const float DefaultMedGainFactor = 0.0f;
        private const float DefaultHiDriveFactor = 30.0f;
        private const float DefaultHiGainFactor = 0.0f;

        #region Constructors

        public Player()
        {
            _s = new SoundTouchSharp();
        }

        #endregion

        #region Public Methods

        public Player(IEnumerable<Chord> chordProgression)
        {
            _chordProgression = chordProgression;
            _s = new SoundTouchSharp();
        }

        public void PlayChords(int numStrums = 4)
        {
            if (_chordProgression != null && _chordProgression.Any())
            {
                PlayChords(_chordProgression, numStrums, 1.0f);
            }
        }

        public void PlayChords(int numStrums, float tempoMultiplier)
        {
            if (_chordProgression != null && _chordProgression.Any())
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
                InitializeEqualizerEffect(waveChannel);

                var format = waveChannel.WaveFormat;
                var inputProvider = new AdvancedBufferedWaveProvider(format) {MaxQueuedBuffers = 100};

                SetupSoundTouch(format);

                // Here we set the tempo changes we want to apply...
                _s.SetTempo(chord.NormalTempoDelta * tempoMultiplier);

                var inputBuffer = new byte[BufferSamples * sizeof(float)];
                var soundTouchOutBuffer = new byte[BufferSamples * sizeof(float)];
                var convertInputBuffer = new ByteAndFloatsConverter { Bytes = inputBuffer };
                var convertOutputBuffer = new ByteAndFloatsConverter { Bytes = soundTouchOutBuffer };
                var outBufferSizeFloats = (uint)convertOutputBuffer.Bytes.Length / (uint)(sizeof(float) * format.Channels);
                uint samplesProcessed = 0;

                while (waveChannel.Position < waveChannel.Length)
                {
                    int bytesRead = waveChannel.Read(convertInputBuffer.Bytes, 0, convertInputBuffer.Bytes.Length);
                    int floatsRead = bytesRead / ((sizeof(float)) * format.Channels);

                    // Apply DSP effects here (preset equalizer settings)

                    ApplyDspEffects(convertInputBuffer.Floats, floatsRead);

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
                                var currentBufferTime = waveChannel.CurrentTime;
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
                            var currentBufferTime = waveChannel.CurrentTime;
                            inputProvider.AddSamples(convertOutputBuffer.Bytes, 0, (int)samplesProcessed * sizeof(float) * format.Channels, currentBufferTime);
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

        #endregion

        #region Private Methods

        private void PlayUsingNAudio(AdvancedBufferedWaveProvider input)
        {
            if (input != null)
            {
                // using (var audioOutput = new WasapiOut(0, Latency))
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

        private void InitializeEqualizerEffect(WaveChannel32 waveChannel)
        {
            // Initialize Equalizer
            _eqEffect = new EqualizerEffect {SampleRate = waveChannel.WaveFormat.SampleRate};
            _eqEffect.LoDriveFactor.Value = DefaultLoDriveFactor;
            _eqEffect.LoGainFactor.Value = DefaultLoGainFactor;
            _eqEffect.MedDriveFactor.Value = DefaultMedDriveFactor;
            _eqEffect.MedGainFactor.Value = DefaultMedGainFactor;
            _eqEffect.HiDriveFactor.Value = DefaultHiDriveFactor;
            _eqEffect.HiGainFactor.Value = DefaultHiGainFactor;
            _eqEffect.Init();
            _eqEffect.OnFactorChanges();
        }

        private void ApplyDspEffects(float[] buffer, int count)
        {
            int samples = count * 2;

            // Run each sample in the buffer through the equalizer effect
            for (int sample = 0; sample < samples; sample += 2)
            {
                // Get the samples, per audio channel
                float sampleLeft = buffer[sample];
                float sampleRight = buffer[sample + 1];

                // Apply the equalizer effect to the samples
                _eqEffect.Sample(ref sampleLeft, ref sampleRight);

                // Put the modified samples back into the buffer
                buffer[sample] = sampleLeft;
                buffer[sample + 1] = sampleRight;
            }
        }

        #endregion
    }
}
