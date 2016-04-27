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

        private void chordProgressions_Click(object sender, EventArgs e)
        {
            var cprForm = new CprForm();
            cprForm.Show();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close(); ;

        }
    }
}
