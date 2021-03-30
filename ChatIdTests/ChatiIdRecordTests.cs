using System;
using Xunit;

namespace ChatiIdRecordTests
{
    using ChatId = ChatIdRecord;

    public class ChatiIdRecordTests
    {
        [Theory]
        [InlineData("@username")]
        [InlineData("@UserName")]
        [InlineData("@User1")]
        [InlineData("@12345")]
        //[InlineData("12345")]
        //[InlineData("-12345")]
        //[InlineData("999999999999999")]
        //[InlineData("-999999999999999")]
        [InlineData("@99999999999999999999999999999999")]
        public void Should_Create_ChatId_From_Username(string userName)
        {
            ChatId chatId = ChatId.New(userName);

            Assert.Equal(chatId, userName);
            //Assert.True(((string)chatId).Equals(userName));
            Assert.True(chatId == userName);
            Assert.False(chatId != userName);
            Assert.False(chatId == "@somerandomchat");
            Assert.False(chatId == "1111");
        }

        [Theory]
        [InlineData(12345L)]
        [InlineData(0L)]
        [InlineData(999999999999999L)]
        [InlineData(-12345L)]
        [InlineData(-999999999999999L)]
        public void Should_Create_ChatId_From_Identifier(long identifier)
        {
            ChatId chatId = ChatId.New(identifier);

            Assert.Equal(chatId, identifier);
            Assert.True(((long)chatId).Equals(identifier));
            Assert.True(chatId == identifier);
            Assert.False(chatId != identifier);
            Assert.False(chatId == "@somerandomchat");
            Assert.False(chatId == 1111);
        }

        [Fact]
        public void Should_Throw_If_Username_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ChatId.New(null!)
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData("username")]
        [InlineData("@u")]
        [InlineData("@User")]
        [InlineData("@1234")]
        [InlineData("999999999999999999999999")]
        [InlineData("12.345")]
        [InlineData("12,345")]
        [InlineData("12 345")]
        //[InlineData("12345")]
        //[InlineData("999999999999999")]
        //[InlineData("-999999999999999")]
        public void Should_Throw_On_Invalid_Username(string userName)
        {
            Assert.Throws<ArgumentException>(() =>
                ChatId.New(userName)
            );
        }

        [Theory]
        [InlineData("@username")]
        [InlineData("@UserName")]
        [InlineData("@User1")]
        [InlineData("@12345")]
        public void Should_Throw_On_Implicit_Conversion(string userName)
        {
            ChatId chatId = ChatId.New(userName);

            Assert.Throws<InvalidCastException>(() =>
                _ = (long)chatId == 111
            );
        }
    }
}
