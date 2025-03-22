/*
* (c) Copyright, Real-Time Innovations, 2012.  All rights reserved.
* RTI grants Licensee a license to use, modify, compile, and create derivative
* works of the software solely for use with RTI Connext DDS. Licensee may
* redistribute copies of the software provided that all such copies are subject
* to this license. The software is provided "as is", with no warranty of any
* type, including any warranty for fitness for any purpose. RTI is under no
* obligation to maintain or support the software. RTI shall not be liable for
* any incidental or consequential damages arising out of the use or inability
* to use the software.
* 
* Additional modifications to integrate with Mission Planner have been made.
*/

using System;
using Omg.Dds.Core;
using Rti.Dds.Core;
using Rti.Dds.Core.Status;
using Rti.Dds.Domain;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;

namespace DDS_Subscriber
{
    //DDS Subscriber 
    public class A3MPDataMsgSubscriber
    {
        //initialize: 
        public DomainParticipant participant;
        public Subscriber subscriber;
        public DataReader<A3MPDataMsg> reader;
        public Topic<A3MPDataMsg> topic;
        public bool isRunning = false; //status of subscriber, could be useful for debug
        public  WaitSet waitset; //for status condition
        public StatusCondition statusCondition; //gets added to waitset, also useful for debug/monitoring

        private int ProcessData(DataReader<A3MPDataMsg> reader)
        {
            // Take all samples. Samples are loaned to application; loan is
            // returned when the samples collection is Disposed.
            int samplesRead = 0;
            using (var samples = reader.Take())
            {
                foreach (var sample in samples)
                {
                    if (sample.Info.ValidData)
                    {
                        Console.WriteLine(sample.Data);
                        samplesRead++;
                    }
                    else
                    {
                        Console.WriteLine($"Received instance update: {sample.Info.State.Instance}");
                    }
                }
            }

            return samplesRead;
        }

        //Runs subscriber
        public void RunSubscriber(int domainId = 0, int sampleCount = int.MaxValue)
        {
            Console.WriteLine("Running DDS Subscriber!");
            participant = DomainParticipantFactory.Instance.CreateParticipant(domainId);

            // Create topic for A3MPDataMsg
            topic = participant.CreateTopic<A3MPDataMsg>("A3MPDataMsg");

            //create subscriber
            subscriber = participant.CreateSubscriber();

            //create data reader on predefined topic 
            reader = subscriber.CreateDataReader(topic);

            // obtain the DataReader's Status Condition
            statusCondition = reader.StatusCondition;

            // enable the 'data available' status.
            statusCondition.EnabledStatuses = StatusMask.DataAvailable;

            //event based on item read
            int samplesRead = 0;
            statusCondition.Triggered += _ => samplesRead += ProcessData(reader);

            // allow code to sleep for 4s
            isRunning = true;
            waitset = new WaitSet();
            waitset.AttachCondition(statusCondition);
            while (samplesRead < sampleCount)
            {                
                Console.WriteLine("A3MPDataMsg subscriber sleeping for 4 sec...");
                waitset.Dispatch(Duration.FromSeconds(4));
            }
        }

        // Gracefully stops the subscriber
        public void StopSubscriber()
        {
            if (!isRunning)
            {
                Console.WriteLine("Subscriber is not running.");
                return;
            }

            // Detach conditions and clean up resources
            waitset.DetachCondition(statusCondition);

            // Stop the participant, which stops the subscriber and reader
            reader.Dispose();
            subscriber.Dispose();
            participant.Dispose();

            isRunning = false;

            Console.WriteLine("Subscriber has been stopped.");
        }
    }

}
