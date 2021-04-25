using System.Drawing;
using System.Windows.Forms;
using System;
using System.IO;

namespace AsciiArtLibrary
{
    public class TaskBarIcon
    {
        public NotifyIcon notifyIcon { get; } = new NotifyIcon();
        public static TaskBarIcon From_File(string filePath)
        {
            TaskBarIcon taskBarIcon = new();
            try
            {
                taskBarIcon.notifyIcon.Icon = new Icon($@"{filePath}");
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show($@"{e.Message}", "錯誤", MessageBoxButtons.OK);
            }
            return taskBarIcon;
        }

    }
}
