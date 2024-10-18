using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class Server
{
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>();

    public void StartServer(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine("Server started on port " + port);

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            lock (clients)
            {
                clients.Add(client);
            }

            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.Start(client);
        }
    }

    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int byteCount;

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
            Console.WriteLine("Received: " + message);
            Broadcast(message);
        }


    }

    private void Broadcast(string message)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(message);
    }

    public static void Main(string[] args)
    {
        Server server = new Server();
        server.StartServer(3000);
    }
}
