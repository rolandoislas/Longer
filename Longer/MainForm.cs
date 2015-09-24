using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Longer {
    public partial class MainForm : Form {
        private CreateGif gif;
        public MainForm() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            new ToolTip().SetToolTip(labelStep, "Seconds between each frame.");
            gif = new CreateGif();
            gif.progressUpdateEvent += updateProgressBar;
            gif.startedEvent += doStarted;
            gif.finishedEvent += doFinished;
        }

        delegate void doFinishedCallback(object sender, EventArgs e);

        private void doFinished(object sender, EventArgs e) {
            if (this.textBoxYear.InvokeRequired) {
                doFinishedCallback d = new doFinishedCallback(doFinished);
                this.Invoke(d, new object[] { sender, e });
            } else {
                this.textBoxYear.Enabled = true;
                this.textBoxStep.Enabled = true;
                this.progressBar.Visible = false;
            }
        }

        delegate void doStartedCallback(object sender, EventArgs e);

        private void doStarted(object sender, EventArgs e) {
            if (this.textBoxYear.InvokeRequired) {
                doStartedCallback d = new doStartedCallback(doStarted);
                this.Invoke(d, new object[] { sender, e });
            } else {
                this.textBoxYear.Enabled = false;
                this.textBoxStep.Enabled = false;
                this.progressBar.Visible = true;
            }
        }

        delegate void updateProgressBarCallback(object sender, EventArgs e);

        private void updateProgressBar(object sender, EventArgs e) {
            if (this.textBoxYear.InvokeRequired) {
                updateProgressBarCallback d = new updateProgressBarCallback(updateProgressBar);
                this.Invoke(d, new object[] { sender, e });
            } else {
                this.progressBar.Value = ((ProgressUpdateEventArgs)e).progress;
                Console.WriteLine(((ProgressUpdateEventArgs)e).progress);
            }
        }

        private void checkSubmit(object sender, KeyPressEventArgs e) {
            if (e.KeyChar.Equals((Char)Keys.Enter) && isInputValid()) {
                FolderBrowserDialog browse = new FolderBrowserDialog();
                DialogResult result = browse.ShowDialog();
                if (!result.Equals(DialogResult.OK))
                    return;
                new Thread(() => gif.create(int.Parse(textBoxYear.Text), int.Parse(textBoxStep.Text), browse.SelectedPath, 1280, 720)).Start();
                //new Thread(() => gif.create(1, 100000, browse.SelectedPath, 1280, 720)).Start();
            }
        }

        private bool isInputValid() {
            return true; // dev
            if (textBoxYear.Text.Equals("") || !textBoxYear.Text.All(Char.IsDigit) || int.Parse(textBoxYear.Text) == 0)
                return false;
            if (textBoxStep.Text.Equals("") || !textBoxStep.Text.All(Char.IsDigit) || int.Parse(textBoxStep.Text) == 0)
                return false;
            return true;
        }
    }
}
