using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveClasses.ViewModelNS
{
    internal class SocketSeverSet
    {
        static string options = "null";
        static string clientChoice = null;
        static ManualResetEvent dataReceived = new ManualResetEvent(false);
        public static string listSaves { get; set; }

    /// <summary>
    /// Launches the server, waits for a client connection, receives client's data, and sends a response.
    /// </summary>
    /// <param name="listSaves">Data to send to the client.</param>
    /// <returns>Message indicating the end of communication.</returns>
    public static void LaunchServer( string listSavesFromMain)
        {
            listSaves = listSavesFromMain;

            // Start the server
            StartServer((choice) =>
            {
                // Callback function to handle client's choice
                Console.WriteLine("Received user choice in Main: " + choice);
                // Here you can perform any action based on the user's choice
            });
        }

        public static void StartServer(Action<string> handleChoice)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 8888
                Int32 port = 8888;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server
                server = new TcpListener(localAddr, port);

                // Start listening for client requests
                server.Start();

                // Enter the listening loop
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    // Perform a blocking call to accept requests
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Create a separate thread to handle the client
                    Thread clientThread = new Thread(() => HandleClient(client, handleChoice));
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                server.Stop();
            }
        }

        public static void HandleClient(Object obj, Action<string> handleChoice)
        {
            TcpClient client = (TcpClient)obj;

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            NetworkStream stream = client.GetStream();

            int i;

            string output = JsonConvert.SerializeObject(listSaves);

            try
            {
                // Send options to the client continuously
                while (true)
                {
                    // Send the option to the client
                    byte[] msg = Encoding.ASCII.GetBytes(output);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine($"Sent: {options}");

                    Thread.Sleep(100); // Wait for 10 seconds before sending the next option

                    // Receive client's response
                    data = null;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine($"Received: {data}");

                        // Call the callback function with the client's choice
                        handleChoice(data);
                        // Set the choice in the main method
                        clientChoice = data;
                        dataReceived.Set(); // Signal that data is received
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                // Shutdown and end connection
                client.Close();
            }
        }
    }
}
