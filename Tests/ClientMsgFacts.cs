﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteamKit2;
using SteamKit2.Internal;
using Xunit;
using Xunit.Sdk;

namespace Tests
{
    public class ClientMsgFacts
    {
        // this test vector is a packet meant for a ClientMsg<MsgClientChatEnter>
        static byte[] structMsgData =
        {
            0x27, 0x03, 0x00, 0x00, 0x24, 0x02, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF, 0xAC, 0x15, 0x89, 0x00, 0x01, 0x00, 0x10, 0x01,
            0x8E, 0x56, 0x11, 0x00, 0xBC, 0x4E, 0x2A, 0x00, 0x00, 0x00, 0x88, 0x01, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0xBC, 0x4E, 0x2A, 0x00, 0x00, 0x00, 0x70, 0x01,
            0xBC, 0x4E, 0x2A, 0x00, 0x00, 0x00, 0x70, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00,
            0x00, 0x53, 0x61, 0x78, 0x74, 0x6F, 0x6E, 0x20, 0x48, 0x65, 0x6C, 0x6C, 0x00, 0x00, 0x4D, 0x65,
            0x73, 0x73, 0x61, 0x67, 0x65, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x00, 0x07, 0x73, 0x74, 0x65,
            0x61, 0x6D, 0x69, 0x64, 0x00, 0xAC, 0x15, 0x89, 0x00, 0x01, 0x00, 0x10, 0x01, 0x02, 0x70, 0x65,
            0x72, 0x6D, 0x69, 0x73, 0x73, 0x69, 0x6F, 0x6E, 0x73, 0x00, 0x7B, 0x03, 0x00, 0x00, 0x02, 0x44,
            0x65, 0x74, 0x61, 0x69, 0x6C, 0x73, 0x00, 0x01, 0x00, 0x00, 0x00, 0x08, 0x08, 0x00, 0x4D, 0x65,
            0x73, 0x73, 0x61, 0x67, 0x65, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x00, 0x07, 0x73, 0x74, 0x65,
            0x61, 0x6D, 0x69, 0x64, 0x00, 0x00, 0x28, 0x90, 0x00, 0x01, 0x00, 0x10, 0x01, 0x02, 0x70, 0x65,
            0x72, 0x6D, 0x69, 0x73, 0x73, 0x69, 0x6F, 0x6E, 0x73, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x44,
            0x65, 0x74, 0x61, 0x69, 0x6C, 0x73, 0x00, 0x04, 0x00, 0x00, 0x00, 0x08, 0x08, 0x00, 0x4D, 0x65,
            0x73, 0x73, 0x61, 0x67, 0x65, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x00, 0x07, 0x73, 0x74, 0x65,
            0x61, 0x6D, 0x69, 0x64, 0x00, 0xB0, 0xDC, 0x5B, 0x04, 0x01, 0x00, 0x10, 0x01, 0x02, 0x70, 0x65,
            0x72, 0x6D, 0x69, 0x73, 0x73, 0x69, 0x6F, 0x6E, 0x73, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x44,
            0x65, 0x74, 0x61, 0x69, 0x6C, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x08, 0x00, 0x4D, 0x65,
            0x73, 0x73, 0x61, 0x67, 0x65, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x00, 0x07, 0x73, 0x74, 0x65,
            0x61, 0x6D, 0x69, 0x64, 0x00, 0x39, 0xCB, 0x77, 0x05, 0x01, 0x00, 0x10, 0x01, 0x02, 0x70, 0x65,
            0x72, 0x6D, 0x69, 0x73, 0x73, 0x69, 0x6F, 0x6E, 0x73, 0x00, 0x1A, 0x03, 0x00, 0x00, 0x02, 0x44,
            0x65, 0x74, 0x61, 0x69, 0x6C, 0x73, 0x00, 0x02, 0x00, 0x00, 0x00, 0x08, 0x08, 0xE8, 0x03, 0x00,
            0x00,
        };

        [Fact]
        public void PayloadReaderReadsNullTermString()
        {
            var msg = new ClientMsg<MsgClientChatEnter>( BuildStructMsg() );

            string chatName = msg.ReadNullTermString();

            Assert.Equal( chatName, "Saxton Hell" );
        }

        [Fact]
        public void PayloadReaderDoesNotOverflowPastNullTermString()
        {
            var msg = new ClientMsg<MsgClientChatEnter>( BuildStructMsg() );

            string chatName = msg.ReadNullTermString();

            Assert.Equal( chatName, "Saxton Hell" );

            byte nextByte = msg.ReadByte();
            char mByte = (char)msg.ReadByte();

            // next byte should be a null
            Assert.Equal( nextByte, 0 );
            // and the one after should be the beginning of a MessageObject
            Assert.Equal( mByte, 'M' );
        }

        [Fact]
        public void ClientMsgAssertsInitializedWithNonProtoMsg()
        {
            var packetMsgData = new ClientMsgProtobuf<CMsgClientLogon>( EMsg.ClientLogon ).Serialize();
            var packetMsg = new PacketClientMsgProtobuf( MsgUtil.MakeMsg( EMsg.ClientLogon, protobuf: true ), packetMsgData );


            var exception = Record.Exception( () => new ClientMsg<MsgClientLogon>( packetMsg ) );
            Assert.NotNull( exception );
            Assert.IsType<TraceAssertException>( exception );

            var tae = (TraceAssertException)exception;

            // Can't nameof(ClientMsg) - nameof doesn't support open generic types (yet).
            Assert.Contains( $"ClientMsg<{typeof( MsgClientLogon ).FullName}>", tae.AssertMessage );
        }

        [Fact]
        public void ClientMsgProtobufAssertsInitializedWithProtoMsg()
        {
            var packetMsgData = new ClientMsg<MsgClientLogon>().Serialize();
            var packetMsg = new PacketClientMsg( MsgUtil.MakeMsg( EMsg.ClientLogon, protobuf: false ), packetMsgData );

            var exception = Record.Exception( () => new ClientMsgProtobuf<CMsgClientLogon>( packetMsg ) );
            Assert.NotNull( exception );
            Assert.IsType<TraceAssertException>( exception );

            var tae = (TraceAssertException)exception;

            // Can't nameof(ClientMsgProtobuf) - nameof doesn't support open generic types (yet).
            Assert.Contains( $"ClientMsgProtobuf<{typeof( CMsgClientLogon ).FullName}>", tae.AssertMessage );
        }

        static IPacketMsg BuildStructMsg()
        {
            return CMClient.GetPacketMsg( structMsgData );
        }
    }
}