using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS_Subscriber
{
    public class A3MPDataMsgSupport
    {       
        public static A3MPDataMsgSupport Instance { get; } = new A3MPDataMsgSupport();

        // Print A3MPDataMsg
        public string ToString(A3MPDataMsg data)
        {
            return $"Hour: {data.hour}, Minute: {data.minute}, Second: {data.second}, " +
                   $"XCoord: {data.boundingBoxXCoord}, YCoord: {data.boundingBoxYCoord}, " +
                   $"Length: {data.boundingBoxLengthPixels}, Width: {data.boundingBoxWidthPixels}, " +
                   $"Is Aligned: {data.isAligned}";
        }
    }
}

