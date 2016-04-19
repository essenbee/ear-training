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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IEnumerable<Chord> _chordProgression;

        public Player()
        {

        }

        public Player(IEnumerable<Chord> chordProgression)
        {
            _chordProgression = chordProgression;
        }

        public void PlayChords(int numStrums = 4)
        {
            if (_chordProgression != null && _chordProgression.Count() > 0)
            {
                PlayChords(_chordProgression, numStrums);
            }
        }

        public void PlayChords(IEnumerable<Chord> chordProgression, int numStrums = 4)
        {
            foreach (var chord in chordProgression)
            {
                logger.Debug($"Playing chord {chord.Name}...");
                PlayAudioResource(Resources.ResourceManager.GetStream(chord.GetAudioResourceName(numStrums), 
                    CultureInfo.InvariantCulture));

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
