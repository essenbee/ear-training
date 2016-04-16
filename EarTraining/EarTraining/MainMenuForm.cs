using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.Threading;
using EarTraining.Classes;
using System.IO;

namespace EarTraining
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {

        }

        private void play_Click(object sender, EventArgs e)
        {
            var AMajor = new Chord
            {
                Name = "A major",
                Quality = ChordQuality.Major,
                AudioStreams = new Dictionary<int, UnmanagedMemoryStream>
                {
                    {4, Properties.Resources.AChordFourStrums },
                    {2, Properties.Resources.AChordTwoStrums },
                    {1, Properties.Resources.AChordSingleStrum }
                }
            };

            var DMajor = new Chord
            {
                Name = "D major",
                Quality = ChordQuality.Major,
                AudioStreams = new Dictionary<int, UnmanagedMemoryStream>
                {
                    {4, Properties.Resources.DChordFourStrums },
                    {2, Properties.Resources.DChordTwoStrums },
                    {1, Properties.Resources.DChordSingleStrum }
                }
            };

            var EMajor = new Chord
            {
                Name = "E major",
                Quality = ChordQuality.Major,
                AudioStreams = new Dictionary<int, UnmanagedMemoryStream>
                {
                    {4, Properties.Resources.EChordFourStrums },
                    {2, Properties.Resources.EChordTwoStrums },
                    {1, Properties.Resources.EChordSingleStrum }
                }
            };

            var numStrums = 4;

            var soundFiles = new[] 
            {
                AMajor.AudioStreams[numStrums],
                DMajor.AudioStreams[numStrums],
                EMajor.AudioStreams[numStrums],
                AMajor.AudioStreams[numStrums],
            };
            foreach (var soundFile in soundFiles)
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
        }
    }
}
