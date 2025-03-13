using System;
using System.Linq.Expressions;
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

class Program
{
    static void Main(string[] args)
    {
        // Set the IP address and port of the telnet server
        string ipAddress = "192.168.0.100";
        int port = 23;

        while (true)
        {
            // Create a new TcpClient object
            TcpClient client = new();

            try
            {
                // Connect to the Telnet server
                client.Connect(ipAddress, port);
            }
            catch (Exception ex)
            {
                // Print any errors
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                if (client.Connected)
                {
                    // Get the network stream
                    NetworkStream stream = client.GetStream();

                    // Create request for asset position data
                    char[] positionRequest = new char[] { 'P', 'O', 'S', '\n' };
                    byte[] positionRequestBytes = Encoding.ASCII.GetBytes(positionRequest);
                    stream.Write(positionRequestBytes, 0, positionRequestBytes.Length);

                    byte[] buffer = new byte[32];
                    stream.Read(buffer, 0, buffer.Length);

                    // Arduino zero-pads the int to match the size of the doubles
                    int latOffset = sizeof(double);
                    int lonOffset = latOffset + sizeof(double);
                    int altOffset = lonOffset + sizeof(double);

                    byte[] seqNumBytes = buffer.Take(sizeof(int)).ToArray();
                    byte[] latBytes = buffer.Skip(latOffset).Take(sizeof(double)).ToArray();
                    byte[] lonBytes = buffer.Skip(lonOffset).Take(sizeof(double)).ToArray();
                    byte[] altBytes = buffer.Skip(altOffset).Take(sizeof(double)).ToArray();

                    PositionData positionData = new PositionData();
                    positionData.seqNum = BitConverter.ToInt32(buffer, 0);
                    positionData.lat = BitConverter.ToDouble(latBytes, 0);
                    positionData.lon = BitConverter.ToDouble(lonBytes, 0);
                    positionData.alt = BitConverter.ToDouble(altBytes, 0);

                    Console.WriteLine("SeqNum: " + positionData.seqNum + " Lat: " + positionData.lat + " Lon: " + positionData.lon + " Alt: " + positionData.alt);

                    // Close the connection
                    stream.Close();
                }
                else
                {
                    Console.WriteLine("Client failed to connect!");
                }

                // Close the connection
                client.Close();

            }
            catch (Exception ex)
            {
                // Print any errors
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}