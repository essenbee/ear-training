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
            // Major Chords
            // ============

            var AMajor = new Chord
            {
                Name = "A",
                Quality = ChordQuality.Major,
            };

            var CMajor = new Chord
            {
                Name = "C",
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

            var GMajor = new Chord
            {
                Name = "G",
                Quality = ChordQuality.Major,
            };

            // Minor Chords
            // ============

            var AMinor = new Chord
            {
                Name = "Am",
                Quality = ChordQuality.Minor,
            };

            var DMinor = new Chord
            {
                Name = "Dm",
                Quality = ChordQuality.Minor,
            };

            var EMinor = new Chord
            {
                Name = "Em",
                Quality = ChordQuality.Minor,
            };

            // Dominant 7th Chords
            // ===================

            var A7 = new Chord
            {
                Name = "A7",
                Quality = ChordQuality.Dominant7th,
            };

            var B7 = new Chord
            {
                Name = "B7",
                Quality = ChordQuality.Dominant7th,
            };

            var C7 = new Chord
            {
                Name = "C7",
                Quality = ChordQuality.Dominant7th,
            };

            var D7 = new Chord
            {
                Name = "D7",
                Quality = ChordQuality.Dominant7th,
            };

            var E7 = new Chord
            {
                Name = "E7",
                Quality = ChordQuality.Dominant7th,
            };

            var G7 = new Chord
            {
                Name = "G7",
                Quality = ChordQuality.Dominant7th,
            };

            // Major 7th Chords
            // ================

            var FMajor7 = new Chord
            {
                Name = "Fmaj7",
                Quality = ChordQuality.Major7th,
            };

            // =======================================================

            var chordProgression = new List<Chord> 
            {
                CMajor,
                DMinor,
                E7,
                FMajor7,
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
