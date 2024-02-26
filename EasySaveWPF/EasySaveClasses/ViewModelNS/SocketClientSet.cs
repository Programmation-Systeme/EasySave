using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveClasses.ViewModelNS
{
    internal class SocketClientSet
    {
        /// <summary>
        /// Launches the client, connects to the server, sends user choice, and receives response.
        /// </summary>
        /// <param name="args">Command-line arguments (not used in this method).</param>
        /// <param name="choice">User's choice to send to the server.</param>
        /// <returns>Response received from the server.</returns>
        public static string LaunchClient(string[] args, string choice)
        {
            // Informing the user that the client is starting
            Console.WriteLine("Démarrage du client...");

            // Establishing a connection with the server
            Socket clientSocket = Connection();

            // Connection successful message
            Console.WriteLine("Connecté au serveur !");

            // Get list saves
            string listSaves = GetSavesList(clientSocket);

            // Sending user's choice to the server and receiving response
            string clientResponse = ListenToNetwork(clientSocket, choice);

            if(listSaves != clientResponse) {
                listSaves = clientResponse;
            }

            // Closing the connection to the server
            LogOff(clientSocket);

            // Returning the response received from the server
            return listSaves;
        }

        /// <summary>
        /// Establishes a socket connection to the server.
        /// </summary>
        /// <returns>Connected Socket object.</returns>
        private static Socket Connection()
        {
            // Server's IP address (localhost in this case)
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            // Server's endpoint
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8080);

            // Creating a socket using TCP protocol
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connecting the socket to the server
            clientSocket.Connect(remoteEP);

            // Returning the connected socket
            return clientSocket;
        }

        /// <summary>
        /// Sends user's choice to the server and receives a response.
        /// </summary>
        /// <param name="clientSocket">Connected Socket object.</param>
        /// <param name="choice">User's choice to send to the server.</param>
        /// <returns>Response received from the server.</returns>
        private static string ListenToNetwork(Socket clientSocket, string choice)
        {
            // Buffer to hold incoming data
            byte[] buffer = new byte[1024];
            int bytesRead;

            // Sending the user's choice to the server
            Console.Write("Client: ");
            clientSocket.Send(Encoding.ASCII.GetBytes(choice));

            // Receiving response from the server
            bytesRead = clientSocket.Receive(buffer);

            // Converting the received bytes to a string
            string listSaves = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Displaying the received message from the server
            Console.WriteLine($"Server: {listSaves}");

            // Returning the received message
            return listSaves;
        }

                /// <summary>
        /// Get form server the data list.
        /// </summary>
        /// <param name="clientSocket">Connected Socket object.</param>
        /// <returns>Response received from the server.</returns>
        private static string GetSavesList(Socket clientSocket)
        {
            // Buffer to hold incoming data
            byte[] buffer = new byte[1024];
            int bytesRead;

            // Getting updates
            clientSocket.Send(Encoding.ASCII.GetBytes("getSavesList"));

            // Receiving response from the server
            bytesRead = clientSocket.Receive(buffer);

            // Converting the received bytes to a string
            string listSaves = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Displaying the received message from the server
            Console.WriteLine($"Server: {listSaves}");

            // Returning the received message
            return listSaves;
        }

        /// <summary>
        /// Closes the socket connection to the server.
        /// </summary>
        /// <param name="socket">Socket object to close.</param>
        private static void LogOff(Socket socket)
        {
            // Shutdown the socket
            socket.Shutdown(SocketShutdown.Both);

            // Close the socket
            socket.Close();
        }

    }
}
