using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EarTraining.Classes;
using NLog;

namespace EarTraining
{
    public partial class MainMenuForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {

        }

        private void play_Click(object sender, EventArgs e)
        {
            var chordPalette = ChordLibrary.Stage5ChordPalette;
            chordPalette.Shuffle();
            var chordProgression = new List<Chord>();
            var rng = new CryptoRandom();

            for (var i = 0; i < 4; i++)
            {
                var rnd = rng.Next(0, chordPalette.Count - 1);
                if (i != 0)
                {
                    var nextChord = chordPalette[rnd];
                    while (nextChord.Equals(chordProgression[i - 1]))
                    {
                        rnd = rng.Next(0, chordPalette.Count - 1);
                        nextChord = chordPalette[rnd];
                    }
                    chordProgression.Add(chordPalette[rnd]);
                }
                else
                {
                    chordProgression.Add(chordPalette[rnd]);
                }
            }

            try
            {
                var player = new Player(chordProgression);
                player.PlayChords();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "EXCEPTION: ");
            }
        }

        private void chordProgressions_Click(object sender, EventArgs e)
        {
            var cprForm = new CprForm();
            cprForm.Show();
        }
    }
}
