using System;

namespace A3MP_Shared
{
    public static class A3MP_MessageBus
    {
        public static string command = "DEFAULT";

        public static void SetCommand(string message)
        {
            Console.WriteLine("******* A3MP_MessageBus.setCommand() ********");
            command = message;
            //TODO set as array of values (change command to array not string)

            //OnDDSMessageReceived?.Invoke(message);
        }

        //command accessor function
        public static string GetCommand()
        {
            return command;
        }

        //public static void UpdateFlightData(string command)
        //{
        //    Console.WriteLine("--------------UPDATE FLIGHT DATA-----------------");
        //    Console.WriteLine("command received: " + command);

        //    //FlightData.isAligned = command.Length < 0; //command comes from A3 (x, y, z) - continuosly
        //    //we're given X, Y in center of box 
        //    // vector = what direction does ARV need to move to align A3 center X/Y with 
        //    // ARV center (cross hair location) - placeholder value for now 

        //    //vector = (center x - ARV X, CENTER y - ARV y)
        //    ////- purpose of this is simply to know what 'direction' or angle to go, then approach slowly....


        //    //+ need a helper function that can determine if we're aligned based on x and y, then update
        //    // FlightData.isAligned (which will needed to be checked before PatsFly2Here()) 
        //    //(check if vector X, Vector Y each are < THRESHOLD AMOUNT) 


        //    //switch (command)
        //    //{
        //    //    case "left": //TODO: map A3 output to these commands
        //    //        await MoveDirection(-1, 0, 0); //call flytohere instead
        //    //        break;
        //    //    case "right":
        //    //        await MoveDirection(1, 0, 0);
        //    //        break;
        //    //    case "forward":
        //    //        await MoveDirection(0, 1, 0);
        //    //        break;
        //    //    case "backward":
        //    //        await MoveDirection(0, -1, 0);
        //    //        break;
        //    //    case "up":
        //    //        await MoveDirection(0, 0, -0.5f);
        //    //        break;
        //    //    case "down":
        //    //        await MoveDirection(0, 0, 0.5f);
        //    //        break;
        //    //    default:
        //    //        Console.WriteLine($"Unknown command: {command}");
        //    //        break;
        //    //}
        //}
    }
}
