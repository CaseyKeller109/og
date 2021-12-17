using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using static Assets.Scripts.GameController;
using static Assets.Scripts.GoFunctions;

namespace Tests
{
    public class GameController_UnitTests
    {
        public Assets.Scripts.GoFunctions goFunctions = new Assets.Scripts.GoFunctions();
        public Assets.Scripts.GameController gameController = new Assets.Scripts.GameController();

        //tests placing at same spot
        [Test]
        public void PlaceGoStone_0_0B_0_0_B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(ScoreCheck(0, 0));

            CheckIfObjectCountCorrect(1);
        }

        [Test]
        public void PlaceGoStone_18_18B_18_18_B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            ScoreCheck(0, 0);

            PlayStoneIfValid(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            CheckIfObjectCountCorrect(1);

        }

        [Test]
        public void PlaceGoStone_0_0W_0_0_W()
        {
            InitialSetup();

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            CheckIfObjectCountCorrect(1);

        }

        [Test]
        public void PlaceGoStone_18_18W_18_18_W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            CheckIfObjectCountCorrect(1);

        }


        //tests single capture at 0,0
        [Test]
        public void PlaceGoStone_0_1B_0_0W_1_0B()
        {
            InitialSetup();



            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));


            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));


            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));


            CheckIfObjectCountCorrect(2);



            
        }

        [Test]
        public void PlaceGoStone_1_0B_0_0W_0_1B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void PlaceGoStone_0_1W_0_0B_1_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void PlaceGoStone_1_0W_0_0B_0_1W()
        {
            InitialSetup();

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }


        //tests double capture at 0,0 and 1,0
        [Test]
        public void PlaceGoStone_0_1B_0_0W_1_1B_1_0W_2_0B()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void PlaceGoStone_2_0B_1_0W_1_1B_0_0W_0_1B()
        {
            InitialSetup();

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void PlaceGoStone_0_1W_0_0B_1_1W_1_0B_2_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void PlaceGoStone_2_0W_1_0B_1_1W_0_0B_0_1W()
        {
            InitialSetup();

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }


        //tests single and double capture at 1,0 and 0,1 and 0,2
        [Test]
        public void PlaceGoStone_1_1B_1_0W_2_0B_0_1W_1_2B_0_2W_0_3B_0_0B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 2, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 2, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 3, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(3, 0));

            CheckIfObjectCountCorrect(5);
        }




        //tests single capture at 18,18
        [Test]
        public void PlaceGoStone_18_17B_18_18W_17_18B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void PlaceGoStone_17_18B_18_18W_18_17B()
        {
            InitialSetup();

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void PlaceGoStone_18_17W_18_18B_17_18W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void PlaceGoStone_17_18W_18_18B_18_17W()
        {
            InitialSetup();

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }


        //tests double capture at 0,0 and 1,0
        [Test]
        public void PlaceGoStone_18_17B_18_18W_17_17B_17_18W_16_18B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void PlaceGoStone_16_18B_17_18W_17_17B_18_18W_18_17B()
        {
            InitialSetup();

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void PlaceGoStone_18_17W_18_18B_17_17W_17_18B_16_18W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(16, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void PlaceGoStone_16_18W_17_18B_17_17W_18_18B_18_17W()
        {
            InitialSetup();

            PlayStoneIfValid(16, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }


        //tests single and double capture at 17,18 and 18,17 and 18,16
        [Test]
        public void PlaceGoStone_17_17B_17_18W_16_18B_18_17W_17_16B_18_16W_18_15B_18_18B()
        {
            InitialSetup();

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(17, 16, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 16, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 15, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(3, 0));

            CheckIfObjectCountCorrect(5);
        }





        //todo make sure it's testing all parts of GoStone

        //throwing
        //tests single capture at 0,0
        [Test]
        public void ThrowGoStone_0_1B_0_0W_1_0B()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));


            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));


            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);


            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void ThrowGoStone_1_0B_0_0W_0_1B()
        {
            InitialSetup();

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void ThrowGoStone_0_1W_0_0B_1_0W()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void ThrowGoStone_1_0W_0_0B_0_1W()
        {
            InitialSetup();

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);

        }


        //throwing
        //tests double capture at 0,0 and 1,0
        [Test]
        public void ThrowGoStone_0_1B_0_0W_1_1B_1_0W_2_0B()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void ThrowGoStone_2_0B_1_0W_1_1B_0_0W_0_1B()
        {
            InitialSetup();

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void ThrowGoStone_0_1W_0_0B_1_1W_1_0B_2_0W()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(2, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void ThrowGoStone_2_0W_1_0B_1_1W_0_0B_0_1W()
        {
            InitialSetup();

            ThrowStone(2, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }


        //throwing
        //tests single and double capture at 1,0 and 0,1 and 0,2
        [Test]
        public void ThrowGoStone_1_1B_1_0W_2_0B_0_1W_1_2B_0_2W_0_3B_0_0B()
        {
            InitialSetup();

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(1, 2, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 2, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 3, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(3, 0));

            CheckIfObjectCountCorrect(5);
        }



        //throwing
        //tests single capture at 18,18
        [Test]
        public void ThrowGoStone_18_17B_18_18W_17_18B()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void ThrowGoStone_17_18B_18_18W_18_17B()
        {
            InitialSetup();

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void ThrowGoStone_18_17W_18_18B_17_18W()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }

        [Test]
        public void ThrowGoStone_17_18W_18_18B_18_17W()
        {
            InitialSetup();

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(2);
        }


        //throwing
        //tests double capture at 0,0 and 1,0
        [Test]
        public void ThrowGoStone_18_17B_18_18W_17_17B_17_18W_16_18B()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void ThrowGoStone_16_18B_17_18W_17_17B_18_18W_18_17B()
        {
            InitialSetup();

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(2, 0));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void ThrowGoStone_18_17W_18_18B_17_17W_17_18B_16_18W()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(16, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }

        [Test]
        public void ThrowGoStone_16_18W_17_18B_17_17W_18_18B_18_17W()
        {
            InitialSetup();

            ThrowStone(16, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 2));

            CheckIfObjectCountCorrect(3);
        }


        //throwing
        //tests single and double capture at 17,18 and 18,17 and 18,16
        [Test]
        public void ThrowGoStone_17_17B_17_18W_16_18B_18_17W_17_16B_18_16W_18_15B_18_18B()
        {
            InitialSetup();

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(17, 16, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 16, StoneColor.White);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 15, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout(BoardHistory);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(3, 0));

            CheckIfObjectCountCorrect(5);
        }

        //todo rename funcitons with more than just positions

        // simple ko
        [Test]
        public void PlaceGoStone_Ko_0_1B_1_1W_1_0B_2_0W_0_0W_0_0B()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 1));

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsTrue(ScoreCheck(0, 1));

            CheckIfObjectCountCorrect(4);
        }

        [Test]
        public void PlaceGoStone_Ko_0_1W_1_1B_1_0W_2_0B_0_0B_0_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(0, 0));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(1, 0));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(ScoreCheck(1, 0));

            CheckIfObjectCountCorrect(4);
        }




        public void InitialSetup()
        {
            foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Stone"))
            {
                GameObject.DestroyImmediate(fooObj);
            }

            //2todo improve this?
            gameController.sensorStone = new GoStone(new BoardCoordinates(20, 20), StoneColor.Black, GameObject.Instantiate(Resources.Load("Stone") as GameObject));
            gameController.sensorStone.gameObject.GetComponent<MeshCollider>().enabled = false;
            gameController.sensorStone.gameObject.name = "BlackSensorStone";
            gameController.sensorStone.gameObject.layer = 0;

            Assets.Scripts.GameController.genericStoneObject = Resources.Load("Stone") as GameObject;

            gameController.whiteTextObject = new GameObject();
            gameController.blackTextObject = new GameObject();
            gameController.whiteTextObject.AddComponent<Text>();
            gameController.blackTextObject.AddComponent<Text>();

            Assets.Scripts.GoFunctions.BoardHistory = new List<GoBoard>();
            Assets.Scripts.GoFunctions.BoardHistory.Add(new GoBoard());

            PlayerScore.blackScore = 0;
            PlayerScore.whiteScore = 0;
        }

        public void PlayStoneIfValid(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            Assets.Scripts.GoFunctions.Currents.currentGameState = GameState.CanPlaceStone;
            Assets.Scripts.GoFunctions.Currents.currentPlayerColor = stoneColor;
            //GoStone newStone = new GoStone
            //{
            //    coordinates = {
            //    x = xCoordinate,
            //    y = yCoordinate
            //    },
            //    stoneColor = stoneColor
            //};

            BoardCoordinates newStoneCoordinates = new BoardCoordinates
            (
                xCoordinate,
                yCoordinate
            );

            //validPlayData = new ValidPlayData();
            ValidPlayData validPlayData = Assets.Scripts.GoFunctions.ValidPlayCheck(newStoneCoordinates, Assets.Scripts.GoFunctions.Currents.currentPlayerColor);
            if (validPlayData.playValidityLocal == PlayValidity.Valid)
            {
                gameController.PlaceGoStoneUnity(newStoneCoordinates, validPlayData.groupStonesToKill);
            }
        }

        public void ThrowStone(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            Assets.Scripts.GoFunctions.Currents.currentPlayerColor = stoneColor;


            BoardCoordinates newStoneCoordinates = new BoardCoordinates
            (
               xCoordinate,
               yCoordinate
            );

            ValidPlayData validPlayData = new ValidPlayData(PlayValidity.Valid, new List<BoardCoordinates>());



            gameController.PlaceGoStoneUnity(newStoneCoordinates, validPlayData.groupStonesToKill);
            Assets.Scripts.GoFunctions.Currents.currentGameState = GameState.CanThrowStone;
        }

        public bool StoneExists(int searchX, int searchY, StoneColor searchColor)
        {
            bool isFoundStoneExists = false;
            GoStone foundStone = Assets.Scripts.GoFunctions.BoardHistory.Last().boardStones.Find(s => s.Coordinates.xCoord == searchX &&
                                                                                          s.Coordinates.yCoord == searchY &&
                                                                                          s.stoneColor == searchColor);

            if (foundStone != null)
            {
                isFoundStoneExists = true;
            }

            bool isFoundStoneObjectExists = false;

            foreach (GameObject stoneObject in GameObject.FindGameObjectsWithTag("Stone"))
            {
                //stone object name format
                //$"{stoneCoordinates.x}x{stoneCoordinates.y}x{currentPlayerColor}Stone"
                if (stoneObject.name.Contains(searchX.ToString() + "x" + (searchY.ToString())) &&
                    stoneObject.name.Contains(searchColor.ToString()))
                {
                    isFoundStoneObjectExists = true;
                }
            }
            return (isFoundStoneExists && isFoundStoneObjectExists);
        }

        public bool ScoreCheck(int blackScoreToCheck, int whiteScoreToCheck)
        {
            if (PlayerScore.blackScore == blackScoreToCheck &&
                PlayerScore.whiteScore == whiteScoreToCheck)
            {
                return true;
            }
            else { return false; }
        }

        static void CheckIfObjectCountCorrect(int stonesLeft)
        {
            var coroutineHandler = (new GameObject("_coroutineHandler")).AddComponent<Text>();
            coroutineHandler.StartCoroutine(WaitForDestroyCoroutine());

            IEnumerator WaitForDestroyCoroutine()
            {
                yield return new WaitForSeconds(2);
                Assert.AreEqual(stonesLeft, GameObject.FindGameObjectsWithTag("Stone").Count());
            }
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
