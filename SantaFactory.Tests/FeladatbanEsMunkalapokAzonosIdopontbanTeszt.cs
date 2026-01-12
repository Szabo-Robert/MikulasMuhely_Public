namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class FeladatbanEsMunkalapokAzonosIdopontbanTeszt
    {
        [Test]
        public void FeladatbanEsMunkalapokAzonosIdopontban_EzKonfliktustOkoz()
        {
            // --- Arrange ---
            // Meglévő munkalap: 08:00 - 12:00
            var letezoMunkalapKido = new TimeSpan(8, 0, 0);
            var letezoMunkalapBido = new TimeSpan(12, 0, 0);

            // Új tervezett feladat: 10:00 - 14:00 (Átfedés van!)
            var ujFeladatKido = new TimeSpan(10, 0, 0);
            var ujFeladatBIdo = new TimeSpan(14, 0, 0);

            // --- Act ---
            // A kódodból kimentett logika:
            bool vanUtkozes = (letezoMunkalapKido > ujFeladatKido && ujFeladatBIdo > letezoMunkalapKido)
                           || (letezoMunkalapKido < ujFeladatKido && letezoMunkalapBido > ujFeladatKido);

            // --- Assert ---
            Assert.That(vanUtkozes, Is.True, "Az időpontok ütköznek, a rendszernek jeleznie kellene!");
        }
    }
}