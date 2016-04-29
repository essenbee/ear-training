using EarTraining.Classes;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;

namespace EarTraining
{
    public partial class SsrForm : Form
    {
        // Private Variables
        // =================
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Chord _prevQuestion = null;
        private IList<Chord> _chordPalette = null;
        private Level _currentLevel;
        private int _currentStage;
        private Guid _selectedSoundDevice = Guid.Empty;

        // Private Constants
        // =================
        private const string BeginnerText1 = "Placeholder for Stages 1 - 4";
        private const string BeginnerText2 = "Placeholder for Stages 5 - 7";
        private const string BeginnerText3 = "Placeholder for Stages 8+";

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

        public SsrForm()
        {
            InitializeComponent();
            if (_prevQuestion == null)
            {
                InitialiseQuestion(1);
            }

            var soundDevices = DirectSoundOut.Devices;
            foreach (var device in soundDevices)
            {
                deviceComboBox.Items.Add(device.Description);
            }
            deviceComboBox.SelectedIndex = 0;

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
            var selectedChord = SelectChord(chordPalette);
            _prevQuestion = selectedChord;

            try
            {
                var player = new Player();
                player.PlayChord(selectedChord, _selectedSoundDevice, Tempo);
                play.Enabled = true;
                repeat.Enabled = true;
                reveal.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "SsrForm.play_Click() :: EXCEPTION - ");
            }
        }

        private Chord SelectChord(IList<Chord> chordPalette)
        {
            var rng = new CryptoRandom();
            var rnd = rng.Next(0, chordPalette.Count - 1);
            return chordPalette[rnd];
        }

        private void repeat_Click(object sender, EventArgs e)
        {
            answer.Text = string.Empty;
            play.Enabled = false;
            reveal.Enabled = false;

            try
            {
                var player = new Player();
                player.PlayChord(_prevQuestion, _selectedSoundDevice, Tempo);
                play.Enabled = true;
                reveal.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "SsrForm.repeat_Click() :: EXCEPTION - ");
            }
        }

        private void reveal_Click(object sender, EventArgs e)
        {
            answer.Text = _prevQuestion.Name;
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

        private void InitialiseQuestion(int stage)
        {
            _prevQuestion = null;
            repeat.Enabled = false;
            reveal.Enabled = false;
            answer.Text = string.Empty;
            _currentStage = stage;
        }

        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var deviceGuid = DirectSoundOut.Devices.ElementAtOrDefault(deviceComboBox.SelectedIndex);
            if (deviceGuid != null)
            {
                _selectedSoundDevice = deviceGuid.Guid;
            }
        }
    }
}
