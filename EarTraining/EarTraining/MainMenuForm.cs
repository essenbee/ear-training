using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EarTraining.Classes;

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
                Name = "A",
                Quality = ChordQuality.Major,
                AudioFiles = new Dictionary<int, string>
                {
                    {4, "AChordFourStrums" },
                    {2, "AChordTwoStrums" },
                    {1, "AChordSingleStrum" }
                }
            };

            var DMajor = new Chord
            {
                Name = "D",
                Quality = ChordQuality.Major,
                AudioFiles = new Dictionary<int, string>
                {
                    {4, "DChordFourStrums" },
                    {2, "DChordTwoStrums" },
                    {1, "DChordSingleStrum" }
                }
            };

            var EMajor = new Chord
            {
                Name = "E",
                Quality = ChordQuality.Major,
                AudioFiles = new Dictionary<int, string>
                {
                    {4, "EChordFourStrums" },
                    {2, "EChordTwoStrums" },
                    {1, "EChordSingleStrum" }
                }
            };

            var chordProgression = new[] 
            {
                AMajor,
                DMajor,
                EMajor,
                AMajor,
            };

            var player = new Player(chordProgression);
            player.PlayChords();

        }
    }
}
