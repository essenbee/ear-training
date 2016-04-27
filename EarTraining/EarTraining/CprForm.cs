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

        public CprForm()
        {
            InitializeComponent();
            if (_prevQuestion == null)
            {
                repeat.Enabled = false;
            }

            _chordPalette = ChordLibrary.Stage1ChordPalette;
            beginnerRadioButton.Checked = true;
            stage1RadioButton.Checked = true;
            chordPaletteLabel.Text = GetChordNames(_chordPalette);

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
            }
        }

        private void customRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _prevQuestion = null;
            repeat.Enabled = false;
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
        }

        private void play_Click(object sender, EventArgs e)
        {
            var chordPalette = _chordPalette;
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

            _prevQuestion = chordProgression;
            repeat.Enabled = true;

            try
            {
                var player = new Player(chordProgression);
                player.PlayChords();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CprForm.play_Click() :: EXCEPTION - ");
            }
        }

        private void repeat_Click(object sender, EventArgs e)
        {
            try
            {
                var player = new Player(_prevQuestion);
                player.PlayChords();
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

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
            }
        }

        private void stage2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage2RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage2ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
            }
        }

        private void stage3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage3RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage3ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
            }
        }

        private void stage4RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage4RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage4ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Intermediate Level coming soon!!";
            }
        }

        private void stage5RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage5RadioButton.Checked && beginnerRadioButton.Checked)
            {
                _chordPalette = ChordLibrary.Stage5ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
            else
            {
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Intermediate Level coming soon!";
            }
        }

        private void stage6RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage6RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage6ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Stage 6 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
        }

        private void stage7RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage7RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage7ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Stage 7 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
        }

        private void stage8RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage8RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage8ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Stage 8 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
            }
        }

        private void stage9RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (stage9RadioButton.Checked && beginnerRadioButton.Checked)
            {
                // _chordPalette = ChordLibrary.Stage9ChordPalette;
                _prevQuestion = null;
                repeat.Enabled = false;

                chordPaletteLabel.Text = "Stage 9 coming soon!";
                // chordPaletteLabel.Text = GetChordNames(_chordPalette);
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
    }
}
