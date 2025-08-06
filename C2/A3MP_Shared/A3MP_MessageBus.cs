using System;
using System.Security.Cryptography.X509Certificates;

namespace A3MP_Shared
{
    public static class A3MP_MessageBus
    {
        public static int[] command = {-9999, -9999, -9999, -9999}; //default
        public static string[] a3Output = new string[8];
        private static bool commandReady = false;

        public static void SetCommand(int[] cmd)
        {
            command = cmd;
        }
        
        public static void ParseCommandString(string message)
        {
            Console.WriteLine("******* A3MP_MessageBus.setCommand() ********");

            //TODO set as array of values (change command to array not string)
            //String order: {timestamp}, {x1}, {y1}, {width}, {height}, {center_x}, {center_y}, {latency:.2f}
            a3Output = message.Split(',');
            //message comes from A3 (x, y) - continuously
            //we're given X, Y in center of box 
            command[0] = Int32.Parse(a3Output[5]); //X-component of directional vector
            command[1] = Int32.Parse(a3Output[6]); //Y-component of directional vector
            command[2] = Int32.Parse(a3Output[3]); //Width of bounding box
            command[3] = Int32.Parse(a3Output[4]); //Height of bounding box
            //vector = (center x - ARV X, CENTER y - ARV y) - need to pass to MP code to know ARV X/Y
            ////- purpose of this is simply to know what 'direction' or angle to go, then approach slowly....

            commandReady = true;
        }

        //command accessor function
        public static int[] GetCommand()
        {
            return command;
        }

        public static int[] GetCommandSnapshot()
        {
            Console.WriteLine("******* A3MP_MessageBus.GetCommandSnapshot() ********");

            if (commandReady)
            {
                commandReady = false;
                return command;
            }
            else
            {
                return new int[] { -9999, -9999, -9999, -9999 };
            }
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
