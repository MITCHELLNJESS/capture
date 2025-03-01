
# (c) Copyright, Real-Time Innovations, 2022.  All rights reserved.
# RTI grants Licensee a license to use, modify, compile, and create derivative
# works of the software solely for use with RTI Connext DDS. Licensee may
# redistribute copies of the software provided that all such copies are subject
# to this license. The software is provided "as is", with no warranty of any
# type, including any warranty for fitness for any purpose. RTI is under no
# obligation to maintain or support the software. RTI shall not be liable for
# any incidental or consequential damages arising out of the use or inability
# to use the software.

import time
import sys
import rti.connextdds as dds
from A3_MP import A3MP_Data

class A3MP_DataSubscriber:

    @staticmethod
    def process_data(reader):
        # take_data() returns copies of all the data objects in the reader
        # and removes them. To also take the objectInfo meta-data, use take().
        # To not remove the data from the reader, use read_data() or read().
        objects = reader.take_data()
        for object in objects:
            print(f"Received: {object}")
            print(object.name)
    
        return len(objects)

    @staticmethod
    def run_subscriber(domain_id: int, object_count: int):

        # A DomainParticipant allows an application to begin communicating in
        # a DDS domain. Typically there is one DomainParticipant per application.
        # DomainParticipant QoS is configured in USER_QOS_PROFILES.xml
        participant = dds.DomainParticipant(domain_id)

        # A Topic has a name and a datatype.
        topic = dds.Topic(participant, "A3MP_Data", A3MP_Data)

        # This DataReader reads data on Topic "A3MP_Data".
        # DataReader QoS is configured in USER_QOS_PROFILES.xml
        reader = dds.DataReader(participant.implicit_subscriber, topic)

        # Initialize objects_read to zero
        objects_read = 0

        # Associate a handler with the status condition. This will run when the
        # condition is triggered, in the context of the dispatch call (see below)
        # condition argument is not used
        def condition_handler(_):
            nonlocal objects_read
            nonlocal reader
            objects_read += A3MP_DataSubscriber.process_data(reader)

        # Obtain the DataReader's Status Condition
        status_condition = dds.StatusCondition(reader)

        # Enable the "data available" status and set the handler.
        status_condition.enabled_statuses = dds.StatusMask.DATA_AVAILABLE
        status_condition.set_handler(condition_handler)

        # Create a WaitSet and attach the StatusCondition
        waitset = dds.WaitSet()
        waitset += status_condition

        while objects_read < object_count:
            # Catch control-C interrupt
            try:
                # Dispatch will call the handlers associated to the WaitSet conditions
                # when they activate
                print("Hello World subscriber sleeping for 1 seconds...")

                waitset.dispatch(dds.Duration(1))  # Wait up to 1s each time
            except KeyboardInterrupt:
                break

        print("preparing to shut down...")


if __name__ == "__main__":
    A3MP_DataSubscriber.run_subscriber(
            domain_id=0,
            object_count=sys.maxsize)
