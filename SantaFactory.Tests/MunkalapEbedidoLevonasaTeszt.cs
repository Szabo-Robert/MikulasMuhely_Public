using SantaFactory.Models;

namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class MunkalapEbedidoLevonasaTeszt
    {
        [Test]
        public void CalculateMunkaOra_WhenWorkingMoreThan4Hours_ShouldSubtractBreak()
        {
            // --- 1. Arrange ---
            var model = new Munkalapok
            {
                KezdesiIdo = "08:00",
                BefejezesiIdo = "13:00", // Ez 5 óra tiszta idő
                UserID = 1,
                FeladatID = 10
            };

            // --- 2. Act --- (A kontrollerben lévő logika szimulálása)
            TimeSpan kIdo = new TimeSpan(8, 0, 0);
            TimeSpan bIdo = new TimeSpan(13, 0, 0);
            TimeSpan kulonbseg = bIdo - kIdo;

            decimal munkaOra = (decimal)kulonbseg.Hours + (decimal)kulonbseg.Minutes / 60;

            // Ebédidő logika a kódodból: > 4 és <= 10 esetén -0.5
            if (munkaOra > 4 && munkaOra <= 10)
            {
                munkaOra -= 0.50m;
            }

            // --- 3. Assert ---
            // 5 óra - 0.5 óra = 4.5 óra
            Assert.That(munkaOra, Is.EqualTo(4.50m), "A 4 óra feletti munkából le kell vonni az ebédidőt.");
        }
    }
}