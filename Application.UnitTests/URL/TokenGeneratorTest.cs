using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniURL.Application.URL;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniURL.Application.Tests.URL
{
    [TestClass]
    public class TokenGeneratorTest
    {
        [TestMethod]
        [DataRow(2)]
        [DataRow(7)]
        [DataRow(298)]
        public void GetUniqueKey_GenerateTokenOfCorrectLength(int length)
        {
            var token = TokenGenerator.GetUniqueKey(length);

            token.ShouldNotBeNull();
            token.Length.ShouldBe(length);
        }

        [TestMethod]
        [DataRow(-2)]
        [DataRow(-7)]
        public void GetUniqueKey_InvalidInput_ShouldThrow(int length)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => TokenGenerator.GetUniqueKey(length));
        }
    }
}
