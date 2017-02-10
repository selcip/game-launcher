using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IDMStory.Utils
{
    class News
    {
        public static void get()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("http://144.217.128.81/ala.xml");
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = doc.SelectNodes("/LINKS/noticia");
            int i = 0;
            foreach (XmlNode node in nodes)
            {
                set(node["titulo"].InnerText.Trim(), node["url"].InnerText.Trim(), i);
                i++;
            }
        }

        public static void set(string titulo, string url, int pos)
        {
            switch (pos)
            {
                case 0:
                    MainWindow.main.Noticia1 = titulo;
                    MainWindow.main.Link1 = url;
                    break;
                case 1:
                    MainWindow.main.Noticia2 = titulo;
                    MainWindow.main.Link2 = url;
                    break;
                case 2:
                    MainWindow.main.Noticia3 = titulo;
                    MainWindow.main.Link3 = url;
                    break;
            }
        }
    }
}
