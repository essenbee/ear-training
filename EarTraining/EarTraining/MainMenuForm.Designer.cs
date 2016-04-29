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
            this.chordProgressions = new System.Windows.Forms.Button();
            this.ssrButton = new System.Windows.Forms.Button();
            this.cqrButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chordProgressions
            // 
            this.chordProgressions.Location = new System.Drawing.Point(54, 159);
            this.chordProgressions.Name = "chordProgressions";
            this.chordProgressions.Size = new System.Drawing.Size(112, 41);
            this.chordProgressions.TabIndex = 1;
            this.chordProgressions.Text = "Chord Progression Recognition";
            this.chordProgressions.UseVisualStyleBackColor = true;
            this.chordProgressions.Click += new System.EventHandler(this.chordProgressions_Click);
            // 
            // ssrButton
            // 
            this.ssrButton.Location = new System.Drawing.Point(54, 112);
            this.ssrButton.Name = "ssrButton";
            this.ssrButton.Size = new System.Drawing.Size(112, 41);
            this.ssrButton.TabIndex = 2;
            this.ssrButton.Text = "Single Sound Recognition";
            this.ssrButton.UseVisualStyleBackColor = true;
            this.ssrButton.Click += new System.EventHandler(this.ssrButton_Click);
            // 
            // cqrButton
            // 
            this.cqrButton.Location = new System.Drawing.Point(54, 65);
            this.cqrButton.Name = "cqrButton";
            this.cqrButton.Size = new System.Drawing.Size(112, 41);
            this.cqrButton.TabIndex = 3;
            this.cqrButton.Text = "Chord Quality Recognition";
            this.cqrButton.UseVisualStyleBackColor = true;
            this.cqrButton.Click += new System.EventHandler(this.cqrButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "J.U.S.T.I.N. Training";
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(73, 252);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 5;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 287);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cqrButton);
            this.Controls.Add(this.ssrButton);
            this.Controls.Add(this.chordProgressions);
            this.Name = "MainMenuForm";
            this.Text = "Ear Trainer";
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button chordProgressions;
        private System.Windows.Forms.Button ssrButton;
        private System.Windows.Forms.Button cqrButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exit;
    }
}

