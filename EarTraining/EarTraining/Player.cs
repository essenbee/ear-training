using System.IO;
using NAudio.Wave;
using System.Threading;
using EarTraining.Classes;
using EarTraining.Properties;

namespace EarTraining
{
    public class Player
    {
        Chord[] _chordProgression;

        public Player()
        {

        }

        public Player(Chord[] chordProgression)
        {
            _chordProgression = chordProgression;
        }

        public void PlayChords(int numStrums = 4)
        {
            if (_chordProgression != null && _chordProgression.Length > 0)
            {
                PlayChords(_chordProgression, numStrums);
            }
        }

        public void PlayChords(Chord[] chordProgression, int numStrums = 4)
        {
            foreach (var chord in chordProgression)
            {
                var soundFile = Resources.ResourceManager.GetStream(chord.AudioFiles[numStrums]);

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
        }
    }
}
