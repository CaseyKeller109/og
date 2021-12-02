﻿using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //todo put global variables in classes
    //todo add constructors to classes. GoStone, etc

    //todo sensor stone shouldn't cast shadow
    //todo more functions should return value
    //todo reduce number of parameters in functions
    //todo reduce function size
    //todo get rid of magic numbers everywhere
    //todo make more unit tests?

    //todo change coordinates to go from 1 to 19?
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

    //TODO make more things local
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
    public readonly float boardCoordinateSeparationX = 0.2211f;
    public readonly float boardCoordinateSeparationY = 0.2366f;
    public Material whiteMaterial;
    public Material blackMaterial;
    public Material whiteMaterialTransparent;
    public Material blackMaterialTransparent;
    public float stoneZValue = -0.095f;
    public GoStone sensorStone;
    public GoStone thrownStone;
    public GameObject explosionObjectParent;
    public GameObject explosion;

    public GameObject whiteTextObject;
    public GameObject blackTextObject;


    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;

    public List<List<GoStone>> stonePosHistory = new List<List<GoStone>>();

    public Vector3 mousePos;
    public GoStone previousMouseCoordinates;

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
        //todo use sensorstone rotation instead of this
        public static Quaternion curentGoStoneRotation;
        public static StoneColor currentPlayerColor = StoneColor.Black;
        public static GameState currentGameState = GameState.CanPlaceStone;
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

        previousMouseCoordinates = new GoStone();
        sensorStone = new GoStone();
        thrownStone = new GoStone();
    }

    // Start is called before the first frame update
    void Start()
    {
        Console.Write("hello world");

        Application.targetFrameRate = 60;
        resetButton.onClick.AddListener(ResetGame);

        sensorStone = new GoStone();

        sensorStone.gameObject = Instantiate(genericStoneObject, new Vector3(0, 0, 0), Quaternion.identity);
        sensorStone.gameObject.gameObject.GetComponent<MeshCollider>().enabled = false;
        sensorStone.gameObject.name = "BlackSensorStone";
        sensorStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterialTransparent;

        stonePosHistory.Add(new List<GoStone>());

        ogNowButton.onClick.AddListener(OgNow);
        plusOgSpeedButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogVelocity, 1, ogSpeedText, "Throw Speed: ", false); });
        minusOgSpeedButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogVelocity, -1, ogSpeedText, "Throw Speed: ", false); });
        plusOgBaseProbButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogBaseProb, 10, ogBaseProbText, "Base Og Prob: ", true); });
        minusOgBaseProbButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ThrowingData.ogBaseProb, -10, ogBaseProbText, "Base Og Prob: ", true); });

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

            GoStone possibleStoneCoordinates =
                new GoStone
                {
                    x = Convert.ToInt32(mousePos.x / boardCoordinateSeparationX),
                    y = -Convert.ToInt32(mousePos.y / boardCoordinateSeparationY)
                };

            if (Convert.ToInt32(mousePos.x / boardCoordinateSeparationX) < 0 ||
                Convert.ToInt32(mousePos.x / boardCoordinateSeparationX) > 18 ||
                Convert.ToInt32(mousePos.y / boardCoordinateSeparationY) > 0 ||
                Convert.ToInt32(mousePos.y / boardCoordinateSeparationY) < -18
                )
            {
                sensorStone.gameObject.GetComponent<Renderer>().enabled = false;
            }

            else
            {
                //todo can improve by not checking every time
                ValidPlayData validPlayData = new ValidPlayData();
                validPlayData = ValidPlayCheck(possibleStoneCoordinates);
                if (!SameStoneCoordinates(previousMouseCoordinates, possibleStoneCoordinates))
                {
                    CurrentStateData.isValidPlay = null;
                }

                if (CurrentStateData.isValidPlay == null)
                {
                    CurrentStateData.isValidPlay = validPlayData.isValidPlayLocal;
                }

                CopyStoneCoordinates(possibleStoneCoordinates, previousMouseCoordinates);


                if (CurrentStateData.isValidPlay == true)
                {
                    sensorStone.gameObject.GetComponent<Renderer>().enabled = true;
                }
                else if (CurrentStateData.isValidPlay == false)
                {
                    sensorStone.gameObject.GetComponent<Renderer>().enabled = false;
                }

                sensorStone.gameObject.GetComponent<Transform>().position = new Vector3(possibleStoneCoordinates.x * boardCoordinateSeparationX,
                                                                              -possibleStoneCoordinates.y * boardCoordinateSeparationY,
                                                                              -stoneZValue);
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
            if (Input.GetKey(KeyCode.LeftControl) && camtran.position.z < stoneZValue)
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

    public void PlaceGoStone(GoStone stoneCoordinates, List<GoStone> groupStonesToKill)
    {
        CurrentStateData.isValidPlay = false;

        stonePosHistory.Add(new List<GoStone>());

        for (int i = 0; i < stonePosHistory[stonePosHistory.Count - 2].Count; i++)
        {
            stonePosHistory.Last().Add(stonePosHistory[stonePosHistory.Count - 2][i]);
        }

        if (groupStonesToKill != null)
        {
            KillGroupStones(groupStonesToKill);
        }

        sensorStone.gameObject.GetComponent<Renderer>().enabled = false;

        GameObject newStoneObject = Instantiate(genericStoneObject,
                                          new Vector3(stoneCoordinates.x * boardCoordinateSeparationX,
                                                       -stoneCoordinates.y * boardCoordinateSeparationY,
                                                       -stoneZValue),
                                          Quaternion.identity);

        newStoneObject.name = $"{stoneCoordinates.x}x{stoneCoordinates.y}x{CurrentStateData.currentPlayerColor}Stone";
        newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);


        stonePosHistory.Last().Add(new GoStone
        {
            x = stoneCoordinates.x,
            y = stoneCoordinates.y,
            stoneColor = CurrentStateData.currentPlayerColor,
            gameObject = newStoneObject

        });

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
    }

    //todo make unit tests for ThrowGoStone?
    private void ThrowGoStone(Vector3 StonePosition,
                              Quaternion StoneRotation,
                              Vector3 StoneVelocity)
    {
        thrownStone.gameObject = Instantiate(genericStoneObject, StonePosition, Quaternion.identity);
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
    public ValidPlayData ValidPlayCheck(GoStone newStone)
    {
        List<GoStone> groupStonesToKill = new List<GoStone>();

        if (stonePosHistory.Last().Find(s => (s.x == newStone.x) &&
                                             (s.y == newStone.y)) != null)
        {
            return new ValidPlayData() { isValidPlayLocal = false };
        }

        List<GoStone> boardIfStoneIsPlayed = new List<GoStone>();

        for (int i = 0; i < stonePosHistory.Last().Count(); i++)
        {
            boardIfStoneIsPlayed.Add(stonePosHistory.Last()[i]);
        }

        boardIfStoneIsPlayed.Add(new GoStone
        {
            x = newStone.x,
            y = newStone.y,
            stoneColor = CurrentStateData.currentPlayerColor
        });
        newStone.stoneColor = CurrentStateData.currentPlayerColor;

        //Simple Ko rule
        bool isSameBoard = IsSameBoardSimpleCheck(boardIfStoneIsPlayed);
        if (isSameBoard) { return new ValidPlayData() { isValidPlayLocal = false }; }

        string openSides = OpenSidesCheck(newStone, boardIfStoneIsPlayed, groupStonesToKill);
        if (openSides.Length > 0)
        { return new ValidPlayData() { isValidPlayLocal = true, groupStonesToKill = groupStonesToKill }; }
        else { return new ValidPlayData() { isValidPlayLocal = false }; }
    }

    private string OpenSidesCheck(GoStone centerStone,
                                  List<GoStone> boardIfStoneIsPlayed,
                                  List<GoStone> groupStonesToKill)
    {
        string openSides = "";

        //top side
        if (centerStone.y > 0)
        {
            if (LibertiesFromSideExists(centerStone, new GoStone { x = 0, y = -1 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "top";
            }
        }

        //bottom side
        if (centerStone.y < 18)
        {
            if (LibertiesFromSideExists(centerStone, new GoStone { x = 0, y = 1 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "bottom";
            }
        }

        //left side
        if (centerStone.x > 0)
        {
            if (LibertiesFromSideExists(centerStone, new GoStone { x = -1, y = 0 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "left";
            }
        }

        //right side
        if (centerStone.x < 18)
        {
            if (LibertiesFromSideExists(centerStone, new GoStone { x = 1, y = 0 }, boardIfStoneIsPlayed, groupStonesToKill))
            {
                openSides += "right";
            }
        }
        return openSides;
    }

    public bool IsSameBoardSimpleCheck(List<GoStone> boardIfStoneIsPlayed)
    {
        bool isSameBoard = false;
        if (stonePosHistory.Count > 3
            && boardIfStoneIsPlayed.Count == stonePosHistory[stonePosHistory.Count - 2].Count
            )
        {
            isSameBoard = true;
            for (int i = 0; i < stonePosHistory[stonePosHistory.Count - 2].Count; i++)
            {
                if (boardIfStoneIsPlayed.Find(s => s.x == (stonePosHistory[stonePosHistory.Count - 2][i].x) &&
                                                   s.y == (stonePosHistory[stonePosHistory.Count - 2][i].y) &&
                                                   s.stoneColor == (stonePosHistory[stonePosHistory.Count - 2][i].stoneColor)) == null)
                {
                    isSameBoard = false;
                    break;
                }
            }
        }
        return isSameBoard;
    }

    private bool LibertiesFromSideExists(GoStone centerStone,
                                         GoStone offsetFromCenterStone,
                                         List<GoStone> boardIfStoneIsPlayed,
                                         List<GoStone> groupStonesToKill)
    {
        GoStoneGroup stoneGroup = new GoStoneGroup();

        GoStone sideStone = new GoStone();

        sideStone = stonePosHistory.Last().Find(s => s.x == centerStone.x + offsetFromCenterStone.x &&
                                                     s.y == centerStone.y + offsetFromCenterStone.y);

        if (sideStone == null)
        {
            sideStone = new GoStone();
        }

        if (boardIfStoneIsPlayed.Find(s => (s.x == sideStone.x) &&
                                           (s.y == sideStone.y)) == null)
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
            foreach (GoStone stone in stoneGroup.stones)
            {
                groupStonesToKill.Add(stone);
            }

            foreach (GoStone stone in groupStonesToKill)
            {
                boardIfStoneIsPlayed.Remove(boardIfStoneIsPlayed.Find(s => (s.x == stone.x) &&
                                                                           (s.y == stone.y)));
            }

            return true;
        }
        else { return false; }
    }

    //starts searching at centerStoneCoordinates
    private void FindGroupAndLibertyCoordinates(GoStone startStone,
                                                List<GoStone> boardIfStoneIsPlayed,
                                                ref GoStoneGroup stoneGroup)
    {
        if (stoneGroup.stones.Find(s => SameStoneCoordinates(s, startStone)) == null)
        {
            stoneGroup.stones.Add(new GoStone
            {
                x = startStone.x,
                y = startStone.y,
                stoneColor = startStone.stoneColor
            });
        }

        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.x > 0), StoneDirectionOffset.left, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.x < 18), StoneDirectionOffset.right, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.y < 18), StoneDirectionOffset.up, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.y > 0), StoneDirectionOffset.down, ref stoneGroup);
    }

    private void FindGroupAndLibertyCoordinatesSideStone(GoStone sideStoneCoordinates,
                                                         List<GoStone> boardIfStoneIsPlayed,
                                                         bool isPositionGood,
                                                         BoardCoordinates offset,
                                                         ref GoStoneGroup stoneGroup)
    {
        if (isPositionGood)
        {
            GoStone sideStone = boardIfStoneIsPlayed.Find(s => (s.x == sideStoneCoordinates.x) &&
                                                               (s.y == sideStoneCoordinates.y));

            GoStone otherStone = boardIfStoneIsPlayed.Find(s => (s.x == sideStone.x + offset.x) &&
                                                                (s.y == sideStone.y + offset.y));

            if (sideStone == null)
            {
                sideStone = new GoStone();
            }

            if (otherStone == null)
            {
                otherStone = new GoStone();
            }

            if (otherStone.stoneColor == sideStone.stoneColor
                &&
                sideStone.stoneColor != StoneColor.None
                &&
                stoneGroup.stones.Find(p => (p.x == sideStone.x + offset.x) &&
                                            (p.y == sideStone.y + offset.y)) == null
                )
            {
                FindGroupAndLibertyCoordinates(new GoStone
                {
                    x = sideStone.x + offset.x,
                    y = sideStone.y + offset.y
                },
                boardIfStoneIsPlayed,
                ref stoneGroup);
            }
            else if ((otherStone.stoneColor == StoneColor.None)
                     &&
                     (stoneGroup.libertyCoordinates.Find(p => (p.x == sideStone.x + offset.x) &&
                                                              (p.y == sideStone.y + offset.y))) == null)

            {

                stoneGroup.libertyCoordinates.Add(new GoStone
                {
                    x = sideStone.x + offset.x,
                    y = sideStone.y + offset.y
                });
            }
        }
    }

    public void GetNewBoardLayout()
    {
        stonePosHistory.Add(new List<GoStone>());

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
                    GoStone localStone = stonePosHistory.Last().Find(s => s.x == iteratedX &&
                                                                          s.y == iteratedY);


                    if (localStone == null) { localStone = new GoStone(); }

                    if (localStone.stoneColor != StoneColor.None) { continue; }

                    Collider[] stonesInRange = Physics.OverlapSphere(new Vector3(boardCoordinateSeparationX * iteratedX,
                                                                                -boardCoordinateSeparationY * iteratedY,
                                                                                 0),
                                                                     boardCoordinateSeparationX * 19 * 2 * (r / searchIncrement),
                                                                     LayerMask.GetMask("SortingStone"));

                    if (stonesInRange.Length == 0)
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
            new Vector3(CoordinateX * boardCoordinateSeparationX,
                       -CoordinateY * boardCoordinateSeparationY,
                       -stoneZValue);
        stoneToSort.GetComponent<Transform>().rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);
        stoneToSort.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        StoneColor stoneToSortColor = StoneColor.None;



        stoneToSortColor = stoneToSort.name.Contains("Black") ? StoneColor.Black : StoneColor.White;

        stonePosHistory.Last().Add(new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor, gameObject = stoneToSort.gameObject });

        //changes layer to "Stone"
        stoneToSort.gameObject.layer = 8;

        //todo use this return value
        return new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor, gameObject = stoneToSort.gameObject };
    }

    public void KillSortedStones()
    {
        List<GoStone> alreadyGroupedStones = new List<GoStone>();

        List<StoneColor> StoneColors = new List<StoneColor>() {
                                            CurrentStateData.currentPlayerColor == StoneColor.White ? StoneColor.Black :StoneColor.White
                                            ,
                                            CurrentStateData.currentPlayerColor
        };

        //todo improve this?
        foreach (StoneColor iteratedColor in StoneColors)
        {
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {
                    GoStone localStone = stonePosHistory.Last().Find(s => (s.x == iteratedX) &&
                                                                          (s.y == iteratedY));

                    if (localStone == null)
                    {
                        continue;
                    }

                    if (localStone.stoneColor == StoneColor.None ||
                        localStone.stoneColor != iteratedColor ||
                        alreadyGroupedStones.Find(p => p.x == iteratedX &&
                                                       p.y == iteratedY) != null)
                    {
                        continue;
                    }

                    GoStoneGroup stoneGroup = new GoStoneGroup();

                    //return groupstonestokill?
                    //todo make sure all FindGroupAndLibertyCoordinates are passing color in?
                    FindGroupAndLibertyCoordinates(new GoStone
                    {
                        x = iteratedX,
                        y = iteratedY,

                        stoneColor = localStone.stoneColor
                    },
                    stonePosHistory.Last(),
                    ref stoneGroup
                    );

                    foreach (GoStone entry in stoneGroup.stones)
                    {
                        if (!alreadyGroupedStones.Any(p => p == (entry)))
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

    public void KillGroupStones(List<GoStone> groupStonesToKill)
    {
        foreach (GoStone stone in groupStonesToKill)
        {
            //todo just get color data from foreach GoStone
            GoStone stoneToDestroy = stonePosHistory.Last().Find(s => (s.x == stone.x) &&
                                                                      (s.y == stone.y));

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
        GoStone historyStone = stonePosHistory.Last().Find(s => (s.x == StoneToDestroy.x) &&
                                                                (s.y == StoneToDestroy.y));

        if (historyStone != null)
        {
            stonePosHistory.Last().Remove(stonePosHistory.Last().Find(s => (s.x == StoneToDestroy.x) &&
                                                                           (s.y == StoneToDestroy.y)));
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

        //todo use something other than Text?
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

    public bool SameStoneCoordinates(GoStone first, GoStone second)
    {
        if ((first.x == second.x) &&
            (first.y == second.y))
        { return true; }
        else { return false; }
    }

    public void CopyStoneCoordinates(GoStone source, GoStone destination)
    {
        destination.x = source.x;
        destination.y = source.y;
    }




    public class BoardCoordinates
    {
        public int x;
        public int y;
    }

    public enum StoneColor
    {
        None,
        Black,
        White
    }

    public class GoStone : BoardCoordinates
    {
        public StoneColor stoneColor = StoneColor.None;

        public GameObject gameObject;
        public GameObject exploderGameObject;

        public readonly static float diameter = 0.22f;

        public GoStone()
        {



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


        public GoStone(String name)
        {



            if (genericStoneObject == null)
            {
                genericStoneObject = new GameObject();
            }

            //GameController newGameController = new GameController().genericStoneObject;
            //GameObject genericStoneObjectInstance = newGameController.genericStoneObject;

            gameObject = Instantiate(genericStoneObject, new Vector3(0, 0, 0), Quaternion.identity);

            //gameObject = Instantiate(genericStoneObjectInstance, new Vector3(0, 0, 0), Quaternion.identity);
            //gameObject = Instantiate(Resources.Load("Assets/Resources/Stone") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
            //gameObject.gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.name = name;
            //gameObject.GetComponent<MeshRenderer>().material = Resources.Load("TransparentBlack") as Material;

            //exploderGameObject.GetComponent<Renderer>().enabled = false
        }


    }

    public class GoStoneGroup
    {
        public List<GoStone> stones = new List<GoStone>();
        public List<BoardCoordinates> libertyCoordinates = new List<BoardCoordinates>();
    }


    public class ValidPlayData
    {
        public bool isValidPlay;
        public List<GoStone> groupStonesToKill;
    }

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