using NUnit.Framework;
using Immo.Models;

namespace Immo.Tests
{
    [TestFixture]
    [SetCulture("nl-BE")]
    public class EigendomTests
    {
        [Test]
        public void ToString_Returns_CorrectOutput()
        {
            var subject = new Eigendom(42000, 58, "steenstraat 44");

            Assert.AreEqual("Eigendom - Prijs: € 42.000,00", subject.ToString());
        }

        [Test]
        public void Prijs_ThrowsException_WhenNegative()
        {
            Assert.Throws<CustomException>(delegate
            {
                var subject = new Eigendom(-1, 53, "adres");
            });
        }

        [Test]
        public void Overzicht_Returns_CorrectOutput()
        {
            var subject = new Eigendom(42000, 69, "steenstraat 44");
            string control =
                "Eigendom\n" +
                "Oppervlakte: 69\n" +
                "Adres: steenstraat 44\n" +
                "Prijs: € 42.000,00\n";

            Assert.AreEqual(control, subject.Overzicht()); ;
        }

        [TestCase("Adres1", "Adres1", true)]
        [TestCase("Adres1", "Adres2", false)]
        public void Equals_Returns_CorrectOutput(string adres1, string adres2, bool control)
        {
            var subject1 = new Eigendom(42000, 54, adres1);
            var subject2 = new Eigendom(42000, 55, adres2);

            Assert.AreEqual(control, subject1.Equals(subject2));
        }
    }
}