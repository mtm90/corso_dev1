using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Client
{
    private TcpClient client;
    private NetworkStream stream;

    public void StartClient(string serverIP, int port)
    {
        try
        {
            client = new TcpClient(serverIP, port);
            stream = client.GetStream();
            Console.WriteLine("Connected to server.");

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            SendMessages();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        finally
        {
            stream?.Close();
            client?.Close();
        }
    }

    private void SendMessages()
    {
        string messageToSend;
        while ((messageToSend = Console.ReadLine()) != null && messageToSend != "")
        {
            byte[] buffer = Encoding.ASCII.GetBytes(messageToSend);
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    private void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int byteCount;

        try
        {
            while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
                Console.WriteLine("Server: " + message);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection closed: " + e.Message);
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the server IP:");
        string serverIP = Console.ReadLine();

        Client client = new Client();
        client.StartClient(serverIP, 3000);
    }
}
