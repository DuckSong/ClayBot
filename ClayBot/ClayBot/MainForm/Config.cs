using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClayBot
{
    partial class MainForm : Form
    {
        public Config Config;
        
        private void InitializeConfig()
        {
            Config = Config.ReadConfig();
            Config.Initialize();

            while (!ValidateConfig())
            {
                ShowConfigForm();
            }
        }

        private bool ValidateConfig()
        {
            bool result = true;

            if (!Directory.Exists(Config.LolDirectory))
            {
                MessageBox.Show(Strings.Strings.LolDirectoryNotFound);
                result = false;
            }

            if (!File.Exists(Config.LolLauncherPath))
            {
                MessageBox.Show(Strings.Strings.LolLauncherNotFound);
                result = false;
            }

            if (string.IsNullOrEmpty(Config.LolLocale))
            {
                MessageBox.Show(Strings.Strings.LolLocaleNotFound);
                result = false;
            }
            else
            {
                if (!Static.SUPPORTED_LOCALES.Contains(Config.LolLocale))
                {
                    MessageBox.Show(string.Format(Strings.Strings.LolLocaleNotSupported, Config.LolLocale));
                    Application.Exit();
                }
            }

            if (string.IsNullOrEmpty(Config.LolId))
            {
                MessageBox.Show(Strings.Strings.LolIdNull);
                result = false;
            }

            if (string.IsNullOrEmpty(Config.LolPassword))
            {
                MessageBox.Show(Strings.Strings.LolPasswordNull);
                result = false;
            }

            return result;
        }

        private void OnLolDirectoryButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK && lolDirectoryTextBox != null)
            {
                lolDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private TextBox lolDirectoryTextBox;
        private Button lolDirectoryButton;
        private TextBox lolIdTextBox;
        private TextBox lolPasswordTextBox;
        private Button okButton;

        private void ShowConfigForm()
        {
            Form configForm = new Form();
            components.Add(configForm);

            configForm.StartPosition = FormStartPosition.Manual;
            configForm.FormBorderStyle = FormBorderStyle.None;
            configForm.ShowInTaskbar = false;
            configForm.TopMost = true;
            configForm.AutoScroll = true;

            configForm.Width = Screen.PrimaryScreen.WorkingArea.Width / 2;
            configForm.Height = Screen.PrimaryScreen.WorkingArea.Height / 2;
            configForm.Left = Screen.PrimaryScreen.WorkingArea.Left + Screen.PrimaryScreen.WorkingArea.Width / 4;
            configForm.Top = Screen.PrimaryScreen.WorkingArea.Top + Screen.PrimaryScreen.WorkingArea.Height / 4;

            CreateForm(configForm.Controls, new Point(5, 5), 10, false,
                new Label()
                {
                    AutoSize = true,
                    Text = Strings.Strings.LolDirectoryLabel
                },
                lolDirectoryTextBox = new TextBox()
                {
                    Width = configForm.Width - 10,
                    Text = Config.LolDirectory,
                    ReadOnly = true
                },
                lolDirectoryButton = new Button()
                {
                    Text = "..."
                },
                new Label()
                {
                    AutoSize = true,
                    Text = Strings.Strings.LolIdLabel
                },
                lolIdTextBox = new TextBox()
                {
                    Width = configForm.Width - 10,
                    Text = Config.LolId
                },
                new Label()
                {
                    AutoSize = true,
                    Text = Strings.Strings.LolPasswordLabel
                },
                lolPasswordTextBox = new TextBox()
                {
                    Width = configForm.Width - 10,
                    Text = Config.LolPassword,
                    UseSystemPasswordChar = true
                },
                okButton = new Button()
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK
                });

            lolDirectoryButton.Click += OnLolDirectoryButtonClick;
            configForm.AcceptButton = okButton;

            if (configForm.ShowDialog(this) == DialogResult.OK)
            {
                Config.LolDirectory = lolDirectoryTextBox.Text;
                Config.LolId = lolIdTextBox.Text;
                Config.LolPassword = lolPasswordTextBox.Text;

                Config.Initialize();

                Config.WriteConfig(Config);
            }

            components.Remove(configForm);
        }
    }
}
