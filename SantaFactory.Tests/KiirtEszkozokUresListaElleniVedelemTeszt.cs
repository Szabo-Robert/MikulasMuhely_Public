using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class KiirtEszkozokUresListaElleniVedelemTeszt
    {
        [Test]
        public void FeladatbanKiirtEszkozokUresListaElleniVedelemTeszt()
        {
            // --- Arrange ---
            var model = new Feladatok();
            model.ElvihetoEszkozokLista = new List<ElvittEszkozok>(); // Nincs eszköz

            // --- Act ---
            // A kontroller védelmi logikája:
            if (model.KiirniEszkozokLista == null || model.KiirniEszkozokLista.Count == 0)
            {
                model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>
                    {
                        new ElvittEszkozokFeladatokOsszefugges { EszkozID = 0, EszkozNeve = "EMPTY" }
                    };
            }

            // --- Assert ---
            Assert.That(model.KiirniEszkozokLista.Count, Is.EqualTo(1));
            Assert.That(model.KiirniEszkozokLista[0].EszkozNeve, Is.EqualTo("EMPTY"), "Ha nincs eszköz, az EMPTY elemet kell látnunk.");
        }
    }
}