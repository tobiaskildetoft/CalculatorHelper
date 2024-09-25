namespace CalculatorHelper.Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var result = CalculatorHelper.Calculate("2+2");

            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        [TestCase("1 + 2(3+4)",15)]
        [TestCase("", 0)]
        [TestCase("1.2 + 1,3", 2.5f)]
        [TestCase("3 + 4 * 5", 23)]
        [TestCase("3 * 4 + 5", 17)]
        [TestCase("-5 -- 2", -3)]
        [TestCase("-(-5 -- 2)", 3)]
        [TestCase("1+((2+3))", 6)]
        [TestCase("1+-1", 0)]
        [TestCase("1++1", 2)]
        [TestCase("1-+1", 0)]
        public void TestValidCases(string input, float expectedResult)
        {
            var result = CalculatorHelper.Calculate(input);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("1+((2+3)")]
        [TestCase("1+(2+3))")]
        [TestCase("--5")]
        [TestCase("---5")]
        [TestCase("2+*3")]
        public void TestInvalidCases(string input)
        {
            Assert.Throws<ArgumentException>(() => CalculatorHelper.Calculate(input));
        }
    }
}