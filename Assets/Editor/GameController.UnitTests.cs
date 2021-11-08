using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests
{
    public class GameController_UnitTests
    {
        GameController gameController = new GameController();


        // A Test behaves as an ordinary method
        [Test]
        public void gamecSimplePasses()
        {
            // Use the Assert class to test conditions
            gameController.mousePos.x = 0;
            gameController.mousePos.y = 0;
            //GameController.

        }



        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator gamecWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
