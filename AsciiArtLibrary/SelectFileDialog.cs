using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsciiArtLibrary
{
    public class SelectFileDialog
    {
        private HashSet<string> fileExtensions = new();
        public OpenFileDialog fileDialog { get; } = new OpenFileDialog();

        public void AddExtensionFilter(string fileExtension)
        {
            fileExtensions.Add(fileExtension.ToLower());
        }

        public void RemoveExtensionFilter(string fileExtension)
        {
            fileExtensions.Remove(fileExtension.ToLower());
        }

        public bool Show()
        {
            switch (fileExtensions.Count)
            {
                case 0:
                    fileDialog.Filter = "All Files (*.*)|*.*";
                    break;
                case >= 1:
                    string extension = fileExtensions.ElementAt(0);
                    fileDialog.Filter = $"{extension.ToUpper()} Files (*.{extension})|*.{extension}";
                    for (int i = 1; i < fileExtensions.Count; i++)
                    {
                        extension = fileExtensions.ElementAt(i);
                        fileDialog.Filter += $"|{extension.ToUpper()} Files (*.{extension})|*.{extension}";
                    }
                    break;
            }
            return fileDialog.ShowDialog() == DialogResult.OK;
        }
    }
}
