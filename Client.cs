using System;
using System.Net.Sockets;
using System.Text;

public class Client
{
    private string ServerAddress;
    private const int Port = 50001;
    private bool opencheck = true;
    private TcpClient? client;
    private NetworkStream? stream;

    public Client(int i)
    {
        ServerAddress = "127.0.0." + i;
    }

    public void Connect()
    {
        client = new TcpClient(ServerAddress, Port);
        stream = client.GetStream();
        Console.WriteLine("서버에 연결되었습니다.");

        // 서버로부터 메시지 수신을 위한 스레드 시작
        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();
        opencheck = true;

        // 사용자 입력 받아서 서버로 메시지 전송
        while (true)
        {
            Console.Write("메시지 작성 : ");
            string? message = Console.ReadLine();
            if(message == null)
            {
                Console.WriteLine();
                Console.WriteLine("1글자 이상 작성해야 합니다.");
                continue;
            }
            SendMessage(message);
            if(message == "exit")
            {
                Console.WriteLine();
                Console.WriteLine("클라이언트 종료");
                opencheck = false;
                break;
            }
        }
    }

    private void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while (opencheck)
        {
            bytesRead = stream!.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if(message == "client 1 has left")
            {
                break;
            }
            Console.WriteLine("서버로부터 메시지 수신: " + message);
        }
    }

    public void SendMessage(string message)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        stream!.Write(bytes, 0, bytes.Length);
    }
}
