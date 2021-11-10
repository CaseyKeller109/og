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
        

        // A Test behaves as an ordinary method
        [Test]
        public void PlaceGoStone_01B_00W_10B()
        //public void PlaceGoStone_12B_11W_21B()
        {
            InitialSetup();




            PlayStoneIfValid(0, 1, StoneColor.black);
            Assert.IsTrue(FindStone(0, 1, StoneColor.black) != null);


            PlayStoneIfValid(0, 0, StoneColor.white);
            //Assert.IsTrue(FindStone(0, 1, StoneColor.black) != null);
            //Assert.IsTrue(FindStone(0, 0, StoneColor.white) != null);


            PlayStoneIfValid(1, 0, StoneColor.black);
            //Assert.IsTrue(FindStone(0, 1, StoneColor.black) != null);
            //Assert.IsTrue(FindStone(0, 0, StoneColor.white) == null);
            //Assert.IsTrue(FindStone(1, 0, StoneColor.black) != null);




            //GameController.GoStone stone_12B = new GameController.GoStone { x = 0, y = 1, stoneColor = GameController.StoneColor.black };

            //GameController.ValidPlayData validPlayData = new GameController.ValidPlayData();
            //validPlayData = gameController.ValidPlayCheck(stone_12B);
            //if (validPlayData.isValidPlay)
            //{
            //    gameController.PlaceGoStone(stone_12B, validPlayData.groupStonesToKill);
            //}
            







            //Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
            //                                                                      s.y == 1 &&
            //                                                                      s.stoneColor == StoneColor.black));





            //gameController.currentPlayerColor = GameController.StoneColor.white;
            //GameController.GoStone stone_11W = new GameController.GoStone { x = 0, y = 0, stoneColor = GameController.StoneColor.white };

            //validPlayData = gameController.ValidPlayCheck(stone_11W);
            //if (validPlayData.isValidPlay)
            //{
            //    gameController.PlaceGoStone(stone_11W, validPlayData.groupStonesToKill);
            //}
            


            //Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
            //                                                                      s.y == 0 &&
            //                                                                      s.stoneColor == GameController.StoneColor.white));



            //gameController.currentPlayerColor = GameController.StoneColor.black;
            //GameController.GoStone stone_21B = new GameController.GoStone { x = 1, y = 0, stoneColor = GameController.StoneColor.black };

            ////GameController.ValidPlayData validPlayData = new GameController.ValidPlayData();
            //validPlayData = gameController.ValidPlayCheck(stone_21B);
            //gameController.PlaceGoStone(stone_21B, validPlayData.groupStonesToKill);
            ////gameController.PlaceGoStone(stone_21B, validPlayData.groupStonesToKill, sensorStone, genericStoneObject);



            //Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
            //                                                                      s.y == 1 &&
            //                                                                      s.stoneColor == GameController.StoneColor.black));

            //Assert.IsTrue(null == gameController.stonePosHistory.Last().Find(s => s.x == 0 &&
            //                                                                      s.y == 0 &&
            //                                                                      s.stoneColor == GameController.StoneColor.white));

            //Assert.IsTrue(null != gameController.stonePosHistory.Last().Find(s => s.x == 1 &&
            //                                                                      s.y == 0 &&
            //                                                                      s.stoneColor == GameController.StoneColor.black));

        }


        public void InitialSetup()
        {
            gameController.sensorStone = GameObject.Instantiate(Resources.Load("StoneSensor") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
            gameController.genericStoneObject = Resources.Load("Stone") as GameObject;

            gameController.whiteTextObject = new GameObject();
            gameController.blackTextObject = new GameObject();
            gameController.whiteTextObject.AddComponent<Text>();
            gameController.blackTextObject.AddComponent<Text>();

            gameController.stonePosHistory.Add(new List<GoStone>());

            //gameController.currentPlayerColor = GameController.StoneColor.black;
        }

        public void PlayStoneIfValid(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            GoStone newStone = new GoStone { x = xCoordinate,
                                             y = yCoordinate,
                                             stoneColor = stoneColor };

            ValidPlayData validPlayData = new ValidPlayData();
            validPlayData = gameController.ValidPlayCheck(newStone);
            if (validPlayData.isValidPlay)
            {
                gameController.PlaceGoStone(newStone, validPlayData.groupStonesToKill);
            }
        }

        public GoStone FindStone(int xCoordinate, int yCoordinate,  StoneColor stoneColor)
        {
            GoStone foundStone = gameController.stonePosHistory.Last().Find(s => s.x == xCoordinate &&
                                                                                 s.y == yCoordinate &&
                                                                                 s.stoneColor == stoneColor);
            return foundStone;
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
