using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AsciiArtLibrary;

namespace AsciiArt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskBarIcon taskBarIcon = TaskBarIcon.From_File(@"./ascii.ico");
        Bitmap selectedImage = null;
        Task task = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            SelectFileDialog sfd = new();
            sfd.AddExtensionFilter("JPG");
            sfd.AddExtensionFilter("PNG");
            sfd.AddExtensionFilter("JPEG");
            sfd.AddExtensionFilter("GIF");
            if (sfd.Show() != true)
            {
                return;
            }
            SelectedImageName.Text = Path.GetFileName(sfd.fileDialog.FileName);

            selectedImage = new(sfd.fileDialog.FileName);

            ImageWidth.Content = "寬度 : " + ((int)selectedImage.Width).ToString();
            ImageHeight.Content = "高度 : " + ((int)selectedImage.Height).ToString();

            ImageBrush imageBrush = BitmapProcessor.GetImageBrush(selectedImage);
            SelectedImage.Background = imageBrush;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (AsciiOutput.Text != "")
            {
                var dir = Directory.CreateDirectory("AsciiOutputFiles");
                string fileName = Path.GetFileNameWithoutExtension(SelectedImageName.Text);
                using var file = File.Open($@"{dir.FullName}{Path.DirectorySeparatorChar}{fileName}.txt", FileMode.OpenOrCreate, FileAccess.Write);
                using var sw = new StreamWriter(file);
                sw.Write(AsciiOutput.Text, Encoding.UTF8);
                MessageBox.Show($"已將輸出結果存至{file.Name}", "轉換成功", MessageBoxButton.OK);
            }
        }

        private void Main_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Minimized:
                    this.Visibility = Visibility.Hidden;
                    this.taskBarIcon.notifyIcon.Visible = true;
                    this.taskBarIcon.notifyIcon.DoubleClick += _NotifyIcon_DoubleClick;
                    break;
                default:
                    break;
            }
        }

        private void _NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.taskBarIcon.notifyIcon.DoubleClick -= _NotifyIcon_DoubleClick;
            this.taskBarIcon.notifyIcon.Visible = false;
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (byte.TryParse(AsciiWidth.Text, out byte Width) != true)
            {
                MessageBox.Show($"{AsciiWidth.Text}不是個有效的數字(1 ~ 255)", "錯誤", MessageBoxButton.OK);
                return;
            }
            if (task != null)
            {
                MessageBox.Show($"轉換中...");
                return;
            }
            using Bitmap bm = BitmapProcessor.NewSizeBitmap(selectedImage, Width, (int)((double)Width / selectedImage.Width * selectedImage.Height));
            AsciiOutput.Text = "";

            var pixels = BitmapProcessor.GetPixels(bm);

            task = Task.Run(() =>
           {
               string s = "";
               pixels.ForEach((row) =>
               {
                   row.ForEach((pixel) => s += $"{AsciiChars.GetAsciiChar(pixel)}");
                   s += Environment.NewLine;
               });
               this.Dispatcher.Invoke(() => AsciiOutput.Text += s);
               task = null;
           });

            await task;

            AsciiHeight.Text = bm.Height.ToString();
        }
    }
}
