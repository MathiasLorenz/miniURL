using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;

namespace MiniURL.Application.Tests.Common
{
    [TestClass]
    public class TokenGeneratorTest
    {
        // We are now dependent on the implementation and the given string, but that's how it is
        private readonly HashSet<char> _allowedChars =
            new HashSet<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-");

        [TestMethod]
        [DataRow(-2)]
        [DataRow(-7)]
        public void Constructor_InvalidTokenLength_ShouldThrow(int length)
        {
            var configuration = GetMockConfigurationObject(length);

            Should.Throw<ArgumentOutOfRangeException>(() => new TokenGenerator(new RNGCrypto(), configuration));
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(7)]
        [DataRow(298)]
        public void GetUniqueKey_GenerateTokenOfCorrectLength(int length)
        {
            var configuration = GetMockConfigurationObject(length);
            var generator = new TokenGenerator(new RNGCrypto(), configuration);

            var token = generator.GetUniqueKey();

            token.ShouldNotBeNull();
            token.Length.ShouldBe(length);
        }

        [TestMethod]
        [DataRow(500000)]
        public void GetUniqueKey_ShouldReturnOnlyAllowedChars(int length)
        {
            var configuration = GetMockConfigurationObject(length);
            var generator = new TokenGenerator(new RNGCrypto(), configuration);

            var token = generator.GetUniqueKey();

            token.ShouldAllBe(x => _allowedChars.Contains(x));
        }

        private IConfiguration GetMockConfigurationObject(int tokenLength)
        {
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(x => x.Value).Returns(tokenLength.ToString());
            
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("ShortURLStringLength")).Returns(configurationSection.Object);

            return configuration.Object;
        }
    }
}
