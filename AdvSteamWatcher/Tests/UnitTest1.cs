using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(2, 2, ExpectedResult = 4)]
        [TestCase(3, 3, ExpectedResult = 6)]
        [TestCase(4, 4, ExpectedResult = 8)]
        public int SamplePassSumTest(int a, int b)
        {
            return a + b;
        }

        [Test]
        [TestCase(3, 2, ExpectedResult = 4)]
        [TestCase(3, 2, ExpectedResult = 6)]
        [TestCase(3, 2, ExpectedResult = 8)]
        public int SampleFailSumTest(int a, int b)
        {
            return a + b;
        }
    }
}