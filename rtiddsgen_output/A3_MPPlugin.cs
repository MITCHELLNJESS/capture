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

        private NativeString data;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            data.Destroy();
        }

        public void FromNative(global::A3MPDataMsg sample, bool keysOnly = false)
        {

            sample.data = data.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            data.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
        }

        public void ToNative(global::A3MPDataMsg sample, bool keysOnly = false)
        {
            data.ToNative(sample.data, ((int) 255));
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
                new StructMember("data", dtf.CreateString(((int) 255)), id: 0)
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

