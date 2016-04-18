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
            var AMajor = new Chord
            {
                Name = "A",
                Quality = ChordQuality.Major,
            };

            var DMajor = new Chord
            {
                Name = "D",
                Quality = ChordQuality.Major,
            };

            var EMajor = new Chord
            {
                Name = "E",
                Quality = ChordQuality.Major,
            };

            var AMinir = new Chord
            {
                Name = "Am",
                Quality = ChordQuality.Minor,
            };

            var DMinor = new Chord
            {
                Name = "Dm",
                Quality = ChordQuality.Minor,
            };

            var chordProgression = new[] 
            {
                AMajor,
                DMajor,
                EMajor,
                AMajor,
            };

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
