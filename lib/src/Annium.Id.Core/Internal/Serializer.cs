using System;
using MessagePack;

namespace Annium.Id.Core.Internal;

internal static class Serializer
{
    public static string Serialize<T>(T data)
    {
        var result = Convert.ToBase64String(MessagePackSerializer.Serialize(
            data,
            MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray)
        ));

        return result;
    }

    public static T Deserialize<T>(string raw)
    {
        var data = MessagePackSerializer.Deserialize<T>(
            Convert.FromBase64String(raw),
            MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray)
        );

        return data;
    }
}