using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

[StructLayout(LayoutKind.Sequential)]
public struct PositionData
{
    public int seqNum;
    public double lat;
    public double lon;
    public double alt;
}

class TelnetServer
{
    private TcpListener listener;
    private int seqNum = 1;
    private double lat = 38.7509174;
    private double lon = -77.4971858;
    private double alt = 0;
    public void Start()
    {
        listener = new TcpListener(IPAddress.Loopback, 23);
        listener.Start();

        Console.WriteLine("Telnet server started on port 23");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected");

            Thread thread = new Thread(new ParameterizedThreadStart(HandleClient));
            thread.Start(client);
        }
    }

    private void HandleClient(object client)
    {
        TcpClient tcpClient = (TcpClient)client;
        NetworkStream stream = tcpClient.GetStream();

        PositionData data = new PositionData { seqNum = seqNum, lat = lat, lon = lon, alt = alt };
        byte[] dataBytes = new byte[Marshal.SizeOf(data)];
        GCHandle handle = GCHandle.Alloc(dataBytes, GCHandleType.Pinned);
        Marshal.StructureToPtr(data, handle.AddrOfPinnedObject(), false);
        handle.Free();

        stream.Write(dataBytes, 0, dataBytes.Length);
        Console.WriteLine("Data sent!");

        seqNum++;

        stream.Close();
        tcpClient.Close();
    }
}

class Program
{
    static void Main(string[] args)
    {
        TelnetServer server = new TelnetServer();
        server.Start();
    }
}
