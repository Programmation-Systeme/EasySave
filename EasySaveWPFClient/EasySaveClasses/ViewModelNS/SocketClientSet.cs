using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EasySaveClasses.ViewModelNS
{
    public static class SocketClientSet
    {
        public static event EventHandler<string> DataReceived;
        public static string ourDataList = "default";
        public static ManualResetEvent dataUpdatedEvent = new ManualResetEvent(false);


        public static void LaunchClient(string choice)
        {
            Console.WriteLine("Client is running...");

            using (TcpClient client = new TcpClient("127.0.0.1", 8888))
            {
                Console.WriteLine("Connected to server.");

                var receiveThread = new Thread(() => ReceiveOptions(client));
                receiveThread.Start();

                while (true)
                {
                    Console.WriteLine("Select an option (type 'exit' to quit): ");
                    string selectedOption = "populate";

                    if (selectedOption.ToLower() == "exit")
                    {
                        break;
                    }

                    SendOption(client, selectedOption);
                }
            }
        }

        private static object verrou = new object();

        private static void ReceiveOptions(TcpClient client)
        {
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string received = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    string outputs = JsonConvert.DeserializeObject<string>(received);
                    lock (verrou)
                    {
                        OnDataReceived(outputs);
                        dataUpdatedEvent.Set();
                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Disconnected from server." + e);
            }
        }

        private static void SendOption(TcpClient client, string option)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(option);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to send option to server.");
            }
        }

        public static void OnDataReceived(string outputs)
        {
            ourDataList = outputs;

            if (outputs != null)
            {
                DataReceived?.Invoke(null, outputs);
            }
        }
    }
}
