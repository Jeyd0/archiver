namespace archiver
{
    public partial class AddFilesProgressForm : Form
    {
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblPercentage;
        private Label lblTimeRemaining;
        private Label lblCurrentFile;
        private Button btnCancel;
        private CancellationTokenSource? _cancellationTokenSource;
        private DateTime _startTime;

        public bool CancelRequested => _cancellationTokenSource?.IsCancellationRequested ?? false;
        public CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? CancellationToken.None;

        public AddFilesProgressForm()
        {
            InitializeAddFilesProgressForm();
        }

        private void InitializeAddFilesProgressForm()
        {
            // Form settings
            this.Text = "Adding Files...";
            this.Size = new Size(450, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.BackColor = SystemColors.ActiveCaption;

            // Status label
            lblStatus = new Label
            {
                Text = "Preparing to add files...",
                Location = new Point(20, 15),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblStatus);

            // Current file label
            lblCurrentFile = new Label
            {
                Text = "",
                Location = new Point(20, 38),
                Size = new Size(400, 20),
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(lblCurrentFile);

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(20, 65),
                Size = new Size(400, 25),
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous
            };
            this.Controls.Add(progressBar);

            // Percentage label
            lblPercentage = new Label
            {
                Text = "0%",
                Location = new Point(20, 95),
                Size = new Size(60, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblPercentage);

            // Time remaining label
            lblTimeRemaining = new Label
            {
                Text = "Estimated time: Calculating...",
                Location = new Point(200, 95),
                Size = new Size(220, 20),
                TextAlign = ContentAlignment.TopRight
            };
            this.Controls.Add(lblTimeRemaining);

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(175, 125),
                Size = new Size(100, 30),
                BackColor = Color.Silver,
                FlatStyle = FlatStyle.Popup,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel adding files?",
                "Cancel Adding Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _cancellationTokenSource?.Cancel();
                lblStatus.Text = "Cancelling...";
                btnCancel.Enabled = false;
            }
        }

        /// <summary>
        /// Starts the progress tracking timer
        /// </summary>
        public void StartProgress()
        {
            _startTime = DateTime.Now;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Updates the progress display
        /// </summary>
        /// <param name="currentFile">Current file being processed</param>
        /// <param name="currentIndex">Current file index (1-based)</param>
        /// <param name="totalFiles">Total number of files</param>
        public void UpdateProgress(string currentFile, int currentIndex, int totalFiles)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => UpdateProgress(currentFile, currentIndex, totalFiles));
                return;
            }

            // Calculate percentage
            int percentage = (int)((double)currentIndex / totalFiles * 100);
            progressBar.Value = Math.Min(percentage, 100);
            lblPercentage.Text = $"{percentage}%";

            // Update status
            lblStatus.Text = $"Adding file {currentIndex} of {totalFiles}";
            lblCurrentFile.Text = $"Current: {currentFile}";

            // Calculate estimated time remaining
            if (currentIndex > 0)
            {
                TimeSpan elapsed = DateTime.Now - _startTime;
                double avgTimePerFile = elapsed.TotalSeconds / currentIndex;
                int remainingFiles = totalFiles - currentIndex;
                double remainingSeconds = avgTimePerFile * remainingFiles;

                lblTimeRemaining.Text = $"Estimated time: {FormatTimeRemaining(remainingSeconds)}";
            }
        }

        /// <summary>
        /// Marks the adding as complete
        /// </summary>
        public void SetComplete(int addedCount, int skippedCount)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetComplete(addedCount, skippedCount));
                return;
            }

            progressBar.Value = 100;
            lblPercentage.Text = "100%";
            lblStatus.Text = $"Complete! Added {addedCount} file(s), skipped {skippedCount}";
            lblCurrentFile.Text = "";
            lblTimeRemaining.Text = "Done!";
        }

        /// <summary>
        /// Formats time remaining into a readable string
        /// </summary>
        private string FormatTimeRemaining(double seconds)
        {
            if (seconds < 1)
                return "Less than a second";
            if (seconds < 60)
                return $"{(int)seconds} second(s)";
            if (seconds < 3600)
            {
                int minutes = (int)(seconds / 60);
                int secs = (int)(seconds % 60);
                return $"{minutes}m {secs}s";
            }

            int hours = (int)(seconds / 3600);
            int mins = (int)((seconds % 3600) / 60);
            return $"{hours}h {mins}m";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
