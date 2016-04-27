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
                NormalTempoDelta = 1.0f,
            };

            var CMajor = new Chord
            {
                Name = "C",
                Quality = ChordQuality.Major,
                NormalTempoDelta = 1.2f,
            };

            var DMajor = new Chord
            {
                Name = "D",
                Quality = ChordQuality.Major,
                NormalTempoDelta = 1.0f,
            };

            var EMajor = new Chord
            {
                Name = "E",
                Quality = ChordQuality.Major,
                NormalTempoDelta = 1.0f,
            };

            var GMajor = new Chord
            {
                Name = "G",
                Quality = ChordQuality.Major,
                NormalTempoDelta = 1.2f,
            };

            // Minor Chords
            // ============

            var AMinor = new Chord
            {
                Name = "Am",
                Quality = ChordQuality.Minor,
                NormalTempoDelta = 1.2f,
            };

            var DMinor = new Chord
            {
                Name = "Dm",
                Quality = ChordQuality.Minor,
                NormalTempoDelta = 1.25f,
            };

            var EMinor = new Chord
            {
                Name = "Em",
                Quality = ChordQuality.Minor,
                NormalTempoDelta = 1.2f,
            };

            // Dominant 7th Chords
            // ===================

            var A7 = new Chord
            {
                Name = "A7",
                Quality = ChordQuality.Dominant7th,
                NormalTempoDelta = 1.2f,
            };

            var B7 = new Chord
            {
                Name = "B7",
                Quality = ChordQuality.Dominant7th,
                NormalTempoDelta = 1.25f,
            };

            var C7 = new Chord
            {
                Name = "C7",
                Quality = ChordQuality.Dominant7th,
                NormalTempoDelta = 1.25f,
            };

            var D7 = new Chord
            {
                Name = "D7",
                Quality = ChordQuality.Dominant7th,
                NormalTempoDelta = 1.4f,
            };

            var E7 = new Chord
            {
                Name = "E7",
                Quality = ChordQuality.Dominant7th,
                NormalTempoDelta = 1.1f,
            };

            var G7 = new Chord
            {
                Name = "G7",
                Quality = ChordQuality.Dominant7th,
                NormalTempoDelta = 1.2f,
            };

            // Major 7th Chords
            // ================

            var FMajor7 = new Chord
            {
                Name = "Fmaj7",
                Quality = ChordQuality.Major7th,
                NormalTempoDelta = 1.15f,
            };

            // =======================================================

            var chordPalette = new List<Chord>
            {
                AMajor, CMajor, GMajor,
                DMajor, EMajor, AMinor,
                DMinor, EMinor,
                D7, A7, E7, B7, G7, C7,
                FMajor7,
            };

            chordPalette.Shuffle<Chord>();

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
                player.PlayChords(4);
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
