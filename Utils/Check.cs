using IDMStory.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace IDMStory.Utils
{
    class Check
    {       
        private static bool ServerStatus()
        {
            TcpClient tcpClient = new TcpClient();
            bool status = false;
            try
            {
                tcpClient.Connect(Program.IP, 8484);
                status = true;                
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        private static bool VPSStatus()
        {
            TcpClient tcpClient = new TcpClient();
            bool status = false;
            try
            {
                tcpClient.Connect(Program.IP, 80);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public static void getHashAll()
        {
            MainWindow.main.Status = "Verificando arquivos";

            if (ServerStatus())
            {
                MainWindow.main.online = "Online";
            }

            Dictionary<string, string> files = new Dictionary<string, string>();
            String dir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            foreach (string path in Directory.GetFiles(dir))
            {
                string extension = System.IO.Path.GetExtension(path);
                string name = System.IO.Path.GetFileName(path);                
                if (extension.Contains("wz") || extension.Contains("dll") || name == "main.exe" || name == "version")
                {
                    files.Add(name, GetMD5HashFromFile(path));
                }
            }            

            if (VPSStatus())
            {
                compareFiles(files);
            }
            else
            {
                MainWindow.main.Status = "O servidor de downloads está desligado ou com algum problema.";
            }
        }

        public static void playGame()
        {
            String dir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = dir + "\\main.exe";
            Console.WriteLine(path);
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Não foi possível encontrar o MapleStory.exe"));
                return;
            }

            Process jogo = new Process();
            jogo.StartInfo.FileName = path;
            jogo.StartInfo.Arguments = Program.IP + " 8484";
            try
            {
                if (jogo.Start())
                {
                    MainWindow.main.fechar = "";
                }
            }

            catch (Exception ex)
            {
                enableStart();
                MessageBox.Show("Execute como administrador!");
            }
        }

        public static void compareFiles(Dictionary<string, string> files)
        {
            string[] arquivos = new string[] { "Base.wz", "Character.wz", "Effect.wz", "Etc.wz", "Item.wz", "List.wz", "Map.wz", "Mob.wz", "Morph.wz", "Npc.wz", "Quest.wz", "Reactor.wz", "Skill.wz", "Sound.wz", "String.wz", "TamingMob.wz", "UI.wz",
  "Canvas.dll", "bz32ex.dll", "aossdk.dll", "dinut8.dll", "Gr2D_DX8.dll", "ijl15.dll", "NameSpace.dll", "PCOM.dll", "ResMan.dll", "Shape2D.dll", "Sound_DX8.dll", "WzFlashRenderer.dll", "ZLZ.dll", "v3hunt.dll", "suipre.dll", "main.exe", "version"};
            List<string> baixar = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load("http://144.217.128.81/hashes.xml");
            
            foreach (var arquivo in arquivos)
            {
                bool tem = files.ContainsKey(arquivo);
                Console.Write(arquivo + ": ");
                Console.WriteLine(doc.DocumentElement.SelectSingleNode("/hashes/" + arquivo).InnerText);
                if (!tem || (tem && files[arquivo].ToLower() != doc.DocumentElement.SelectSingleNode("/hashes/" + arquivo).InnerText))
                {
                    if(arquivo == "version" && !baixar.Contains("main.exe")) baixar.Add("main.exe");
                    baixar.Add(arquivo);                    
                }                                       
            }

            /*if ((doc.DocumentElement.SelectSingleNode("/hashes/version").InnerText != files["version"]) || !files.ContainsKey("version"))
            {
                baixar.Add("main.exe");
                baixar.Add("version");
            }*/

            if (MainWindow.main.isPlay && !baixar.Any())
            {
                playGame();
                return;
            }

            if (MainWindow.main.isPlay && baixar.Any())
            {
                Stream str = Properties.Resources.deuruim;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
                Thread.Sleep(2000);
                MainWindow.main.messagebox = "Algum arquivo foi alterado! Seu launcher irá fechar ;)";
                MainWindow.main.fechar = "fechou";
            }

            if (baixar.Count == 0)
            {
                enableStart();
            }
            else
            {
                Download.Start(baixar);
            }
        }

        public static void enableStart()
        {
            SystemSounds.Beep.Play();

            if (ServerStatus())
            {
                MainWindow.main.playEnable = true;
                MainWindow.main.fileDownload = "100";
                MainWindow.main.fullDownload = "100";
                MainWindow.main.Status = "Todos os arquivos estão atualizados!";
            }else
            {
                MainWindow.main.Status = "Servidor offline ou em manutenção";
            }            
        }

        private static string GetMD5HashFromFile(string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("X2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                MainWindow.main.messagebox = "O jogo já está aberto";
                MainWindow.main.fechar = "a";
                return "porra";
            }
        }
    }
}
