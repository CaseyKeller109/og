using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //0todo move methods inside related classes
    //0todo fix throw bug where it doesn't properly destroy stones
    //0todo put global variables in classes
    //0todo add constructors to classes. GoStone, etc

    //3todo sensor stone shouldn't cast shadow
    //0todo more functions should return value
    //2todo reduce number of parameters in functions
    //3todo reduce function size
    //1todo get rid of magic numbers everywhere
    //2todo make more unit tests?

    //3todo change coordinates to go from 1 to 19?
    //Stone position coordinates are 19x19 and are used as follows. x is left to right, y is top to bottom
    //   0 1 2 .. 16 17 18 
    // 0
    // 1
    // 2
    // ..
    // 16
    // 17
    // 18

    // upper left coordinates (0,0) are at (0,0) in real-world space
    // coordinates increment in real-world unity space by 0.2211 in x, and 0.2366 in y.

    // center of board coordinates (9,9) in real-world space are
    // (1.9899, 2.1294)

    //lower-right corner coordinates (18,18) in real-world space are
    // (3.9798, 4.2588)

    //stone diameter is 0.22

    //0TODO make more things local
    public GameObject title;
    public GameObject title2;
    public float timer;
    public Button resetButton;
    public Camera mainCamera;
    public Vector3 cameraStartPosition;
    public Vector3 defaultCameraPosition;
    public Quaternion defaultCameraRotation;
    public Camera defaultThrowingCamera;
    public static GameObject genericStoneObject;
    public Material whiteMaterial;
    public Material blackMaterial;
    public Material whiteMaterialTransparent;
    public Material blackMaterialTransparent;
    public GoStone sensorStone;
    public GameObject sensorStoneObject;
    public GoStone thrownStone;
    public GameObject explosionObjectParent;
    public GameObject explosion;

    public GameObject whiteTextObject;
    public GameObject blackTextObject;

    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;

    public static List<GoBoard> BoardHistory = new List<GoBoard>();
    //public static List<GoStone> latestBoardStones = BoardHistory.Last().boardStones;

    public Vector3 mousePos;
    public BoardCoordinates previousMouseCoordinates;

    private string codeEntered = "";
    private readonly string konamiCode = "UUDDLRLRBAS";
    public GameObject konamiText;
    public GameObject ogDebugStuff;
    public Button ogNowButton;
    public bool isOgNowFirstFrame;
    public Button plusOgSpeedButton;
    public Button minusOgSpeedButton;
    public Button plusOgBaseProbButton;
    public Button minusOgBaseProbButton;

    public GameObject ogSpeedText;
    public GameObject ogProbText;
    public GameObject ogBaseProbText;

    public static List<GoStone> LatestBoardStones()
    {
        return BoardHistory.Last().boardStones;
    }

    public static List<GoStone> PreviousBoardStones()
    {
        return BoardHistory[BoardHistory.Count - 2].boardStones;
    }

    public static class Currents
    {
        //0todo use sensorstone rotation instead of this?
        //sensorStone.gameObject.GetComponent<Transform>().rotation
        public static Quaternion curentGoStoneRotation;
        public static StoneColor currentPlayerColor = StoneColor.Black;
        public static GameState currentGameState = GameState.CanPlaceStone;

        //0todo use better validation
        //0todouse stuff more verbose than bool
        public static bool? isValidPlay = true;
    }

    public static class CameraMouseMovementData
    {
        public static float speedH = 5.0f;
        public static float speedV = 3.0f;

        public static float yaw = 0.0f;
        public static float pitch = 0.0f;
    }

    public static class ThrowingData
    {
        public static float ogBaseProb = -70;
        public static float ogProb = -70;
        public static float ogVelocity = 6;
        public static bool isOgFired = false;
        public static bool isOnFirstOgPlay = true;
    }

    public static class PlayerScore
    {
        public static float whiteScore = 0;
        public static float blackScore = 0;
    }



    private void Awake()
    {
        genericStoneObject = Resources.Load("Stone") as GameObject;

        //0todo here
        previousMouseCoordinates = new BoardCoordinates(-1, -1);
    }

    // Start is called before the first frame update
    void Start()
    {

        Console.Write("hello world");

        Application.targetFrameRate = 60;
        resetButton.onClick.AddListener(ResetGame);

        sensorStone = new GoStone(new BoardCoordinates(8, 8), StoneColor.Black, sensorStoneObject);

        //sensorStone.gameObject.layer = 0;
        sensorStone.gameObject.gameObject.GetComponent<MeshCollider>().enabled = false;
        sensorStone.gameObject.name = "BlackSensorStone";
        sensorStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterialTransparent;

        BoardHistory.Add(new GoBoard());

        ogNowButton.onClick.AddListener(OgNow);
        plusOgSpeedButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogVelocity, 1, ogSpeedText, "Throw Speed: ", false); });
        minusOgSpeedButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogVelocity, -1, ogSpeedText, "Throw Speed: ", false); });
        plusOgBaseProbButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogBaseProb, 10, ogBaseProbText, "Base Og Prob: ", true); });
        minusOgBaseProbButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogBaseProb, -10, ogBaseProbText, "Base Og Prob: ", true); });

        //0todo set these in editor?
        ogSpeedText.GetComponent<Text>().text = "Throw Speed: " + ThrowingData.ogVelocity;
        ogProbText.GetComponent<Text>().text = "Og prob: " + ThrowingData.ogProb + "%";
        ogBaseProbText.GetComponent<Text>().text = "Base Og prob: " + ThrowingData.ogBaseProb + "%";

        cameraStartPosition = new Vector3(1.9883f, -2.1326f, -5);
        defaultCameraPosition = mainCamera.GetComponent<Transform>().position;
        defaultCameraRotation = mainCamera.GetComponent<Transform>().rotation;

        //og at start of game
        //ogPlay = true;
        //mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
        //mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;
        //sensorStone.GetComponent<MeshRenderer>().material.color = new Color(0f, 1.0f, 1.0f, 0.5f);

        StartCoroutine(TitleScreen());

        IEnumerator TitleScreen()
        {
            yield return new WaitForSeconds(0.0f);
            mainCamera.GetComponent<Transform>().position = new Vector3(10, 10, 10);
            //yield return new WaitForSeconds(1.0f);
            mainCamera.GetComponent<Transform>().position = defaultCameraPosition;
            title.SetActive(false);
            //yield return new WaitForSeconds(1.0f);
            title2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //2todo change so this doesn't clear input history
        //debug code
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown("up"))
            {
                codeEntered += "U";
            }
            if (Input.GetKeyDown("down"))
            {
                codeEntered += "D";
            }
            if (Input.GetKeyDown("left"))
            {
                codeEntered += "L";
            }
            if (Input.GetKeyDown("right"))
            {
                codeEntered += "R";
            }
            if (Input.GetKeyDown("b"))
            {
                codeEntered += "B";
            }
            if (Input.GetKeyDown("a"))
            {
                codeEntered += "A";
            }
            if (Input.GetKeyDown("s"))
            {
                codeEntered += "S";
            }

            if (codeEntered == konamiCode)
            {
                codeEntered = "";
                GameObject exploder = Instantiate(explosion, mainCamera.transform.position + mainCamera.transform.forward, Quaternion.identity);
                exploder.transform.localScale += new Vector3(25, 8, 0);
                exploder.transform.LookAt(mainCamera.transform, new Vector3(0, 0, 1));

                exploder.GetComponent<UnityEngine.Video.VideoPlayer>().frame = 7;
                exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                exploder.GetComponent<Renderer>().enabled = true;

                StartCoroutine(DelaySetDebugActive());

                IEnumerator DelaySetDebugActive()
                {
                    yield return new WaitForSeconds(0.5f);
                    ogDebugStuff.SetActive(true);
                }
            }

            while (codeEntered.Length > 0 && !konamiCode.StartsWith(codeEntered))
            {
                codeEntered = codeEntered.Substring(1);
            }

            konamiText.GetComponent<Text>().text = codeEntered;
        }


        //normal go play
        if (Currents.currentGameState == GameState.CanPlaceStone && !ThrowingData.isOgFired)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            BoardCoordinates possibleStoneCoordinates =
                new BoardCoordinates
                (
                    Convert.ToInt32(mousePos.x / GoBoard.boardCoordinateSeparationX),
                   -Convert.ToInt32(mousePos.y / GoBoard.boardCoordinateSeparationY)
                );

            if (Convert.ToInt32(mousePos.x / GoBoard.boardCoordinateSeparationX) < 0 ||
                Convert.ToInt32(mousePos.x / GoBoard.boardCoordinateSeparationX) > 18 ||
                Convert.ToInt32(mousePos.y / GoBoard.boardCoordinateSeparationY) > 0 ||
                Convert.ToInt32(mousePos.y / GoBoard.boardCoordinateSeparationY) < -18
                )
            {
                sensorStone.gameObject.GetComponent<Renderer>().enabled = false;
            }

            else
            {
                //3todo can improve by not checking every time
                ValidPlayData validPlayData = ValidPlayCheck(possibleStoneCoordinates, Currents.currentPlayerColor);

                if (!previousMouseCoordinates.SameCoordinatesAs(possibleStoneCoordinates))
                {
                    Currents.isValidPlay = null;
                }

                if (Currents.isValidPlay == null)
                {
                    Currents.isValidPlay = validPlayData.isValidPlayLocal;
                }

                possibleStoneCoordinates.CopyCoordinatesTo(previousMouseCoordinates);

                //0todo comment here?
                //0todo implement this
                //sensorStone.gameObject.GetComponent<Renderer>().enabled = Currents.isValidPlay;
                if (Currents.isValidPlay == true)
                {
                    sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
                }
                else if (Currents.isValidPlay == false)
                {
                    sensorStone.gameObject.GetComponent<Renderer>().enabled = false;
                }

                sensorStone.gameObject.GetComponent<Transform>().position =
                        new Vector3((float)(possibleStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
                                    (float)(-possibleStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
                                    -GoStone.ZHeightValue);
                sensorStone.gameObject.transform.rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

                if (Input.GetMouseButtonUp(0) && Currents.isValidPlay == true)
                {
                    PlaceGoStone(possibleStoneCoordinates, validPlayData.groupStonesToKill);

                    if (Currents.currentPlayerColor == StoneColor.Black)
                    { Currents.currentPlayerColor = StoneColor.White; }
                    else if (Currents.currentPlayerColor == StoneColor.White)
                    { Currents.currentPlayerColor = StoneColor.Black; }

                    if (UnityEngine.Random.Range(0, 100) < ThrowingData.ogProb)
                    {
                        //to og play
                        Currents.currentGameState = GameState.CanThrowStone;
                        Currents.curentGoStoneRotation = Quaternion.identity;
                        ThrowingData.ogProb = ThrowingData.ogBaseProb;
                        ogProbText.GetComponent<Text>().text = "Og prob: " + ThrowingData.ogProb + "%";
                        mainCamera.GetComponent<Transform>().position = defaultThrowingCamera.GetComponent<Transform>().position;
                        mainCamera.GetComponent<Transform>().rotation = defaultThrowingCamera.GetComponent<Transform>().rotation;
                        DisableButtons();
                    }

                    else
                    {
                        ThrowingData.ogProb += 8;
                        ogProbText.GetComponent<Text>().text = "Og Prob: " + ThrowingData.ogProb + "%";
                    }
                }
            }
        }

        //og play (throwing)
        else if (Currents.currentGameState == GameState.CanThrowStone)
        {
            sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
            sensorStone.gameObject.transform.position = mainCamera.GetComponent<Transform>().position + 1 * mainCamera.GetComponent<Transform>().forward;
            sensorStone.gameObject.transform.rotation = mainCamera.transform.rotation * Currents.curentGoStoneRotation;

            mainCamera.orthographic = false;

            CameraMouseMovementData.yaw += CameraMouseMovementData.speedH * Input.GetAxis("Mouse X");
            CameraMouseMovementData.pitch -= CameraMouseMovementData.speedV * Input.GetAxis("Mouse Y");

            mainCamera.GetComponent<Transform>().Rotate(new Vector3(CameraMouseMovementData.pitch, 0, 0), Space.Self);
            mainCamera.GetComponent<Transform>().Rotate(new Vector3(0, 0, -CameraMouseMovementData.yaw), Space.World);

            CameraMouseMovementData.yaw = 0;
            CameraMouseMovementData.pitch = 0;

            Transform camtran = mainCamera.GetComponent<Transform>();

            float horizontalSpeed = 0.06f;
            float verticalSpeed = 0.03f;
            float scrollSpeed = 0.15f;
            Quaternion rotateSpeed = Quaternion.Euler(5,0,0);
            float shiftSlow = 0.4f;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                horizontalSpeed *= shiftSlow;
                verticalSpeed *= shiftSlow;
                scrollSpeed *= shiftSlow;
            }
            
            if (Input.GetKey("w"))
            {
                camtran.position += horizontalSpeed * (new Vector3(camtran.forward.x, camtran.forward.y, 0)).normalized;
            }
            if (Input.GetKey("a"))
            {
                camtran.position += horizontalSpeed * (new Vector3(-camtran.right.x, -camtran.right.y, 0)).normalized;
            }
            if (Input.GetKey("s"))
            {
                camtran.position += horizontalSpeed * (new Vector3(-camtran.forward.x, -camtran.forward.y, 0)).normalized;
            }
            if (Input.GetKey("d"))
            {
                camtran.position += horizontalSpeed * (new Vector3(camtran.right.x, camtran.right.y, 0)).normalized;
            }
            if (Input.GetKey("e"))
            {
                Currents.curentGoStoneRotation *= rotateSpeed;
            }
            if (Input.GetKey("q"))
            {
                Currents.curentGoStoneRotation *= Quaternion.Inverse( rotateSpeed );
            }
            if (Input.GetKey(KeyCode.LeftControl) && camtran.position.z < GoStone.ZHeightValue)
            {
                camtran.position += verticalSpeed * (new Vector3(0, 0, 1));
            }
            if (Input.GetKey("space") && camtran.position.z > -5)
            {
                camtran.position += verticalSpeed * (new Vector3(0, 0, -1));
            }
            if (Input.mouseScrollDelta.y > 0)
            {
                camtran.position += scrollSpeed * camtran.forward;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                camtran.position += scrollSpeed * -camtran.forward;
            }

            if (Input.GetMouseButtonUp(0) && !isOgNowFirstFrame)
            {
                ThrowGoStone(sensorStone.gameObject.transform.position,
                             mainCamera.transform.rotation * Currents.curentGoStoneRotation,
                             ThrowingData.ogVelocity * mainCamera.transform.forward);
            }
            isOgNowFirstFrame = false;
        }

        else if (ThrowingData.isOgFired)
        {
            if (thrownStone.gameObject != null)
            {
                mainCamera.GetComponent<Transform>().position = Vector3.Lerp(mainCamera.GetComponent<Transform>().position,
                    thrownStone.gameObject.GetComponent<Transform>().position - 1.5f * mainCamera.GetComponent<Transform>().forward,
                    0.04f);

                if (Vector3.Angle(mainCamera.GetComponent<Transform>().forward, (Vector3.forward)) < 60)
                {
                    mainCamera.GetComponent<Transform>().localRotation = Quaternion.Lerp(mainCamera.GetComponent<Transform>().localRotation,
                                                                                mainCamera.GetComponent<Transform>().localRotation * Quaternion.Euler(-45, 0, 0),
                                                                                0.02f);
                }
            }

            if (Time.time - timer > 2 && Currents.currentGameState == GameState.StoneHasBeenThrown)
            {
                GetNewBoardLayout();

                if (Currents.currentPlayerColor == StoneColor.Black)
                { Currents.currentPlayerColor = StoneColor.White; }
                else if (Currents.currentPlayerColor == StoneColor.White)
                { Currents.currentPlayerColor = StoneColor.Black; }

                Currents.currentGameState = GameState.StonesHaveBeenSorted;
            }

            if (Time.time - timer > 3 && Currents.currentGameState == GameState.StonesHaveBeenSorted)
            {
                //stay in og play for other player
                if (ThrowingData.isOnFirstOgPlay)
                {
                    Currents.currentGameState = GameState.CanThrowStone;
                    Currents.curentGoStoneRotation = Quaternion.identity;

                    ThrowingData.ogProb = ThrowingData.ogBaseProb;
                    ogProbText.GetComponent<Text>().text = "Og Prob: " + ThrowingData.ogProb + "%";
                    mainCamera.GetComponent<Transform>().position = defaultThrowingCamera.GetComponent<Transform>().position;
                    mainCamera.GetComponent<Transform>().rotation = defaultThrowingCamera.GetComponent<Transform>().rotation;
                    DisableButtons();

                    sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
                    ThrowingData.isOnFirstOgPlay = false;
                }

                //back to go play
                else
                {
                    Currents.currentGameState = GameState.CanPlaceStone;
                    ThrowingData.isOgFired = false;

                    mainCamera.GetComponent<Transform>().position = defaultCameraPosition;
                    mainCamera.GetComponent<Transform>().rotation = defaultCameraRotation;
                    mainCamera.orthographic = true;
                    EnableButtons();

                    ThrowingData.isOnFirstOgPlay = true;
                }
            }
        }
    }

    public void PlaceGoStone(BoardCoordinates newStoneCoordinates, List<BoardCoordinates> groupStonesToKill)
    {
        Currents.isValidPlay = false;

        BoardHistory.Add(new GoBoard());

        for (int i = 0; i < PreviousBoardStones().Count; i++)
        {
            LatestBoardStones().Add(PreviousBoardStones()[i]);
        }

        if (groupStonesToKill != null)
        {
            KillGroupStones(groupStonesToKill);
        }

        sensorStone.gameObject.GetComponent<Renderer>().enabled = false;

        GameObject newStoneObject = Instantiate(genericStoneObject,
                                          new Vector3((float)(newStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
                                                      (float)(-newStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
                                                      -GoStone.ZHeightValue),
                                          Quaternion.identity);

        newStoneObject.name = $"{newStoneCoordinates.xCoord}x" +
                              $"{newStoneCoordinates.yCoord}x" +
                              $"{Currents.currentPlayerColor}Stone";
        newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);



        if (Currents.currentPlayerColor == StoneColor.Black)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material = whiteMaterialTransparent;
            sensorStone.gameObject.name = "WhiteSensorStone";
        }
        else if (Currents.currentPlayerColor == StoneColor.White)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterialTransparent;
            sensorStone.gameObject.name = "BlackSensorStone";
        }

        //0todo format this kind of stuff better
        LatestBoardStones().Add(new GoStone(
            newStoneCoordinates,
            Currents.currentPlayerColor,
            newStoneObject));
    }

    //2todo make unit tests for ThrowGoStone?
    private void ThrowGoStone(Vector3 StonePosition,
                              Quaternion StoneRotation,
                              Vector3 StoneVelocity)
    {
        //0todo put this in constructor
        thrownStone = new GoStone();

        Transform thrownTransform = thrownStone.gameObject.GetComponent<Transform>();
        Rigidbody thrownRigidbody = thrownStone.gameObject.GetComponent<Rigidbody>();
        Renderer sensorRenderer = sensorStone.gameObject.GetComponent<Renderer>();

        thrownTransform.position = StonePosition;
        thrownTransform.rotation = StoneRotation;
        thrownRigidbody.velocity = StoneVelocity;
        sensorRenderer.enabled = false;
        
        if (Currents.currentPlayerColor == StoneColor.Black)
        {
            thrownStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.50f);
            sensorStone.gameObject.name = "WhiteSensorStone";
            thrownStone.gameObject.name = "19x19xBlackThrownStone";
        }
        else if (Currents.currentPlayerColor == StoneColor.White)
        {
            thrownStone.gameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 0f, 0.50f);
            sensorStone.gameObject.name = "BlackSensorStone";
            thrownStone.gameObject.name = "19x19xWhiteThrownStone";
        }

        Currents.currentGameState = GameState.StoneHasBeenThrown;
        ThrowingData.isOgFired = true;
        timer = Time.time;
    }

    //0todo put gostone check stuff in separate file
    public ValidPlayData ValidPlayCheck(BoardCoordinates newStoneCoordinates, StoneColor newStoneColor)
    {
        GoStoneLite newStone = new GoStoneLite(newStoneCoordinates, newStoneColor);

        List<BoardCoordinates> newGroupStonesToKill = new List<BoardCoordinates>();

        if (LatestBoardStones().Find(stone => (stone.SameCoordinatesAs(newStoneCoordinates))) != null)
        {
            return new ValidPlayData() { isValidPlayLocal = false };
        }

        List<GoStoneLite> boardIfStoneIsPlayed = new List<GoStoneLite>();

        //0todo look for deep level clone
        for (int i = 0; i < LatestBoardStones().Count(); i++)
        {
            boardIfStoneIsPlayed.Add(new GoStoneLite
                (LatestBoardStones()[i]));
        }

        boardIfStoneIsPlayed.Add(new GoStoneLite
            (newStoneCoordinates, Currents.currentPlayerColor));

        //Simple Ko rule
        bool isSameBoard = IsSameBoardSimpleCheck(boardIfStoneIsPlayed);
        if (isSameBoard) { return new ValidPlayData() { isValidPlayLocal = false }; }

        string openSides = OpenSidesCheck(newStone, boardIfStoneIsPlayed, newGroupStonesToKill);
        if (openSides.Length > 0)
        { return new ValidPlayData() { isValidPlayLocal = true, groupStonesToKill = newGroupStonesToKill }; }
        else { return new ValidPlayData() { isValidPlayLocal = false }; }
    }

    public bool IsSameBoardSimpleCheck(List<GoStoneLite> boardIfStoneIsPlayed)
    {
        bool isSameBoard = false;
        if (BoardHistory.Count > 3
            && boardIfStoneIsPlayed.Count == PreviousBoardStones().Count
            )
        {
            isSameBoard = true;
            for (int i = 0; i < PreviousBoardStones().Count; i++)
            {
                if (boardIfStoneIsPlayed.Find(stone => stone.SameCoordinatesAndColorAs(PreviousBoardStones()[i])) == null)
                {
                    isSameBoard = false;
                    break;
                }
            }
        }
        return isSameBoard;
    }

    private string OpenSidesCheck(GoStoneLite centerStone,
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


    //0todo pass global variables as arguments
    //0todo have better names
    //0todo rename centerStone
    private bool LibertiesFromSideExists(GoStoneLite centerStone,
                                         BoardCoordinates offsetFromCenterStone,
                                         List<GoStoneLite> boardIfStoneIsPlayed,
                                         List<BoardCoordinates> groupStonesToKill)
    {
        GoStoneGroup stoneGroup = new GoStoneGroup();

        BoardCoordinates offsetCoordinatesToCheck = centerStone.Coordinates + offsetFromCenterStone;

        GoStoneLite sideStone = ToLite(
            LatestBoardStones().Find(stone => stone.SameCoordinatesAs(offsetCoordinatesToCheck)));

        if (boardIfStoneIsPlayed.Find(stone => (stone.SameCoordinatesAs(sideStone))) == null)
        {
            return true;
        }

        stoneGroup = FindGroupAndLibertyCoordinates(sideStone, boardIfStoneIsPlayed, stoneGroup);

        if (sideStone == null)
        {
            return true;
        }
        else if (stoneGroup.libertyCoordinates.Count > 0 && sideStone.stoneColor == centerStone.stoneColor)
        {
            return true;
        }
        else if (stoneGroup.libertyCoordinates.Count > 0 && sideStone.stoneColor != centerStone.stoneColor)
        {
            return false;
        }
        else if (stoneGroup.libertyCoordinates.Count == 0 && sideStone.stoneColor == centerStone.stoneColor)
        {
            return false;
        }
        else if (stoneGroup.libertyCoordinates.Count == 0 && sideStone.stoneColor != centerStone.stoneColor)
        {
            print(groupStonesToKill.Count);

            foreach (BoardCoordinates stone in stoneGroup.stones)
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

    private GoStoneGroup FindGroupAndLibertyCoordinates(GoStoneLite sideStone,
                                                List<GoStoneLite> boardIfStoneIsPlayed,
                                                GoStoneGroup stoneGroup)

    {
        if (stoneGroup.stones.Find(groupStone => (groupStone.SameCoordinatesAs(sideStone))) == null)
        {
            stoneGroup.stones.Add(new BoardCoordinates(sideStone));
        }

        FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.yCoord > 0), StoneDirectionOffset.up, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.yCoord < 18), StoneDirectionOffset.down, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.xCoord > 0), StoneDirectionOffset.left, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(sideStone, boardIfStoneIsPlayed, (sideStone.Coordinates.xCoord < 18), StoneDirectionOffset.right, ref stoneGroup);

        return stoneGroup;
    }

    private void FindGroupAndLibertyCoordinatesSideStone(GoStoneLite sideStone,
                                                         List<GoStoneLite> boardIfStoneIsPlayed,
                                                         bool isPositionGood,
                                                         BoardCoordinates offset,
                                                         ref GoStoneGroup stoneGroup)
    {
        if (isPositionGood)
        {
            //GoStoneLite sideStone = boardIfStoneIsPlayed.Find(stoneIf => (stoneIf.Coordinates.SameCoordinatesAs(sideStoneCoordinates)));
            BoardCoordinates offsetCoordinatesToCheck = sideStone.Coordinates + offset;
            GoStoneLite otherStone = boardIfStoneIsPlayed.Find(stoneIf => (stoneIf.SameCoordinatesAs(offsetCoordinatesToCheck)));

            if (sideStone != null
                &&
                otherStone != null
                &&
                otherStone.stoneColor == sideStone.stoneColor
                &&
                stoneGroup.stones.Find(coord => (coord.SameCoordinatesAs(offsetCoordinatesToCheck))) == null
                )
            {
                stoneGroup = FindGroupAndLibertyCoordinates(new GoStoneLite(offsetCoordinatesToCheck),
                             boardIfStoneIsPlayed,
                             stoneGroup);
            }
            else if ((otherStone == null)
                     &&
                     (stoneGroup.libertyCoordinates.Find(libertyCoord => (libertyCoord.SameCoordinatesAs(offsetCoordinatesToCheck)))) == null)

            {
                stoneGroup.libertyCoordinates.Add(new BoardCoordinates
                (offsetCoordinatesToCheck));
            }
        }
    }

    public void GetNewBoardLayout()
    {
        BoardHistory.Add(new GoBoard());

        //finds stones in layer 8, "Stone"
        Collider[] sortedStonesInRange = Physics.OverlapSphere(new Vector3(0, 0, 0), 10, LayerMask.GetMask("Stone"));

        foreach (Collider sortedStone in sortedStonesInRange)
        {
            //changes layer to "SortingStone"
            sortedStone.gameObject.layer = 9;
        }

        FindAndSortAllStones();
        KillSortedStones();
    }

    public void FindAndSortAllStones()
    {
        int searchIncrement = 200;
        for (float r = 1; r < searchIncrement; r++)
        {
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {
                    float boardDiagonalLength = GoBoard.boardCoordinateSeparationY * 19 * 1.42f;

                    Collider[] stonesInRange = Physics.OverlapSphere(new Vector3(GoBoard.boardCoordinateSeparationX * iteratedX,
                                                                                -GoBoard.boardCoordinateSeparationY * iteratedY,
                                                                                 0),
                                                                     boardDiagonalLength * (r / searchIncrement),
                                                                     LayerMask.GetMask("SortingStone"));

                    if (stonesInRange.Length == 0)
                    {
                        continue;
                    }
                    if (stonesInRange[0].name.Contains("Sensor"))
                    {
                        continue;
                    }

                    string foundStoneColor = stonesInRange[0].name.Contains("Black") ? "Black" : "White";

                    stonesInRange[0].name = $"{iteratedX}x{iteratedY}x{foundStoneColor}Stone";

                    GoStone sortedStone = SortStone(stonesInRange[0], iteratedX, iteratedY);
                }
            }
        }
    }

    public GoStone SortStone(Collider stoneToSort,
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

        //0todo have construct different?
        LatestBoardStones().Add(new GoStone(new BoardCoordinates(CoordinateX, 
                                                                 CoordinateY),
                                            stoneToSortColor, 
                                            stoneToSort.gameObject));

        //changes layer to "Stone"
        stoneToSort.gameObject.layer = 8;

        //0todo use this return value
        return new GoStone(new BoardCoordinates(CoordinateX, 
                                                CoordinateY), 
                           stoneToSortColor, 
                           stoneToSort.gameObject);
    }

    public void KillSortedStones()
    {
        List<BoardCoordinates> alreadyGroupedStones = new List<BoardCoordinates>();

        List<StoneColor> StoneColors = new List<StoneColor>() {
                                            Currents.currentPlayerColor == StoneColor.White ? StoneColor.Black :StoneColor.White
                                            ,
                                            Currents.currentPlayerColor
        };

        //3todo improve this?
        foreach (StoneColor iteratedColor in StoneColors)
        {
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {
                    GoStone localStone = LatestBoardStones().Find(latestStone => (latestStone.Coordinates.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))));

                    if (localStone == null)
                    {
                        continue;
                    }

                    if (localStone.stoneColor != iteratedColor ||
                        alreadyGroupedStones.Find(groupedStone => groupedStone.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))) != null)
                    {
                        continue;
                    }

                    GoStoneGroup stoneGroup = new GoStoneGroup();

                    List<GoStoneLite> LatestBoardStonesLite = new List<GoStoneLite>();
                    foreach (GoStone stone in LatestBoardStones())
                    {
                        LatestBoardStonesLite.Add(ToLite(stone));
                    }

                    //return groupstonestokill?
                    //3todo make sure all FindGroupAndLibertyCoordinates are passing color in?
                    stoneGroup = FindGroupAndLibertyCoordinates(new GoStoneLite
                        (iteratedX, iteratedY, localStone.stoneColor),
                        LatestBoardStonesLite,
                        stoneGroup
                    );

                    foreach (BoardCoordinates newStone in stoneGroup.stones)
                    {
                        if (!alreadyGroupedStones.Any(groupedStone => groupedStone.SameCoordinatesAs(newStone)))
                        {
                            alreadyGroupedStones.Add(newStone);
                        }
                    }

                    if (stoneGroup.libertyCoordinates.Count == 0)
                    {
                        KillGroupStones(stoneGroup.stones);
                    }
                }
            }
        }
    }

    public void KillGroupStones(List<BoardCoordinates> groupStonesToKill)
    {
        foreach (BoardCoordinates killCoord in groupStonesToKill)
        {
            GoStone stoneToDestroy = LatestBoardStones().Find(latestStone => (latestStone.SameCoordinatesAs(killCoord)));

            if (stoneToDestroy == null) { continue; }

            KillStoneWithDelay(
                          stoneToDestroy,
                         0.2f,
                         0.2f * groupStonesToKill.IndexOf(killCoord));
        }
    }

    public void KillStoneWithDelay(GoStone StoneToDestroy,
                                   float destroyDelay,
                                   float totalDelay = 0)
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
        else { print("NO COLOR SET FOR DESTRUCTION"); }

        StoneToDestroy.gameObject.layer = 8;

        //1todo use something other than Text?
        Text coroutineHandler = (new GameObject("_coroutineHandler")).AddComponent<Text>();
        coroutineHandler.StartCoroutine(DelayDestroyCoroutine());

        IEnumerator DelayDestroyCoroutine()
        {
            yield return new WaitForSeconds(totalDelay);
            //yield return new WaitForSeconds(entireDelay);

            GameObject exploder = StoneToDestroy.exploderGameObject;
            exploder = Instantiate(explosion, new Vector3(0, 0, 0), Quaternion.identity);

            exploder.GetComponent<Renderer>().enabled = false;
            exploder.name = $"{StoneToDestroy.gameObject.name}xEXploder";
            exploder.transform.parent = explosionObjectParent.transform;
            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().frame = 7;
            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Play();

            yield return new WaitForSeconds(0.2f);

            exploder.transform.position = StoneToDestroy.gameObject.transform.position;
            exploder.GetComponent<Renderer>().enabled = true;
            exploder.transform.LookAt(mainCamera.transform, new Vector3(0, 0, 1));
            exploder.transform.rotation = mainCamera.transform.rotation;

            Destroy(StoneToDestroy.gameObject);

            Currents.isValidPlay = null;

            yield return new WaitForSeconds(7);
            exploder.GetComponent<Renderer>().enabled = false;
            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
        }
    }

    public void PlusOneToScore(StoneColor stoneColor)
    {
        if (stoneColor == StoneColor.Black)
        {
            ChangeFloatAndText(ref PlayerScore.blackScore, 1, blackTextObject, "Black Captures: ", false);
        }
        if (stoneColor == StoneColor.White)
        {
            ChangeFloatAndText(ref PlayerScore.whiteScore, 1, whiteTextObject, "White Captures: ", false);
        }
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

        public GoStoneLite(BoardCoordinates newCoordinates)
        {
            Coordinates = newCoordinates;
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



        //0todo use this in constructor
        //public StoneColor stoneColor = StoneColor.None;

        public GoStone()
        {
            Coordinates = null;
            //Coordinates = new BoardCoordinates(-1,-1);

            gameObject = Instantiate(genericStoneObject, new Vector3(0, 0, 0), Quaternion.identity);

            //gameObject = Instantiate(genericStoneObjectInstance, new Vector3(0, 0, 0), Quaternion.identity);
            //gameObject = Instantiate(Resources.Load("Assets/Resources/Stone") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
            //gameObject.gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.name = "No Name";
            //gameObject.GetComponent<MeshRenderer>().material = Resources.Load("TransparentBlack") as Material;

            //exploderGameObject.GetComponent<Renderer>().enabled = false
        }


        public GoStone(BoardCoordinates newCoordinates, GameObject newGameObject)
        {
            Coordinates = newCoordinates;

            if (genericStoneObject == null)
            {
                genericStoneObject = new GameObject();
            }

            gameObject = newGameObject;
        }

        public GoStone(BoardCoordinates newCoordinates, StoneColor newColor,  GameObject newGameObject)
        {
            Coordinates = newCoordinates;
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

    public class GoStoneGroup
    {
        //0todo rename stones to stoneCoords? here and in instances of GoStoneGroup
        
        public List<BoardCoordinates> stones = new List<BoardCoordinates>();
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

    //0todo implememt this?
    //public interface IPlay
    //{

    //}

    public class ValidPlayData
    {
        public bool isValidPlayLocal;
        public List<BoardCoordinates> groupStonesToKill;
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




    private void OgNow()
    {
        Currents.currentGameState = GameState.CanThrowStone;
        Currents.curentGoStoneRotation = Quaternion.identity;
        isOgNowFirstFrame = true;
        ThrowingData.ogProb = ThrowingData.ogBaseProb;
        ogProbText.GetComponent<Text>().text = "Og prob: " + ThrowingData.ogProb + "%";
        mainCamera.GetComponent<Transform>().position = defaultThrowingCamera.GetComponent<Transform>().position;
        mainCamera.GetComponent<Transform>().rotation = defaultThrowingCamera.GetComponent<Transform>().rotation;
        sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
        DisableButtons();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene("og");
    }

    private void EnableButtons()
    {
        resetButton.enabled = true;
        ogNowButton.enabled = true;
        plusOgSpeedButton.enabled = true;
        minusOgSpeedButton.enabled = true;
        plusOgBaseProbButton.enabled = true;
        minusOgBaseProbButton.enabled = true;
    }

    private void DisableButtons()
    {
        resetButton.enabled = false;
        ogNowButton.enabled = false;
        plusOgSpeedButton.enabled = false;
        minusOgSpeedButton.enabled = false;
        plusOgBaseProbButton.enabled = false;
        minusOgBaseProbButton.enabled = false;
    }

    private void ChangeFloatAndText(ref float valueToChange, int valueAdded, GameObject textObject, string text, bool isPercent)
    {
        valueToChange += valueAdded;
        textObject.GetComponent<Text>().text = text + valueToChange + (isPercent ? "%" : "");
    }
}


//Explosion effect and sound used under Creative Commons(https://creativecommons.org/licenses/by/3.0/)
//From Youtube user Timothy (https://www.youtube.com/channel/UCrxGMPla5PpIdeSyGQFaXhg) (https://www.youtube.com/watch?v=Q7KmAe8_jZE)

//Go board image used under Public Domain from Wikipedia (https://en.wikipedia.org/wiki/File:Blank_Go_board.svg)

//Sky background used from (sketchuptexture.com/2013/02/panoramic-ski-360.html)

//Original concept from Nicholas Abentroth

//Copyright 2021, Casey Keller, All rights reserved.