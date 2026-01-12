using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class CsakAFeladatbanKiirtEszkozokMenteseTeszt
    {
        [Test]
        public void CsakAFeladatbanKiirtEszkozokMentese()
        {
            // --- Arrange ---
            var feladat = new Feladatok { ID = 100 };
            feladat.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>
            {
                new ElvittEszkozokFeladatokOsszefugges { EszkozID = 1, EszkozKivalasztva = true },  // Ki lett választva
                new ElvittEszkozokFeladatokOsszefugges { EszkozID = 2, EszkozKivalasztva = false } // NEM lett kiválasztva
            };

            var mentendoKapcsolatok = new List<Feladat_ElvittEszkoz_ID>();

            // --- Act ---
            // A kontroller mentési logikája:
            foreach (var item in feladat.KiirniEszkozokLista)
            {
                if (item.EszkozKivalasztva)
                {
                    mentendoKapcsolatok.Add(new Feladat_ElvittEszkoz_ID
                    {
                        ElvittEszkozID = item.EszkozID,
                        FeladatID = feladat.ID
                    });
                }
            }

            // --- Assert ---
            Assert.That(mentendoKapcsolatok.Count, Is.EqualTo(1), "Csak a kiválasztott eszközt szabadna hozzáadni az adatbázishoz.");
            Assert.That(mentendoKapcsolatok[0].ElvittEszkozID, Is.EqualTo(1), "A mentett eszköznek az 1-es ID-júnak kell lennie.");
        }
    }
}