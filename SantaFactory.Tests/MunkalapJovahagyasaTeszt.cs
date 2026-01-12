using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class MunkalapJovahagyasaTeszt
    {
        [Test]
        public void FilterMunkalapok_CsakAJovahagyottakatKellMutassa()
        {
            // --- 1. Arrange (Előkészítés) ---
            var tesztAdatok = new List<Munkalapok>
    {
        new Munkalapok { ID = 1, Magyjegyzes = "Jóváhagyott 1", JovahagyvaCB = true },
        new Munkalapok { ID = 2, Magyjegyzes = "Nem jóváhagyott", JovahagyvaCB = false },
        new Munkalapok { ID = 3, Magyjegyzes = "Jóváhagyott 2", JovahagyvaCB = true }
    }.AsQueryable();

            // --- 2. Act (Végrehajtás) ---
            // A logikát szimuláljuk, amit a kontrollerben használsz
            var result = tesztAdatok.Where(x => x.JovahagyvaCB).ToList();

            // --- 3. Assert (Ellenőrzés) ---
            Assert.That(result.Count, Is.EqualTo(2), "A listának csak 2 elemet szabadna tartalmaznia.");
            Assert.That(result.Any(x => x.ID == 2), Is.False, "A 2-es ID-jú (nem jóváhagyott) elemnek nem szabadna szerepelnie a listában.");
            Assert.That(result.All(x => x.JovahagyvaCB), Is.True, "Minden visszakapott elemnek jóváhagyottnak kell lennie.");
        }
    }
}