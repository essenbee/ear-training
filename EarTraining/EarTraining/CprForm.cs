using EarTraining.Classes;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EarTraining
{
    public partial class CprForm : Form
    {
        // Private Variables
        // =================
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Bar> _prevQuestion = null;
        private List<Chord> _chordPalette = null;
        private Level _currentLevel;
        private int _currentStage;

        // Private Constants
        // =================
        private const string BeginnerText1 = "You will hear a chord progression made up of 4 bars,\n" +
            "each bar will be a chord strummed four times. Listen\n" +
            "carefully and write down the chords you hear in order.\n" +
            "Don't worry if you find it hard! Practise makes perfect.";
        private const string BeginnerText2 = "You will hear a chord progression made up of 4 bars,\n" +
            "but now you may get some bars that contain two chords!\n" +
            "I am also going to play the chords a bit faster. This is a\n" +
            "lot harder, but we are moving towards being able to transcribe\n" +
            "actual songs, so stick with it!";
        private const string BeginnerText3 = "Placeholder for Stages 8+";
        private const int MaxBars = 4;
        private const int TwoChordsInBarProbabilityMultiplier = 2;

        // Properties
        private float Tempo
        {
            get
            {
                var tempo = 1.0f;
                if (_currentLevel.Equals(Level.Beginner))
                {
                    if (_currentStage > 4)
                    {
                        tempo = 1.25f;
                    }
                    if (_currentStage > 7)
                    {
                        tempo = 1.50f;
                    }
                }
                else
                {
                    tempo = 1.5f;
                }
                return tempo;
            }
        }

        public CprForm()
        {
            InitializeComponent();
            if (_prevQuestion == null)
            {
                InitialiseQuestion(1);
            }

            _chordPalette = ChordLibrary.Stage1ChordPalette;
            beginnerRadioButton.Checked = true;
            _currentLevel = Level.Beginner;
            stage1RadioButton.Checked = true;
            _currentStage = 1;
            chordPaletteLabel.Text = GetChordNames(_chordPalette);
            notes.Text = BeginnerText1;
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beginnerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (beginnerRadioButton.Checked)
            {
                InitialiseQuestion(1);
                stage1RadioButton.Enabled = true;
                stage2RadioButton.Enabled = true;
                stage3RadioButton.Enabled = true;
                stage4RadioButton.Enabled = true;
                stage5RadioButton.Enabled = true;
                stage6RadioButton.Enabled = true;
                stage7RadioButton.Enabled = true;
                stage8RadioButton.Enabled = true;
                stage9RadioButton.Enabled = true;

                _chordPalette = ChordLibrary.Stage1ChordPalette;
                beginnerRadioButton.Checked = true;
                _currentLevel = Level.Beginner;
                stage1RadioButton.Checked = true;
                _currentStage = 1;
                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
        }

        private void levelGroup_Enter(object sender, EventArgs e)
        {

        }

        private void interRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (interRadioButton.Checked)
            {
                InitialiseQuestion(1);

                stage1RadioButton.Enabled = true;
                stage2RadioButton.Enabled = true;
                stage3RadioButton.Enabled = true;
                stage4RadioButton.Enabled = true;
                stage5RadioButton.Enabled = true;
                stage6RadioButton.Enabled = false;
                stage7RadioButton.Enabled = false;
                stage8RadioButton.Enabled = false;
                stage9RadioButton.Enabled = false;

                _currentLevel = Level.Intermediate;
                _chordPalette = ChordLibrary.Stage1ChordPalette;
                stage1RadioButton.Checked = true;
                _currentStage = 1;
                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void customRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            InitialiseQuestion(1);

            stage1RadioButton.Enabled = false;
            stage2RadioButton.Enabled = false;
            stage3RadioButton.Enabled = false;
            stage4RadioButton.Enabled = false;
            stage5RadioButton.Enabled = false;
            stage6RadioButton.Enabled = false;
            stage7RadioButton.Enabled = false;
            stage8RadioButton.Enabled = false;
            stage9RadioButton.Enabled = false;

            _currentLevel = Level.Custom;
            _chordPalette = null;
            chordPaletteLabel.Text = "Custom Level coming soon!";
            notes.Text = string.Empty;
        }

        private void play_Click(object sender, EventArgs e)
        {
            play.Enabled = false;
            repeat.Enabled = false;
            reveal.Enabled = false;
            answer.Text = string.Empty;

            // make a copy of the current chord palette...
            var chordPalette = new List<Chord>();
            foreach (var chord in _chordPalette)
            {
                chordPalette.Add(chord);
            }

            // Shuffle the fuller palettes to improve randomness...
            if (chordPalette.Count > 4)
            {
                chordPalette.Shuffle();
            }
            var chordProgression = new List<Bar>();
            var rng = new CryptoRandom();

            if (chordPalette.Count > 4)
            {
                for (var barNumber = 0; barNumber < MaxBars; barNumber++)
                {
                    var rnd = rng.Next(1, 100);
                    if (_currentLevel.Equals(Level.Beginner) && _currentStage < 5)
                    {
                        OneChordInBar(rng, chordPalette, barNumber, chordProgression);
                    }
                    else if (_currentLevel.Equals(Level.Beginner))
                    {
                        if (rnd > TwoChordsInBarProbabilityMultiplier * _currentStage)
                        {
                            OneChordInBar(rng, chordPalette, barNumber, chordProgression);
                        }
                        else
                        {
                            TwoChordsInBar(rng, chordPalette, barNumber, chordProgression);
                        }
                    }
                    else
                    {
                        OneChordInBar(rng, chordPalette, barNumber, chordProgression);
                        // TwoChordsInBar(rng, chordPalette, barNumber, chordProgression);
                    }
                }
            }
            else
            {
                // I want each chord in the progression once; there is no need
                // to consider more than one chord per bar at Beginner Stage 1...
                foreach (var chord in chordPalette)
                {
                    var aBar = new Bar();
                    aBar.Chords.Add(chord);
                    chordProgression.Add(aBar);
                }

                chordProgression.Shuffle();
                var rnd = rng.Next(0, chordPalette.Count - 1);
                var lastChord = chordPalette[rnd];
                var lastBar = new Bar();
                lastBar.Chords.Add(lastChord);
                chordProgression.Add(lastBar);
            }

            _prevQuestion = chordProgression;

            try
            {
                var player = new Player(chordProgression);
                player.PlayChords(Tempo);
                play.Enabled = true;
                repeat.Enabled = true;
                reveal.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CprForm.play_Click() :: EXCEPTION - ");
            }
        }

        private void OneChordInBar(CryptoRandom rng, IList<Chord> chordPalette, int barNumber, IList<Bar> chordProgression)
        {
            var thisBar = new Bar();
            var rnd = rng.Next(0, chordPalette.Count - 1);
            if (barNumber != 0)
            {
                var previousBar = barNumber - 1;
                var nextChord = chordPalette[rnd];
                while (nextChord.Equals(chordProgression[previousBar].Chords.Last()))
                {
                    rnd = rng.Next(0, chordPalette.Count - 1);
                    nextChord = chordPalette[rnd];
                }

                thisBar.Chords.Add(chordPalette[rnd]);
                chordProgression.Add(thisBar);
            }
            else
            {
                thisBar.Chords.Add(chordPalette[rnd]);
                chordProgression.Add(thisBar);
            }
        }

        private void TwoChordsInBar(CryptoRandom rng, IList<Chord> chordPalette, int barNumber, IList<Bar> chordProgression)
        {
            var thisBar = new Bar();
            var rnd = rng.Next(0, chordPalette.Count - 1);
            for (var chordNum = 0; chordNum < 2; chordNum++)
            {
                if (chordNum != 0)
                {
                    var nextChord = chordPalette[rnd];
                    while (nextChord.Equals(thisBar.Chords.First()))
                    {
                        rnd = rng.Next(0, chordPalette.Count - 1);
                        nextChord = chordPalette[rnd];
                    }

                    thisBar.Chords.Add(chordPalette[rnd]);
                }
                else if (barNumber != 0)
                {
                    var previousBar = barNumber - 1;
                    var nextChord = chordPalette[rnd];
                    while (nextChord.Equals(chordProgression[previousBar].Chords.Last()))
                    {
                        rnd = rng.Next(0, chordPalette.Count - 1);
                        nextChord = chordPalette[rnd];
                    }

                    thisBar.Chords.Add(chordPalette[rnd]);
                }
                else
                {
                    thisBar.Chords.Add(chordPalette[rnd]);
                }
            }

            chordProgression.Add(thisBar);
        }

        private void repeat_Click(object sender, EventArgs e)
        {
            answer.Text = string.Empty;
            play.Enabled = false;
            reveal.Enabled = false;

            try
            {
                var player = new Player(_prevQuestion);
                player.PlayChords();
                play.Enabled = true;
                reveal.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CprForm.repeat_Click() :: EXCEPTION - ");
            }
        }

        private void reveal_Click(object sender, EventArgs e)
        {
            answer.Text = GetChordNames(_prevQuestion);
        }

        private void stage1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage1RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage1ChordPalette;
                InitialiseQuestion(1);

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                InitialiseQuestion(1);
                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage2RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage2ChordPalette;
                InitialiseQuestion(2);

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                InitialiseQuestion(2);

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage3RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage3ChordPalette;
                InitialiseQuestion(3);

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                InitialiseQuestion(3);

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage4RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage4RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage4ChordPalette;
                InitialiseQuestion(4);

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                InitialiseQuestion(4);
                chordPaletteLabel.Text = "Intermediate Level coming soon!!";
                notes.Text = string.Empty;
            }
        }

        private void stage5RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage5RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage5ChordPalette;
                InitialiseQuestion(5);

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText2;
            }
            else
            {
                InitialiseQuestion(5);

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage6RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage6RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage6ChordPalette;
                InitialiseQuestion(6);

                chordPaletteLabel.Text = "Stage 6 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText2;
            }
        }

        private void stage7RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage7RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage7ChordPalette;
                InitialiseQuestion(7);

                chordPaletteLabel.Text = "Stage 7 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText2;
            }
        }

        private void stage8RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage8RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage8ChordPalette;
                InitialiseQuestion(8);

                chordPaletteLabel.Text = "Stage 8 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText3;
            }
        }

        private void stage9RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage9RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage9ChordPalette;
                InitialiseQuestion(9);

                chordPaletteLabel.Text = "Stage 9 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText3;
            }
        }

        private string GetChordNames(IEnumerable<Chord> chords)
        {
            var chordNames = chords.Aggregate(string.Empty, (current, chord) => current + (chord + ", "));
            chordNames = chordNames.Remove(chordNames.Length - 2, 2);
            return chordNames;
        }
        private string GetChordNames(IEnumerable<Bar> bars)
        {
            var chordNames = string.Empty;
            foreach (var bar in bars)
            {
                chordNames = bar.Chords.Aggregate(chordNames, (current, chord) => current + (chord + " "));
                chordNames += "| ";
            }

            chordNames = chordNames.Remove(chordNames.Length - 2, 2);
            return chordNames;
        }

        private void InitialiseQuestion(int stage)
        {
            _prevQuestion = null;
            repeat.Enabled = false;
            reveal.Enabled = false;
            answer.Text = string.Empty;
            _currentStage = stage;
        }
    }
}
