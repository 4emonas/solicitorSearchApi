using SolicitorSearch.Utilities.Helpers;

namespace SolicitorSearch.Tests.Helpers
{
    public class StringHelperTests
    {
        [Theory]
        [TestCase("402 – 404 Commercial Road, Whitechapel, London E1 0LB", "402 – 404 Commercial Road, Whitechapel, London E1 0LB")]
        [TestCase("18 Portland Square, Bristol, &nbsp;BS2 8SJ", "18 Portland Square, Bristol,  BS2 8SJ")]
        public void Normalise_ShouldReturnExpectedResult(string input, string expectedOutput)
        {
            var result = input.Normalise();
            Assert.That(result, Is.EqualTo(expectedOutput));
        }
    }
}
