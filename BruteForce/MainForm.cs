using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace BruteForce
{
    public partial class MainForm : Form
    {
        // --- UI Controls ---
        private Label lblInputPrompt;
        private TextBox txtCustomPassword;
        private Button btnGenerateRandom;
        private CheckBox chkMultiThread;

        private Button btnStart;
        private Button btnStop;

        private Label lblTargetHash;
        private Label lblCurrentGuess;
        private Label lblAttempts;
        private Label lblTimeElapsed;
        private Label lblResult;

        // --- Backend Classes ---
        private readonly PasswordHandler _passwordHandler;
        private readonly BruteForceGenerator _generator;
        private readonly TaskOrchestrator _orchestrator;
        private readonly PerformanceLogger _logger;

        private string _targetHash;
        private TimeSpan _singleThreadTime = TimeSpan.Zero;
        private TimeSpan _multiThreadTime = TimeSpan.Zero;

        public MainForm()
        {
            // 1. Initialize backend classes FIRST so they are ready
            _passwordHandler = new PasswordHandler();
            _generator = new BruteForceGenerator();
            _orchestrator = new TaskOrchestrator(_passwordHandler, _generator);
            _logger = new PerformanceLogger();

            // 2. Build the improved UI Layout
            InitializeCodeFirstUI();
        }

        /// <summary>
        /// Manually builds and positions a clean, top-to-bottom UI layout.
        /// </summary>
        private void InitializeCodeFirstUI()
        {
            this.Text = "Multi-Threaded Brute Force Attacker";
            this.Size = new Size(420, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // --- Top Section: Inputs ---
            lblInputPrompt = new Label { Text = "Target Password:", Location = new Point(20, 23), Size = new Size(100, 20) };
            txtCustomPassword = new TextBox { Location = new Point(120, 20), Size = new Size(150, 25) };
            btnGenerateRandom = new Button { Text = "Random", Location = new Point(280, 19), Size = new Size(100, 25) };

            chkMultiThread = new CheckBox { Text = "Enable Multi-Threading (Uses all CPU Cores)", Location = new Point(120, 50), Size = new Size(260, 20), Checked = true };

            // --- Middle Section: Actions ---
            btnStart = new Button
            {
                Text = "START ATTACK",
                Location = new Point(20, 90),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(0, 95, 184),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStop = new Button { Text = "EMERGENCY STOP", Location = new Point(210, 90), Size = new Size(170, 45), BackColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold), Enabled = false };

            // --- Bottom Section: Results & Progress ---
            lblTargetHash = new Label { Text = "Target Hash: ---", Location = new Point(20, 160), Size = new Size(360, 20) };
            lblCurrentGuess = new Label { Text = "Current Guess: ---", Location = new Point(20, 190), Size = new Size(360, 20) };
            lblAttempts = new Label { Text = "Attempts: 0", Location = new Point(20, 220), Size = new Size(360, 20) };
            lblTimeElapsed = new Label { Text = "Time Elapsed: 0.00s", Location = new Point(20, 250), Size = new Size(360, 20) };

            lblResult = new Label { Text = "Status: Waiting for input...", Location = new Point(20, 300), Size = new Size(360, 60), Font = new Font("Arial", 11, FontStyle.Bold), ForeColor = Color.DarkBlue };

            // Hook up click events
            btnGenerateRandom.Click += btnGenerateRandom_Click;
            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;

            // Add controls to the Form
            this.Controls.Add(lblInputPrompt);
            this.Controls.Add(txtCustomPassword);
            this.Controls.Add(btnGenerateRandom);
            this.Controls.Add(chkMultiThread);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);
            this.Controls.Add(lblTargetHash);
            this.Controls.Add(lblCurrentGuess);
            this.Controls.Add(lblAttempts);
            this.Controls.Add(lblTimeElapsed);
            this.Controls.Add(lblResult);
        }

        // --- Event Handlers & Logic ---

        private void btnGenerateRandom_Click(object sender, EventArgs e)
        {
            // Use the backend to generate a random 4-5 char string and put it in the textbox
            txtCustomPassword.Text = _passwordHandler.GenerateTargetPassword();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string customPassword = txtCustomPassword.Text;

            if (string.IsNullOrWhiteSpace(customPassword))
            {
                MessageBox.Show("Please enter a target password or click 'Random'!", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hash the custom input
            _targetHash = _passwordHandler.ComputeHash(customPassword);

            // Display truncated hash for visual confirmation
            string displayHash = _targetHash.Length > 20 ? _targetHash.Substring(0, 20) + "..." : _targetHash;
            lblTargetHash.Text = $"Target Hash: {displayHash}";

            // Determine length dynamically based on user input
            int maxLength = customPassword.Length;
            bool isMultiThreaded = chkMultiThread.Checked;

            await RunAttack(maxLength, isMultiThreaded);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _orchestrator.StopAttack();
            lblResult.Text = "Status: Attack Aborted by User!";
            lblResult.ForeColor = Color.Red;
            ToggleUI(true);
        }

        private async Task RunAttack(int maxLength, bool isMultiThreaded)
        {
            ToggleUI(false); // Disable inputs while running
            lblResult.ForeColor = Color.DarkBlue;
            lblResult.Text = isMultiThreaded ? "Status: Attacking (Multi-Threaded)..." : "Status: Attacking (Single-Threaded)...";

            // Setup real-time UI updates
            var progress = new Progress<BruteForceProgress>(p =>
            {
                lblAttempts.Text = $"Attempts: {p.AttemptsMade:N0}";
                lblCurrentGuess.Text = $"Current Guess: {p.CurrentGuess}";
                lblTimeElapsed.Text = $"Time Elapsed: {p.Elapsed.TotalSeconds:F2}s";
            });

            // Start the Orchestrator
            string foundPassword = await _orchestrator.StartAttackAsync(_targetHash, maxLength, isMultiThreaded, progress);

            // Logging logic
            string attackType = isMultiThreaded ? "Multi-Thread" : "Single-Thread";

            // Safely parse the final results from the UI labels to log them
            string timeStr = lblTimeElapsed.Text.Replace("Time Elapsed: ", "").Replace("s", "");
            double parsedSeconds = double.TryParse(timeStr, out double s) ? s : 0;
            TimeSpan finalTime = TimeSpan.FromSeconds(parsedSeconds);

            string attemptsStr = lblAttempts.Text.Replace("Attempts: ", "").Replace(",", "");
            long finalAttempts = long.TryParse(attemptsStr, out long a) ? a : 0;

            _logger.LogRun(attackType, foundPassword, finalTime, finalAttempts);

            // Track times for A/B comparison
            if (isMultiThreaded) _multiThreadTime = finalTime;
            else _singleThreadTime = finalTime;

            if (_singleThreadTime != TimeSpan.Zero && _multiThreadTime != TimeSpan.Zero)
            {
                _logger.LogComparison(_singleThreadTime, _multiThreadTime);
                MessageBox.Show("Both Single and Multi-Threaded tests complete!\nCheck performance_log.txt for comparison.", "Comparison Logged");

                // Reset for the next batch of tests
                _singleThreadTime = TimeSpan.Zero;
                _multiThreadTime = TimeSpan.Zero;
            }

            // Update UI with final result
            if (foundPassword != null)
            {
                lblResult.Text = $"SUCCESS: Password is '{foundPassword}'!";
                lblResult.ForeColor = Color.Blue;
            }
            else
            {
                lblResult.Text = "Status: Password Not Found.";
                lblResult.ForeColor = Color.Black;
            }

            ToggleUI(true); // Re-enable inputs
        }

        private void ToggleUI(bool isIdle)
        {
            // If idle, inputs are enabled. If running, inputs are disabled.
            txtCustomPassword.Enabled = isIdle;
            btnGenerateRandom.Enabled = isIdle;
            chkMultiThread.Enabled = isIdle;
            btnStart.Enabled = isIdle;

            // Stop button is ONLY enabled when an attack is actively running
            btnStop.Enabled = !isIdle;
        }
    }
}