using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveClasses.ViewModelNS
{
    internal class SocketSeverSet
    {
        static void LaunchServer(string[] args)
        {
            Console.WriteLine("Démarrage du serveur...");
            Socket serverSocket = Connection();

            Console.WriteLine("En attente de connexion...");
            Socket clientSocket = AllowConnection(serverSocket);

            Console.WriteLine("Connexion établie !");
            ListenClient(clientSocket);

            ServerLogOff(serverSocket);
        }

        private static Socket Connection()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(10);

            return serverSocket;
        }

        private static Socket AllowConnection(Socket serverSocket)
        {
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine($"Client connecté: {clientSocket.RemoteEndPoint}");

            return clientSocket;
        }

        private static void ListenClient(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                bytesRead = clientSocket.Receive(buffer);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client: {dataReceived}");

                if (dataReceived.ToLower() == "fin")
                {
                    Console.Write("Fin de communication");
                    break;
                }


                Console.Write("Server: ");
                string message = "auto response";
                clientSocket.Send(Encoding.ASCII.GetBytes(message));
            }
        }

        private static void ServerLogOff(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
