using SolicitorSearch.Tests.HtmlTestData;
using SolicitorSearch.Utilities.Scraping;

namespace SolicitorSearch.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Theory]
        [TestCase(ResultItems.Whitechapel, "QualitySolicitors - Whitechapel", "02038117345", "402 – 404 Commercial Road, Whitechapel, London E1 0LB", "https://www.qualitysolicitors.com/solicitors-whitechapel")]
        [TestCase(ResultItems.Lyons, "Lyons Davidson", "03442510070", "43 Queen Square, Bristol,  BS1 4QP", null)]
        public void ExtractSolicitorInfoFromResultItem(string resultItem, string expectedName, string expectedPhone, string expectedAddress, string? expectedWebsite)
        {
			var scraper = new Scraper();
            var solicitor = scraper.ExtractSolocitors(resultItem)[0];

            Assert.That(solicitor.Name, Is.EqualTo(expectedName));
            Assert.That(solicitor.PhoneNumber, Is.EqualTo(expectedPhone));
            Assert.That(solicitor.Address, Is.EqualTo(expectedAddress));
			Assert.That(solicitor.Website, Is.EqualTo(expectedWebsite));
        }
    }
}
