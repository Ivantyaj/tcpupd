using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            const int port = 5555;
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }

        }

    }

    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                byte[] received = new byte[256];

                stream = client.GetStream();
                stream.Read(received, 0, received.Length);
                String s1 = Encoding.ASCII.GetString(received);
                string[] cmd = s1.Split('|');

                Zoo zoo = new Zoo();

                switch (cmd[0])
                {
                    case "view":
                        {

                            zoo.readFromFile();

                            byte[] toSent = zoo.getData().SelectMany(s => Encoding.ASCII.GetBytes(s)).ToArray();

                            stream.Write(toSent, 0, toSent.Length);
                        }
                        break;
                    case "find":
                        {
                            zoo.readFromFile();

                            byte[] toSent = Encoding.ASCII.GetBytes(zoo.findByName(cmd[1]));
                            stream.Write(toSent, 0, toSent.Length);

                        }
                        break;
                    case "add":
                        {

                            string name = cmd[1];
                            int count = Int32.Parse(cmd[2]);
                            zoo.readFromFile();
                            zoo.addAnimal(new Animal(name, count));
                            zoo.writeToFile("dataXml.xml");
                            byte[] toSent = Encoding.ASCII.GetBytes("Add success");
                            stream.Write(toSent, 0, toSent.Length);
                        }
                        break;
                    case "del":
                        {
                            zoo.readFromFile();
                            int resultOfDeleting = zoo.deleteAnimal(cmd[1]);
                            if (resultOfDeleting < 0)
                            {
                                byte[] toSent = Encoding.ASCII.GetBytes("Not found!\nNothing to delete");
                                stream.Write(toSent, 0, toSent.Length);
                            }
                            else
                            {
                                zoo.writeToFile("dataXml.xml");

                                byte[] toSent = Encoding.ASCII.GetBytes("Delete succesfully");
                                stream.Write(toSent, 0, toSent.Length);
                            }
                        }
                        break;
                    case "file":
                        {
                            IPAddress remoteIPAddress = IPAddress.Parse("127.0.0.1");
                            UdpClient senderUdp = new UdpClient();
                            IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, 5555);

                            FileStream fs = new FileStream("dataXml.xml", FileMode.Open, FileAccess.Read);

                            Byte[] bytes = new Byte[fs.Length];
                            fs.Read(bytes, 0, bytes.Length);

                            try
                            {
                                senderUdp.Send(bytes, bytes.Length, endPoint);
                            }
                            catch (Exception eR)
                            {
                                Console.WriteLine(eR.ToString());
                            }
                            finally
                            {
                                fs.Close();
                                senderUdp.Close();
                            }
                        }
                        break;
                      
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
