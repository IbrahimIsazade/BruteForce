using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BruteForce
{
    public class LogViewerForm : Form
    {
        private TextBox txtLogs;
        private Button btnRefresh;
        private readonly string _logFilePath;

        public LogViewerForm(string logFilePath)
        {
            _logFilePath = logFilePath;
            InitializeUI();
            LoadLogs();
        }

        private void InitializeUI()
        {
            this.Text = "Performance Logs Viewer";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            // Using Consolas (a monospace font) ensures your formatted columns line up perfectly
            txtLogs = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(15, 15),
                Size = new Size(550, 340),
                Font = new Font("Consolas", 10, FontStyle.Regular),
                BackColor = Color.White
            };

            btnRefresh = new Button
            {
                Text = "Refresh Logs",
                Location = new Point(15, 365),
                Size = new Size(120, 30)
            };
            btnRefresh.Click += (s, e) => LoadLogs();

            this.Controls.Add(txtLogs);
            this.Controls.Add(btnRefresh);
        }

        private void LoadLogs()
        {
            if (File.Exists(_logFilePath))
            {
                txtLogs.Text = File.ReadAllText(_logFilePath);

                // Automatically scroll to the very bottom to show the newest entries
                txtLogs.SelectionStart = txtLogs.Text.Length;
                txtLogs.ScrollToCaret();
            }
            else
            {
                txtLogs.Text = "No logs found yet. Run an attack to generate data!";
            }
        }
    }
}