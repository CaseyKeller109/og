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

    public List<GoBoard> BoardHistory = new List<GoBoard>();

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

    public static class CurrentStateData
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

    public static class StoneDirectionOffset
    {
        public static BoardCoordinates left = new BoardCoordinates { x = -1, y = 0 };
        public static BoardCoordinates right = new BoardCoordinates { x = +1, y = 0 };
        public static BoardCoordinates up = new BoardCoordinates { x = 0, y = +1 };
        public static BoardCoordinates down = new BoardCoordinates { x = 0, y = -1 };
    }

    private void Awake()
    {
        genericStoneObject = Resources.Load("Stone") as GameObject;

        previousMouseCoordinates = new BoardCoordinates();
    }

    // Start is called before the first frame update
    void Start()
    {
        Console.Write("hello world");

        Application.targetFrameRate = 60;
        resetButton.onClick.AddListener(ResetGame);

        //0todo put stuff in constructor
        sensorStone = new GoStone(new BoardCoordinates { x = 8, y = 8 }, sensorStoneObject);

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

        //0todo set these in editor
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
        if (CurrentStateData.currentGameState == GameState.CanPlaceStone && !ThrowingData.isOgFired)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            BoardCoordinates possibleStoneCoordinates =
                new BoardCoordinates
                {
                    x = Convert.ToInt32(mousePos.x / GoBoard.boardCoordinateSeparationX),
                    y = -Convert.ToInt32(mousePos.y / GoBoard.boardCoordinateSeparationY)

                };

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
                ValidPlayData validPlayData = ValidPlayCheck(possibleStoneCoordinates, CurrentStateData.currentPlayerColor);

                if (!previousMouseCoordinates.SameCoordinatesAs(possibleStoneCoordinates))
                {
                    CurrentStateData.isValidPlay = null;
                }

                if (CurrentStateData.isValidPlay == null)
                {
                    CurrentStateData.isValidPlay = validPlayData.isValidPlayLocal;
                }

                possibleStoneCoordinates.CopyCoordinatesTo(previousMouseCoordinates);

                //0todo comment here?
                //0todo implement this
                //sensorStone.gameObject.GetComponent<Renderer>().enabled = CurrentStateData.isValidPlay;
                if (CurrentStateData.isValidPlay == true)
                {
                    sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
                }
                else if (CurrentStateData.isValidPlay == false)
                {
                    sensorStone.gameObject.GetComponent<Renderer>().enabled = false;
                }

                //0todo don't use GoStone for possibleStoneCoordinates?
                sensorStone.gameObject.GetComponent<Transform>().position =
                        new Vector3(possibleStoneCoordinates.x * GoBoard.boardCoordinateSeparationX,
                                   -possibleStoneCoordinates.y * GoBoard.boardCoordinateSeparationY,
                                   -GoStone.ZHeightValue);
                sensorStone.gameObject.transform.rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

                if (Input.GetMouseButtonUp(0) && CurrentStateData.isValidPlay == true)
                {
                    PlaceGoStone(possibleStoneCoordinates, validPlayData.groupStonesToKill);

                    if (CurrentStateData.currentPlayerColor == StoneColor.Black)
                    { CurrentStateData.currentPlayerColor = StoneColor.White; }
                    else if (CurrentStateData.currentPlayerColor == StoneColor.White)
                    { CurrentStateData.currentPlayerColor = StoneColor.Black; }

                    if (UnityEngine.Random.Range(0, 100) < ThrowingData.ogProb)
                    {
                        //to og play
                        CurrentStateData.currentGameState = GameState.CanThrowStone;
                        CurrentStateData.curentGoStoneRotation = Quaternion.identity;
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
        else if (CurrentStateData.currentGameState == GameState.CanThrowStone)
        {
            sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
            sensorStone.gameObject.transform.position = mainCamera.GetComponent<Transform>().position + 1 * mainCamera.GetComponent<Transform>().forward;
            sensorStone.gameObject.transform.rotation = mainCamera.transform.rotation * CurrentStateData.curentGoStoneRotation;

            mainCamera.orthographic = false;

            CameraMouseMovementData.yaw += CameraMouseMovementData.speedH * Input.GetAxis("Mouse X");
            CameraMouseMovementData.pitch -= CameraMouseMovementData.speedV * Input.GetAxis("Mouse Y");

            mainCamera.GetComponent<Transform>().Rotate(new Vector3(CameraMouseMovementData.pitch, 0, 0), Space.Self);
            mainCamera.GetComponent<Transform>().Rotate(new Vector3(0, 0, -CameraMouseMovementData.yaw), Space.World);

            CameraMouseMovementData.yaw = 0;
            CameraMouseMovementData.pitch = 0;

            Transform camtran = mainCamera.GetComponent<Transform>();

            if (Input.GetKey("w"))
            {
                camtran.position += 0.05f * (new Vector3(camtran.forward.x, camtran.forward.y, 0)).normalized;
            }
            if (Input.GetKey("a"))
            {
                camtran.position += 0.05f * (new Vector3(-camtran.right.x, -camtran.right.y, 0)).normalized;
            }
            if (Input.GetKey("s"))
            {
                camtran.position += 0.05f * (new Vector3(-camtran.forward.x, -camtran.forward.y, 0)).normalized;
            }
            if (Input.GetKey("d"))
            {
                camtran.position += 0.05f * (new Vector3(camtran.right.x, camtran.right.y, 0)).normalized;
            }
            if (Input.GetKey("e"))
            {
                CurrentStateData.curentGoStoneRotation *= Quaternion.Euler(5, 0, 0);
            }
            if (Input.GetKey("q"))
            {
                CurrentStateData.curentGoStoneRotation *= Quaternion.Euler(-5, 0, 0);
            }
            if (Input.GetKey(KeyCode.LeftControl) && camtran.position.z < GoStone.ZHeightValue)
            {
                camtran.position += 0.02f * (new Vector3(0, 0, 1));
            }
            if (Input.GetKey("space") && camtran.position.z > -5)
            {
                camtran.position += 0.02f * (new Vector3(0, 0, -1));
            }
            if (Input.mouseScrollDelta.y > 0)
            {
                camtran.position += 0.15f * camtran.forward;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                camtran.position += 0.15f * -camtran.forward;
            }

            if (Input.GetMouseButtonUp(0) && !isOgNowFirstFrame)
            {
                ThrowGoStone(sensorStone.gameObject.transform.position,
                             mainCamera.transform.rotation * CurrentStateData.curentGoStoneRotation,
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

            if (Time.time - timer > 2 && CurrentStateData.currentGameState == GameState.StoneHasBeenThrown)
            {
                GetNewBoardLayout();

                if (CurrentStateData.currentPlayerColor == StoneColor.Black)
                { CurrentStateData.currentPlayerColor = StoneColor.White; }
                else if (CurrentStateData.currentPlayerColor == StoneColor.White)
                { CurrentStateData.currentPlayerColor = StoneColor.Black; }

                CurrentStateData.currentGameState = GameState.StonesHaveBeenSorted;
            }

            if (Time.time - timer > 3 && CurrentStateData.currentGameState == GameState.StonesHaveBeenSorted)
            {
                //stay in og play for other player
                if (ThrowingData.isOnFirstOgPlay)
                {
                    CurrentStateData.currentGameState = GameState.CanThrowStone;
                    CurrentStateData.curentGoStoneRotation = Quaternion.identity;

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
                    CurrentStateData.currentGameState = GameState.CanPlaceStone;
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

    public void PlaceGoStone(BoardCoordinates newStoneCoordinates, List<GoStoneHypothetical> groupStonesToKill)
    {
        CurrentStateData.isValidPlay = false;

        BoardHistory.Add(new GoBoard());

        for (int i = 0; i < BoardHistory[BoardHistory.Count - 2].boardStones.Count; i++)
        {
            BoardHistory.Last().boardStones.Add(BoardHistory[BoardHistory.Count - 2].boardStones[i]);
        }

        if (groupStonesToKill != null)
        {
            KillGroupStones(groupStonesToKill);
        }

        sensorStone.gameObject.GetComponent<Renderer>().enabled = false;

        GameObject newStoneObject = Instantiate(genericStoneObject,
                                          new Vector3(newStoneCoordinates.x * GoBoard.boardCoordinateSeparationX,
                                                     -newStoneCoordinates.y * GoBoard.boardCoordinateSeparationY,
                                                     -GoStone.ZHeightValue),
                                          Quaternion.identity);

        newStoneObject.name = $"{newStoneCoordinates.x}x{newStoneCoordinates.y}x{CurrentStateData.currentPlayerColor}Stone";
        newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);



        if (CurrentStateData.currentPlayerColor == StoneColor.Black)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material = whiteMaterialTransparent;
            sensorStone.gameObject.name = "WhiteSensorStone";
        }
        else if (CurrentStateData.currentPlayerColor == StoneColor.White)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterialTransparent;
            sensorStone.gameObject.name = "BlackSensorStone";
        }

        //0todo format this kind of stuff better
        BoardHistory.Last().boardStones.Add(new GoStone(
            newStoneCoordinates,
            newStoneObject)
        {
            stoneColor = CurrentStateData.currentPlayerColor,
        });
    }

    //2todo make unit tests for ThrowGoStone?
    private void ThrowGoStone(Vector3 StonePosition,
                              Quaternion StoneRotation,
                              Vector3 StoneVelocity)
    {
        //0todo put this in constructor
        thrownStone = new GoStone();
        thrownStone.gameObject.GetComponent<Transform>().position = StonePosition;
        thrownStone.gameObject.GetComponent<Transform>().rotation = StoneRotation;
        sensorStone.gameObject.GetComponent<Renderer>().enabled = false;
        thrownStone.gameObject.GetComponent<Rigidbody>().velocity = StoneVelocity;

        if (CurrentStateData.currentPlayerColor == StoneColor.Black)
        {
            thrownStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.50f);
            sensorStone.gameObject.name = "WhiteSensorStone";
            thrownStone.gameObject.name = "19x19xBlackThrownStone";
        }
        else if (CurrentStateData.currentPlayerColor == StoneColor.White)
        {
            thrownStone.gameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 0f, 0.50f);
            sensorStone.gameObject.name = "BlackSensorStone";
            thrownStone.gameObject.name = "19x19xWhiteThrownStone";
        }

        CurrentStateData.currentGameState = GameState.StoneHasBeenThrown;
        ThrowingData.isOgFired = true;
        timer = Time.time;
    }

    //0todo put gostone check stuff in separate file
    public ValidPlayData ValidPlayCheck(BoardCoordinates newStoneCoordinates, StoneColor newStoneColor)
    {
        GoStoneHypothetical newStone = new GoStoneHypothetical(newStoneCoordinates, newStoneColor);

        List<GoStoneHypothetical> groupStonesToKill = new List<GoStoneHypothetical>();

        if (BoardHistory.Last().boardStones.Find(stone => (stone.SameCoordinatesAs(newStoneCoordinates))) != null)
        {
            return new ValidPlayData() { isValidPlayLocal = false };
        }
        
        List<GoStoneHypothetical> boardIfStoneIsPlayed = new List<GoStoneHypothetical>();

        //0todo look for deep level clone
        for (int i = 0; i < BoardHistory.Last().boardStones.Count(); i++)
        {            
            boardIfStoneIsPlayed.Add(new GoStoneHypothetical
                (BoardHistory.Last().boardStones[i]));
        }

        boardIfStoneIsPlayed.Add(new GoStoneHypothetical
            (newStoneCoordinates, CurrentStateData.currentPlayerColor));
                
        //Simple Ko rule
        bool isSameBoard = IsSameBoardSimpleCheck(boardIfStoneIsPlayed);
        if (isSameBoard) { return new ValidPlayData() { isValidPlayLocal = false }; }

        string openSides = OpenSidesCheck(newStone, boardIfStoneIsPlayed, groupStonesToKill);
        if (openSides.Length > 0)
        { return new ValidPlayData() { isValidPlayLocal = true, groupStonesToKill = groupStonesToKill }; }
        else { return new ValidPlayData() { isValidPlayLocal = false }; }
    }

    private string OpenSidesCheck(GoStoneHypothetical centerStone,
                                  List<GoStoneHypothetical> boardIfStoneIsPlayed,
                                  List<GoStoneHypothetical> groupStonesToKill)
    {
        string openSides = "";

        //top side
        if (centerStone.Coordinates.y > 0)
        {
            if (LibertiesFromSideExists(centerStone, new BoardCoordinates { x = 0, y = -1 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "top";
            }
        }

        //bottom side
        if (centerStone.Coordinates.y < 18)
        {
            if (LibertiesFromSideExists(centerStone, new BoardCoordinates { x = 0, y = 1 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "bottom";
            }
        }

        //left side
        if (centerStone.Coordinates.x > 0)
        {
            if (LibertiesFromSideExists(centerStone, new BoardCoordinates { x = -1, y = 0 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "left";
            }
        }

        //right side
        if (centerStone.Coordinates.x < 18)
        {
            if (LibertiesFromSideExists(centerStone, new BoardCoordinates { x = 1, y = 0 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "right";
            }
        }
        return openSides;
    }

    public bool IsSameBoardSimpleCheck(List<GoStoneHypothetical> boardIfStoneIsPlayed)
    {
        bool isSameBoard = false;
        if (BoardHistory.Count > 3
            && boardIfStoneIsPlayed.Count == BoardHistory[BoardHistory.Count - 2].boardStones.Count
            )
        {
            isSameBoard = true;
            for (int i = 0; i < BoardHistory[BoardHistory.Count - 2].boardStones.Count; i++)
            {
                //0todo use equals method for this? == or .Equals

                if (boardIfStoneIsPlayed.Find(stone => stone.SameCoordinatesAndColorAs(BoardHistory[BoardHistory.Count - 2].boardStones[i])) == null)
                {
                    isSameBoard = false;
                    break;
                }
            }
        }
        return isSameBoard;
    }

    //0todo pass global variables as arguments
    //0todo have better names
    //0todo rename centerStone
    private bool LibertiesFromSideExists(GoStoneHypothetical centerStone,
                                         BoardCoordinates offsetFromCenterStone,
                                         List<GoStoneHypothetical> boardIfStoneIsPlayed,
                                         List<GoStoneHypothetical> groupStonesToKill)
    {
        GoStoneGroup stoneGroup = new GoStoneGroup();
                
        GoStoneHypothetical sideStone = new GoStoneHypothetical();

        GoStone sideStoneReal = BoardHistory.Last().boardStones.Find(stone => stone.SameCoordinatesAs(centerStone.Coordinates + offsetFromCenterStone));

        if (sideStoneReal != null)
        {
            //0todo improve this
            sideStone.Coordinates = sideStoneReal.Coordinates;
            sideStone.stoneColor = sideStoneReal.stoneColor;
        }

        //0todo return false if null?
        //if (sideStone == null)
        //{
        //    sideStone = new GoStoneHypothetical();
        //}

        if (boardIfStoneIsPlayed.Find(stone => (stone.SameCoordinatesAs(sideStone))) == null)
        {
            return true;
        }
        
        FindGroupAndLibertyCoordinates(sideStone, boardIfStoneIsPlayed, ref stoneGroup);

        if (sideStone.stoneColor == StoneColor.None)
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
            foreach (GoStoneHypothetical stone in stoneGroup.stones)
            {
                groupStonesToKill.Add(stone);
            }

            foreach (GoStoneHypothetical stone in groupStonesToKill)
            {
                boardIfStoneIsPlayed.Remove(boardIfStoneIsPlayed.Find(QStone => (QStone.SameCoordinatesAs(stone))));
            }

            return true;
        }
        else { return false; }
    }

    //starts searching at centerStoneCoordinates
    private void FindGroupAndLibertyCoordinates(GoStoneHypothetical startStone,
                                                List<GoStoneHypothetical> boardIfStoneIsPlayed,
                                                ref GoStoneGroup stoneGroup)
    {
        if (stoneGroup.stones.Find(QStone => (QStone.SameCoordinatesAs(startStone))) == null)
        {


            stoneGroup.stones.Add(new GoStoneHypothetical(startStone));
        }

        FindGroupAndLibertyCoordinatesSideStone(startStone.Coordinates, boardIfStoneIsPlayed, (startStone.Coordinates.x > 0), StoneDirectionOffset.left, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone.Coordinates, boardIfStoneIsPlayed, (startStone.Coordinates.x < 18), StoneDirectionOffset.right, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone.Coordinates, boardIfStoneIsPlayed, (startStone.Coordinates.y < 18), StoneDirectionOffset.up, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone.Coordinates, boardIfStoneIsPlayed, (startStone.Coordinates.y > 0), StoneDirectionOffset.down, ref stoneGroup);
    }

    private void FindGroupAndLibertyCoordinatesSideStone(BoardCoordinates sideStoneCoordinates,
                                                         List<GoStoneHypothetical> boardIfStoneIsPlayed,
                                                         bool isPositionGood,
                                                         BoardCoordinates offset,
                                                         ref GoStoneGroup stoneGroup)
    {
        if (isPositionGood)
        {
            GoStoneHypothetical sideStone = boardIfStoneIsPlayed.Find(QStone => (QStone.Coordinates.SameCoordinatesAs(sideStoneCoordinates)));

            if (sideStone == null)
            {
                sideStone = new GoStoneHypothetical();
            }

            GoStoneHypothetical otherStone = boardIfStoneIsPlayed.Find(QStone => (QStone.SameCoordinatesAs(sideStone.Coordinates + offset)));


            if (otherStone == null)
            {
                otherStone = new GoStoneHypothetical();
            }

            if (otherStone.stoneColor == sideStone.stoneColor
                &&
                sideStone.stoneColor != StoneColor.None
                &&
                stoneGroup.stones.Find(QStone => (QStone.Coordinates.SameCoordinatesAs(sideStone.Coordinates + offset))) == null
                )
            {
                FindGroupAndLibertyCoordinates(new GoStoneHypothetical(sideStone.Coordinates + offset),
                boardIfStoneIsPlayed,
                ref stoneGroup);
            }
            else if ((otherStone.stoneColor == StoneColor.None)
                     &&
                     (stoneGroup.libertyCoordinates.Find(Qcoord => (Qcoord.SameCoordinatesAs(sideStone.Coordinates + offset)))) == null)

            {
                stoneGroup.libertyCoordinates.Add(new BoardCoordinates
                (sideStone.Coordinates + offset));
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
                    GoStoneHypothetical localStone = new GoStoneHypothetical();
                    //GoStone localStoneReal = BoardHistory.Last().Find(s => s.coordinates.x == iteratedX &&
                    //                                                          s.coordinates.y == iteratedY);





                    if (BoardHistory.Last().boardStones.Find(QStone => QStone.Coordinates.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))) == null) { localStone = new GoStoneHypothetical(); }


                    //if (localStoneReal == null) { localStone = new GoStoneHypothetical(); }

                    if (localStone.stoneColor != StoneColor.None) { continue; }

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

        StoneColor stoneToSortColor = StoneColor.None;



        stoneToSortColor = stoneToSort.name.Contains("Black") ? StoneColor.Black : StoneColor.White;

        //0todo have construct different?
        BoardHistory.Last().boardStones.Add(new GoStone(new BoardCoordinates { x = CoordinateX, y = CoordinateY }, stoneToSort.gameObject) { stoneColor = stoneToSortColor });

        //changes layer to "Stone"
        stoneToSort.gameObject.layer = 8;

        //0todo use this return value
        return new GoStone(new BoardCoordinates { x = CoordinateX, y = CoordinateY }, stoneToSort.gameObject) { stoneColor = stoneToSortColor };
    }

    public void KillSortedStones()
    {
        List<GoStoneHypothetical> alreadyGroupedStones = new List<GoStoneHypothetical>();

        List<StoneColor> StoneColors = new List<StoneColor>() {
                                            CurrentStateData.currentPlayerColor == StoneColor.White ? StoneColor.Black :StoneColor.White
                                            ,
                                            CurrentStateData.currentPlayerColor
        };

        //3todo improve this?
        foreach (StoneColor iteratedColor in StoneColors)
        {
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {
                    GoStone localStone = BoardHistory.Last().boardStones.Find(QStone => (QStone.Coordinates.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))));

                    if (localStone == null)
                    {
                        continue;
                    }

                    if (localStone.stoneColor == StoneColor.None ||
                        localStone.stoneColor != iteratedColor ||
                        alreadyGroupedStones.Find(QStone => QStone.Coordinates.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))) != null)
                    {
                        continue;
                    }

                    GoStoneGroup stoneGroup = new GoStoneGroup();

                    //0todo improve this
                    List<GoStoneHypothetical> stonePosHistoryHypothetical = new List<GoStoneHypothetical>();
                    foreach (GoStone stone in BoardHistory.Last().boardStones)
                    {
                        stonePosHistoryHypothetical.Add(stone.ToHypothetical());
                    }

                    //return groupstonestokill?
                    //3todo make sure all FindGroupAndLibertyCoordinates are passing color in?
                    FindGroupAndLibertyCoordinates(new GoStoneHypothetical
                        (iteratedX, iteratedY, localStone.stoneColor),
                        stonePosHistoryHypothetical,
                        ref stoneGroup
                    );

                    foreach (GoStoneHypothetical entry in stoneGroup.stones)
                    {
                        if (!alreadyGroupedStones.Any(QStone => QStone.SameCoordinatesAndColorAs(entry)))
                        {
                            alreadyGroupedStones.Add(entry);
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

    public void KillGroupStones(List<GoStoneHypothetical> groupStonesToKill)
    {
        foreach (GoStoneHypothetical stone in groupStonesToKill)
        {
            //0todo just get color data from foreach GoStone

            GoStone stoneToDestroy = BoardHistory.Last().boardStones.Find(QStone => (QStone.SameCoordinatesAs(stone)));

            if (stoneToDestroy == null) { continue; }

            KillStoneWithDelay(
                          stoneToDestroy,
                         0.2f,
                         0.2f * groupStonesToKill.IndexOf(stone));
        }
    }

    public void KillStoneWithDelay(GoStone StoneToDestroy,
                                   float destroyDelay,
                                   float totalDelay = 0)
    {
        GoStone historyStone = BoardHistory.Last().boardStones.Find(QStone => (QStone.SameCoordinatesAs(StoneToDestroy)));

        if (historyStone != null)
        {
            BoardHistory.Last().boardStones.Remove(BoardHistory.Last().boardStones.Find(QStone => (QStone.SameCoordinatesAs(StoneToDestroy))));
        }

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

            CurrentStateData.isValidPlay = null;

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

    public class BoardCoordinates
    {
        public int x;
        public int y;

        //0todo get rid of this?
        public BoardCoordinates()
        {
        }

        public BoardCoordinates(BoardCoordinates newBoardCoordinates)
        {
            x = newBoardCoordinates.x;
            y = newBoardCoordinates.y;
        }

        public BoardCoordinates(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public static BoardCoordinates operator +(BoardCoordinates left, BoardCoordinates right)
        {
            return new BoardCoordinates(right.x + left.x, right.y + left.y);
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

        public bool SameCoordinatesAs(BoardCoordinates second)
        {
            string errorString = "";
            if (this.x != second.x)
            {
                errorString += "x values not equal. ";
            }
            if (this.y != second.y)
            {
                errorString += "y values not equal. ";
            }

            if (errorString == "") { return true; }
            else { return false; }
        }

        public void CopyCoordinatesTo(BoardCoordinates destination)
        {
            destination.x = this.x;
            destination.y = this.y;
        }
    }

    public enum StoneColor
    {
        None,
        Black,
        White
    }

    public class GoStoneHypothetical : BoardCoordinates
    {
        //0todo don't have StoneColor.None at all?
        public StoneColor stoneColor = StoneColor.None;

        public BoardCoordinates Coordinates
        {
            get { return new BoardCoordinates(x, y); }
            set { x = value.x; y = value.y; }
        }

        //0todo get rid of this?
        public GoStoneHypothetical()
        {
        }

        public GoStoneHypothetical(GoStone newGoStone)
        {
            Coordinates = newGoStone.Coordinates;
            stoneColor = newGoStone.stoneColor;
        }

        public GoStoneHypothetical(BoardCoordinates newCoordinates)
        {
            Coordinates = newCoordinates;
        }


        public GoStoneHypothetical(BoardCoordinates newCoordinates, StoneColor newStoneColor)
        {
            Coordinates = new BoardCoordinates(newCoordinates.x, newCoordinates.y);
            stoneColor = newStoneColor;
        }

        public GoStoneHypothetical(int newX, int newY, StoneColor newStoneColor)
        {
            Coordinates = new BoardCoordinates(newX, newY);
            stoneColor = newStoneColor;
        }



        public bool SameColorAs(GoStoneHypothetical otherStone)
        {
            string errorString = "";
            if (this.stoneColor != otherStone.stoneColor)
            {
                errorString += "colors not same. ";
            }

            if (errorString == "") { return true; }
            else { return false; }
        }

        public bool SameCoordinatesAndColorAs(GoStoneHypothetical otherStone)
        {
            return (this.SameCoordinatesAs(otherStone) && this.SameColorAs(otherStone));
        }
    }


    //implement equals function for this?
    public class GoStone : GoStoneHypothetical
    {
        public readonly static float diameter = 0.22f;
        public readonly static float ZHeightValue = 0.095f;

        public GameObject gameObject;
        public GameObject exploderGameObject;

        //0todo use this
        public GoStoneHypothetical ToHypothetical()
        {
            return new GoStoneHypothetical
            {
                Coordinates = this.Coordinates,
                stoneColor = this.stoneColor
            };
        }

        //0todo use this in constructor
        //public StoneColor stoneColor = StoneColor.None;

        //0todo don't use this?
        public GoStone()
        {
            Coordinates = new BoardCoordinates { x = 100, y = 100 };


            if (genericStoneObject == null)
            {
                genericStoneObject = new GameObject();
            }

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
    }

    public class GoStoneGroup
    {
        public List<GoStoneHypothetical> stones = new List<GoStoneHypothetical>();
        public List<BoardCoordinates> libertyCoordinates = new List<BoardCoordinates>();
    }

    public class GoBoard
    {
        public readonly static float boardCoordinateSeparationX = 0.2211f;
        public readonly static float boardCoordinateSeparationY = 0.2366f;

        public List<GoStone> boardStones = new List<GoStone>();
    }


    //0todo implememt this?
    //public interface IPlay
    //{

    //}

    public class ValidPlayData
    {
        public bool isValidPlayLocal;
        public List<GoStoneHypothetical> groupStonesToKill;
    }

    //public class InvalidPlayData : IPlay
    //{
    //    public bool isValidPlayLocal;
    //    public List<GoStoneHypothetical> groupStonesToKill;
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
        CurrentStateData.currentGameState = GameState.CanThrowStone;
        CurrentStateData.curentGoStoneRotation = Quaternion.identity;
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