using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test2
    {
        [Test]
        public void TestIncrement()
        {
            var addexp = new LevelSystem();
        
            addexp.AddExp(150);

            Assert.AreEqual(150, addexp.GetExp());
        }
    }
}