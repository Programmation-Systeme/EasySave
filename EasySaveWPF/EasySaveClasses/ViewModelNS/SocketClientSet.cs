﻿using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace EasySaveClasses.ViewModelNS
{
    public class SocketClientSet
    {
        // Define an event for when data is received
        public event EventHandler<string[]> DataReceived;

        /// <summary>
        /// Launches the client, connects to the server, sends user choice, and receives response.
        /// </summary>
        /// <param name="args">Command-line arguments (not used in this method).</param>
        /// <param name="choice">User's choice to send to the server.</param>
        public void LaunchClient(string[] args, string choice)
        {
            Console.WriteLine("Client is running...");

            // Connect to the server
            TcpClient client = new TcpClient("127.0.0.1", 8888);
            Console.WriteLine("Connected to server.");

            // Start a thread to receive options from the server
            var receiveThread = new System.Threading.Thread(() => ReceiveOptions(client));
            receiveThread.Start();

            // Select options from the list
            while (true)
            {
                Console.WriteLine("Select an option (type 'exit' to quit): ");
                string selectedOption = Console.ReadLine();

                if (selectedOption.ToLower() == "exit")
                {
                    break;
                }

                // Send the selected option to the server
                SendOption(client, selectedOption);
            }

            // Close the connection
            client.Close();
        }

        private void ReceiveOptions(TcpClient client)
        {
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string received = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    string[] outputs = JsonConvert.DeserializeObject<string[]>(received);
                    OnDataReceived(outputs); // Raise event when data is received
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Disconnected from server." + e);
            }
        }

        private void SendOption(TcpClient client, string option)
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

        // Helper method to raise the event
        protected virtual void OnDataReceived(string[] outputs)
        {
            DataReceived?.Invoke(this, outputs);
        }
    }
}
