namespace FireWallie
{
    public partial class Form1 : Form
    {
        public string[] Files;
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if (recursiveCheckBox.Checked)
                    {
                        Files = Directory.EnumerateFiles(fbd.SelectedPath, "*.exe", SearchOption.AllDirectories).ToArray();
                    }
                    else
                    {
                        Files = Directory.EnumerateFiles(fbd.SelectedPath, "*.exe").ToArray();
                    }
                    goButton.Enabled = true;
                }
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            foreach (string file in Files)
            {
                DialogResult result = MessageBox.Show("Block Internet? (Yes/No): ", file, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if ( result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("netsh.exe", "advfirewall firewall add rule name=\"Blocked (FireWallie): "+file.Split('\\').Last()+"\" dir=\"out\" action=\"block\"");
                }
                else if (result == DialogResult.No)
                {
                    continue;

                } else if (result == DialogResult.Cancel)
                {
                    break;
                }
            }
        }
    }
}
