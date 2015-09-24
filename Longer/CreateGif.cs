using System;
using BumpKit;
using System.IO;
using System.Drawing;

namespace Longer {
    class CreateGif {
        public EventHandler progressUpdateEvent;
        public EventHandler startedEvent;
        public EventHandler finishedEvent;

        public void create(int years, int step, string path, int width, int height) {
            startedEvent.Invoke(this, EventArgs.Empty);
            double seconds = years * 365.25 * 24 * 60 * 60;
            double frames = Math.Floor(seconds / step);
            int fontSize = (int)(width * .1);
            using (FileStream gif = File.OpenWrite(Path.Combine(path, "Longer_" + frames + "_frames.gif")))
            using (GifEncoder encoder = new GifEncoder(gif)) {
                encoder.FrameDelay = TimeSpan.FromSeconds(step);
                for (double i = 0; i < frames; i++) {
                    using (Bitmap frame = new Bitmap(width, height))
                    using (Graphics g = Graphics.FromImage(frame))
                    using (Font font = new Font("Helvetica", fontSize)) {
                        string frameText = (i + 1).ToString();
                        SizeF textSize = g.MeasureString(frameText, font);
                        g.DrawString(frameText, font, Brushes.White, width / 2 - textSize.Width / 2, height / 2 - textSize.Height / 2);
                        encoder.AddFrame(frame);
                    }
                    progressUpdateEvent.Invoke(this, new ProgressUpdateEventArgs((int)((i+1D) / frames * 100D)));
                }
            }
            finishedEvent.Invoke(this, EventArgs.Empty);
        }

        internal void createThreaded(object obj) {
            throw new NotImplementedException();
        }
    }

    class ProgressUpdateEventArgs : EventArgs {
        public int progress;
        public ProgressUpdateEventArgs(int progress) {
            this.progress = progress;
        }
    }
}
