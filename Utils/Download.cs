using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IDMStory.Utils
{
    class Download
    {
        private static Queue<string> _downloadUrls = new Queue<string>();
        private static int total = 0;
        private static int current = 0;

        public static void Start(List<string> baixar)
        {
            total = baixar.Count;
            MainWindow.main.fullDownload = "0";
            foreach (var arquivo in baixar)
            {
                _downloadUrls.Enqueue("http://144.217.128.81/cdn/" + arquivo);
            }
            DownloadFile();
        }

        private static void DownloadFile()
        {
            if (_downloadUrls.Any())
            {
                WebClient wc = new WebClient();
                wc.DownloadProgressChanged += client_DownloadProgressChanged;
                wc.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = _downloadUrls.Dequeue();
                string arquivo = url.Substring(url.LastIndexOf("/") + 1, (url.Length - url.LastIndexOf("/") - 1));

                wc.DownloadFileAsync(new Uri(url), arquivo);
                MainWindow.main.Status = "Baixando: " + arquivo + "  " + (current+1) + " de " + (total);
                MainWindow.main.fileDownload = "0";
                return;
            }
            Check.enableStart();
        }

        private static void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MainWindow.main.messagebox = "Algum erro aconteceu durante o download. Baixe os arquivos pelo site.";
            }
            if (e.Cancelled)
            {
                
            }
            current++;
            double percentage = (double.Parse(current.ToString()) / double.Parse(total.ToString())) * 100;
            Console.WriteLine(percentage);
            MainWindow.main.fullDownload = Math.Truncate(percentage).ToString();
            DownloadFile();
        }

        private static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            MainWindow.main.fileDownload = Math.Truncate(percentage).ToString();
        }
    }
}
