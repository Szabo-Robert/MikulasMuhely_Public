using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class RegisztraciosJelszoTeszt
    {
        [Test]
        public void Regisztracio_JelszoEsAktivaloKodEgyediGeneralasa()
        {
            // --- 1. Arrange (Előkészítés) ---
            var felhasznalo = new Users
            {
                Elerhetosegek = new Elerhetosegek()
                {
                    Email = "teszt@santa.hu"
                },
                Jelszo = "SzuperTitkos123",
                JelszoMegerosites = "SzuperTitkos123",
                GDPR = true
            };

            // --- 2. Act (Végrehajtás - a kontroller logikájának szimulálása) ---
            // GUID generálás szimulálása
            felhasznalo.AktivaloKod = Guid.NewGuid();

            // Hash-elés szimulálása (feltételezve, hogy a Varazslas.Hash működik)
            string eredetiJelszo = felhasznalo.Jelszo;
            felhasznalo.Jelszo = Varazslas.Hash(felhasznalo.Jelszo);

            // --- 3. Assert (Ellenőrzés) ---
            Assert.That(felhasznalo.AktivaloKod, Is.Not.EqualTo(Guid.Empty), "Az aktiváló kódnak (GUID) generálódnia kell!");
            Assert.That(felhasznalo.AktivaloKod.ToString().Length, Is.GreaterThan(0), "A GUID nem lehet üres.");

            Assert.That(felhasznalo.Jelszo, Is.Not.EqualTo(eredetiJelszo), "A jelszónak meg kell változnia a hash-elés után (biztonság)!");
        }
    }
}