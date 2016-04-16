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
            var soundFile = Properties.Resources.AChordFourStrums;
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
