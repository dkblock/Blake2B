using Blake2B.Core;
using System;
using Xunit;

namespace Blake2B.Tests
{
    public class Blake2BTests
    {
        private readonly Blake2B_Algorithm _blake;

        public Blake2BTests()
        {
            _blake = new Blake2B_Algorithm();
        }

        [Fact]
        public void ComputeHash_ReturnsCorrectHash_1()
        {
            var testString = "The quick brown fox jumps over the lazy dog";

            var expected = "A8ADD4BDDDFD93E4877D2746E62817B116364A1FA7BC148D95090BC7333B3673F82401CF7AA2E4CB1ECD90296E3F14CB5413F8ED77BE73045B13914CDCD6A918";
            var actual = _blake.ComputeHash(testString);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void ComputeHash_ReturnsCorrectHash_2()
        {
            var testString = "abc";

            var expected = "BA80A53F981C4D0D6A2797B69F12F6E94C212F14685AC4B74B12BB6FDBFFA2D17D87C5392AAB792DC252D5DE4533CC9518D38AA8DBF1925AB92386EDD4009923";
            var actual = _blake.ComputeHash(testString);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void ComputeHash_ReturnsCorrectHash_3()
        {
            var testString = "Hello world";

            var expected = "6ff843ba685842aa82031d3f53c48b66326df7639a63d128974c5c14f31a0f33343a8c65551134ed1ae0f2b0dd2bb495dc81039e3eeb0aa1bb0388bbeac29183";
            var actual = _blake.ComputeHash(testString);

            Assert.True(AreEqual(expected, actual));
        }

        [Fact]
        public void ComputeHash_ReturnsTheSameHashesOnTheSameInputs()
        {
            var testString = Guid.NewGuid().ToString();

            var firstHash = _blake.ComputeHash(testString);
            var secondHash = _blake.ComputeHash(testString);

            Assert.Equal(firstHash, secondHash);
        }

        private bool AreEqual(string expected, string actual)
        {
            return expected.ToLower() == actual.ToLower().Replace(" ", "");
        }
    }
}
