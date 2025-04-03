using System;
using Rti.Dds.Core;
using Rti.Dds.Domain;
using Rti.Dds.Publication;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;
using Rti.Types.Dynamic;
using static Rti.Types.Dynamic.DynamicTypeFactory;

public class A3MPDataMsgSupport
{
   
    public static A3MPDataMsgSupport Instance { get; } = new A3MPDataMsgSupport();
    public static String TypeName = "A3MPDataMsg";

    public static Topic<string> CreateTopic(DomainParticipant participant, string topicName)
    {
        return participant.CreateTopic<string>(topicName);
    }

    public static DataWriter<string> CreateWriter(Publisher publisher, Topic<string> topic)
    {
        return publisher.CreateDataWriter(topic);
    }

    public static DataReader<string> CreateReader(Subscriber subscriber, Topic<string> topic)
    {
        return subscriber.CreateDataReader(topic);
    }

    //public static void RegisterType(DomainParticipant participant)
    //{
    //    // ✅ Create a DynamicType for A3MPDataMsg
    //    DynamicTypeFactory factory = DynamicTypeFactory.Instance;
    //    StructBuilder builder = factory.CreateStruct();

    //    builder.SetName(TypeName);
    //    builder.AddMember(0, "Data", factory.CreateStringType(255)); // String with max length 255

    //    DynamicType dynamicType = builder.Create();

    //    // ✅ Register the DynamicType
    //    participant.RegisterType(TypeName, dynamicType);
    //}
}















//using Rti.Dds.Topics;
//using Rti.Dds.Core;
//using Rti.Dds.Domain;
//using System;

//namespace DDS_Subscriber
//{
//    public class A3MPDataMsgDataSupport : TypeSupport<A3MPDataMsg>
//    {
//        // Constructor for A3MPDataMsgTypeSupport with no dynamic type support
//        //public A3MPDataMsgDataSupport()
//        //{
//        //    // The TypeSupport for A3MPDataMsg is now initialized without dynamic types.
//        //}

//        // Singleton instance for accessing TypeSupport
//        public static A3MPDataMsgDataSupport Instance { get; } = new A3MPDataMsgDataSupport();

//        // ToString method to print the contents of an A3MPDataMsg
//        //public string ToString(A3MPDataMsg data)
//        //{
//        //    return $"Hour: {data.hour}, Minute: {data.minute}, Second: {data.second}, " +
//        //           $"XCoord: {data.boundingBoxXCoord}, YCoord: {data.boundingBoxYCoord}, " +
//        //           $"Length: {data.boundingBoxLengthPixels}, Width: {data.boundingBoxWidthPixels}, " +
//        //           $"Is Aligned: {data.isAligned}";
//        //}

//        // Additional methods or overrides can be implemented here if needed
//    }
//}
//*/