namespace EarTraining
{
    partial class CprForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.close = new System.Windows.Forms.Button();
            this.beginnerRadioButton = new System.Windows.Forms.RadioButton();
            this.interRadioButton = new System.Windows.Forms.RadioButton();
            this.customRadioButton = new System.Windows.Forms.RadioButton();
            this.levelGroup = new System.Windows.Forms.GroupBox();
            this.stage1RadioButton = new System.Windows.Forms.RadioButton();
            this.stage2RadioButton = new System.Windows.Forms.RadioButton();
            this.stage3RadioButton = new System.Windows.Forms.RadioButton();
            this.stage4RadioButton = new System.Windows.Forms.RadioButton();
            this.stage5RadioButton = new System.Windows.Forms.RadioButton();
            this.stage6RadioButton = new System.Windows.Forms.RadioButton();
            this.stage7RadioButton = new System.Windows.Forms.RadioButton();
            this.stage8RadioButton = new System.Windows.Forms.RadioButton();
            this.stage9RadioButton = new System.Windows.Forms.RadioButton();
            this.stageGroupBox = new System.Windows.Forms.GroupBox();
            this.play = new System.Windows.Forms.Button();
            this.repeat = new System.Windows.Forms.Button();
            this.chordPaletteLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.levelGroup.SuspendLayout();
            this.stageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(607, 308);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // beginnerRadioButton
            // 
            this.beginnerRadioButton.AutoSize = true;
            this.beginnerRadioButton.Checked = true;
            this.beginnerRadioButton.Location = new System.Drawing.Point(14, 19);
            this.beginnerRadioButton.Name = "beginnerRadioButton";
            this.beginnerRadioButton.Size = new System.Drawing.Size(75, 17);
            this.beginnerRadioButton.TabIndex = 1;
            this.beginnerRadioButton.TabStop = true;
            this.beginnerRadioButton.Text = "Beginner";
            this.beginnerRadioButton.UseVisualStyleBackColor = true;
            this.beginnerRadioButton.CheckedChanged += new System.EventHandler(this.beginnerRadioButton_CheckedChanged);
            // 
            // interRadioButton
            // 
            this.interRadioButton.AutoSize = true;
            this.interRadioButton.Location = new System.Drawing.Point(14, 42);
            this.interRadioButton.Name = "interRadioButton";
            this.interRadioButton.Size = new System.Drawing.Size(95, 17);
            this.interRadioButton.TabIndex = 2;
            this.interRadioButton.Text = "Intermediate";
            this.interRadioButton.UseVisualStyleBackColor = true;
            this.interRadioButton.CheckedChanged += new System.EventHandler(this.interRadioButton_CheckedChanged);
            // 
            // customRadioButton
            // 
            this.customRadioButton.AutoSize = true;
            this.customRadioButton.Location = new System.Drawing.Point(14, 65);
            this.customRadioButton.Name = "customRadioButton";
            this.customRadioButton.Size = new System.Drawing.Size(66, 17);
            this.customRadioButton.TabIndex = 3;
            this.customRadioButton.Text = "Custom";
            this.customRadioButton.UseVisualStyleBackColor = true;
            this.customRadioButton.CheckedChanged += new System.EventHandler(this.customRadioButton_CheckedChanged);
            // 
            // levelGroup
            // 
            this.levelGroup.Controls.Add(this.beginnerRadioButton);
            this.levelGroup.Controls.Add(this.interRadioButton);
            this.levelGroup.Controls.Add(this.customRadioButton);
            this.levelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelGroup.Location = new System.Drawing.Point(25, 12);
            this.levelGroup.Name = "levelGroup";
            this.levelGroup.Size = new System.Drawing.Size(120, 107);
            this.levelGroup.TabIndex = 4;
            this.levelGroup.TabStop = false;
            this.levelGroup.Text = "Level";
            this.levelGroup.Enter += new System.EventHandler(this.levelGroup_Enter);
            // 
            // stage1RadioButton
            // 
            this.stage1RadioButton.AutoSize = true;
            this.stage1RadioButton.Checked = true;
            this.stage1RadioButton.Location = new System.Drawing.Point(14, 28);
            this.stage1RadioButton.Name = "stage1RadioButton";
            this.stage1RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage1RadioButton.TabIndex = 5;
            this.stage1RadioButton.TabStop = true;
            this.stage1RadioButton.Text = "Stage 1";
            this.stage1RadioButton.UseVisualStyleBackColor = true;
            this.stage1RadioButton.CheckedChanged += new System.EventHandler(this.stage1RadioButton_CheckedChanged);
            // 
            // stage2RadioButton
            // 
            this.stage2RadioButton.AutoSize = true;
            this.stage2RadioButton.Location = new System.Drawing.Point(14, 51);
            this.stage2RadioButton.Name = "stage2RadioButton";
            this.stage2RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage2RadioButton.TabIndex = 6;
            this.stage2RadioButton.Text = "Stage 2";
            this.stage2RadioButton.UseVisualStyleBackColor = true;
            this.stage2RadioButton.CheckedChanged += new System.EventHandler(this.stage2RadioButton_CheckedChanged);
            // 
            // stage3RadioButton
            // 
            this.stage3RadioButton.AutoSize = true;
            this.stage3RadioButton.Location = new System.Drawing.Point(14, 74);
            this.stage3RadioButton.Name = "stage3RadioButton";
            this.stage3RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage3RadioButton.TabIndex = 7;
            this.stage3RadioButton.Text = "Stage 3";
            this.stage3RadioButton.UseVisualStyleBackColor = true;
            this.stage3RadioButton.CheckedChanged += new System.EventHandler(this.stage3RadioButton_CheckedChanged);
            // 
            // stage4RadioButton
            // 
            this.stage4RadioButton.AutoSize = true;
            this.stage4RadioButton.Location = new System.Drawing.Point(14, 97);
            this.stage4RadioButton.Name = "stage4RadioButton";
            this.stage4RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage4RadioButton.TabIndex = 8;
            this.stage4RadioButton.Text = "Stage 4";
            this.stage4RadioButton.UseVisualStyleBackColor = true;
            this.stage4RadioButton.CheckedChanged += new System.EventHandler(this.stage4RadioButton_CheckedChanged);
            // 
            // stage5RadioButton
            // 
            this.stage5RadioButton.AutoSize = true;
            this.stage5RadioButton.Location = new System.Drawing.Point(14, 120);
            this.stage5RadioButton.Name = "stage5RadioButton";
            this.stage5RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage5RadioButton.TabIndex = 9;
            this.stage5RadioButton.Text = "Stage 5";
            this.stage5RadioButton.UseVisualStyleBackColor = true;
            this.stage5RadioButton.CheckedChanged += new System.EventHandler(this.stage5RadioButton_CheckedChanged);
            // 
            // stage6RadioButton
            // 
            this.stage6RadioButton.AutoSize = true;
            this.stage6RadioButton.Location = new System.Drawing.Point(14, 143);
            this.stage6RadioButton.Name = "stage6RadioButton";
            this.stage6RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage6RadioButton.TabIndex = 10;
            this.stage6RadioButton.Text = "Stage 6";
            this.stage6RadioButton.UseVisualStyleBackColor = true;
            this.stage6RadioButton.CheckedChanged += new System.EventHandler(this.stage6RadioButton_CheckedChanged);
            // 
            // stage7RadioButton
            // 
            this.stage7RadioButton.AutoSize = true;
            this.stage7RadioButton.Location = new System.Drawing.Point(14, 166);
            this.stage7RadioButton.Name = "stage7RadioButton";
            this.stage7RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage7RadioButton.TabIndex = 11;
            this.stage7RadioButton.Text = "Stage 7";
            this.stage7RadioButton.UseVisualStyleBackColor = true;
            this.stage7RadioButton.CheckedChanged += new System.EventHandler(this.stage7RadioButton_CheckedChanged);
            // 
            // stage8RadioButton
            // 
            this.stage8RadioButton.AutoSize = true;
            this.stage8RadioButton.Location = new System.Drawing.Point(14, 189);
            this.stage8RadioButton.Name = "stage8RadioButton";
            this.stage8RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage8RadioButton.TabIndex = 12;
            this.stage8RadioButton.Text = "Stage 8";
            this.stage8RadioButton.UseVisualStyleBackColor = true;
            this.stage8RadioButton.CheckedChanged += new System.EventHandler(this.stage8RadioButton_CheckedChanged);
            // 
            // stage9RadioButton
            // 
            this.stage9RadioButton.AutoSize = true;
            this.stage9RadioButton.Location = new System.Drawing.Point(14, 212);
            this.stage9RadioButton.Name = "stage9RadioButton";
            this.stage9RadioButton.Size = new System.Drawing.Size(69, 17);
            this.stage9RadioButton.TabIndex = 13;
            this.stage9RadioButton.Text = "Stage 9";
            this.stage9RadioButton.UseVisualStyleBackColor = true;
            this.stage9RadioButton.CheckedChanged += new System.EventHandler(this.stage9RadioButton_CheckedChanged);
            // 
            // stageGroupBox
            // 
            this.stageGroupBox.Controls.Add(this.stage9RadioButton);
            this.stageGroupBox.Controls.Add(this.stage1RadioButton);
            this.stageGroupBox.Controls.Add(this.stage8RadioButton);
            this.stageGroupBox.Controls.Add(this.stage2RadioButton);
            this.stageGroupBox.Controls.Add(this.stage7RadioButton);
            this.stageGroupBox.Controls.Add(this.stage3RadioButton);
            this.stageGroupBox.Controls.Add(this.stage6RadioButton);
            this.stageGroupBox.Controls.Add(this.stage4RadioButton);
            this.stageGroupBox.Controls.Add(this.stage5RadioButton);
            this.stageGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stageGroupBox.Location = new System.Drawing.Point(151, 12);
            this.stageGroupBox.Name = "stageGroupBox";
            this.stageGroupBox.Size = new System.Drawing.Size(120, 248);
            this.stageGroupBox.TabIndex = 14;
            this.stageGroupBox.TabStop = false;
            this.stageGroupBox.Text = "Stage";
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(25, 308);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(120, 23);
            this.play.TabIndex = 15;
            this.play.Text = "Play Next Question";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // repeat
            // 
            this.repeat.Location = new System.Drawing.Point(151, 308);
            this.repeat.Name = "repeat";
            this.repeat.Size = new System.Drawing.Size(120, 23);
            this.repeat.TabIndex = 16;
            this.repeat.Text = "Repeat Question";
            this.repeat.UseVisualStyleBackColor = true;
            this.repeat.Click += new System.EventHandler(this.repeat_Click);
            // 
            // chordPaletteLabel
            // 
            this.chordPaletteLabel.AutoSize = true;
            this.chordPaletteLabel.Location = new System.Drawing.Point(278, 40);
            this.chordPaletteLabel.Name = "chordPaletteLabel";
            this.chordPaletteLabel.Size = new System.Drawing.Size(0, 13);
            this.chordPaletteLabel.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(278, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Current Chord Palette";
            // 
            // CprForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 345);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chordPaletteLabel);
            this.Controls.Add(this.repeat);
            this.Controls.Add(this.play);
            this.Controls.Add(this.close);
            this.Controls.Add(this.levelGroup);
            this.Controls.Add(this.stageGroupBox);
            this.Name = "CprForm";
            this.Text = "J.U.S.T.I.N. Chord Progression Recognition Training";
            this.levelGroup.ResumeLayout(false);
            this.levelGroup.PerformLayout();
            this.stageGroupBox.ResumeLayout(false);
            this.stageGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.RadioButton beginnerRadioButton;
        private System.Windows.Forms.RadioButton interRadioButton;
        private System.Windows.Forms.RadioButton customRadioButton;
        private System.Windows.Forms.GroupBox levelGroup;
        private System.Windows.Forms.RadioButton stage1RadioButton;
        private System.Windows.Forms.RadioButton stage2RadioButton;
        private System.Windows.Forms.RadioButton stage3RadioButton;
        private System.Windows.Forms.RadioButton stage4RadioButton;
        private System.Windows.Forms.RadioButton stage5RadioButton;
        private System.Windows.Forms.RadioButton stage6RadioButton;
        private System.Windows.Forms.RadioButton stage7RadioButton;
        private System.Windows.Forms.RadioButton stage8RadioButton;
        private System.Windows.Forms.RadioButton stage9RadioButton;
        private System.Windows.Forms.GroupBox stageGroupBox;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button repeat;
        private System.Windows.Forms.Label chordPaletteLabel;
        private System.Windows.Forms.Label label2;
    }
}