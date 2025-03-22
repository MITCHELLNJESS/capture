/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from A3_MP.idl
using RTI Code Generator (rtiddsgen) version 4.3.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

using System;
using System.Runtime.InteropServices;
using Omg.Types;
using Omg.Types.Dynamic;
using Rti.Types;
using Rti.Dds.Core;
using Rti.Types.Dynamic;
using Rti.Dds.NativeInterface.TypePlugin;

namespace Implementation
{

    public struct A3MPDataMsgUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::A3MPDataMsg>
    {

        private short hour;
        private short minute;
        private short second;
        private double boundingBoxXCoord;
        private double boundingBoxYCoord;
        private double boundingBoxLengthPixels;
        private double boundingBoxWidthPixels;
        private byte isAligned;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::A3MPDataMsg sample, bool keysOnly = false)
        {

            sample.hour = hour;
            sample.minute = minute;
            sample.second = second;
            sample.boundingBoxXCoord = boundingBoxXCoord;
            sample.boundingBoxYCoord = boundingBoxYCoord;
            sample.boundingBoxLengthPixels = boundingBoxLengthPixels;
            sample.boundingBoxWidthPixels = boundingBoxWidthPixels;
            sample.isAligned = Convert.ToBoolean(isAligned);
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            hour = (short) (0);
            minute = (short) (0);
            second = (short) (0);
            boundingBoxXCoord = (double) (0.0);
            boundingBoxYCoord = (double) (0.0);
            boundingBoxLengthPixels = (double) (0.0);
            boundingBoxWidthPixels = (double) (0.0);
            isAligned = 0;
        }

        public void ToNative(global::A3MPDataMsg sample, bool keysOnly = false)
        {
            hour = sample.hour;
            minute = sample.minute;
            second = sample.second;
            boundingBoxXCoord = sample.boundingBoxXCoord;
            boundingBoxYCoord = sample.boundingBoxYCoord;
            boundingBoxLengthPixels = sample.boundingBoxLengthPixels;
            boundingBoxWidthPixels = sample.boundingBoxWidthPixels;
            isAligned = Convert.ToByte(sample.isAligned);
        }
    }

    internal class A3MPDataMsgPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::A3MPDataMsg, A3MPDataMsgUnmanaged>
    {

        internal A3MPDataMsgPlugin() : base("global::A3MPDataMsg", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // A3MPDataMsg struct
            var A3MPDataMsgStructMembers = new StructMember[]
            {
                new StructMember("hour", dtf.GetPrimitiveType<short>(), id: 0),
                new StructMember("minute", dtf.GetPrimitiveType<short>(), id: 1),
                new StructMember("second", dtf.GetPrimitiveType<short>(), id: 2),
                new StructMember("boundingBoxXCoord", dtf.GetPrimitiveType<double>(), id: 3),
                new StructMember("boundingBoxYCoord", dtf.GetPrimitiveType<double>(), id: 4),
                new StructMember("boundingBoxLengthPixels", dtf.GetPrimitiveType<double>(), id: 5),
                new StructMember("boundingBoxWidthPixels", dtf.GetPrimitiveType<double>(), id: 6),
                new StructMember("isAligned", dtf.GetPrimitiveType<bool>(), id: 7)
            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<A3MPDataMsgUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("A3MPDataMsg")
                .AddMembers(A3MPDataMsgStructMembers));

            return result;
        }
    }
}
public class A3MPDataMsgSupport : Rti.Dds.Topics.TypeSupport<global::A3MPDataMsg>
{
    public A3MPDataMsgSupport() : base(
        new Implementation.A3MPDataMsgPlugin(),
        new Lazy<DynamicType>(() =>Implementation.A3MPDataMsgPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static A3MPDataMsgSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<A3MPDataMsgSupport, global::A3MPDataMsg>();

}

