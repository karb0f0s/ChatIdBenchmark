using System;
using System.Globalization;
using ChatIdJsonConverter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace ChatIdJsonConverter
{
    using ChatId = ChatIdRecord;
    internal class ChatIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(ChatId) == objectType;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            var chatId = (ChatId)value;

            switch (chatId)
            {
                case ChatId.Username username:
                    writer.WriteValue(username.Value);
                    break;
                case ChatId.Identifier identifier:
                    writer.WriteValue(identifier.Value);
                    break;
                default:
                    writer.WriteNull();
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = JToken.ReadFrom(reader).Value<string>();

            if (long.TryParse(value, NumberStyles.Integer, null, out long identifier))
            {
                return new ChatId.Identifier(identifier);
            }
            else
            {
                return new ChatId.Username(value);
            }
        }
    }
}

namespace JsonConvertTest
{
    using ChatId = ChatIdRecord;

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class TestPOCO
    {
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(ChatIdConverter))]
        public ChatId ChatId { get; init; }
    }

    public class JsonConvertTest
    {
        [Theory]
        [InlineData("@User1", @"""@User1""")]
        public void Should_Serialize_POCO_string(string chatId, string serializeAs)
        {
            TestPOCO request = new TestPOCO
            {
                ChatId = ChatId.New(chatId)
            };

            string serializeRequest = JsonConvert.SerializeObject(request);

            Assert.Contains($@"{{""chat_id"":{serializeAs}}}", serializeRequest);
        }

        [Theory]
        [InlineData(999999999999999, "999999999999999")]
        [InlineData(-999999999999999, "-999999999999999")]
        public void Should_Serialize_POCO_long(long chatId, string serializeAs)
        {
            TestPOCO request = new TestPOCO
            {
                ChatId = ChatId.New(chatId)
            };

            string serializeRequest = JsonConvert.SerializeObject(request);

            Assert.Contains($@"{{""chat_id"":{serializeAs}}}", serializeRequest);
        }

        [Theory]
        [InlineData("@User1", @"{""chat_id"":""@User1""}")]
        public void Should_Deserialize_POCO_string(string chatId, string json)
        {
            TestPOCO request = new TestPOCO
            {
                ChatId = ChatId.New(chatId)
            };

            TestPOCO serializeRequest = JsonConvert.DeserializeObject<TestPOCO>(json);

            Assert.Equal(request.ChatId, serializeRequest.ChatId);
            Assert.IsType<ChatId.Username>(serializeRequest.ChatId);
            Assert.IsNotType<ChatId.Identifier>(serializeRequest.ChatId);
        }

        [Theory]
        [InlineData(999999999999999, @"{""chat_id"":999999999999999}")]
        public void Should_Deserialize_POCO2_long(long chatId, string json)
        {
            TestPOCO request = new TestPOCO
            {
                ChatId = ChatId.New(chatId)
            };

            TestPOCO serializeRequest = JsonConvert.DeserializeObject<TestPOCO>(json);

            Assert.Equal(request.ChatId, serializeRequest.ChatId);
            Assert.IsType<ChatId.Identifier>(serializeRequest.ChatId);
            Assert.IsNotType<ChatId.Username>(serializeRequest.ChatId);
        }
    }

    //public class JsonConvert2Test
    //{
    //    [Theory]
    //    [InlineData("@User1", @"""@User1""")]
    //    [InlineData("999999999999999", "999999999999999")]
    //    public void Should_Serialize_POCO2_string(string chatId, string serializeAs)
    //    {
    //        TestPOCO2 request = new TestPOCO2
    //        {
    //            ChatId = chatId
    //        };

    //        string serializeRequest = JsonConvert.SerializeObject(request);

    //        Assert.Contains($@"{{""chat_id"":{serializeAs}}}", serializeRequest);
    //    }

    //    [Theory]
    //    [InlineData(999999999999999, "999999999999999")]
    //    [InlineData(-999999999999999, "-999999999999999")]
    //    public void Should_Serialize_POCO2_long(long chatId, string serializeAs)
    //    {
    //        TestPOCO2 request = new TestPOCO2
    //        {
    //            ChatId = chatId
    //        };

    //        string serializeRequest = JsonConvert.SerializeObject(request);

    //        Assert.Contains($@"{{""chat_id"":{serializeAs}}}", serializeRequest);
    //    }

    //    [Theory]
    //    [InlineData("@User1", @"{""chat_id"":""@User1""}")]
    //    [InlineData("999999999999999", @"{""chat_id"":999999999999999}")]
    //    public void Should_Deserialize_POCO2_string(string chatId, string json)
    //    {
    //        TestPOCO2 request = new TestPOCO2
    //        {
    //            ChatId = chatId
    //        };

    //        TestPOCO2 serializeRequest = JsonConvert.DeserializeObject<TestPOCO2>(json);

    //        Assert.Equal(request.ChatId, serializeRequest.ChatId);
    //    }

    //    [Theory]
    //    [InlineData(999999999999999, @"{""chat_id"":999999999999999}")]
    //    [InlineData(-999999999999999, @"{""chat_id"":-999999999999999}")]
    //    public void Should_Deserialize_POCO2_long(long chatId, string json)
    //    {
    //        TestPOCO2 request = new TestPOCO2
    //        {
    //            ChatId = chatId
    //        };

    //        TestPOCO2 serializeRequest = JsonConvert.DeserializeObject<TestPOCO2>(json);

    //        Assert.Equal(request.ChatId, serializeRequest.ChatId);
    //    }
    //}

    //public class JsonConvertTest
    //{
    //    [Theory]
    //    [InlineData("@User1", @"""@User1""")]
    //    [InlineData("999999999999999", "999999999999999")]
    //    public void Should_Serialize_POCO_string(string chatId, string serializeAs)
    //    {
    //        TestPOCO request = new TestPOCO
    //        {
    //            ChatId = chatId
    //        };

    //        string serializeRequest = JsonConvert.SerializeObject(request);

    //        Assert.Contains($@"{{""chat_id"":{serializeAs}}}", serializeRequest);
    //    }

    //    [Theory]
    //    [InlineData(999999999999999, "999999999999999")]
    //    [InlineData(-999999999999999, "-999999999999999")]
    //    public void Should_Serialize_POCO_long(long chatId, string serializeAs)
    //    {
    //        TestPOCO request = new TestPOCO
    //        {
    //            ChatId = chatId
    //        };

    //        string serializeRequest = JsonConvert.SerializeObject(request);

    //        Assert.Contains($@"{{""chat_id"":{serializeAs}}}", serializeRequest);
    //    }
    //}
}
