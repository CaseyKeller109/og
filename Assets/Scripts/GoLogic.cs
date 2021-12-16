using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GameObject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



using static Assets.Scripts.GameController;
using static Assets.Scripts.GoFunctions;

using PV = Assets.Scripts.GoFunctions.PlayValidity;


//1todo use KillGroupStones?
//1todo use better referencing for gameController.stuff, and GameObject
namespace Assets.Scripts
{
    public class GoFunctions
    {
        public static List<GoBoard> BoardHistory = new List<GoBoard>();
        //public static List<GoStone> latestBoardStones = BoardHistory.Last().boardStones;


        public static class Currents
        {
            public static StoneColor currentPlayerColor = StoneColor.Black;
            public static GameState currentGameState = GameState.CanPlaceStone;

            public static PlayValidity playValidity = PV.Valid;
        }

        public static class PlayerScore
        {
            public static float whiteScore = 0;
            public static float blackScore = 0;
        }

        public static List<GoStone> LatestBoardStones()
        {
            return BoardHistory.Last().boardStones;
        }

        public static List<GoStone> PreviousBoardStones()
        {
            return BoardHistory[BoardHistory.Count - 2].boardStones;
        }

        public static List<GoStoneLite> PreviousBoardStonesLite()
        {
            List<GoStone> previousBoardStones = BoardHistory[BoardHistory.Count - 2].boardStones;
            return previousBoardStones.Select(stone => ToLite(stone)).ToList();
        }


        //static GameController gameController = new GameController();

        public static void PlaceGoStone(BoardCoordinates newStoneCoordinates,
                                 List<BoardCoordinates> groupStonesToKill,
                                 List<GoBoard> BoardHistory,
                                 GameObject newStoneObject)
        {
            Currents.playValidity = PV.Invalid;

            BoardHistory.Add(new GoBoard());

            for (int i = 0; i < PreviousBoardStones().Count; i++)
            {
                LatestBoardStones().Add(PreviousBoardStones()[i]);
            }

            //if (groupStonesToKill != null)
            //{
            //    gameController.KillGroupStones(groupStonesToKill);
            //}

            //gameController.RenderSensorStone(false);

            //GameObject newStoneObject = GameObject.Instantiate(genericStoneObject,
            //                                  new Vector3((float)(newStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
            //                                              (float)(-newStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
            //                                              -GoStone.ZHeightValue),
            //                                  Quaternion.identity);

            //newStoneObject.name = $"{newStoneCoordinates.xCoord}x" +
            //                      $"{newStoneCoordinates.yCoord}x" +
            //                      $"{Currents.currentPlayerColor}Stone";
            //newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);





            LatestBoardStones().Add(new GoStone(
                newStoneCoordinates,
                Currents.currentPlayerColor,
                newStoneObject));
        }


        public static ValidPlayData ValidPlayCheck(BoardCoordinates newStoneCoordinates, StoneColor newStoneColor)
        {
            GoStoneLite newStone = new GoStoneLite(newStoneCoordinates, newStoneColor);

            List<BoardCoordinates> newGroupStonesToKill = new List<BoardCoordinates>();

            if (LatestBoardStones().Find(stone => (stone.SameCoordinatesAs(newStoneCoordinates))) != null)
            {
                return new ValidPlayData(PV.Invalid);
            }

            List<GoStoneLite> boardIfStoneIsPlayed = new List<GoStoneLite>();

            //2todo look for deep level clone
            for (int i = 0; i < LatestBoardStones().Count(); i++)
            {
                boardIfStoneIsPlayed.Add(new GoStoneLite
                    (LatestBoardStones()[i]));
            }

            boardIfStoneIsPlayed.Add(new GoStoneLite
                (newStoneCoordinates, Currents.currentPlayerColor));


            //Simple Ko rule
            if (BoardHistory.Count > 3)
            {
                bool isSameBoard = IsSameBoardSimpleCheck(boardIfStoneIsPlayed, PreviousBoardStonesLite());
                if (isSameBoard) { return new ValidPlayData(PV.Invalid); }
            }

            string openSides = OpenSidesCheck(newStone, boardIfStoneIsPlayed, newGroupStonesToKill);
            if (openSides.Length > 0)
            { return new ValidPlayData(PV.Valid, newGroupStonesToKill); }
            else { return new ValidPlayData(PV.Invalid); }
        }

        public static bool IsSameBoardSimpleCheck(List<GoStoneLite> boardIfStoneIsPlayed,
                                           List<GoStoneLite> previousBoardStonesLite)
        {
            bool isSameBoard = false;
            //if (BoardHistory.Count > 3
            //    && boardIfStoneIsPlayed.Count == previousBoardStonesLite.Count
            if (boardIfStoneIsPlayed.Count == previousBoardStonesLite.Count
             )
            {
                isSameBoard = true;
                for (int i = 0; i < previousBoardStonesLite.Count; i++)
                {
                    if (boardIfStoneIsPlayed.Find(stone => stone.SameCoordinatesAndColorAs(previousBoardStonesLite[i])) == null)
                    {
                        isSameBoard = false;
                        break;
                    }
                }
            }
            return isSameBoard;
        }

        private static string OpenSidesCheck(GoStoneLite centerStone,
                                      List<GoStoneLite> boardIfStoneIsPlayed,
                                      List<BoardCoordinates> groupStonesToKill)
        {
            string openSides = "";
            int xStoneCoord = (int)centerStone.Coordinates.xCoord;
            int yStoneCoord = (int)centerStone.Coordinates.yCoord;

            //top side
            if (yStoneCoord > 0)
            {
                if (LibertiesFromSideExists(centerStone, StoneDirectionOffset.up, boardIfStoneIsPlayed, groupStonesToKill))
                {
                    openSides += "top";
                }
            }

            //bottom side
            if (yStoneCoord < 18)
            {
                if (LibertiesFromSideExists(centerStone, StoneDirectionOffset.down, boardIfStoneIsPlayed, groupStonesToKill))
                {
                    openSides += "bottom";
                }
            }

            //left side
            if (xStoneCoord > 0)
            {
                if (LibertiesFromSideExists(centerStone, StoneDirectionOffset.left, boardIfStoneIsPlayed, groupStonesToKill))
                {
                    openSides += "left";
                }
            }

            //right side
            if (xStoneCoord < 18)
            {
                if (LibertiesFromSideExists(centerStone, StoneDirectionOffset.right, boardIfStoneIsPlayed, groupStonesToKill))
                {
                    openSides += "right";
                }
            }
            return openSides;
        }


        //1todo have better names
        //2todo rename centerStone?
        public static bool LibertiesFromSideExists(GoStoneLite centerStone,
                                             BoardCoordinates offsetFromCenterStone,
                                             List<GoStoneLite> boardIfStoneIsPlayed,
                                             List<BoardCoordinates> groupStonesToKill)
        {
            GoStoneGroupData stoneGroupData = new GoStoneGroupData();

            BoardCoordinates offsetCoordinatesToCheck = centerStone.Coordinates + offsetFromCenterStone;

            GoStoneLite sideStone = ToLite(
                LatestBoardStones().Find(stone => stone.SameCoordinatesAs(offsetCoordinatesToCheck)));

            if (boardIfStoneIsPlayed.Find(stone => (stone.SameCoordinatesAs(sideStone))) == null)
            {
                return true;
            }

            stoneGroupData = FindGroupAndLibertyCoordinates(sideStone, boardIfStoneIsPlayed, stoneGroupData);

            if (sideStone == null)
            {
                return true;
            }
            else if (stoneGroupData.libertyCoordinates.Count > 0 && sideStone.stoneColor == centerStone.stoneColor)
            {
                return true;
            }
            else if (stoneGroupData.libertyCoordinates.Count > 0 && sideStone.stoneColor != centerStone.stoneColor)
            {
                return false;
            }
            else if (stoneGroupData.libertyCoordinates.Count == 0 && sideStone.stoneColor == centerStone.stoneColor)
            {
                return false;
            }
            else if (stoneGroupData.libertyCoordinates.Count == 0 && sideStone.stoneColor != centerStone.stoneColor)
            {
                foreach (BoardCoordinates stone in stoneGroupData.stonesCoord)
                {
                    groupStonesToKill.Add(stone);
                }

                foreach (BoardCoordinates stoneToKill in groupStonesToKill)
                {
                    boardIfStoneIsPlayed.Remove(boardIfStoneIsPlayed.Find(stoneIf => (stoneIf.SameCoordinatesAs(stoneToKill))));
                }

                return true;
            }
            else { return false; }
        }

        //starts searching at centerStoneCoordinates

        public static GoStoneGroupData FindGroupAndLibertyCoordinates(GoStoneLite sideStone,
                                                    List<GoStoneLite> boardIfStoneIsPlayed,
                                                    GoStoneGroupData stoneGroupData)

        {
            if (stoneGroupData.stonesCoord.Find(groupStone => (groupStone.SameCoordinatesAs(sideStone))) == null)
            {
                stoneGroupData.stonesCoord.Add(new BoardCoordinates(sideStone));
            }

            FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.yCoord > 0), StoneDirectionOffset.up, ref stoneGroupData);
            FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.yCoord < 18), StoneDirectionOffset.down, ref stoneGroupData);
            FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.xCoord > 0), StoneDirectionOffset.left, ref stoneGroupData);
            FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.xCoord < 18), StoneDirectionOffset.right, ref stoneGroupData);

            return stoneGroupData;
        }

        private static void FindGroupAndLibertyCoordinatesSideStone(BoardCoordinates sideStoneCoordinates,
                                                             List<GoStoneLite> boardIfStoneIsPlayed,
                                                             bool isPositionOnBoard,
                                                             BoardCoordinates offset,
                                                             ref GoStoneGroupData stoneGroupData)
        {
            if (isPositionOnBoard)
            {
                GoStoneLite sideStone = boardIfStoneIsPlayed.Find(stoneIf => (stoneIf.Coordinates.SameCoordinatesAs(sideStoneCoordinates)));
                BoardCoordinates offsetCoordinatesToCheck = sideStone.Coordinates + offset;
                GoStoneLite otherStone = boardIfStoneIsPlayed.Find(stoneIf => (stoneIf.SameCoordinatesAs(offsetCoordinatesToCheck)));

                if (sideStone != null
                    &&
                    otherStone != null
                    &&
                    otherStone.stoneColor == sideStone.stoneColor
                    &&
                    stoneGroupData.stonesCoord.Find(coord => (coord.SameCoordinatesAs(offsetCoordinatesToCheck))) == null
                    )
                {
                    stoneGroupData = FindGroupAndLibertyCoordinates((otherStone),
                                 boardIfStoneIsPlayed,
                                 stoneGroupData);
                }
                else if ((otherStone == null)
                         &&
                         (stoneGroupData.libertyCoordinates.Find(libertyCoord => (libertyCoord.SameCoordinatesAs(offsetCoordinatesToCheck)))) == null)

                {
                    stoneGroupData.libertyCoordinates.Add(new BoardCoordinates
                    (offsetCoordinatesToCheck));
                }
            }
        }





        public static void KillGroupStones(List<BoardCoordinates> groupStonesToKill)
        {
            foreach (BoardCoordinates killCoord in groupStonesToKill)
            {
                GoStone stoneToDestroy = LatestBoardStones().Find(latestStone => (latestStone.SameCoordinatesAs(killCoord)));

                if (stoneToDestroy == null) { continue; }

                KillStone(stoneToDestroy);
            }
        }

        public static void KillStone(GoStone StoneToDestroy)
        {
            LatestBoardStones().Remove(LatestBoardStones().Find(latestStone => (latestStone.SameCoordinatesAs(StoneToDestroy))));

            if (StoneToDestroy.stoneColor == StoneColor.Black)
            {
                PlusOneToScore(StoneColor.White);
            }
            else if (StoneToDestroy.stoneColor == StoneColor.White)
            {
                PlusOneToScore(StoneColor.Black);
            }
        }

        public static void PlusOneToScore(StoneColor stoneColor)
        {
            if (stoneColor == StoneColor.Black)
            {
                PlayerScore.blackScore += 1;
            }
            if (stoneColor == StoneColor.White)
            {
                PlayerScore.whiteScore += 1;
            }
        }

        public static GoStone SortStone(Collider stoneToSort,
                                 int CoordinateX,
                                 int CoordinateY)
        {
            stoneToSort.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            stoneToSort.GetComponent<Transform>().position =
                new Vector3(CoordinateX * GoBoard.boardCoordinateSeparationX,
                           -CoordinateY * GoBoard.boardCoordinateSeparationY,
                           -GoStone.ZHeightValue);
            stoneToSort.GetComponent<Transform>().rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);
            stoneToSort.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);



            StoneColor stoneToSortColor = stoneToSort.name.Contains("Black") ? StoneColor.Black : StoneColor.White;

            //LatestBoardStones().Add(new GoStone(CoordinateX,
            //                                    CoordinateY,
            //                                    stoneToSortColor,
            //                                    stoneToSort.gameObject));

            //changes layer to "Stone"
            stoneToSort.gameObject.layer = 8;

            //0todo use this return value
            return new GoStone(CoordinateX,
                               CoordinateY,
                               stoneToSortColor,
                               stoneToSort.gameObject);
        }

        public static class StoneDirectionOffset
        {
            public static BoardCoordinates left = new BoardCoordinates(-1, 0);
            public static BoardCoordinates right = new BoardCoordinates(+1, 0);
            public static BoardCoordinates up = new BoardCoordinates(0, -1);
            public static BoardCoordinates down = new BoardCoordinates(0, 1);
        }

        public class BoardCoordinates
        {
            public int? xCoord;
            public int? yCoord;

            protected BoardCoordinates() { }

            public BoardCoordinates(BoardCoordinates newBoardCoordinates)
            {
                xCoord = newBoardCoordinates.xCoord;
                yCoord = newBoardCoordinates.yCoord;
            }

            public BoardCoordinates(int newX, int newY)
            {
                xCoord = newX;
                yCoord = newY;
            }

            public static BoardCoordinates operator +(BoardCoordinates left, BoardCoordinates right)
            {
                return new BoardCoordinates((int)right.xCoord + (int)left.xCoord, (int)right.yCoord + (int)left.yCoord);
            }

            //public static bool Equals(BoardCoordinates left, BoardCoordinates right)
            //{
            //    return (left.x == right.x) && (left.y == right.y);
            //}

            //public static bool operator ==(BoardCoordinates left, BoardCoordinates right)
            //{
            //    return (right.x == left.x && right.y == left.y);
            //}

            //public static bool operator !=(BoardCoordinates left, BoardCoordinates right)
            //{
            //    return (right.x != left.x || right.y != left.y);
            //}

            public bool SameCoordinatesAs(BoardCoordinates secondCoord)
            {
                if (this == null ||
                    secondCoord == null)
                {
                    return false;
                }

                string errorString = "";
                if (this.xCoord != secondCoord.xCoord)
                {
                    errorString += "x values not equal. ";
                }
                if (this.yCoord != secondCoord.yCoord)
                {
                    errorString += "y values not equal. ";
                }

                if (errorString == "") { return true; }
                else { return false; }
            }

            public void CopyCoordinatesTo(BoardCoordinates destination)
            {
                destination.xCoord = this.xCoord;
                destination.yCoord = this.yCoord;
            }
        }


        public enum StoneColor
        {
            Black,
            White
        }

        public class GoStoneLite : BoardCoordinates
        {
            public StoneColor stoneColor;

            public BoardCoordinates Coordinates
            {
                get
                {
                    if (xCoord == null || yCoord == null) { return null; }
                    else { return new BoardCoordinates((int)xCoord, (int)yCoord); }
                }
                set
                {
                    if (value == null)
                    {
                        xCoord = null;
                        yCoord = null;
                    }
                    else
                    {
                        xCoord = value.xCoord;
                        yCoord = value.yCoord;
                    }
                }
            }

            protected GoStoneLite() { }

            public GoStoneLite(GoStone newGoStone)
            {
                Coordinates = newGoStone.Coordinates;
                stoneColor = newGoStone.stoneColor;
            }

            public GoStoneLite(GoStoneLite newGoStoneLite)
            {
                Coordinates = newGoStoneLite.Coordinates;
                stoneColor = newGoStoneLite.stoneColor;
            }

            public GoStoneLite(BoardCoordinates newCoordinates, StoneColor newStoneColor)
            {
                Coordinates = new BoardCoordinates((int)newCoordinates.xCoord, (int)newCoordinates.yCoord);
                stoneColor = newStoneColor;
            }

            public GoStoneLite(int newX, int newY, StoneColor newStoneColor)
            {
                Coordinates = new BoardCoordinates(newX, newY);
                stoneColor = newStoneColor;
            }



            public bool SameColorAs(GoStoneLite otherStone)
            {
                string errorString = "";
                if (this.stoneColor != otherStone.stoneColor)
                {
                    errorString += "colors not same. ";
                }

                if (errorString == "") { return true; }
                else { return false; }
            }

            public bool SameCoordinatesAndColorAs(GoStoneLite otherStone)
            {
                return (this.SameCoordinatesAs(otherStone) && this.SameColorAs(otherStone));
            }
        }


        public class GoStone : GoStoneLite
        {
            public readonly static float diameter = 0.22f;
            public readonly static float ZHeightValue = 0.095f;

            public GameObject gameObject;
            public GameObject exploderGameObject;

            protected GoStone() { }

            public GoStone(BoardCoordinates newCoordinates, StoneColor newColor, GameObject newGameObject)
            {
                Coordinates = newCoordinates;
                stoneColor = newColor;

                if (genericStoneObject == null)
                {
                    genericStoneObject = new GameObject();
                }

                gameObject = newGameObject;
            }

            public GoStone(int newXCoord, int newYCoord, StoneColor newColor, GameObject newGameObject)
            {
                Coordinates = new BoardCoordinates(newXCoord, newYCoord);
                stoneColor = newColor;

                if (genericStoneObject == null)
                {
                    genericStoneObject = new GameObject();
                }

                gameObject = newGameObject;
            }
        }

        public static GoStoneLite ToLite(GoStone stone)
        {
            if (stone != null)
            {
                return new GoStoneLite
                (
                    stone.Coordinates,
                    stone.stoneColor
                );
            }

            else
            {
                return null;
            }
        }

        public class GoStoneGroupData
        {
            public List<BoardCoordinates> stonesCoord = new List<BoardCoordinates>();
            public List<BoardCoordinates> libertyCoordinates = new List<BoardCoordinates>();
        }

        public class GoBoard
        {
            public readonly static float boardCoordinateSeparationX = 0.2211f;
            public readonly static float boardCoordinateSeparationY = 0.2366f;

            public List<GoStone> boardStones = new List<GoStone>();
        }

        public class GoBoardLite
        {
            public List<GoStone> boardStones = new List<GoStone>();
        }

        //2todo implememt this?
        //public interface IPlay
        //{

        //}

        public class ValidPlayData
        {
            public PlayValidity playValidityLocal;
            public List<BoardCoordinates> groupStonesToKill;

            protected ValidPlayData() { }

            public ValidPlayData(PlayValidity newPlayValidity)
            {
                playValidityLocal = newPlayValidity;
            }

            public ValidPlayData(PlayValidity newPlayValidity, List<BoardCoordinates> newGroupStonesToKill)
            {
                playValidityLocal = newPlayValidity;
                groupStonesToKill = newGroupStonesToKill;
            }
        }

        //public class InvalidPlayData : IPlay
        //{
        //    public bool isValidPlayLocal;
        //    public List<GoStoneLite> groupStonesToKill;
        //}

        public enum GameState
        {
            CanPlaceStone,
            CanThrowStone,
            StoneHasBeenThrown,
            StonesHaveBeenSorted
        }

        public enum PlayValidity
        {
            Valid,
            Invalid,
            NotYetSet
        }
    }
}
