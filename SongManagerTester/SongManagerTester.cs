using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SongManagerTester
{
    [TestClass]
    public class SongManagerTester
    {
        [TestMethod]
        public void TestMethod1()
        {
            Pleer.Models.SongManager _songManager = Pleer.Models.SongManager.Instance(); // обращение к songManager (через него методы испоьзовать)
            Assert.AreEqual(0, 0); // проверка теста
        }
    }
}
