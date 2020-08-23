using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.Common;
using Shouldly;
using System;
using System.Collections.Generic;

namespace MiniURL.Application.Tests.URL
{
    [TestClass]
    public class TokenGeneratorTest
    {
        // We are now dependent on the implementation and the given string, but that's how it is
        private readonly HashSet<char> _allowedChars =
            new HashSet<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-");
        private readonly TokenGenerator _generator = new TokenGenerator(new RNGCrypto());

        [TestMethod]
        [DataRow(2)]
        [DataRow(7)]
        [DataRow(298)]
        public void GetUniqueKey_GenerateTokenOfCorrectLength(int length)
        {
            var token = _generator.GetUniqueKey(length);

            token.ShouldNotBeNull();
            token.Length.ShouldBe(length);
        }

        [TestMethod]
        [DataRow(-2)]
        [DataRow(-7)]
        public void GetUniqueKey_InvalidInput_ShouldThrow(int length)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => _generator.GetUniqueKey(length));
        }

        [TestMethod]
        [DataRow(500000)]
        public void GetUniqueKey_ShouldReturnOnlyAllowedChars(int length)
        {
            var token = _generator.GetUniqueKey(length);

            token.ShouldAllBe(x => _allowedChars.Contains(x));
        }
    }
}
