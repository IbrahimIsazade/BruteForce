using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BruteForce
{
    public class LogViewerForm : Form
    {
        private RichTextBox rtbLogs;
        private Button btnRefresh;
        private Button btnClear;
        private readonly string _logFilePath;

        public LogViewerForm(string logFilePath)
        {
            _logFilePath = logFilePath;
            InitializeUI();
            LoadAndFormatLogs();
        }

        private void InitializeUI()
        {
            this.Text = "Performance Logs Viewer";
            this.Size = new Size(650, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            // Upgraded to RichTextBox for color formatting
            rtbLogs = new RichTextBox
            {
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Location = new Point(15, 15),
                Size = new Size(600, 380),
                Font = new Font("Consolas", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(30, 30, 30), // Dark mode background
                ForeColor = Color.LightGray
            };

            btnRefresh = new Button
            {
                Text = "Refresh Logs",
                Location = new Point(15, 410),
                Size = new Size(120, 35),
                BackColor = Color.LightGray
            };
            btnRefresh.Click += (s, e) => LoadAndFormatLogs();

            btnClear = new Button
            {
                Text = "Clear Logs",
                Location = new Point(145, 410),
                Size = new Size(120, 35),
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold)
            };
            btnClear.Click += BtnClear_Click;

            this.Controls.Add(rtbLogs);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(btnClear);
        }

        private void LoadAndFormatLogs()
        {
            rtbLogs.Clear();

            if (!File.Exists(_logFilePath) || new FileInfo(_logFilePath).Length == 0)
            {
                AppendText("No logs found yet. Run an attack to generate data!", Color.Gray, false);
                return;
            }

            string[] logLines = File.ReadAllLines(_logFilePath);

            foreach (string line in logLines)
            {
                // Apply dynamic color coding based on the content of the line
                if (line.Contains("--- PERFORMANCE SUMMARY ---") || line.Contains("---------------------------"))
                {
                    AppendText(line, Color.Yellow, true);
                }
                else if (line.StartsWith("Result"))
                {
                    AppendText(line, Color.LightGreen, true);
                }
                else if (line.Contains("Single-Threaded Time") || line.Contains("Multi-Threaded Time"))
                {
                    AppendText(line, Color.White, false);
                }
                else if (line.Contains("Multi-Thread"))
                {
                    // Multi-thread runs are colored Cyan
                    AppendText(line, Color.Cyan, false);
                }
                else if (line.Contains("Single-Thread"))
                {
                    // Single-thread runs are colored Orange
                    AppendText(line, Color.Orange, false);
                }
                else
                {
                    // Default fallback
                    AppendText(line, Color.LightGray, false);
                }
            }

            // Scroll to bottom
            rtbLogs.SelectionStart = rtbLogs.Text.Length;
            rtbLogs.ScrollToCaret();
        }

        // Helper method to append text with specific colors and font styles
        private void AppendText(string text, Color color, bool isBold)
        {
            rtbLogs.SelectionStart = rtbLogs.TextLength;
            rtbLogs.SelectionLength = 0;

            rtbLogs.SelectionColor = color;
            rtbLogs.SelectionFont = new Font(rtbLogs.Font, isBold ? FontStyle.Bold : FontStyle.Regular);

            rtbLogs.AppendText(text + Environment.NewLine);

            rtbLogs.SelectionColor = rtbLogs.ForeColor; // Reset color
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before deleting
            var result = MessageBox.Show("Are you sure you want to permanently delete all logs?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Overwrite the file with an empty string
                File.WriteAllText(_logFilePath, string.Empty);
                LoadAndFormatLogs(); // Refresh the UI
            }
        }
    }
}