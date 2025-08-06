//namespace A3MP_Shared
//{
//    public static class A3MP_CommandProcessor
//    {
//        // This method processes incoming DDS messages (string here for example)
//        public static void HandleMessage(string message)
//        {
//            if (string.IsNullOrWhiteSpace(message))
//                return;

//            // Normalize or parse the message
//            string command = message.Trim().ToLowerInvariant();

//            // TODO: parse A3 output into fields for command
            
//            A3MP_MAVLink.A3MP_MAVLinkCommandPublisher.HandleDDSCommand(command);
//        }
//    }
//}
