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
//        public string ToString(A3MPDataMsg data)
//        {
//            return $"Hour: {data.hour}, Minute: {data.minute}, Second: {data.second}, " +
//                   $"XCoord: {data.boundingBoxXCoord}, YCoord: {data.boundingBoxYCoord}, " +
//                   $"Length: {data.boundingBoxLengthPixels}, Width: {data.boundingBoxWidthPixels}, " +
//                   $"Is Aligned: {data.isAligned}";
//        }

//        // Additional methods or overrides can be implemented here if needed
//    }
//}
