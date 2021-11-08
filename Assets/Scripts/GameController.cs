using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //todo make buttons unclickable during play
    //todo have separate GoStone List for new board layout. last in stonePosHistory should always be current layout.
    //todo more functions should return value?
    //todo reduce number of parameters in functions
    //todo reduce function size




    //groupStonesToKill, groupStones are same? todo


    //todo get rid of magic numbers everywhere

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

    //TODO make more things private, local
    public GameObject title;
    public GameObject title2;
    public float timer;
    public Button resetButton;
    public Camera mainCamera;
    public Vector3 defaultCameraPosition;
    public Quaternion defaultCameraRotation;
    public Quaternion curentGoStoneRotation;
    public Camera ogCameraReference;
    public Vector3 cameraStartPosition;
    public GameObject genericStoneObject;
    public readonly float boardCoordinateSeparationX = 0.2211f;
    public readonly float boardCoordinateSeparationY = 0.2366f;
    public Material whiteMaterial;
    public Material blackMaterial;
    public float stoneZValue = -0.095f;
    public GameObject sensorStone;
    Transform sensorStoneTrans;
    public GameObject explosionObjectParent;
    public GameObject explosion;
    //todo add these to GoStone class
    private GameObject[,] boardExploderGameObjects = new GameObject[20, 20];
    public GameObject whiteTextObject;
    public GameObject blackTextObject;

    public float whiteScore = 0;
    public float blackScore = 0;
    public StoneColor currentPlayerColor = StoneColor.black;

    private GameState currentGameState = GameState.PlaceStone;
    public float ogBaseProb = -70;
    public float ogProb = -70;
    public float ogVelocity = 6;
    private bool isOgFired = false;
    private bool isOnFirstOgPlay = true;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    private int ogFiredStage = 0;

    public List<List<GoStone>> stonePosHistory = new List<List<GoStone>>();

    public Vector3 mousePos;
    public GoStone previousMouseCoordinates = new GoStone();
    public bool? isValidPlay = true;

    public float speedH = 5.0f;
    public float speedV = 3.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

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

    // Start is called before the first frame update
    void Start()
    {
        Console.Write("hello world");

        Application.targetFrameRate = 60;
        resetButton.onClick.AddListener(ResetGame);
        sensorStoneTrans = sensorStone.GetComponent<Transform>();
        stonePosHistory.Add(new List<GoStone>());

        ogNowButton.onClick.AddListener(ogNow);
        plusOgSpeedButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ogVelocity, 1, ogSpeedText, "Throw Speed: ", false); });
        minusOgSpeedButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ogVelocity, -1, ogSpeedText, "Throw Speed: ", false); });
        plusOgBaseProbButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ogBaseProb, 10, ogBaseProbText, "Base Og Prob: ", true); });
        minusOgBaseProbButton.onClick.AddListener(delegate { ChangeFloatAndText(ref ogBaseProb, -10, ogBaseProbText, "Base Og Prob: ", true); });

        ogSpeedText.GetComponent<Text>().text = "Throw Speed: " + ogVelocity;
        ogProbText.GetComponent<Text>().text = "Og prob: " + ogProb + "%";
        ogBaseProbText.GetComponent<Text>().text = "Base Og prob: " + ogBaseProb + "%";

        cameraStartPosition = new Vector3(1.9883f, -2.1326f, -5);
        defaultCameraPosition = mainCamera.GetComponent<Transform>().position;
        defaultCameraRotation = mainCamera.GetComponent<Transform>().rotation;

        //og at start of game
        //ogPlay = true;
        //mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
        //mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;
        //sensorStone.GetComponent<MeshRenderer>().material.color = new Color(0f, 1.0f, 1.0f, 0.5f);

        //Instansiates Exploders
        //This one is for sensor/og stone. [19,19] is outside board.
        boardExploderGameObjects[19, 19] = Instantiate(explosion, new Vector3(0, 0, 0), Quaternion.identity);
        boardExploderGameObjects[19, 19].name = "19x19xEXploder";
        boardExploderGameObjects[19, 19].transform.parent = explosionObjectParent.transform;

        for (int iteratedY = 0; iteratedY < 19; iteratedY++)
        {
            for (int iteratedX = 0; iteratedX < 19; iteratedX++)
            {
                boardExploderGameObjects[iteratedX, iteratedY] = Instantiate(explosion,
                                                             new Vector3(iteratedX * boardCoordinateSeparationX,
                                                                        -iteratedY * boardCoordinateSeparationY,
                                                                        stoneZValue),
                                                             Quaternion.identity);

                boardExploderGameObjects[iteratedX, iteratedY].GetComponent<Renderer>().enabled = false;
                boardExploderGameObjects[iteratedX, iteratedY].name = $"{iteratedX}x{iteratedY}xEXploder";
                boardExploderGameObjects[iteratedX, iteratedY].transform.parent = explosionObjectParent.transform;
            }
        }

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
        if (currentGameState == GameState.PlaceStone && !isOgFired)
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
                sensorStone.GetComponent<Renderer>().enabled = false;
                //SensorStoneRenderEnabled(false);
            }

            else
            {
                //todo remove many instances of isValidPlay?
                ValidPlayData validPlayData = new ValidPlayData();
                validPlayData = ValidPlayCheck(possibleStoneCoordinates);
                if (!SameStoneCoordinates(previousMouseCoordinates, possibleStoneCoordinates))
                {
                    isValidPlay = null;
                }

                if (isValidPlay == null)
                {
                    //todo this should return both isValidPlay bool, and groupStonesToKill?
                    //validPlayData = ValidPlayCheck(possibleStoneCoordinates);
                    isValidPlay = validPlayData.isValidPlay;
                }

                if (isValidPlay == true)
                {
                    sensorStone.GetComponent<Renderer>().enabled = true;
                    //SensorStoneRenderEnabled(true);
                }
                else if (isValidPlay == false)
                {
                    sensorStone.GetComponent<Renderer>().enabled = false;
                    //SensorStoneRenderEnabled(false);
                }

                CopyStoneCoordinates(possibleStoneCoordinates, previousMouseCoordinates);

                sensorStone.GetComponent<Transform>().position = new Vector3(possibleStoneCoordinates.x * boardCoordinateSeparationX,
                                                                              -possibleStoneCoordinates.y * boardCoordinateSeparationY,
                                                                              -stoneZValue);
                sensorStoneTrans.rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

                //todo fix Input things
                //todo make background more transparent
                if (Input.GetMouseButtonUp(0) && isValidPlay == true)
                //if (Input.GetMouseButtonUp(0) && isValidPlay == true)
                {
                    PlaceGoStone(possibleStoneCoordinates, validPlayData.groupStonesToKill);
                }
            }
        }

        //og play (throwing)
        else if (currentGameState == GameState.ThrowStone)
        {
            sensorStone.GetComponent<Renderer>().enabled = true;
            //SensorStoneRenderEnabled(true);

            sensorStoneTrans.position = mainCamera.GetComponent<Transform>().position + 1 * mainCamera.GetComponent<Transform>().forward;

            sensorStoneTrans.rotation = mainCamera.transform.rotation * curentGoStoneRotation;
            //sensorStoneTrans.Rotate( curentGoStoneRotation);


            //sensorStoneTrans.rotation = mainCamera.transform.rotation;

            mainCamera.orthographic = false;

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            mainCamera.GetComponent<Transform>().Rotate(new Vector3(pitch, 0, 0), Space.Self);
            mainCamera.GetComponent<Transform>().Rotate(new Vector3(0, 0, -yaw), Space.World);

            yaw = 0;
            pitch = 0;

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
                curentGoStoneRotation *= Quaternion.Euler(5, 0, 0);
            }
            if (Input.GetKey("q"))
            {
                curentGoStoneRotation *= Quaternion.Euler(-5, 0, 0);
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
                ThrowGoStone();
            }
            isOgNowFirstFrame = false;
        }

        else if (isOgFired)
        {
            //todo rework ogFiredStage?
            if (Time.time - timer > 1 && ogFiredStage == 0)
            {
                ogFiredStage = 1;
            }

            if (Time.time - timer > 2 && ogFiredStage == 1)
            {
                GetNewBoardLayout();
                //StonePosHistoryEntry stonePosHistoryEntry = GetNewBoardLayout();



                //stonePosHistory.Add(new List<GoStone>());
                //for (int i = 0; i < stonePosHistoryEntry.stones.Count; i++)
                //{
                //    stonePosHistory.Last().Add(stonePosHistoryEntry.stones[i]);
                //}
                ////stonePosHistory.Add(stonePosHistoryEntry.stones);
                //stonePosHistoryGameObjects = stonePosHistoryEntry.gameObjects;


                    //stonePosHistory.Last().Add(new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor });
                    //stonePosHistoryGameObjects[CoordinateX, CoordinateY] = stoneToSort.gameObject;


                //stonePosHistory.Add(new List<GoStone>());
                //stonePosHistoryGameObjects = new GameObject[19, 19];






                if (currentPlayerColor == StoneColor.black)
                { currentPlayerColor = StoneColor.white; }
                else if (currentPlayerColor == StoneColor.white)
                { currentPlayerColor = StoneColor.black; }

                ogFiredStage = 2;
            }

            if (Time.time - timer > 3 && ogFiredStage == 2)
            {
                ogFiredStage = 0;

                //stay in og play for other player
                if (isOnFirstOgPlay)
                {
                    currentGameState = GameState.ThrowStone;
                    curentGoStoneRotation = Quaternion.identity;

                    ogProb = ogBaseProb;
                    ogProbText.GetComponent<Text>().text = "Og Prob: " + ogProb + "%";
                    mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
                    mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;

                    sensorStone.GetComponent<Renderer>().enabled = true;
                    //SensorStoneRenderEnabled(true);
                    isOnFirstOgPlay = false;
                }

                //back to go play
                else
                {
                    isOgFired = false;

                    mainCamera.GetComponent<Transform>().position = defaultCameraPosition;
                    mainCamera.GetComponent<Transform>().rotation = defaultCameraRotation;
                    mainCamera.orthographic = true;

                    isOnFirstOgPlay = true;
                }
            }
        }
    }

    private void MakeSensorStoneTransparent(float flo)
    {
        Color currentColor = sensorStone.GetComponent<MeshRenderer>().material.color;
        currentColor.a = flo;
        sensorStone.GetComponent<MeshRenderer>().material.color = currentColor;
    }

    private void SensorStoneRenderEnabled(bool isEnabled)
    {
        sensorStone.GetComponent<Renderer>().enabled = isEnabled;
    }

    private void PlaceGoStone(GoStone stoneCoordinates, List<GoStone> groupStonesToKill)
    {
        isValidPlay = false;

        stonePosHistory.Add(new List<GoStone>());

        for (int i = 0; i < stonePosHistory[stonePosHistory.Count - 2].Count; i++)
        {
            stonePosHistory.Last().Add(stonePosHistory[stonePosHistory.Count - 2][i]);
        }

        if (groupStonesToKill != null)
        {
            //print(groupStonesToKill.Count);
            //todo move this further down?
            KillGroupStones(groupStonesToKill);
        }


        sensorStone.GetComponent<Renderer>().enabled = false;
        //SensorStoneRenderEnabled(false);

        GameObject newStoneObject = Instantiate(genericStoneObject,
                                          new Vector3(stoneCoordinates.x * boardCoordinateSeparationX,
                                                       -stoneCoordinates.y * boardCoordinateSeparationY,
                                                       -stoneZValue),
                                          Quaternion.identity);

        newStoneObject.name = $"{stoneCoordinates.x}x{stoneCoordinates.y}xStone";
        newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);
        //print(stoneCoordinates.x + " " + stoneCoordinates.y);




        //stonePosHistoryGameObjects[stoneCoordinates.x, stoneCoordinates.y] = newStoneObject;

        stonePosHistory.Last().Add(new GoStone
        {
            x = stoneCoordinates.x,
            y = stoneCoordinates.y,
            stoneColor = currentPlayerColor,
            gameObject = newStoneObject
            
        });

        //here.
        //stonePosHistoryGameObjects[stoneCoordinates.x, stoneCoordinates.y] = newStoneObject;

        //stonePosHistory.Last().Add(new GoStone
        //{
        //    x = stoneCoordinates.x,
        //    y = stoneCoordinates.y,
        //    stoneColor = currentPlayerColor
        //});





        if (currentPlayerColor == StoneColor.black)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = blackMaterial;

            sensorStone.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.50f);
            currentPlayerColor = StoneColor.white;
        }
        else if (currentPlayerColor == StoneColor.white)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = whiteMaterial;

            sensorStone.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 0f, 0.50f);
            currentPlayerColor = StoneColor.black;
        }

        if (UnityEngine.Random.Range(0, 100) < ogProb)
        {
            //to og play
            currentGameState = GameState.ThrowStone;
            curentGoStoneRotation = Quaternion.identity;
            ogProb = ogBaseProb;
            ogProbText.GetComponent<Text>().text = "Og prob: " + ogProb + "%";
            mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
            mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;
        }

        else
        {
            ogProb += 8;
            ogProbText.GetComponent<Text>().text = "Og Prob: " + ogProb + "%";
        }
    }

    //todo have direction, speed, etc passed here
    private void ThrowGoStone()
    {
        GameObject newStone = Instantiate(genericStoneObject, sensorStoneTrans.position, Quaternion.identity);
        newStone.name = $"19x19xStone";
        //newStone.GetComponent<Transform>().rotation *= curentGoStoneRotation;
        newStone.GetComponent<Transform>().rotation = mainCamera.transform.rotation * curentGoStoneRotation;
        //newStone.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);
        sensorStone.GetComponent<Renderer>().enabled = false;
        //SensorStoneRenderEnabled(false);

        if (currentPlayerColor == StoneColor.black)
        //if (isBlackTurn)
        {
            newStone.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.50f);
            //currentPlayerColor = StoneColor.white;
            //isBlackTurn = false;
        }
        else if (currentPlayerColor == StoneColor.white)
        {
            newStone.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 0f, 0.50f);
            //currentPlayerColor = StoneColor.black;
            //isBlackTurn = true;
        }

        newStone.GetComponent<Rigidbody>().velocity = ogVelocity * mainCamera.transform.forward;

        currentGameState = GameState.PlaceStone;
        //isOgPlay = false;
        isOgFired = true;
        timer = Time.time;
    }

    private ValidPlayData ValidPlayCheck(GoStone centerStone)
    {
        List<GoStone> groupStonesToKill = new List<GoStone>();

        if (stonePosHistory.Last().Find(s => (s.x == centerStone.x) &&
                                             (s.y == centerStone.y)) != null)
        {
            return new ValidPlayData() { isValidPlay = false };
            //return false;
        }

        List<GoStone> boardIfStoneIsPlayed = new List<GoStone>();

        for (int i = 0; i < stonePosHistory.Last().Count(); i++)
        {
            boardIfStoneIsPlayed.Add(stonePosHistory.Last()[i]);
        }

        boardIfStoneIsPlayed.Add(new GoStone
        {
            x = centerStone.x,
            y = centerStone.y,
            stoneColor = currentPlayerColor
        });
        centerStone.stoneColor = currentPlayerColor;

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

        // Ko rule
        bool sameBoard = false;
        if (stonePosHistory.Count > 3
            && boardIfStoneIsPlayed.Count == stonePosHistory[stonePosHistory.Count - 2].Count
            )
        {
            sameBoard = true;
            for (int i = 0; i < stonePosHistory[stonePosHistory.Count - 2].Count; i++)
            {
                if (boardIfStoneIsPlayed.Find(s => s.x == (stonePosHistory[stonePosHistory.Count - 2][i].x) &&
                                                   s.y == (stonePosHistory[stonePosHistory.Count - 2][i].y) &&
                                                   s.stoneColor == (stonePosHistory[stonePosHistory.Count - 2][i].stoneColor)) == null)
                {
                    sameBoard = false;
                    break;
                }
            }
        }

        if (sameBoard) { return new ValidPlayData() { isValidPlay = false }; }
        //if (sameBoard) { return false; }

        if (openSides.Length > 0)
        { return new ValidPlayData() { isValidPlay = true, groupStonesToKill = groupStonesToKill }; }
        else { return new ValidPlayData() { isValidPlay = false }; }
        // { return true; }
        //else { return false; }
    }

    private bool LibertiesFromSideExists(GoStone centerStone,
                                         GoStone offsetFromCenterStone,
                                         List<GoStone> boardIfStoneIsPlayed,
                                         List<GoStone> groupStonesToKill)
    {
        GoStoneGroup stoneGroup = new GoStoneGroup();
        //groupLiberties = new List<GoStone>();

        //groupStones = new List<GoStone>();
        //groupLiberties = new List<GoStone>();
        //List<GoStone> groupLiberties = new List<GoStone>();

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
        //groupLiberties = FindGroupAndLibertyCoordinates(sideStone, boardIfStoneIsPlayed);

        if (sideStone.stoneColor == StoneColor.none)
        {
            return true;
        }
        else if (stoneGroup.libertyCoordinates.Count > 0 && sideStone.stoneColor == centerStone.stoneColor)
        //else if (groupLiberties.Count > 0 && sideStone.stoneColor == centerStone.stoneColor)
        {
            return true;
        }
        else if (stoneGroup.libertyCoordinates.Count > 0 && sideStone.stoneColor != centerStone.stoneColor)
        //else if (groupLiberties.Count > 0 && sideStone.stoneColor != centerStone.stoneColor)
        {
            return false;
        }
        else if (stoneGroup.libertyCoordinates.Count == 0 && sideStone.stoneColor == centerStone.stoneColor)
        //else if (groupLiberties.Count == 0 && sideStone.stoneColor == centerStone.stoneColor)
        {
            return false;
        }
        else if (stoneGroup.libertyCoordinates.Count == 0 && sideStone.stoneColor != centerStone.stoneColor)
        //else if (groupLiberties.Count == 0 && sideStone.stoneColor != centerStone.stoneColor)
        {
            foreach (GoStone stone in stoneGroup.stones)
            //foreach (GoStone arr in groupStones)
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
    private void FindGroupAndLibertyCoordinates(GoStone startStone, List<GoStone> boardIfStoneIsPlayed, ref GoStoneGroup stoneGroup)
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

        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.x > 0), StoneOffset.left, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.x < 18), StoneOffset.right, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.y < 18), StoneOffset.up, ref stoneGroup);
        FindGroupAndLibertyCoordinatesSideStone(startStone, boardIfStoneIsPlayed, (startStone.y > 0), StoneOffset.down, ref stoneGroup);
        //print(stoneGroup.libertyCoordinates.Count);
    }



    private void FindGroupAndLibertyCoordinatesSideStone(GoStone sideStoneCoordinates, List<GoStone> boardIfStoneIsPlayed,
        bool isPositionGood, GoStoneCoordinates offset, ref GoStoneGroup stoneGroup)
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
                sideStone.stoneColor != StoneColor.none
                &&
                stoneGroup.stones.Find(p => (p.x == sideStone.x + offset.x) &&
                                            (p.y == sideStone.y + offset.y)) == null
                )
            {
               // print("1");
                //return
                FindGroupAndLibertyCoordinates(new GoStone
                {
                    x = sideStone.x + offset.x,
                    y = sideStone.y + offset.y
                },
                boardIfStoneIsPlayed,
                ref stoneGroup);
            }
            else if ((otherStone.stoneColor == StoneColor.none)
                     &&
                     (stoneGroup.libertyCoordinates.Find(p => (p.x == sideStone.x + offset.x) &&
                                                              (p.y == sideStone.y + offset.y))) == null)

            {
                //print("2");

                stoneGroup.libertyCoordinates.Add(new GoStone
                {
                    x = sideStone.x + offset.x,
                    y = sideStone.y + offset.y
                });
                //return 1;
            }
            //else { return 0; }
        }
        //else { return 0; }
        //print(stoneGroup.libertyCoordinates.Count);
    }

    private void GetNewBoardLayout()
    //private StonePosHistoryEntry GetNewBoardLayout()
    {
        //test
        //todo this
        //StonePosHistoryEntry newStonePosHistoryEntry = new StonePosHistoryEntry();

        stonePosHistory.Add(new List<GoStone>());



        //here.
        //stonePosHistoryGameObjects = new GameObject[19, 19];




        //finds stones in layer 8, "Stone"

        Collider[] sortedStonesInRange = Physics.OverlapSphere(new Vector3(0, 0, 0), 10, LayerMask.GetMask("Stone"));
        //Collider[] sortedStonesInRange = Physics.OverlapSphere(new Vector3(0, 0, 0), 10, 8);


        //Collider[] sortedStonesInRange = Physics.OverlapSphere(new Vector3(0, 0, 0), 10, LayerMask.GetMask("SortedStone"));

        foreach (Collider sortedStone in sortedStonesInRange)
        {
            //changes layer to "SortingStone"
            sortedStone.gameObject.layer = 9;

            ////changes layer to "Stone"
            //sortedStone.gameObject.layer = 8;
        }

        int searchIncrement = 200;
        for (float r = 1; r < searchIncrement; r++)
        {
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {
                    //GoStone localStone = newStonePosHistoryEntry.stones.Find(s => s.x == iteratedX &&
                    //                                                       s.y == iteratedY);

                    //here.
                    GoStone localStone = stonePosHistory.Last().Find(s => s.x == iteratedX &&
                                                                          s.y == iteratedY);



                    if (localStone == null) { localStone = new GoStone(); }

                    if (localStone.stoneColor != StoneColor.none) { continue; }

                    Collider[] stonesInRange = Physics.OverlapSphere(new Vector3(boardCoordinateSeparationX * iteratedX,
                                                                                -boardCoordinateSeparationY * iteratedY,
                                                                                 0),
                                                                     boardCoordinateSeparationX * 19 * 2 * (r / searchIncrement),
                                                                     //9);
                                                                     LayerMask.GetMask("SortingStone"));
                    //LayerMask.GetMask("Stone"));

                    if (stonesInRange.Length == 0)
                    {
                        continue;
                    }

                    stonesInRange[0].name = $"{iteratedX}x{iteratedY}xStone";

                    GoStone sortedStone = SortStone(stonesInRange[0], iteratedX, iteratedY);




                    //newStonePosHistoryEntry.stones.Add(new GoStone { x = sortedStone.x, y = sortedStone.y, stoneColor = sortedStone.stoneColor });
                    //newStonePosHistoryEntry.gameObjects[sortedStone.x, sortedStone.y] = sortedStone.gameObject;


                    //stonePosHistory.Last().Add(new GoStone { x = sortedStone.x, y = sortedStone.y, stoneColor = sortedStone.stoneColor });
                    //stonePosHistoryGameObjects[sortedStone.x, sortedStone.y] = sortedStone.gameObject;

                }
            }
        }

        List<GoStone> alreadyGroupedStones = new List<GoStone>();

        List<StoneColor> StoneColors = new List<StoneColor>() {
                                            currentPlayerColor == StoneColor.white ? StoneColor.black :StoneColor.white
                                            ,
                                            currentPlayerColor

        //List<StoneColor> StoneColors = new List<StoneColor>() {currentPlayerColor,
        //                                    currentPlayerColor == StoneColor.white ? StoneColor.black :StoneColor.white
        };

        foreach (StoneColor iteratedColor in StoneColors)
        {

            //for (int i = 0; i<2; i++) { 
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {

                    //GoStone localStone = newStonePosHistoryEntry.stones.Find(s => (s.x == iteratedX) &&
                    //                                                              (s.y == iteratedY));

                    //here.
                    GoStone localStone = stonePosHistory.Last().Find(s => (s.x == iteratedX) &&
                                                                          (s.y == iteratedY));

                    if (localStone == null)
                    {
                        continue;
                    }

                    if (localStone.stoneColor == StoneColor.none ||



                        localStone.stoneColor != iteratedColor ||
                        //localStone.stoneColor == iteratedColor ||
                        alreadyGroupedStones.Find(p => p.x == iteratedX &&
                                                       p.y == iteratedY) != null)
                    {
                        continue;
                    }
                    //print(iteratedColor);


                    GoStoneGroup stoneGroup = new GoStoneGroup();
                    List<GoStone> groupStonesToKill = new List<GoStone>();

                    //return groupstonestokill?
                    //todo make sure all FindGroupAndLibertyCoordinates are passing color in?
                    FindGroupAndLibertyCoordinates(new GoStone
                    {
                        x = iteratedX,
                        y = iteratedY,

                        stoneColor = localStone.stoneColor
                    },
                    //newStonePosHistoryEntry.stones,
                    //here.
                    stonePosHistory.Last(),
                    ref stoneGroup
                    );

                    foreach (GoStoneCoordinates coordinates in stoneGroup.libertyCoordinates)
                    {
                        print("xy: " + coordinates.x + " " + coordinates.y);
                    }


                    //print("liberties: " + stoneGroup.libertyCoordinates.Count);

                    foreach (GoStone entry in stoneGroup.stones)
                    {
                        if (!alreadyGroupedStones.Any(p => p == (entry)))
                        {
                            alreadyGroupedStones.Add(entry);
                        }
                    }
                    foreach (GoStone entry in groupStonesToKill)
                    {
                        alreadyGroupedStones.Add(entry);
                    }

                    if (stoneGroup.libertyCoordinates.Count == 0)
                    {
                        foreach (GoStone stone in stoneGroup.stones)
                        {
                            //print("1: " + stone.x + " " + stone.y);
                            //print(arr.stoneColor);
                            groupStonesToKill.Add(stone);
                        }

                        KillGroupStones(stoneGroup.stones);



                    }
                }
            }
        }

        //if (currentPlayerColor == StoneColor.black)
        //{ currentPlayerColor = StoneColor.white; }
        //else if (currentPlayerColor == StoneColor.white)
        //{ currentPlayerColor = StoneColor.black; }

        //return newStonePosHistoryEntry;

    }

    public void PlusOneToScore(StoneColor stoneColor)
    {
        if (stoneColor == StoneColor.black)
        {
            ChangeFloatAndText(ref blackScore, 1, blackTextObject, "Black Captures: ", false);
            //blackScore += 1;
            //blackTextObject.GetComponent<Text>().text = "Black Captures: " + blackScore;
        }
        if (stoneColor == StoneColor.white)
        {
            ChangeFloatAndText(ref whiteScore, 1, whiteTextObject, "White Captures: ", false);
            //whiteScore += 1;
            //whiteTextObject.GetComponent<Text>().text = "White Captures: " + whiteScore;
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadScene("og");
    }

    private enum GameState
    {
        PlaceStone,
        ThrowStone
    }

    private void ogNow()
    {
        currentGameState = GameState.ThrowStone;
        curentGoStoneRotation = Quaternion.identity;
        //isOgPlay = true;
        isOgNowFirstFrame = true;
        ogProb = ogBaseProb;
        ogProbText.GetComponent<Text>().text = "Og prob: " + ogProb + "%";
        mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
        mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;
        sensorStone.GetComponent<Renderer>().enabled = true;
        //SensorStoneRenderEnabled(true);
    }

    private void ChangeFloatAndText(ref float valueToChange, int valueAdded, GameObject textObject, string text, bool isPercent)
    {
        valueToChange += valueAdded;
        textObject.GetComponent<Text>().text = text + valueToChange + (isPercent ? "%" : "");
    }

    public void KillGroupStones(List<GoStone> groupStonesToKill)
    {

        foreach (GoStone stone in groupStonesToKill)
        {
            //todo just get color data from foreach GoStone
            GoStone stoneToDestroy = stonePosHistory.Last().Find(s => (s.x == stone.x) &&
                                                                      (s.y == stone.y));



            if (stoneToDestroy == null) { continue; }

            //here.
            //if (stonePosHistoryGameObjects[stone.x, stone.y] == null) { continue; }





            KillStoneWithDelay(
                          stoneToDestroy,
                         0.2f,
                         0.2f * groupStonesToKill.IndexOf(stone));
            //here.
            //KillStoneWithDelay(stonePosHistoryGameObjects[stone.x, stone.y],
            //              stoneToDestroy,
            //             0.2f,
            //             0.2f * groupStonesToKill.IndexOf(stone));



        }

    }

    public void KillStoneWithDelay(GoStone StoneToDestroy, float destroyDelay, float entireDelay = 0)
    //here.
    //public void KillStoneWithDelay(GameObject objectToDestroy, GoStone StoneToDestroy, float destroyDelay, float entireDelay = 0)
    {

        stonePosHistory.Last().Remove(stonePosHistory.Last().Find(s => (s.x == StoneToDestroy.x) &&
                                                                       (s.y == StoneToDestroy.y)));

        if (StoneToDestroy.stoneColor == StoneColor.black)
        {
            PlusOneToScore(StoneColor.white);
        }
        else if (StoneToDestroy.stoneColor == StoneColor.white)
        {
            PlusOneToScore(StoneColor.black);
        }
        else { print("NO COLOR SET FOR DESTRUCTION"); }

        StoneToDestroy.gameObject.layer = 8;
        //here.
        //to "Stone" layer
        //objectToDestroy.layer = 8;

        ////to "SortedStone" layer
        //objectToDestroy.layer = 9;
        //objectToDestroy.layer = LayerMask.NameToLayer("SortedStone");
        StartCoroutine(DelayDestroyCoroutine());

        IEnumerator DelayDestroyCoroutine()
        {
            yield return new WaitForSeconds(entireDelay);

            //check
            GameObject exploder = boardExploderGameObjects[StoneToDestroy.y, StoneToDestroy.x];

            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().frame = 7;
            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Play();

            yield return new WaitForSeconds(0.2f);


            
            exploder.transform.position = StoneToDestroy.gameObject.transform.position;
            //here.
            //exploder.transform.position = objectToDestroy.transform.position;



            exploder.GetComponent<Renderer>().enabled = true;
            exploder.transform.LookAt(mainCamera.transform, new Vector3(0, 0, 1));
            exploder.transform.rotation = mainCamera.transform.rotation;



           
            Destroy(StoneToDestroy.gameObject);
            //here.
            //Destroy(objectToDestroy);





            isValidPlay = null;

            yield return new WaitForSeconds(7);
            exploder.GetComponent<Renderer>().enabled = false;
            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
        }
    }

    public GoStone SortStone(Collider stoneToSort, int CoordinateX, int CoordinateY)
    {
        stoneToSort.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        stoneToSort.GetComponent<Transform>().position =
            new Vector3(CoordinateX * boardCoordinateSeparationX,
                       -CoordinateY * boardCoordinateSeparationY,
                       -stoneZValue);
        stoneToSort.GetComponent<Transform>().rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);
        stoneToSort.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);



        StoneColor stoneToSortColor = StoneColor.none;
        if (stoneToSort.GetComponent<MeshRenderer>().material.name == "Black Stone (Instance)")
        {
            stoneToSortColor = StoneColor.black;
        }

        else if (stoneToSort.GetComponent<MeshRenderer>().material.name == "White Stone (Instance)")
        {
            stoneToSortColor = StoneColor.white;
        }



        //test
        
        //these two needed? test 
        stonePosHistory.Last().Add(new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor, gameObject = stoneToSort.gameObject });

        //changes layer to "Stone"
        stoneToSort.gameObject.layer = 8;

        //todo implement this
        return new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor, gameObject = stoneToSort.gameObject  };
    }

    //todo rename this?
    public class GoStoneCoordinates
    {
        public int x;
        public int y;
    }

    //todo add liberties property?
    public class GoStone : GoStoneCoordinates
    {
        //public int x;
        //public int y;
        public StoneColor stoneColor = StoneColor.none;
        public List<GoStoneCoordinates> libertyCoordinates = new List<GoStoneCoordinates>();
        //public List<LibertyDirection> stoneLiberties = new List<LibertyDirection>(); 

        public GameObject gameObject;
        
        public readonly static float diameter = 0.22f;
    }

    //public class GoStone

    public class GoStoneGroup
    {
        public List<GoStone> stones = new List<GoStone>();
        public List<GoStoneCoordinates> libertyCoordinates = new List<GoStoneCoordinates>();
    }

    public enum StoneColor
    {
        none,
        black,
        white
    }

    public class StoneOffset
    {
        public static GoStoneCoordinates left = new GoStoneCoordinates { x = -1, y = 0 };
        public static GoStoneCoordinates right = new GoStoneCoordinates { x = +1, y = 0 };
        public static GoStoneCoordinates up = new GoStoneCoordinates { x = 0, y = +1 };
        public static GoStoneCoordinates down = new GoStoneCoordinates { x = 0, y = -1 };
    }

    public class StonePosHistoryEntry
    {
        public List<GoStone> stones = new List<GoStone>();
        public GameObject[,] gameObjects = new GameObject[19, 19];


        //stonePosHistory.Add(new List<GoStone>());
        //stonePosHistoryGameObjects = new GameObject[19, 19];

    }

    //public class 

    public class ValidPlayData
    {
        public bool isValidPlay;
        public List<GoStone> groupStonesToKill;
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
}


//Explosion effect and sound used under Creative Commons(https://creativecommons.org/licenses/by/3.0/)
//From Youtube user Timothy (https://www.youtube.com/channel/UCrxGMPla5PpIdeSyGQFaXhg) (https://www.youtube.com/watch?v=Q7KmAe8_jZE)

//Go board image used under Public Domain from Wikipedia (https://en.wikipedia.org/wiki/File:Blank_Go_board.svg)

//Sky background used from (sketchuptexture.com/2013/02/panoramic-ski-360.html)

// Original concept from Nicholas Abentroth

// Copyright 2021, Casey Keller, All rights reserved.