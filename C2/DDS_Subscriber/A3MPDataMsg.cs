
/*

This file was generated from A3_MP.idl
using RTI Code Generator (rtiddsgen) version 4.3.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.

Additional modifications to integrate with Mission Planner have been made.
*/

using System;
using System.Reflection;
using System.Collections.Generic;
using Rti.Types;
using System.Linq;
using Omg.Types;

namespace DDS_Subscriber
{
    public class A3MPDataMsg : IEquatable<A3MPDataMsg>
    {
        public short hour { get; set; }
        public short minute { get; set; }
        public short second { get; set; }
        public double boundingBoxXCoord { get; set; }
        public double boundingBoxYCoord { get; set; }
        public double boundingBoxLengthPixels { get; set; }
        public double boundingBoxWidthPixels { get; set; }
        public bool isAligned { get; set; }

        public A3MPDataMsg()
        {
        }

        public A3MPDataMsg(short hour, short minute, short second, double boundingBoxXCoord, double boundingBoxYCoord, double boundingBoxLengthPixels, double boundingBoxWidthPixels, bool isAligned)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.boundingBoxXCoord = boundingBoxXCoord;
            this.boundingBoxYCoord = boundingBoxYCoord;
            this.boundingBoxLengthPixels = boundingBoxLengthPixels;
            this.boundingBoxWidthPixels = boundingBoxWidthPixels;
            this.isAligned = isAligned;
        }

        public A3MPDataMsg(A3MPDataMsg other)
        {
            if (other == null)
            {
                return;
            }

            this.hour = other.hour;
            this.minute = other.minute;
            this.second = other.second;
            this.boundingBoxXCoord = other.boundingBoxXCoord;
            this.boundingBoxYCoord = other.boundingBoxYCoord;
            this.boundingBoxLengthPixels = other.boundingBoxLengthPixels;
            this.boundingBoxWidthPixels = other.boundingBoxWidthPixels;
            this.isAligned = other.isAligned;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.hour);
            hash.Add(this.minute);
            hash.Add(this.second);
            hash.Add(this.boundingBoxXCoord);
            hash.Add(this.boundingBoxYCoord);
            hash.Add(this.boundingBoxLengthPixels);
            hash.Add(this.boundingBoxWidthPixels);
            hash.Add(this.isAligned);

            return hash.ToHashCode();
        }

        public bool Equals(A3MPDataMsg other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.hour.Equals(other.hour) &&
            this.minute.Equals(other.minute) &&
            this.second.Equals(other.second) &&
            this.boundingBoxXCoord.Equals(other.boundingBoxXCoord) &&
            this.boundingBoxYCoord.Equals(other.boundingBoxYCoord) &&
            this.boundingBoxLengthPixels.Equals(other.boundingBoxLengthPixels) &&
            this.boundingBoxWidthPixels.Equals(other.boundingBoxWidthPixels) &&
            this.isAligned.Equals(other.isAligned);
        }

        public override bool Equals(object obj) => this.Equals(obj as A3MPDataMsg);

        public override string ToString() => A3MPDataMsgSupport.Instance.ToString(this);
    }
}


