using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ClayBot
{
    partial class MainForm : Form
    {
        public Config Config
        {
            get { return config; }
        }

        private Config config;
        private ComboBox lolRegionComboBox;
        private ComboBox lolLanguageComboBox;
        private TextBox lolLauncherPathTextBox;
        private Button lolLauncherPathButton;
        private TextBox lolIdTextBox;
        private TextBox lolPasswordTextBox;
        private Button okButton;

        private void InitializeConfig()
        {
            config = Config.ReadConfig();
            config.Initialize();

            while (!ValidateConfig())
            {
                ShowConfigForm();
            }
        }

        private bool ValidateConfig()
        {
            bool result = true;
            
            if (string.IsNullOrEmpty(config.LolLauncherPath) || !File.Exists(config.LolLauncherPath))
            {
                MessageBox.Show(Strings.Strings.LolLauncherPathNotFound);
                result = false;
            }
            
            if (string.IsNullOrEmpty(config.LolLocale))
            {
                MessageBox.Show(Strings.Strings.LolLocaleNotFound);
                result = false;
            }

            if (string.IsNullOrEmpty(config.LolId))
            {
                MessageBox.Show(Strings.Strings.LolIdNull);
                result = false;
            }

            if (string.IsNullOrEmpty(config.LolPassword))
            {
                MessageBox.Show(Strings.Strings.LolPasswordNull);
                result = false;
            }

            return result;
        }

        private void OnLolRegionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            lolLanguageComboBox.Items.Clear();

            foreach (Static.Locale locale in Static.SUPPORTED_LOCALES)
            {
                if ((lolRegionComboBox.Items[lolRegionComboBox.SelectedIndex] as string) == locale.Region)
                {
                    lolLanguageComboBox.Items.Add(locale.Language);
                }
            }
        }

        private void OnLolLauncherPathButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "lol.launcher.admin.exe|lol.launcher.admin.exe"
            };
            
            if (openFileDialog.ShowDialog() == DialogResult.OK && lolLauncherPathTextBox != null)
            {
                lolLauncherPathTextBox.Text = openFileDialog.FileName;
            }
        }
        
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
                    Text = Strings.Strings.LolRegionLabel
                },
                lolRegionComboBox = new ComboBox()
                {
                    Width = configForm.Width - 10,
                    DropDownStyle = ComboBoxStyle.DropDownList
                },
                new Label()
                {
                    AutoSize = true,
                    Text = Strings.Strings.LolLanguageLabel
                },
                lolLanguageComboBox = new ComboBox()
                {
                    Width = configForm.Width - 10,
                    DropDownStyle = ComboBoxStyle.DropDownList
                },
                new Label()
                {
                    AutoSize = true,
                    Text = Strings.Strings.LolLauncherPathLabel
                },
                lolLauncherPathTextBox = new TextBox()
                {
                    Width = configForm.Width - 10,
                    Text = config.LolLauncherPath,
                    ReadOnly = true
                },
                lolLauncherPathButton = new Button()
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
                    Text = config.LolId
                },
                new Label()
                {
                    AutoSize = true,
                    Text = Strings.Strings.LolPasswordLabel
                },
                lolPasswordTextBox = new TextBox()
                {
                    Width = configForm.Width - 10,
                    Text = config.LolPassword,
                    UseSystemPasswordChar = true
                },
                okButton = new Button()
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK
                });
            
            foreach (Static.Locale locale in Static.SUPPORTED_LOCALES)
            {
                if (!lolRegionComboBox.Items.Contains(locale.Region)) lolRegionComboBox.Items.Add(locale.Region);
            }

            lolRegionComboBox.SelectedIndexChanged += OnLolRegionComboBoxSelectedIndexChanged;

            for (int i = 0; i < lolRegionComboBox.Items.Count; i++)
            {
                if ((lolRegionComboBox.Items[i] as string) == Config.LolRegion) { lolRegionComboBox.SelectedIndex = i; }
            }

            for (int i = 0; i < lolLanguageComboBox.Items.Count; i++)
            {
                if ((lolLanguageComboBox.Items[i] as string) == Config.LolLanguage) { lolLanguageComboBox.SelectedIndex = i; }
            }

            lolLauncherPathButton.Click += OnLolLauncherPathButtonClick;
            configForm.AcceptButton = okButton;

            if (configForm.ShowDialog(this) == DialogResult.OK)
            {
                if (lolRegionComboBox.SelectedIndex != -1 && lolLanguageComboBox.SelectedIndex != -1)
                {
                    Config.LolRegion = lolRegionComboBox.Items[lolRegionComboBox.SelectedIndex] as string;
                    Config.LolLanguage = lolLanguageComboBox.Items[lolLanguageComboBox.SelectedIndex] as string;

                    foreach (Static.Locale locale in Static.SUPPORTED_LOCALES)
                    {
                        if (Config.LolRegion == locale.Region &&
                            Config.LolLanguage == locale.Language)
                        {
                            Config.LolLocale = locale.LocaleCode;
                        }
                    }
                }

                config.LolLauncherPath = lolLauncherPathTextBox.Text;
                config.LolId = lolIdTextBox.Text;
                config.LolPassword = lolPasswordTextBox.Text;

                config.Initialize();

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Config.LolLocale);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Config.LolLocale);

                Config.WriteConfig(config);
            }

            components.Remove(configForm);
        }
    }
}
