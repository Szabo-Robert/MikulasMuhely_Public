using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class MunkalapLetrehozoTesztelese
    {
        [Test] // nUnit-ban [Fact] helyett [Test] van
        public void FilterMunkalapok_ForAlkalmazott_CsakASajátMunkalapokJelenjenekMeg()
        {
            // --- 1. Arrange (Előkészítés) ---
            var currentUserId = 10;

            // Teszt adatok (mintha az adatbázisból jönnének)
            var munkalapok = new List<Munkalapok>
            {
                new Munkalapok { ID = 1, UserID = 10, Magyjegyzes = "Saját munka" },
                new Munkalapok { ID = 2, UserID = 10, Magyjegyzes = "Másik saját" },
                new Munkalapok { ID = 3, UserID = 99, Magyjegyzes = "Idegen munka" }
            }.AsQueryable();

            // --- 2. Act (Végrehajtás) ---
            // Itt ugyanazt a logikát használjuk, amit a képeden láttam
            var result = munkalapok.Where(x => x.UserID == currentUserId).ToList();

            // --- 3. Assert (Ellenőrzés) ---
            // nUnit-os Assert formátum
            Assert.That(result.Count, Is.EqualTo(2), "Az alkalmazottnak pontosan 2 saját munkalapot kellene látnia.");
            Assert.That(result.All(x => x.UserID == currentUserId), Is.True, "Minden talált munkalapnak a sajátjának kell lennie.");
        }

        [Test]
        public void FilterMunkalapok_ForAdmin_MindenFelvittMunkalapMegjelenik()
        {
            // Arrange
            var munkalapok = new List<Munkalapok>
            {
                new Munkalapok { ID = 1, UserID = 10 },
                new Munkalapok { ID = 2, UserID = 99 }
            }.AsQueryable();

            // Act
            // Admin esetén nem szűrünk UserID-ra (ahogy a kódodban volt)
            var result = munkalapok.ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2), "Az adminnak minden munkalapot látnia kell.");
        }
    }
}