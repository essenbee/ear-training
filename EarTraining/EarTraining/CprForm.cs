using EarTraining.Classes;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EarTraining
{
    public partial class CprForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Chord> _prevQuestion = null;
        private List<Chord> _chordPalette = null;
        private const string BeginnerText1 = "You will hear a chord progression made up of 4 chords,\n" +
                "each one strummed four times. Listen carefully and write\n" +
                "down the chords you hear in order. Don't worry if you\n" +
                "find it hard! Practise makes perfect.";
        private const string BeginnerText2 = "Placeholder text for stage 5+";

        public CprForm()
        {
            InitializeComponent();
            if (_prevQuestion == null)
            {
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;
            }

            _chordPalette = ChordLibrary.Stage1ChordPalette;
            beginnerRadioButton.Checked = true;
            stage1RadioButton.Checked = true;
            chordPaletteLabel.Text = GetChordNames(_chordPalette);
            notes.Text = BeginnerText1;
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void beginnerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (beginnerRadioButton.Checked)
            {
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

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
                stage1RadioButton.Checked = true;
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
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                stage1RadioButton.Enabled = true;
                stage2RadioButton.Enabled = true;
                stage3RadioButton.Enabled = true;
                stage4RadioButton.Enabled = true;
                stage5RadioButton.Enabled = true;
                stage6RadioButton.Enabled = false;
                stage7RadioButton.Enabled = false;
                stage8RadioButton.Enabled = false;
                stage9RadioButton.Enabled = false;

                _chordPalette = ChordLibrary.Stage1ChordPalette;
                stage1RadioButton.Checked = true;
                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void customRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _prevQuestion = null;
            repeat.Enabled = false;
            reveal.Enabled = false;
            answer.Text = string.Empty;

            stage1RadioButton.Enabled = false;
            stage2RadioButton.Enabled = false;
            stage3RadioButton.Enabled = false;
            stage4RadioButton.Enabled = false;
            stage5RadioButton.Enabled = false;
            stage6RadioButton.Enabled = false;
            stage7RadioButton.Enabled = false;
            stage8RadioButton.Enabled = false;
            stage9RadioButton.Enabled = false;

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
            var chordProgression = new List<Chord>();
            var rng = new CryptoRandom();

            if (chordPalette.Count > 4)
            {
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
            }
            else
            {
                // I want each chord in the progression once
                chordProgression = chordPalette;
                chordProgression.Shuffle();
                var rnd = rng.Next(0, chordPalette.Count - 1);
                var lastChord = chordPalette[rnd];
                chordProgression.Add(lastChord);
            }

            _prevQuestion = chordProgression;

            try
            {
                var player = new Player(chordProgression);
                player.PlayChords();
                play.Enabled = true;
                repeat.Enabled = true;
                reveal.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CprForm.play_Click() :: EXCEPTION - ");
            }
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

        private void stage1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(stage1RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage1ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage2RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage2ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage3RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage3ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage4RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage4RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage4ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText1;
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Intermediate Level coming soon!!";
                notes.Text = string.Empty;
            }
        }

        private void stage5RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage5RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage5ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = BeginnerText2;
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
                notes.Text = string.Empty;
            }
        }

        private void stage6RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage6RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage6ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Stage 6 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = string.Empty;

            }
        }

        private void stage7RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage7RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage7ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Stage 7 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = string.Empty;
            }
        }

        private void stage8RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage8RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage8ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Stage 8 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = string.Empty;
            }
        }

        private void stage9RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage9RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage9ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;
                reveal.Enabled = false;
                answer.Text = string.Empty;

                chordPaletteLabel.Text = "Stage 9 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
                notes.Text = string.Empty;
            }
        }

        private string GetChordNames(List<Chord> chords)
        {
            var chordNames = string.Empty;
            foreach (var chord in chords)
            {
                chordNames += (chord.ToString() + ", ");
            }

            chordNames = chordNames.Remove(chordNames.Length - 2, 2);

            return chordNames;
        }

        private void reveal_Click(object sender, EventArgs e)
        {
            answer.Text = GetChordNames(_prevQuestion);
        }
    }
}
