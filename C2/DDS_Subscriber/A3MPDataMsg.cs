
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from A3_MP.idl
using RTI Code Generator (rtiddsgen) version 4.3.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
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
        [Bound(255)]
        public string data { get; set; } = string.Empty;

        public A3MPDataMsg()
        {
        }

        public A3MPDataMsg(string data)
        {
            this.data = data;
        }

        public A3MPDataMsg(A3MPDataMsg other)
        {
            if (other == null)
            {
                return;
            }

            this.data = other.data;

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.data);

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

            return this.data.Equals(other.data);
        }

        public override bool Equals(object obj) => this.Equals(obj as A3MPDataMsg);

        public override string ToString() => A3MPDataMsgSupport.Instance.ToString(this);
    }

} // namespace DDS_Subscriber
