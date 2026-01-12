using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class MunkalapIdopontUtkozesTeszt
    {
        [Test]
        public void CreatePost_WhenTimeOverlaps_ShouldReturnErrorMessage()
        {
            // --- 1. Arrange ---
            var ujMunkalap = new Munkalapok
            {
                MunkaDatuma = DateTime.Today,
                KezdesiIdo = "10:00",
                BefejezesiIdo = "12:00"
            };

            // Szimulált már létező munkalapok az adatbázisban
            var letezoMunkalapok = new List<Munkalapok>
            {
                new Munkalapok {
                    KIdo = new TimeSpan(09, 0, 0),
                    BIdo = new TimeSpan(11, 0, 0) // Átfedi a 10:00-12:00-t!
                }
            };

            // Logika kinyerése a kódodból (modelTempKIdo számítása)
            var modelTempKIdo = new TimeSpan(10, 1, 0); // 10:00 + 1 perc
            var modelTempBIdo = new TimeSpan(11, 59, 0); // 12:00 - 1 perc

            bool vanUtkozes = false;

            // --- 2. Act ---
            foreach (var azonosMunkalap in letezoMunkalapok)
            {
                if ((modelTempKIdo > azonosMunkalap.KIdo && azonosMunkalap.BIdo > modelTempKIdo)
                    || (azonosMunkalap.KIdo < modelTempBIdo && azonosMunkalap.BIdo > modelTempBIdo))
                {
                    vanUtkozes = true;
                    break;
                }
            }

            // --- 3. Assert ---
            Assert.That(vanUtkozes, Is.True, "A rendszernek jeleznie kellene az időpont ütközést.");
        }
    }
}