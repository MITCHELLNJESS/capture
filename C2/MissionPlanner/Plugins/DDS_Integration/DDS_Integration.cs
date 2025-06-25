using System;
using System.Threading;
using System.Windows.Forms;
using MissionPlanner;
using Rti.Dds.Core;
using DDS_Subscriber;
using System.Diagnostics;

public class DDS_Integration
{
    private Thread ddsThread;
    //private A3MPDataMsgSubscriber a3MPDataMsgSubscriber;
    private bool isRunning = false;
    Logger logger = new Logger();

    public DDS_Integration()
    {
        // Constructor: Initialize any necessary configurations if needed
    }

    // Start DDS Subscriber in a new thread
    public void StartDDSSubscriber() 
    {
        if (isRunning) return; //closes if another process is running
        logger.WriteDebug("StartDDSSub()!");
        isRunning = true;
        ddsThread = new Thread(RunDDSSubscriber);
        ddsThread.Start();
    }

    // Stop DDS Subscriber
    public void StopDDSSubscriber()
    {
        if (!isRunning) return;

        isRunning = false;
        ddsThread?.Join();  // Ensure the thread stops properly
    }

    // Run the DDS Subscriber (using A3MPDataSubscriber's method)
    private void RunDDSSubscriber()
    {
        logger.WriteDebug("run DDS Sub()!");
        // Run the subscriber and process data (delegated to A3MPDataSubscriber)
        A3MPDataMsgSubscriber.RunSubscriber();
    }

    // Process the data received from the subscriber and send data to ARV via MAVLink
    public void ProcessData(A3MPDataMsg receivedData)
    {
        // Example: Update the UI with the received data (invoke to UI thread if necessary)
        if (Application.OpenForms["MainForm"] != null)
        {
        //    MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
        //    mainForm.Invoke((Action)(() =>
        //    {
        //        mainForm.SomeLabel.Text = $"Received: {receivedData.hour}:{receivedData.minute}:{receivedData.second}";
        //    }));
        }
    }
}
