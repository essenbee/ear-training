namespace EarTraining
{
    partial class MainMenuForm
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
            this.play = new System.Windows.Forms.Button();
            this.chordProgressions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(58, 231);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(75, 23);
            this.play.TabIndex = 0;
            this.play.Text = "Play";
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // chordProgressions
            // 
            this.chordProgressions.Location = new System.Drawing.Point(58, 105);
            this.chordProgressions.Name = "chordProgressions";
            this.chordProgressions.Size = new System.Drawing.Size(112, 41);
            this.chordProgressions.TabIndex = 1;
            this.chordProgressions.Text = "Chord Progression Recognition";
            this.chordProgressions.UseVisualStyleBackColor = true;
            this.chordProgressions.Click += new System.EventHandler(this.chordProgressions_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 287);
            this.Controls.Add(this.chordProgressions);
            this.Controls.Add(this.play);
            this.Name = "MainMenuForm";
            this.Text = "Ear Training";
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button chordProgressions;
    }
}

