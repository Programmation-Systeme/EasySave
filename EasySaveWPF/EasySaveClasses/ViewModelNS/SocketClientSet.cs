using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveClasses.ViewModelNS
{
    internal class SocketClientSet
    {
        public static string LaunchClient(string[] args, string choice)
        {
            Console.WriteLine("Démarrage du client...");
            Socket clientSocket = Connection();
            Console.WriteLine("Connecté au serveur !");
            string clientResponse = ListenToNetwork(clientSocket, choice);
            LogOff(clientSocket);

            return clientResponse;
        }

        private static Socket Connection()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8080);
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(remoteEP);
            return clientSocket;
        }

        private static string ListenToNetwork(Socket clientSocket, string choice)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            Console.Write("Client: ");
            // string message = Console.ReadLine();
            clientSocket.Send(Encoding.ASCII.GetBytes(choice));
 
            bytesRead = clientSocket.Receive(buffer);
            string listSaves = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Server: {listSaves}");

            return listSaves;
            
        }

        private static void LogOff(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
}
