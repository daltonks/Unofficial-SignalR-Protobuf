﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Microsoft.AspNetCore.SignalR.Protocol;
using Nerdbank.Streams;

namespace Unofficial.SignalR.Protobuf.MessageSerializers
{
    public class StreamItemMessageSerializer : BaseMessageSerializer
    {
        public override ProtobufMessageType EnumType => ProtobufMessageType.StreamItem;
        public override Type MessageType => typeof(StreamItemMessage);

        protected override IEnumerable<IMessage> CreateProtobufModels(HubMessage message)
        {
            var streamItemMessage = (StreamItemMessage) message;

            yield return new StreamItemMessageProtobuf
            {
                InvocationId = streamItemMessage.InvocationId,
                Headers = { streamItemMessage.Headers.Flatten() }
            };

            yield return (IMessage) streamItemMessage.Item;
        }

        protected override HubMessage CreateHubMessage(IReadOnlyList<IMessage> protobufModels)
        {
            var metadataProtobuf = (StreamItemMessageProtobuf) protobufModels.First();
            var itemProtobuf = protobufModels[1];
            return new StreamItemMessage(metadataProtobuf.InvocationId, itemProtobuf)
            {
                Headers = metadataProtobuf.Headers.Unflatten()
            };
        }
    }
}
