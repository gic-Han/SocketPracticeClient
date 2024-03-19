namespace SocketPracticeClient;

class Program
{
    static void Main(string[] args)
    {
        Client client = new Client(1);
        client.Connect();
    }
}