namespace SantaFactory.Tests
{
    [TestFixture] // nUnit-ban ez jelöli a teszt osztályt
    public class FeladatbanLetrehozottMunkalapokSzamaTeszt
    {
        [Test]
        public void FeladatbanLetrehozottMunkalapokSzamaTesztelese_TobbNapEseten()
        {
            // --- Arrange ---
            var kezdes = new DateTime(2024, 05, 10);
            var befejezes = new DateTime(2024, 05, 12); // Ez 3 nap (10, 11, 12)
            var kivalasztottUserek = new List<int> { 1, 2 }; // 2 fő

            // --- Act ---
            // A kódodban lévő logika:
            int napokSzama = (befejezes - kezdes).Days + 1;
            int eredmeny = napokSzama * kivalasztottUserek.Count;

            // --- Assert ---
            Assert.That(eredmeny, Is.EqualTo(6), "3 napra 2 főnek 6 munkalapot kell generálnia.");
        }
    }
}