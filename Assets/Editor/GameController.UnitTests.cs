using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameController;

namespace Tests
{
    public class GameController_UnitTests
    {
        GameController gameController = new GameController();

        //tests single capture at 0,0
        [Test]
        public void PlaceGoStone_0_1B_0_0W_1_0B()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_1_0B_0_0W_0_1B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_0_1W_0_0B_1_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
        }

        [Test]
        public void PlaceGoStone_1_0W_0_0B_0_1W()
        {
            InitialSetup();

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
        }


        //tests double capture at 0,0 and 1,0
        [Test]
        public void PlaceGoStone_0_1B_0_0W_1_1B_1_0W_2_0B()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_2_0B_1_0W_1_1B_0_0W_0_1B()
        {
            InitialSetup();

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_0_1W_0_0B_1_1W_1_0B_2_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
        }

        [Test]
        public void PlaceGoStone_2_0W_1_0B_1_1W_0_0B_0_1W()
        {
            InitialSetup();

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
        }


        //tests single and double capture at 1,0 and 0,1 and 0,2
        [Test]
        public void PlaceGoStone_1_1B_1_0W_2_0B_0_1W_1_2B_0_2W_0_3B_0_0B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(1, 2, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(0, 2, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(0, 3, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
        }




        //tests single capture at 18,18
        [Test]
        public void PlaceGoStone_18_17B_18_18W_17_18B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_17_18B_18_18W_18_17B()
        {
            InitialSetup();

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_18_17W_18_18B_17_18W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
        }

        [Test]
        public void PlaceGoStone_17_18W_18_18B_18_17W()
        {
            InitialSetup();

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
        }


        //tests double capture at 0,0 and 1,0
        [Test]
        public void PlaceGoStone_18_17B_18_18W_17_17B_17_18W_16_18B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_16_18B_17_18W_17_17B_18_18W_18_17B()
        {
            InitialSetup();

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_18_17W_18_18B_17_17W_17_18B_16_18W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            PlayStoneIfValid(17, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            PlayStoneIfValid(16, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
        }

        [Test]
        public void PlaceGoStone_16_18W_17_18B_17_17W_18_18B_18_17W()
        {
            InitialSetup();

            PlayStoneIfValid(16, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            PlayStoneIfValid(17, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
        }


        //tests single and double capture at 17,18 and 18,17 and 18,16
        [Test]
        public void PlaceGoStone_17_17B_17_18W_16_18B_18_17W_17_16B_18_16W_18_15B_18_18B()
        {
            InitialSetup();

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(17, 16, StoneColor.Black);
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(18, 16, StoneColor.White);
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(18, 15, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
        }





        //todo make sure it's testing all parts of GoStone

        //throwing
        //tests single capture at 0,0
        [Test]
        public void ThrowGoStone_0_1B_0_0W_1_0B()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_1_0B_0_0W_0_1B()
        {
            InitialSetup();

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_0_1W_0_0B_1_0W()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
        }

        [Test]
        public void ThrowGoStone_1_0W_0_0B_0_1W()
        {
            InitialSetup();

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));

        }


        //throwing
        //tests double capture at 0,0 and 1,0
        [Test]
        public void ThrowGoStone_0_1B_0_0W_1_1B_1_0W_2_0B()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_2_0B_1_0W_1_1B_0_0W_0_1B()
        {
            InitialSetup();

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_0_1W_0_0B_1_1W_1_0B_2_0W()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            ThrowStone(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            ThrowStone(2, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
        }

        [Test]
        public void ThrowGoStone_2_0W_1_0B_1_1W_0_0B_0_1W()
        {
            InitialSetup();

            ThrowStone(2, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            ThrowStone(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
        }


        //throwing
        //tests single and double capture at 1,0 and 0,1 and 0,2
        [Test]
        public void ThrowGoStone_1_1B_1_0W_2_0B_0_1W_1_2B_0_2W_0_3B_0_0B()
        {
            InitialSetup();

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(1, 2, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(0, 2, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(0, 3, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 3, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 2, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 2, StoneColor.Black));
            Assert.IsFalse(StoneExists(0, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
        }



        //throwing
        //tests single capture at 18,18
        [Test]
        public void ThrowGoStone_18_17B_18_18W_17_18B()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_17_18B_18_18W_18_17B()
        {
            InitialSetup();

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_18_17W_18_18B_17_18W()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
        }

        [Test]
        public void ThrowGoStone_17_18W_18_18B_18_17W()
        {
            InitialSetup();

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
        }


        //throwing
        //tests double capture at 0,0 and 1,0
        [Test]
        public void ThrowGoStone_18_17B_18_18W_17_17B_17_18W_16_18B()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_16_18B_17_18W_17_17B_18_18W_18_17B()
        {
            InitialSetup();

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
        }

        [Test]
        public void ThrowGoStone_18_17W_18_18B_17_17W_17_18B_16_18W()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            ThrowStone(17, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));

            ThrowStone(16, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
        }

        [Test]
        public void ThrowGoStone_16_18W_17_18B_17_17W_18_18B_18_17W()
        {
            InitialSetup();

            ThrowStone(16, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            ThrowStone(17, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.White));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.White));
        }


        //throwing
        //tests single and double capture at 17,18 and 18,17 and 18,16
        [Test]
        public void ThrowGoStone_17_17B_17_18W_16_18B_18_17W_17_16B_18_16W_18_15B_18_18B()
        {
            InitialSetup();

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(17, 16, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(18, 16, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(18, 15, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(StoneExists(18, 18, StoneColor.Black));
            Assert.IsTrue(StoneExists(18, 15, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 16, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 16, StoneColor.Black));
            Assert.IsFalse(StoneExists(18, 17, StoneColor.White));
            Assert.IsTrue(StoneExists(16, 18, StoneColor.Black));
            Assert.IsFalse(StoneExists(17, 18, StoneColor.White));
            Assert.IsTrue(StoneExists(17, 17, StoneColor.Black));
        }

        //todo rename funcitons with more than just positions

        // simple ko
        [Test]
        public void PlaceGoStone_Ko_0_1B_1_1W_1_0B_2_0W_0_0W_0_0B()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.White));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.White));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.Black));
        }

        [Test]
        public void PlaceGoStone_Ko_0_1W_1_1B_1_0W_2_0B_0_0B_0_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(StoneExists(0, 0, StoneColor.Black));
            Assert.IsTrue(StoneExists(2, 0, StoneColor.Black));
            Assert.IsFalse(StoneExists(1, 0, StoneColor.White));
            Assert.IsTrue(StoneExists(1, 1, StoneColor.Black));
            Assert.IsTrue(StoneExists(0, 1, StoneColor.White));
        }




        public void InitialSetup()
        {
            foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Stone"))
            {
                GameObject.DestroyImmediate(fooObj);
            }

            //0todo improve this?
            gameController.sensorStone = new GoStone(new BoardCoordinates { x = 20, y = 20 }, GameObject.Instantiate(Resources.Load("Stone") as GameObject));
            gameController.sensorStone.gameObject.GetComponent<MeshCollider>().enabled = false;
            gameController.sensorStone.gameObject.name = "BlackSensorStone";
            gameController.sensorStone.gameObject.layer = 0;

            GameController.genericStoneObject = Resources.Load("Stone") as GameObject;

            gameController.whiteTextObject = new GameObject();
            gameController.blackTextObject = new GameObject();
            gameController.whiteTextObject.AddComponent<Text>();
            gameController.blackTextObject.AddComponent<Text>();

            gameController.BoardHistory = new List<GoBoard>();
            gameController.BoardHistory.Add(new GoBoard());
        }

        public void PlayStoneIfValid(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            GameController.CurrentStateData.currentGameState = GameState.CanPlaceStone;
            GameController.CurrentStateData.currentPlayerColor = stoneColor;
            //GoStone newStone = new GoStone
            //{
            //    coordinates = {
            //    x = xCoordinate,
            //    y = yCoordinate
            //    },
            //    stoneColor = stoneColor
            //};

            BoardCoordinates newStoneCoordinates = new BoardCoordinates
            {
                x = xCoordinate,
                y = yCoordinate
            };

            ValidPlayData validPlayData = new ValidPlayData();
            validPlayData = gameController.ValidPlayCheck(newStoneCoordinates, GameController.CurrentStateData.currentPlayerColor);
            if (validPlayData.isValidPlayLocal)
            {
                gameController.PlaceGoStone(newStoneCoordinates, validPlayData.groupStonesToKill);
            }
        }

        public void ThrowStone(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            GameController.CurrentStateData.currentPlayerColor = stoneColor;
            //GoStone newStone = new GoStone
            //{
            //    coordinates = {
            //    x = xCoordinate,
            //    y = yCoordinate
            //    },
            //    stoneColor = stoneColor
            //};

            BoardCoordinates newStoneCoordinates = new BoardCoordinates
            {
                x = xCoordinate,
                y = yCoordinate
            };

            ValidPlayData validPlayData = new ValidPlayData() { isValidPlayLocal = true, groupStonesToKill = new List<GoStoneHypothetical>() };

            gameController.PlaceGoStone(newStoneCoordinates, validPlayData.groupStonesToKill);
            GameController.CurrentStateData.currentGameState = GameState.CanThrowStone;
        }

        public bool StoneExists(int searchX, int searchY, StoneColor searchColor)
        {
            bool isFoundStoneExists = false;
            GoStone foundStone = gameController.BoardHistory.Last().boardStones.Find(s => s.Coordinates.x == searchX &&
                                                                                          s.Coordinates.y == searchY &&
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
