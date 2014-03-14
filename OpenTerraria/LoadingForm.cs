using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenTerraria {
    public partial class LoadingForm : Form {
        public String currentStep = "Loading...";
        public int currentProgress = 0;
        public LoadingForm() {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            progressBar1.Refresh();
        }
        public void setProgress(String step, int progress) {
            StepLabel.Text = currentStep + " (" + progress + "%)";
            StepLabel.Left = ((this.Width / 2) - (StepLabel.Width / 2));
            if (progressBar1.Value != progress) {
                progressBar1.Value = progress;
            }
            currentStep = step;
            currentProgress = progress;
            Refresh();
        }
    }
}
