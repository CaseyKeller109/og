using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using static GameController;

namespace Tests
{
    public class GameController_UnitTests
    {
        GameController gameController = new GameController();
        



        //public GameObject sensorStone;
        //public Transform sensorStoneTrans;



        // A Test behaves as an ordinary method
        [Test]
        public void PlaceGoStone_12B_11W_21B()
        {

            //todo make functions for playing stones
            //GameObject whiteTextObject;
            //GameObject blackTextObject;

            //todo set these like gameController.sensorStoneObject etc
            GameObject sensorStoneObject = Resources.Load("StoneSensor") as GameObject;
            GameObject sensorStone = GameObject.Instantiate(sensorStoneObject, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject genericStoneObject = Resources.Load("Stone") as GameObject;
            //GameObject whiteTextObject = new GameObject();
            //GameObject blackTextObject = new GameObject();

            gameController.whiteTextObject = new GameObject();
            gameController.blackTextObject = new GameObject();
            gameController.whiteTextObject.AddComponent<Text>();
            gameController.blackTextObject.AddComponent<Text>();

            //gameController.coroutineHandler = new GameObject("_coroutineHandler");

            gameController.stonePosHistory.Add(new List<GoStone>());

            gameController.currentPlayerColor = GameController.StoneColor.black;

            GameController.GoStone stone_12B = new GameController.GoStone { x = 0, y = 1, stoneColor = GameController.StoneColor.black };

            GameController.ValidPlayData validPlayData = new GameController.ValidPlayData();
            validPlayData = gameController.ValidPlayCheck(stone_12B);
            gameController.PlaceGoStone(stone_12B, validPlayData.groupStonesToKill, sensorStone, genericStoneObject);



            //Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
            //                                                                      s.y == 1 &&
            //                                                                      s.stoneColor == GameController.StoneColor.black));



            gameController.currentPlayerColor = GameController.StoneColor.white;
            GameController.GoStone stone_11W = new GameController.GoStone { x = 0, y = 0, stoneColor = GameController.StoneColor.white };

            //GameController.ValidPlayData validPlayData = new GameController.ValidPlayData();
            validPlayData = gameController.ValidPlayCheck(stone_11W);
            gameController.PlaceGoStone(stone_11W, validPlayData.groupStonesToKill, sensorStone, genericStoneObject);


            Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
                                                                                  s.y == 0 &&
                                                                                  s.stoneColor == GameController.StoneColor.white));



            gameController.currentPlayerColor = GameController.StoneColor.black;
            GameController.GoStone stone_21B = new GameController.GoStone { x = 1, y = 0, stoneColor = GameController.StoneColor.black };

            //GameController.ValidPlayData validPlayData = new GameController.ValidPlayData();
            validPlayData = gameController.ValidPlayCheck(stone_21B);
            gameController.PlaceGoStone(stone_21B, validPlayData.groupStonesToKill, sensorStone, genericStoneObject);



            Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
                                                                                  s.y == 1 &&
                                                                                  s.stoneColor == GameController.StoneColor.black));

            Assert.IsTrue(null == gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
                                                                                  s.y == 0 &&
                                                                                  s.stoneColor == GameController.StoneColor.white));

            Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 1 &&
                                                                                  s.y == 0 &&
                                                                                  s.stoneColor == GameController.StoneColor.black));

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
