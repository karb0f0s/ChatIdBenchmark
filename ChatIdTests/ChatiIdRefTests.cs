using System;
using Xunit;

namespace ChatiIdRefTests
{
    using ChatId = ChatIdRef;

    public class ChatiIdRefTests
    {
        [Theory]
        [InlineData("@username")]
        [InlineData("@UserName")]
        [InlineData("@User1")]
        [InlineData("@12345")]
        [InlineData("12345")]
        [InlineData("0")]
        [InlineData("999999999999999")]
        [InlineData("@99999999999999999999999999999999")]
        public void Should_Create_ChatId_From_Username(string userName)
        {
            var chatId = new ChatId(userName);

            Assert.Equal(chatId, userName);
            Assert.Equal(chatId, userName);
            Assert.True(chatId.Equals(userName));
            Assert.True(chatId == userName);
            Assert.False(chatId != userName);
            Assert.False(chatId == "@somerandomchat");
            //Assert.False(chatId == 1111);
        }

        [Theory]
        [InlineData(12345L)]
        [InlineData(0L)]
        [InlineData(999999999999999L)]
        public void Should_Create_ChatId_From_Identifier(long identifier)
        {
            var chatId = new ChatId(identifier);

            Assert.Equal(chatId, identifier);
            Assert.True(chatId.Equals(identifier));
            //Assert.True(chatId == identifier);
            //Assert.False(chatId != identifier);
            Assert.False(chatId == "@somerandomchat");
            //Assert.False(chatId == 1111);
        }

        [Fact]
        public void Should_Throw_If_Username_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ChatId(null!));
        }

        [Theory]
        [InlineData("username")]
        [InlineData("@u")]
        [InlineData("@User")]
        [InlineData("@1234")]
        [InlineData("999999999999999999999999")]
        [InlineData("99.9")]
        [InlineData("")]
        public void Should_Throw_On_Invalid_Username(string userName)
        {
            Assert.Throws<ArgumentException>(() => new ChatId(userName));
        }
    }
}
