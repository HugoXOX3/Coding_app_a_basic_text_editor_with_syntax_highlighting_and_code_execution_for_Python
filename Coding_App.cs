using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Runtime;

namespace CodingApp
{
    public partial class MainForm : Form
    {
        private string currentFile = null;
        private Process pythonProcess = null;

        public MainForm()
        {
            InitializeComponent();
            LoadSyntaxHighlighting();
        }

        private void LoadSyntaxHighlighting()
        {
            codeTextBox.Language = FastColoredTextBoxNS.Language.Python;
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeTextBox.Clear();
            currentFile = null;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog.FileName;
                codeTextBox.Text = File.ReadAllText(currentFile);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFile = saveFileDialog.FileName;
                }
            }
            if (currentFile != null)
            {
                File.WriteAllText(currentFile, codeTextBox.Text);
            }
        }

        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pythonProcess != null && !pythonProcess.HasExited)
            {
                pythonProcess.Kill();
            }
            pythonProcess = new Process();
            pythonProcess.StartInfo.FileName = "python";
            pythonProcess.StartInfo.Arguments = currentFile;
            pythonProcess.StartInfo.UseShellExecute = false;
            pythonProcess.StartInfo.RedirectStandardOutput = true;
            pythonProcess.StartInfo.RedirectStandardError = true;
            pythonProcess.StartInfo.CreateNoWindow = true;
            pythonProcess.OutputDataReceived += PythonProcess_OutputDataReceived;
            pythonProcess.ErrorDataReceived += PythonProcess_ErrorDataReceived;
            pythonProcess.Start();
            pythonProcess.BeginOutputReadLine();
            pythonProcess.BeginErrorReadLine();
        }

        private void PythonProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
            }
        }

        private void PythonProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
            }
        }
    }
}