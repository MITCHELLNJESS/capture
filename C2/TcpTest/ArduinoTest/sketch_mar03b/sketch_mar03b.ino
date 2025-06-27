#include <SPI.h>
#include <WiFiNINA.h>

// WiFi login credentials
char ssid[] = "DrakosDynamicsC2";
char pass[] = "DrakosDynamics23";

int status = WL_IDLE_STATUS;

WiFiServer serverMP(23);
WiFiServer serverRTK(80);

struct position {
  int seqNum;
  double lat;
  double lon;
  double alt;
};

position assetPos{};

union {
  position positionStruct;
  byte bytes[sizeof(position)];
} positionData;

// One client for Mission Planner, and one for RTK
WiFiClient clientMP;
WiFiClient clientRTK;

// Bools indicating whether each client is connected
bool connectedMP = false;
bool connectedRTK = false;

// Strings to store client inputs
String currentLineMP = "";
String currentLineRTK = "";

void setup() {
  // Test position data to send to Mission Planner
  assetPos.seqNum = 0;
  assetPos.lat = 38.7509174;
  assetPos.lon = -77.4971858;
  assetPos.alt = 0;
  
  // initialize serial communication
  Serial.begin(9600);

  // check for the WiFi module:
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");

    // don't continue
    while (true);
  }

  // Attempt to connect to Wifi network:
  while (status != WL_CONNECTED) {
    // Connect to WPA/WPA2 network
    Serial.print("Attempting to connect to Network named: ");
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);

    // Wait 10 seconds for connection:
    delay(10000);
  }

  // Start both servers
  serverMP.begin();
  serverRTK.begin();

  printWifiStatus();
}

int lastTime = millis();

void loop() {
  // Mission Planner
  if (!connectedMP) {
    clientMP = serverMP.available();
    if (clientMP) {
      connectedMP = true;
      Serial.println("Mission Planner connected.");
    }
  } else {
    if (clientMP.connected()) {
      if (clientMP.available()) {
        char c = clientMP.read();
        if (c == '\n') {
          if (!strncmp(currentLineMP.c_str(), "POS", 3)) {
            Serial.println("Received request to send asset GNSS position to Mission Planner");
            clientMP.write((byte*)&assetPos, 32);
            assetPos.seqNum++;
          }
          currentLineMP = "";
        } else if (c != '\r') {
          currentLineMP += c;
        }
      }
    } else {
      clientMP.stop();
      connectedMP = false;
      Serial.println("Mission Planner disconnected.");
    }
  }

  // RTK
  if (!connectedRTK) {
    clientRTK = serverRTK.available();
    if (clientRTK) {
      connectedRTK = true;
      Serial.println("RTK connected.");
    }
  } else {
    if (clientRTK.connected()) {
      if (clientRTK.available()) {
        char c = clientRTK.read();
        if (c == '\n') {
          if (!strncmp(currentLineRTK.c_str(), "ADJ", 3)) {
            int adjustment = currentLineRTK.substring(4).toDouble();
            Serial.print("Received RTK adjustment value: ");
            Serial.println(adjustment);
            assetPos.lat+=adjustment;
            assetPos.lon+=adjustment;
            assetPos.alt+=adjustment;
          }
          currentLineRTK = "";
        } else if (c != '\r') {
          currentLineRTK += c;
        }
      }
    } else {
      clientRTK.stop();
      connectedRTK = false;
      Serial.println("RTK disconnected.");
    }
  }
}

void printWifiStatus() {

  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());
  
  // print your board's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
}
