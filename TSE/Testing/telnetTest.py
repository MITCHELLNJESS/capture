import socket
import struct

# Define server IP and port
HOST = '127.0.0.1'  # Localhost
PORT = 23           # Port you are listening on

# Create a socket (IPv4, TCP)
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Connect to the server
try:
    client_socket.connect((HOST, PORT))
    print(f"Connected to {HOST}:{PORT}")
    
    # Send some test data
    message = "Hello, Unreal Server!"
    print(f"Sending: {message}")
    client_socket.sendall(message.encode('utf-8'))
    
    # Receive the echoed message from the server
    fmt = '<dddd'
    data = client_socket.recv(1024)
    seq_num, lat, lon, alt = struct.unpack(fmt, data)
    print(f"Received: SeqNum: {int(seq_num)}, Lat: {lat}, Lon: {lon}, Alt: {alt}")

except ConnectionRefusedError:
    print(f"Connection to {HOST}:{PORT} failed. Is the server running?")
except Exception as e:
    print(f"Error: {e}")
finally:
    # Close the socket connection
    client_socket.close()