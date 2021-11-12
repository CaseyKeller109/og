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

            //ThrowStone(0, 1, StoneColor.black);
            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            //ThrowStone(0, 0, StoneColor.white);
            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            //ThrowStone(1, 0, StoneColor.black);
            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_1_0B_0_0W_0_1B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_0_1W_0_0B_1_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
        }

        [Test]
        public void PlaceGoStone_1_0W_0_0B_0_1W()
        {
            InitialSetup();

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
        }


        //tests double capture at 0,0 and 1,0
        [Test]
        public void PlaceGoStone_0_1B_0_0W_1_1B_1_0W_2_0B()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_2_0B_1_0W_1_1B_0_0W_0_1B()
        {
            InitialSetup();

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            PlayStoneIfValid(0, 0, StoneColor.White);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            PlayStoneIfValid(0, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_0_1W_0_0B_1_1W_1_0B_2_0W()
        {
            InitialSetup();

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
        }

        [Test]
        public void PlaceGoStone_2_0W_1_0B_1_1W_0_0B_0_1W()
        {
            InitialSetup();

            PlayStoneIfValid(2, 0, StoneColor.White);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            PlayStoneIfValid(1, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            PlayStoneIfValid(1, 1, StoneColor.White);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);
        }


        //tests single and double capture at 1,0 and 0,1 and 0,2
        [Test]
        public void PlaceGoStone_1_1B_1_0W_2_0B_0_1W_1_2B_0_2W_0_3B_0_0B()
        {
            InitialSetup();

            PlayStoneIfValid(1, 1, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(1, 0, StoneColor.White);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(2, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(0, 1, StoneColor.White);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(1, 2, StoneColor.Black);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(0, 2, StoneColor.White);
            Assert.IsTrue(FindStone(0, 2, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(0, 3, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 3, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 2, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            PlayStoneIfValid(0, 0, StoneColor.Black);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 3, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 2, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) == null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

        }




        //tests single capture at 18,18
        [Test]
        public void PlaceGoStone_18_17B_18_18W_17_18B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_17_18B_18_18W_18_17B()
        {
            InitialSetup();

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_18_17W_18_18B_17_18W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
        }

        [Test]
        public void PlaceGoStone_17_18W_18_18B_18_17W()
        {
            InitialSetup();

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
        }


        //tests double capture at 0,0 and 1,0
        [Test]
        public void PlaceGoStone_18_17B_18_18W_17_17B_17_18W_16_18B()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_16_18B_17_18W_17_17B_18_18W_18_17B()
        {
            InitialSetup();

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            PlayStoneIfValid(18, 18, StoneColor.White);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            PlayStoneIfValid(18, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
        }

        [Test]
        public void PlaceGoStone_18_17W_18_18B_17_17W_17_18B_16_18W()
        {
            InitialSetup();

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            PlayStoneIfValid(17, 17, StoneColor.White);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            PlayStoneIfValid(16, 18, StoneColor.White);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
        }

        [Test]
        public void PlaceGoStone_16_18W_17_18B_17_17W_18_18B_18_17W()
        {
            InitialSetup();

            PlayStoneIfValid(16, 18, StoneColor.White);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            PlayStoneIfValid(17, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            PlayStoneIfValid(17, 17, StoneColor.White);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);
        }


        //tests single and double capture at 17,18 and 18,17 and 18,16
        [Test]
        public void PlaceGoStone_17_17B_17_18W_16_18B_18_17W_17_16B_18_16W_18_15B_18_18B()
        {
            InitialSetup();

            PlayStoneIfValid(17, 17, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(17, 18, StoneColor.White);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(16, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(18, 17, StoneColor.White);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(17, 16, StoneColor.Black);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(18, 16, StoneColor.White);
            Assert.IsTrue(FindStone(18, 16, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(18, 15, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 15, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 16, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            PlayStoneIfValid(18, 18, StoneColor.Black);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 15, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 16, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) == null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

        }





        //todo make sure it's testing all parts of GoStone

        //throwing
        //tests single capture at 0,0
        [Test]
        public void ThrowGoStone_0_1B_0_0W_1_0B()
        {
            InitialSetup();





            //PlayStoneIfValid(0, 1, StoneColor.black);
            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            //PlayStoneIfValid(0, 0, StoneColor.white);
            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            //PlayStoneIfValid(1, 0, StoneColor.black);
            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);







            //gameController.KillGroupStones(GameObject.Find())



            //Debug.Log("111asdf");
            //Debug.Log(gameController.stonePosHistory.Count);
            //gameController.KillGroupStones(gameController.stonePosHistory.Last());
            //Debug.Log(gameController.stonePosHistory.Count);
            //Debug.Log("asdf");
        }

        [Test]
        public void ThrowGoStone_1_0B_0_0W_0_1B()
        {
            InitialSetup();

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);

        }

        [Test]
        public void ThrowGoStone_0_1W_0_0B_1_0W()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

        }

        [Test]
        public void ThrowGoStone_1_0W_0_0B_0_1W()
        {
            InitialSetup();

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);

        }


        //throwing
        //tests double capture at 0,0 and 1,0
        [Test]
        public void ThrowGoStone_0_1B_0_0W_1_1B_1_0W_2_0B()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);

        }

        [Test]
        public void ThrowGoStone_2_0B_1_0W_1_1B_0_0W_0_1B()
        {
            InitialSetup();

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            ThrowStone(0, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

            ThrowStone(0, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);

        }

        [Test]
        public void ThrowGoStone_0_1W_0_0B_1_1W_1_0B_2_0W()
        {
            InitialSetup();

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            ThrowStone(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

            ThrowStone(2, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);

        }

        [Test]
        public void ThrowGoStone_2_0W_1_0B_1_1W_0_0B_0_1W()
        {
            InitialSetup();

            ThrowStone(2, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            ThrowStone(1, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            ThrowStone(1, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.White) != null);

        }


        //throwing
        //tests single and double capture at 1,0 and 0,1 and 0,2
        [Test]
        public void ThrowGoStone_1_1B_1_0W_2_0B_0_1W_1_2B_0_2W_0_3B_0_0B()
        {
            InitialSetup();

            ThrowStone(1, 1, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(1, 0, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(2, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(0, 1, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(1, 2, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(0, 2, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 2, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(0, 3, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 3, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 2, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) != null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) != null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);

            ThrowStone(0, 0, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(0, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 3, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 2, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 2, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(0, 1, StoneColor.White) == null);
            Assert.IsTrue(FindStone(2, 0, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(1, 0, StoneColor.White) == null);
            Assert.IsTrue(FindStone(1, 1, StoneColor.Black) != null);


        }



        //throwing
        //tests single capture at 18,18
        [Test]
        public void ThrowGoStone_18_17B_18_18W_17_18B()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

        }

        [Test]
        public void ThrowGoStone_17_18B_18_18W_18_17B()
        {
            InitialSetup();

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);


        }

        [Test]
        public void ThrowGoStone_18_17W_18_18B_17_18W()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

        }

        [Test]
        public void ThrowGoStone_17_18W_18_18B_18_17W()
        {
            InitialSetup();

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);

        }


        //throwing
        //tests double capture at 0,0 and 1,0
        [Test]
        public void ThrowGoStone_18_17B_18_18W_17_17B_17_18W_16_18B()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);

        }

        [Test]
        public void ThrowGoStone_16_18B_17_18W_17_17B_18_18W_18_17B()
        {
            InitialSetup();

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            ThrowStone(18, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

            ThrowStone(18, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);

        }

        [Test]
        public void ThrowGoStone_18_17W_18_18B_17_17W_17_18B_16_18W()
        {
            InitialSetup();

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            ThrowStone(17, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

            ThrowStone(16, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);

        }

        [Test]
        public void ThrowGoStone_16_18W_17_18B_17_17W_18_18B_18_17W()
        {
            InitialSetup();

            ThrowStone(16, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            ThrowStone(17, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            ThrowStone(17, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.Black) == null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.White) != null);

        }


        //throwing
        //tests single and double capture at 17,18 and 18,17 and 18,16
        [Test]
        public void ThrowGoStone_17_17B_17_18W_16_18B_18_17W_17_16B_18_16W_18_15B_18_18B()
        {
            InitialSetup();

            ThrowStone(17, 17, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(17, 18, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(16, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(18, 17, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(17, 16, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(18, 16, StoneColor.White);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 16, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(18, 15, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 15, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 16, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) != null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) != null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);

            ThrowStone(18, 18, StoneColor.Black);
            gameController.GetNewBoardLayout();
            Assert.IsTrue(FindStone(18, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 15, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 16, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 16, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(18, 17, StoneColor.White) == null);
            Assert.IsTrue(FindStone(16, 18, StoneColor.Black) != null);
            Assert.IsTrue(FindStone(17, 18, StoneColor.White) == null);
            Assert.IsTrue(FindStone(17, 17, StoneColor.Black) != null);


        }








        public void InitialSetup()
        {

            foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Stone"))
            {
                GameObject.DestroyImmediate(fooObj);
            }


            gameController.isOgNowFirstFrame = false;
            gameController.isValidPlay = true;
            gameController.ogFiredStage = 0;
            gameController.isOgFired = false;
            gameController.isOnFirstOgPlay = true;


            //gameController = new GameController();
            gameController.sensorStone = GameObject.Instantiate(Resources.Load("StoneSensor") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
            gameController.genericStoneObject = Resources.Load("Stone") as GameObject;

            gameController.whiteTextObject = new GameObject();
            gameController.blackTextObject = new GameObject();
            gameController.whiteTextObject.AddComponent<Text>();
            gameController.blackTextObject.AddComponent<Text>();

            gameController.stonePosHistory = new List<List<GoStone>>();
            gameController.stonePosHistory.Add(new List<GoStone>());


        }

        public void PlayStoneIfValid(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            gameController.currentGameState = GameState.PlaceStone;
            gameController.currentPlayerColor = stoneColor;
            GoStone newStone = new GoStone
            {
                x = xCoordinate,
                y = yCoordinate,
                stoneColor = stoneColor
            };

            ValidPlayData validPlayData = new ValidPlayData();
            validPlayData = gameController.ValidPlayCheck(newStone);
            if (validPlayData.isValidPlay)
            {
                gameController.PlaceGoStone(newStone, validPlayData.groupStonesToKill);
            }
            //else { Assert.IsTrue(false); }
        }

        public void ThrowStone(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {

            gameController.currentPlayerColor = stoneColor;
            GoStone newStone = new GoStone
            {
                x = xCoordinate,
                y = yCoordinate,
                stoneColor = stoneColor
            };

            ValidPlayData validPlayData = new ValidPlayData() { isValidPlay = true, groupStonesToKill = new List<GoStone>() };
            //validPlayData = gameController.ValidPlayCheck(newStone);
            //if (validPlayData.isValidPlay)
            //{
            gameController.PlaceGoStone(newStone, validPlayData.groupStonesToKill);
            //}
            //else { Assert.IsTrue(false); }
            gameController.currentGameState = GameState.ThrowStone;
        }

        public GoStone FindStone(int xCoordinate, int yCoordinate, StoneColor stoneColor)
        {
            //bool isFoundStoneExists = false;
            GoStone foundStone = gameController.stonePosHistory.Last().Find(s => s.x == xCoordinate &&
                                                                                 s.y == yCoordinate &&
                                                                                 s.stoneColor == stoneColor);
            //if (foundStone != null)
            //{
            //    isFoundStoneExists = true;
            //}


            //bool isFoundStoneObjectExists = false;
            //foreach (GameObject stoneObject in GameObject.FindGameObjectsWithTag("Stone"))
            //{
            //    //stone object name format
            //    //$"{stoneCoordinates.x}x{stoneCoordinates.y}x{currentPlayerColor}Stone"
            //    if (stoneObject.name.Contains(foundStone.x.ToString()) &&
            //        stoneObject.name.Contains(foundStone.y.ToString()) &&
            //        stoneObject.name.Contains(foundStone.stoneColor.ToString()))
            //    {
            //        isFoundStoneObjectExists = true;
            //    }
            //}


            //return (isFoundStoneExists && isFoundStoneObjectExists);
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
