using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using IDMStory.Utils;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using System.IO;
using System.Media;

namespace IDMStory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            main = this;
            InitializeComponent();
            
            var fetcher = new NoArgDelegate(
                News.get);

            fetcher.BeginInvoke(null, null);

            var fetcher2 = new NoArgDelegate(
                Check.getHashAll);

            fetcher2.BeginInvoke(null, null);

        }

        internal static MainWindow main;
        
        internal string Status
        {
            get { return status_label.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { status_label.Content = value; })); }
        }

        internal string Noticia1
        {
            set { Dispatcher.Invoke(new Action(() => { titulo1.Text = value; })); }
        }

        internal string Noticia2
        {
            set { Dispatcher.Invoke(new Action(() => { titulo2.Text = value; })); }
        }

        internal string Noticia3
        {
            set { Dispatcher.Invoke(new Action(() => { titulo3.Text = value; })); }
        }

        internal string progress
        {
            set { Dispatcher.Invoke(new Action(() => { fileP.Content = value; })); }
        }

        internal string Link1
        {
            set { Dispatcher.Invoke(new Action(() => { noticia1.NavigateUri = new Uri(value); })); }
        }

        internal string Link2
        {
            set { Dispatcher.Invoke(new Action(() => { noticia2.NavigateUri = new Uri(value); })); }
        }

        internal string Link3
        {
            set { Dispatcher.Invoke(new Action(() => { noticia3.NavigateUri = new Uri(value); })); }
        }

        internal string fileDownload
        {
            set { Dispatcher.Invoke(new Action(() => { fileD.Value = int.Parse(value); fileL.Content = value.ToString() + "%"; })); }
        }

        internal string fullDownload
        {
            set { Dispatcher.Invoke(new Action(() => { fullD.Value = int.Parse(value); fullL.Content = value + "%"; })); }
        }       

        internal bool playEnable
        {
            set { Dispatcher.Invoke(new Action(() => { playButton.IsEnabled = value; })); }
        }

        internal string online
        {
            set { Dispatcher.Invoke(new Action(() => { socorro.Content = "Online"; socorro.Foreground = new SolidColorBrush(Colors.Green); })); }
        }

        internal string fechar
        {
            set { Dispatcher.Invoke(new Action(() => { Close(); })); }
        }

        internal string messagebox
        {
            set { Dispatcher.Invoke(new Action(() => { MessageBox.Show(string.Format(value)); })); }
        }

        internal bool play = false;
        internal bool isPlay
        {
            set { Dispatcher.Invoke(new Action(() => { play = value; })); }
            get { return play; }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private delegate void NoArgDelegate();

        private delegate void OneArgDelegate(string arg);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.isPlay = true;
            playEnable = false;

            var fetcher2 = new NoArgDelegate(
                Check.getHashAll);

            fetcher2.BeginInvoke(null, null);            
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://idmstory.wtf"));
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://forum.idmstory.wtf"));
        }
    }
}
