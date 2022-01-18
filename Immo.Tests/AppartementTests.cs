using NUnit.Framework;
using Immo.Models;

namespace Immo.Tests
{
    [TestFixture]
    [SetCulture("nl-BE")]
    public class AppartementTests
    {
        [Test]
        public void ToString_Returns_CorrectOutput()
        {
            var subject = new Appartement(420, 142, "steenstraat 69", 1969, 86);

            Assert.AreEqual("Appartement - Huurprijs: € 420,00", subject.ToString());
        }

        [Test]
        public void Overzicht_Returns_CorrectOutput()
        {
            var subject = new Appartement(420, 142, "steenstraat 69", 1969, 86);
            string control =
                "Appartement\n" +
                "Oppervlakte: 142\n" +
                "Adres: steenstraat 69\n" +
                "Prijs: € 420,00\n" +
                "Bouwjaar: 1969\n" +
                "Bewoonbare oppervlakte: 86\n" +
                "Huurprijs: € 420,00\n";

            Assert.AreEqual(control, subject.Overzicht());
        }
    }
}
