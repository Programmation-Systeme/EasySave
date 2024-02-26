using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveClasses.ViewModelNS
{
    internal class SocketSeverSet
    {
        /// <summary>
        /// Launches the server, waits for a client connection, receives client's data, and sends a response.
        /// </summary>
        /// <param name="args">Command-line arguments (not used in this method).</param>
        /// <param name="listSaves">Data to send to the client.</param>
        /// <returns>Message indicating the end of communication.</returns>
        static string LaunchServer(string[] args, string listSaves)
        {
            // Informing the user that the server is starting
            Console.WriteLine("Démarrage du serveur...");

            // Establishing the server socket
            Socket serverSocket = Connection();

            // Waiting for a client connection
            Console.WriteLine("En attente de connexion...");
            Socket clientSocket = AllowConnection(serverSocket);

            // Client connected message
            Console.WriteLine("Connexion établie !");

            // Listening to the client and responding
            string serverMessage = ListenClient(clientSocket, listSaves);

            // Closing the server
            ServerLogOff(serverSocket);

            // Indicating the end of communication
            return serverMessage;
        }

        /// <summary>
        /// Establishes a socket connection for the server.
        /// </summary>
        /// <returns>Connected Socket object for the server.</returns>
        private static Socket Connection()
        {
            // Server's IP address (localhost in this case)
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            // Server's endpoint
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            // Creating a socket using TCP protocol
            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Binding the socket to the local endpoint
            serverSocket.Bind(localEndPoint);

            // Listening for incoming connections with a backlog of 10
            serverSocket.Listen(10);

            // Returning the connected server socket
            return serverSocket;
        }

        /// <summary>
        /// Allows a client to connect to the server.
        /// </summary>
        /// <param name="serverSocket">Server's Socket object.</param>
        /// <returns>Connected Socket object for the client.</returns>
        private static Socket AllowConnection(Socket serverSocket)
        {
            // Accepting an incoming connection
            Socket clientSocket = serverSocket.Accept();

            // Client connected message
            Console.WriteLine($"Client connecté: {clientSocket.RemoteEndPoint}");

            // Returning the connected client socket
            return clientSocket;
        }

        /// <summary>
        /// Listens to client's messages and sends a response.
        /// </summary>
        /// <param name="clientSocket">Client's Socket object.</param>
        /// <param name="listSaves">Data to send to the client.</param>
        /// <returns>Message indicating the end of communication.</returns>
        private static string ListenClient(Socket clientSocket, string listSaves)
        {
            // Buffer to hold incoming data
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                // Receiving data from the client
                bytesRead = clientSocket.Receive(buffer);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Displaying the received message from the client
                Console.WriteLine($"Client: {dataReceived}");

                // Checking if the client wants to end communication
                if (dataReceived.ToLower() == "fin")
                {
                    Console.WriteLine("Fin de communication");
                    break;
                }

                // Sending a response to the client
                Console.Write("Server: ");
                string serverMessage = listSaves;
                clientSocket.Send(Encoding.ASCII.GetBytes(serverMessage));
            }
            // Indicating the end of communication
            return "end";
        }

        /// <summary>
        /// Closes the server's socket connection.
        /// </summary>
        /// <param name="socket">Server's Socket object to close.</param>
        private static void ServerLogOff(Socket socket)
        {
            // Shutdown the socket
            socket.Shutdown(SocketShutdown.Both);

            // Close the socket
            socket.Close();
        }
    }
}
