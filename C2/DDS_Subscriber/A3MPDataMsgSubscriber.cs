/////*
////* (c) Copyright, Real-Time Innovations, 2012.  All rights reserved.
////* RTI grants Licensee a license to use, modify, compile, and create derivative
////* works of the software solely for use with RTI Connext DDS. Licensee may
////* redistribute copies of the software provided that all such copies are subject
////* to this license. The software is provided "as is", with no warranty of any
////* type, including any warranty for fitness for any purpose. RTI is under no
////* obligation to maintain or support the software. RTI shall not be liable for
////* any incidental or consequential damages arising out of the use or inability
////* to use the software.
////*/

//using System;
//using Omg.Dds.Core;
//using Rti.Dds.Core;
//using Rti.Dds.Core.Status;
//using Rti.Dds.Domain;
//using Rti.Dds.Subscription;
//using Rti.Dds.Topics;

///// <summary>
///// Example application that subscribes to global::A3MPDataMsg.
///// </summary>
//public static class A3MPDataMsgSubscriber
//{
//    private static int ProcessData(DataReader<string> reader)
//    {
//        // Take all samples. Samples are loaned to application; loan is
//        // returned when the samples collection is Disposed.
//        int samplesRead = 0;
//        using (var samples = reader.Take())
//        {
//            foreach (var sample in samples)
//            {
//                if (sample.Info.ValidData)
//                {
//                    Console.WriteLine(sample.Data);
//                    samplesRead++;
//                }
//                else
//                {
//                    Console.WriteLine($"Received instance update: {sample.Info.State.Instance}");
//                }
//            }
//        }

//        return samplesRead;
//    }

//    /// <summary>
//    /// Runs the subscriber example.
//    /// </summary>
//    public static void RunSubscriber(int domainId = 0, int sampleCount = int.MaxValue)
//    {
//        // A DomainParticipant allows an application to begin communicating in
//        // a DDS domain. Typically there is one DomainParticipant per application.
//        // DomainParticipant QoS is configured in USER_QOS_PROFILES.xml
//        //
//        // A participant needs to be Disposed to release middleware resources.
//        // The 'using' keyword indicates that it will be Disposed when this
//        // scope ends.
//        DomainParticipant participant = DomainParticipantFactory.Instance.CreateParticipant(domainId);

//        // A Topic has a name and a datatype.
//        Topic<string> topic = participant.CreateTopic<string>("A3MPDataMsg");

//        // A Subscriber allows an application to create one or more DataReaders
//        // Subscriber QoS is configured in USER_QOS_PROFILES.xml
//        Subscriber subscriber = participant.CreateSubscriber();

//        // This DataReader reads data on Topic "Example A3MPDataMsg".
//        // DataReader QoS is configured in USER_QOS_PROFILES.xml
//        DataReader<string> reader = subscriber.CreateDataReader(topic);

//        // Obtain the DataReader's Status Condition
//        StatusCondition statusCondition = reader.StatusCondition;

//        // Enable the 'data available' status.
//        statusCondition.EnabledStatuses = StatusMask.DataAvailable;

//        // Associate an event handler with the status condition.
//        // This will run when the condition is triggered, in the context of
//        // the dispatch call (see below)
//        int samplesRead = 0;
//        statusCondition.Triggered += _ => samplesRead += ProcessData(reader);

//        // Create a WaitSet and attach the StatusCondition
//        var waitset = new WaitSet();
//        waitset.AttachCondition(statusCondition);
//        while (samplesRead < sampleCount)
//        {
//            // Dispatch will call the handlers associated with the WaitSet
//            // conditions when they activate
//            Console.WriteLine("A3MPDataMsg subscriber sleeping for 4 sec...");
//            waitset.Dispatch(Duration.FromSeconds(4));
//        }
//    }
//}

//---------------------------------------------------

using Rti.Dds.Core;
using Rti.Dds.Domain;
using Rti.Dds.Topics;
using Rti.Dds.Subscription;
using System;
using System.Threading;
using A3MP_Shared;
//using A3MP_MAVLink;


namespace DDS_Subscriber
{
    public class A3MPDataMsgSubscriber
    {
        private const int DomainId = 0;
        private const string TopicName = "A3MPDataMsg";
        public static Logger logger = new Logger();
       
        public static void RunSubscriber()
        {
            try
            {
                var participant = DomainParticipantFactory.Instance.CreateParticipant(DomainId);

                //A3MPDataMsgSupport.RegisterType(participant);
                // Make sure we use the correct struct type
                var topic = participant.CreateTopic<A3MPDataMsg>(TopicName);

                var subscriber = participant.CreateSubscriber();
                var reader = subscriber.CreateDataReader<A3MPDataMsg>(topic);

                logger.WriteDebug("Waiting for messages...");

                while (true)
                {
                    var data = reader.Take();

                    if (data.Count > 0)
                        //logger.WriteDebug(data.Count.ToString());

                    {
                        foreach (var message in data)
                        {
                            //create offset based on A3 output: 
                            logger.WriteDebug("**TEST 3 - sent DDS message received to A3MP Bus...");
                            //A3MP_MessageBus.OnDDSMessageReceived?.Invoke(message.Data.ToString());
                            logger.WriteDebug($"Received message: {message.Data}");
                            A3MP_MessageBus.SetCommand(message.Data.ToString());
                            logger.WriteDebug("bus command: " + A3MP_MessageBus.GetCommand());

                            //need to make a call to the A3MP_CommandProcessor which will update flight data (MP)
                            //can't use A3MP_Shared to reach MP
                            //can DDS Subscriber point to A3MP_MAVLink? no - circular dependency 

                            //FlightData.isAligned = false;
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                logger.WriteDebug($"Error: {ex.Message}");
            }
        }
    }
}
