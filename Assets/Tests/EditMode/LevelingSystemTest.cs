using NUnit.Framework;

namespace Tests.EditMode
{
    public class LevelingSystemTest
    {
        [Test]
        public void TestingAddingExperience()
        {
            var addexp = new LevelSystem();
    
            addexp.AddExp(150);
    
            Assert.AreEqual(150, addexp.GetExp());
        }
    }
}
