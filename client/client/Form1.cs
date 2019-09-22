using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            TcpClient client = null;
            try
            {
                client = new TcpClient("localhost", 5555);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    if (radioButton1.Checked == true) //view
                    {
                        stream = client.GetStream();

                        String command = "view";
                        String res = command + "|";

                        byte[] sent = Encoding.ASCII.GetBytes(res);
                        byte[] recieved = new byte[10240];

                        stream.Write(sent, 0, sent.Length);

                        stream.Read(recieved, 0, recieved.Length);

                        richTextBox1.Text = Encoding.ASCII.GetString(recieved);
                        String status = "=>Command sent:view";

                        listBox1.Items.Add(status);

                    }
                    else if (radioButton2.Checked == true) //add
                    {
                        stream = client.GetStream();

                        String command = "add";
                        String res = command + "|";
                        res += textBoxName.Text + "|" + textBoxCount.Text + "|";

                        byte[] sent = Encoding.ASCII.GetBytes(res);
                        byte[] recieved = new byte[256];

                        stream.Write(sent, 0, sent.Length);

                        stream.Read(recieved, 0, recieved.Length);

                        richTextBox1.Text = Encoding.ASCII.GetString(recieved);
                        String status = "=>Command sent:add";

                        listBox1.Items.Add(status);
                    }
                    else if (radioButton3.Checked == true) //del
                    {
                        stream = client.GetStream();

                        String command = "del";
                        String res = command + "|";
                        res += textBoxName.Text + "|";

                        byte[] sent = Encoding.ASCII.GetBytes(res);
                        byte[] recieved = new byte[256];

                        stream.Write(sent, 0, sent.Length);

                        stream.Read(recieved, 0, recieved.Length);

                        richTextBox1.Text = Encoding.ASCII.GetString(recieved);

                        String status = "=>Command sent:del";

                        listBox1.Items.Add(status);
                    }
                    else if (radioButton5.Checked == true) //find
                    {
                        stream = client.GetStream();

                        String command = "find";
                        String res = command + "|";
                        res += textBoxName.Text + "|";

                        byte[] sent = Encoding.ASCII.GetBytes(res);
                        byte[] recieved = new byte[256];

                        stream.Write(sent, 0, sent.Length);

                        stream.Read(recieved, 0, recieved.Length);

                        richTextBox1.Text = Encoding.ASCII.GetString(recieved);

                        String status = "=>Command sent:find";

                        listBox1.Items.Add(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }


        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                textBoxCount.Enabled = false;
            }
            else
            {
                textBoxCount.Enabled = true;
            }
        }

        private void RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                textBoxCount.Enabled = false;
            }
            else
            {
                textBoxCount.Enabled = true;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient("localhost", 5555);
                NetworkStream stream = client.GetStream();

                stream = client.GetStream();

                String command = "file";
                String res = command + "|";

                byte[] sent = Encoding.ASCII.GetBytes(res);
                byte[] recieved = new byte[10240];

                stream.Write(sent, 0, sent.Length);

                //UDP
                UdpClient receivingUdpClient = new UdpClient(5555);
                IPEndPoint RemoteIpEndPoint = null;

                FileStream fs = null;
                Byte[] receiveBytes = new Byte[0];

                try
                {
                    receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    fs = new FileStream("recieve.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fs.Write(receiveBytes, 0, receiveBytes.Length);

                    Process.Start(fs.Name);
                }
                catch (Exception eR)
                {
                    Console.WriteLine(eR.ToString());
                }
                finally
                {
                    fs.Close();
                    receivingUdpClient.Close();
                    Console.Read();
                }

                //UDP

                String status = "=>Command sent:file";

                listBox1.Items.Add(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }


        }
    }
}
