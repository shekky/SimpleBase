﻿/*
     Copyright 2014 Sedat Kapanoglu

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Text;
using SimpleBase32;
using MbUnit.Framework;
using System.Diagnostics;

namespace Base32Test
{
    [TestFixture]
    class CrockfordTest
    {
        private static string[][] testData = new[]
        {
            new[] { "", "" },
            new[] { "f", "CR" },
            new[] { "fo", "CSQG" },
            new[] { "foo", "CSQPY" },
            new[] { "foob", "CSQPYRG" },
            new[] { "fooba", "CSQPYRK1" },
            new[] { "foobar", "CSQPYRK1E8" },
            new[] { "1234567890123456789012345678901234567890", "64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE9G64S36D1N6RVKGE9G" },            
        };

        [Test]
        [Factory("testData")]
        void Encode_ReturnsExpectedValues(string input, string expectedOutput)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            string result = Base32.Crockford.Encode(bytes, padding: false);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [Factory("testData")]
        void Decode_ReturnsExpectedValues(string expectedOutput, string input)
        {
            byte[] bytes = Base32.Crockford.Decode(input);
            string result = Encoding.ASCII.GetString(bytes);
            Assert.AreEqual(expectedOutput, result);
            bytes = Base32.Crockford.Decode(input.ToLowerInvariant());
            result = Encoding.ASCII.GetString(bytes);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        void Decode_InvalidInput_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Base32.Crockford.Decode("[];',m."));
        }

        [Test]
        [Row("O0o", "000")]
        [Row("Ll1", "111")]
        [Row("I1i", "111")]
        void Decode_CrockfordChars_DecodedCorrectly(string equivalent, string actual)
        {
            var expectedResult = Base32.Crockford.Decode(actual);
            var result = Base32.Crockford.Decode(equivalent);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        void Encode_NullBytes_ThrowsArgumentNullException([Column(true, false)]bool padding)
        {
            Assert.Throws<ArgumentNullException>(() => Base32.Crockford.Encode(null, padding));
        }

        [Test]
        void Decode_NullString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Base32.Crockford.Decode(null));
        }
    }
}