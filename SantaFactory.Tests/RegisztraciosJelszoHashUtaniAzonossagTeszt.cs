using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class RegisztraciosJelszoHashUtaniAzonossagTeszt
    {
        [Test]
        public void Regisztracio_JelszoÉsJelszoMegerosites_HashelesUtanEgyezniukKell()
        {
            // --- 1. Arrange ---
            var felhasznalo = new Users
            {
                Jelszo = "TitkosJelszo123",
                JelszoMegerosites = "TitkosJelszo123" // Itt szándékosan egyformára állítjuk
            };

            // --- 2. Act ---
            // Megvizsgáljuk az egyezőséget a hash-elés ELŐTT
            bool jelszavakEgyeznek = felhasznalo.Jelszo == felhasznalo.JelszoMegerosites;

            // Ezután jönne a kódodban lévő hash-elés
            if (jelszavakEgyeznek)
            {
                felhasznalo.Jelszo = Varazslas.Hash(felhasznalo.Jelszo);
                felhasznalo.JelszoMegerosites = Varazslas.Hash(felhasznalo.JelszoMegerosites);
            }

            // --- 3. Assert ---
            Assert.Multiple(() =>
            {
                Assert.That(jelszavakEgyeznek, Is.True, "A két jelszónak egyeznie kell a regisztrációhoz.");
                Assert.That(felhasznalo.Jelszo, Is.EqualTo(felhasznalo.JelszoMegerosites), "A hash-elt változatoknak is egyezniük kell.");
                Assert.That(felhasznalo.Jelszo, Is.Not.EqualTo("TitkosJelszo123"), "Az adatbázisba már csak a hash-elt jelszó kerülhet!");
            });
        }
    }
}