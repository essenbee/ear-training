using System;
using System.Globalization;
using NAudio.Wave;
using System.Threading;
using EarTraining.Classes;
using EarTraining.Properties;
using System.IO;
using NLog;
using System.Collections.Generic;
using System.Linq;

namespace EarTraining
{
    public class Player
    {
        // Private static variables
        // ========================
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Private variables
        // =================
        private IEnumerable<Bar> _chordProgression;
        private SoundTouchSharp _s;
        private EqualizerEffect _eqEffect;
        public const int BufferSamples = 5 * 2048; // floats, not bytes

        // Constants
        // =========
        private const int FloatSize = sizeof(float);
        private const int StrumsInBar = 4;

        // Properties
        // ==========
        public int Latency { get; set; }
        public float LoDriveFactor { get; set; }
        public float LoGainFactor { get; set; }
        public float MedDriveFactor { get; set; }
        public float MedGainFactor { get; set; }
        public float HiDriveFactor { get; set; }
        public float HiGainFactor { get; set; }

        #region Constructors

        public Player()
        {
            _s = new SoundTouchSharp();
            Latency = 125;
            // Preset Equalizer Settings
            LoDriveFactor = 75.0f;
            LoGainFactor = 0.0f;
            MedDriveFactor = 40.0f;
            MedGainFactor = 0.0f;
            HiDriveFactor = 30.0f;
            HiGainFactor = 0.0f;
        }
        public Player(IEnumerable<Bar> chordProgression)
        {
            _chordProgression = chordProgression;
            _s = new SoundTouchSharp();
            Latency = 125;
            // Preset Equalizer Settings
            LoDriveFactor = 75.0f;
            LoGainFactor = 0.0f;
            MedDriveFactor = 40.0f;
            MedGainFactor = 0.0f;
            HiDriveFactor = 30.0f;
            HiGainFactor = 0.0f;
        }
        #endregion

        #region Public Methods

        public void PlayChords()
        {
            if (_chordProgression != null && _chordProgression.Any())
            {
                PlayChords(_chordProgression, Guid.Empty, 1.0f);
            }
        }

        public void PlayChords(Guid deviceGuid, float tempoMultiplier)
        {
            if (_chordProgression != null && _chordProgression.Any())
            {
                PlayChords(_chordProgression, deviceGuid, tempoMultiplier);
            }
        }

        public void PlayChords(IEnumerable<Bar> chordProgression, Guid deviceGuid, float tempoMultiplier)
        {
            _s.CreateInstance();
            var inputProviders = new List<AdvancedBufferedWaveProvider>();

            foreach (var bar in chordProgression)
            {
                var numStrums = StrumsInBar / bar.Chords.Count; // 4, 2 or 1

                foreach (var chord in bar.Chords)
                {
                    var waveChannel =
                        GetWaveChannel(Resources.ResourceManager.GetStream(chord.GetAudioResourceName(numStrums),
                            CultureInfo.InvariantCulture));
                    InitializeEqualizerEffect(waveChannel);

                    var format = waveChannel.WaveFormat;
                    var inputProvider = new AdvancedBufferedWaveProvider(format) { MaxQueuedBuffers = 100 };

                    SetupSoundTouch(format);

                    // Here we set the tempo changes we want to apply...
                    _s.SetTempo(chord.NormalTempoDelta * tempoMultiplier);

                    var inputBuffer = new byte[BufferSamples * FloatSize];
                    var soundTouchOutBuffer = new byte[BufferSamples * FloatSize];
                    var convertInputBuffer = new ByteAndFloatsConverter { Bytes = inputBuffer };
                    var convertOutputBuffer = new ByteAndFloatsConverter { Bytes = soundTouchOutBuffer };
                    var outBufferSizeFloats = (uint)convertOutputBuffer.Bytes.Length / (uint)(FloatSize * format.Channels);
                    uint samplesProcessed = 0;

                    while (waveChannel.Position < waveChannel.Length)
                    {
                        var bytesRead = waveChannel.Read(convertInputBuffer.Bytes, 0, convertInputBuffer.Bytes.Length);
                        var floatsRead = bytesRead / (FloatSize * format.Channels);

                        // Apply DSP effects here (preset equalizer settings)
                        ApplyDspEffects(convertInputBuffer.Floats, floatsRead);

                        if (waveChannel.Position >= waveChannel.Length)
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
                                    inputProvider.AddSamples(convertOutputBuffer.Bytes, 0,
                                        (int)samplesProcessed * FloatSize * format.Channels, currentBufferTime);
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
                                inputProvider.AddSamples(convertOutputBuffer.Bytes, 0,
                                    (int)samplesProcessed * FloatSize * format.Channels, currentBufferTime);
                            }
                        } while (samplesProcessed != 0);
                    }

                    inputProviders.Add(inputProvider);
                }
            }

            _s.Dispose();

            foreach (var input in inputProviders)
            {
                PlayUsingNAudio(input, deviceGuid);
            }

        }

        #endregion

        #region Private Methods

        private void PlayUsingNAudio(AdvancedBufferedWaveProvider input, Guid deviceGuid)
        {
            if (input != null)
            {
                // using (var audioOutput = new WasapiOut(0, Latency))
                using (var audioOutput = new DirectSoundOut(deviceGuid, Latency))
                {
                    audioOutput.Init(input);
                    audioOutput.Play();

                    // Sleep current thread whilst the audio plays in another thread...
                    while (input.GetQueueCount() > 0)
                    {
                        Thread.Sleep(10);
                    }
                    // Playback is finished!
                    audioOutput.Stop();
                }
            }
            else
            {
                logger.Debug("Player.PlayUsingNAudio(): input buffer was null - check Resources!");
            }
        }

        private WaveChannel32 GetWaveChannel(Stream stream)
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

        private void InitializeEqualizerEffect(IWaveProvider waveChannel)
        {
            // Initialize Equalizer
            _eqEffect = new EqualizerEffect { SampleRate = waveChannel.WaveFormat.SampleRate };
            _eqEffect.LoDriveFactor.Value = LoDriveFactor;
            _eqEffect.LoGainFactor.Value = LoGainFactor;
            _eqEffect.MedDriveFactor.Value = MedDriveFactor;
            _eqEffect.MedGainFactor.Value = MedGainFactor;
            _eqEffect.HiDriveFactor.Value = HiDriveFactor;
            _eqEffect.HiGainFactor.Value = HiGainFactor;
            _eqEffect.Init();
            _eqEffect.OnFactorChanges();
        }

        private void ApplyDspEffects(IList<float> buffer, int count)
        {
            var samples = count * 2;

            // Run each sample in the buffer through the equalizer effect
            for (var sample = 0; sample < samples; sample += 2)
            {
                // Get the samples, per audio channel
                var sampleLeft = buffer[sample];
                var sampleRight = buffer[sample + 1];

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
