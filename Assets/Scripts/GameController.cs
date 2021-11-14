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
    //todo have separate GoStone List for new board layout. last in stonePosHistory should always be current layout?
    //todo more functions should return value?
    //todo reduce number of parameters in functions
    //todo reduce function size
    //todo have camera follow stone after throwing
    //todo groupStonesToKill, groupStones are same? 
    //todo get rid of magic numbers everywhere

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
    public Material whiteMaterialTransparent;
    public Material blackMaterialTransparent;
    public float stoneZValue = -0.095f;
    public GameObject sensorStone;
    public Transform sensorStoneTrans;
    public GameObject explosionObjectParent;
    public GameObject explosion;

    //todo add these to GoStone class?
    private GameObject[,] boardExploderGameObjects = new GameObject[20, 20];


    public GameObject whiteTextObject;
    public GameObject blackTextObject;

    public float whiteScore = 0;
    public float blackScore = 0;
    public StoneColor currentPlayerColor = StoneColor.Black;

    public GameState currentGameState = GameState.PlaceStone;
    public float ogBaseProb = -70;
    public float ogProb = -70;
    public float ogVelocity = 6;
    public bool isOgFired = false;
    public bool isOnFirstOgPlay = true;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    public int ogFiredStage = 0;

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
        sensorStone.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(0f, 0f, 0f, 0.50f);
        sensorStoneTrans = sensorStone.GetComponent<Transform>();
        stonePosHistory.Add(new List<GoStone>());

        ogNowButton.onClick.AddListener(OgNow);
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

        //Instantiates Exploders
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
            }

            else
            {
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

                CopyStoneCoordinates(possibleStoneCoordinates, previousMouseCoordinates);


                if (isValidPlay == true)
                {
                    sensorStone.GetComponent<Renderer>().enabled = true;
                }
                else if (isValidPlay == false)
                {
                    sensorStone.GetComponent<Renderer>().enabled = false;
                }



                sensorStone.GetComponent<Transform>().position = new Vector3(possibleStoneCoordinates.x * boardCoordinateSeparationX,
                                                                              -possibleStoneCoordinates.y * boardCoordinateSeparationY,
                                                                              -stoneZValue);
                sensorStoneTrans.rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

                if (Input.GetMouseButtonUp(0) && isValidPlay == true)
                {
                    PlaceGoStone(possibleStoneCoordinates, validPlayData.groupStonesToKill);

                    if (currentPlayerColor == StoneColor.Black)
                    { currentPlayerColor = StoneColor.White; }
                    else if (currentPlayerColor == StoneColor.White)
                    { currentPlayerColor = StoneColor.Black; }

                    if (UnityEngine.Random.Range(0, 100) < ogProb)
                    {
                        //to og play
                        currentGameState = GameState.ThrowStone;
                        curentGoStoneRotation = Quaternion.identity;
                        ogProb = ogBaseProb;
                        ogProbText.GetComponent<Text>().text = "Og prob: " + ogProb + "%";
                        mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
                        mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;
                        DisableButtons();
                    }

                    else
                    {
                        ogProb += 8;
                        ogProbText.GetComponent<Text>().text = "Og Prob: " + ogProb + "%";
                    }


                }
            }
        }

        //og play (throwing)
        else if (currentGameState == GameState.ThrowStone)
        {
            sensorStone.GetComponent<Renderer>().enabled = true;
            sensorStoneTrans.position = mainCamera.GetComponent<Transform>().position + 1 * mainCamera.GetComponent<Transform>().forward;
            sensorStoneTrans.rotation = mainCamera.transform.rotation * curentGoStoneRotation;

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

                if (currentPlayerColor == StoneColor.Black)
                { currentPlayerColor = StoneColor.White; }
                else if (currentPlayerColor == StoneColor.White)
                { currentPlayerColor = StoneColor.Black; }

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
                    DisableButtons();

                    sensorStone.GetComponent<Renderer>().enabled = true;
                    isOnFirstOgPlay = false;
                }

                //back to go play
                else
                {
                    isOgFired = false;

                    mainCamera.GetComponent<Transform>().position = defaultCameraPosition;
                    mainCamera.GetComponent<Transform>().rotation = defaultCameraRotation;
                    mainCamera.orthographic = true;
                    EnableButtons();

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

    public void PlaceGoStone(GoStone stoneCoordinates, List<GoStone> groupStonesToKill)
    {
        isValidPlay = false;

        stonePosHistory.Add(new List<GoStone>());

        for (int i = 0; i < stonePosHistory[stonePosHistory.Count - 2].Count; i++)
        {
            stonePosHistory.Last().Add(stonePosHistory[stonePosHistory.Count - 2][i]);
        }

        if (groupStonesToKill != null)
        {
            //todo move this further down?
            KillGroupStones(groupStonesToKill);
        }

        sensorStone.GetComponent<Renderer>().enabled = false;

        GameObject newStoneObject = Instantiate(genericStoneObject,
                                          new Vector3(stoneCoordinates.x * boardCoordinateSeparationX,
                                                       -stoneCoordinates.y * boardCoordinateSeparationY,
                                                       -stoneZValue),
                                          Quaternion.identity);

        newStoneObject.name = $"{stoneCoordinates.x}x{stoneCoordinates.y}x{currentPlayerColor}Stone";
        newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);


        stonePosHistory.Last().Add(new GoStone
        {
            x = stoneCoordinates.x,
            y = stoneCoordinates.y,
            stoneColor = currentPlayerColor,
            gameObject = newStoneObject

        });

        if (currentPlayerColor == StoneColor.Black)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.GetComponent<MeshRenderer>().material = whiteMaterialTransparent;
        }
        else if (currentPlayerColor == StoneColor.White)
        {
            newStoneObject.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.GetComponent<MeshRenderer>().material = blackMaterialTransparent;
        }
    }

    //todo have direction, speed, etc passed here
    private void ThrowGoStone()
    {
        GameObject newStone = Instantiate(genericStoneObject, sensorStoneTrans.position, Quaternion.identity);
        newStone.name = $"19x19xStone";
        newStone.GetComponent<Transform>().rotation = mainCamera.transform.rotation * curentGoStoneRotation;
        sensorStone.GetComponent<Renderer>().enabled = false;

        if (currentPlayerColor == StoneColor.Black)
        {
            newStone.GetComponent<MeshRenderer>().material = blackMaterial;
            sensorStone.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.50f);
        }
        else if (currentPlayerColor == StoneColor.White)
        {
            newStone.GetComponent<MeshRenderer>().material = whiteMaterial;
            sensorStone.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 0f, 0.50f);
        }

        newStone.GetComponent<Rigidbody>().velocity = ogVelocity * mainCamera.transform.forward;

        //todo implement this
        //currentGameState = GameState.ProcessingThrow;

        currentGameState = GameState.PlaceStone;
        isOgFired = true;
        timer = Time.time;
    }

    public ValidPlayData ValidPlayCheck(GoStone centerStone)
    {
        List<GoStone> groupStonesToKill = new List<GoStone>();

        if (stonePosHistory.Last().Find(s => (s.x == centerStone.x) &&
                                             (s.y == centerStone.y)) != null)
        {
            return new ValidPlayData() { isValidPlay = false };
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

        //Simple Ko rule
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

        if (openSides.Length > 0)
        { return new ValidPlayData() { isValidPlay = true, groupStonesToKill = groupStonesToKill }; }
        else { return new ValidPlayData() { isValidPlay = false }; }
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
    //private StonePosHistoryEntry GetNewBoardLayout()
    {

        //todo implement this?
        //StonePosHistoryEntry newStonePosHistoryEntry = new StonePosHistoryEntry();

        stonePosHistory.Add(new List<GoStone>());


        //finds stones in layer 8, "Stone"
        Collider[] sortedStonesInRange = Physics.OverlapSphere(new Vector3(0, 0, 0), 10, LayerMask.GetMask("Stone"));


        foreach (Collider sortedStone in sortedStonesInRange)
        {
            //changes layer to "SortingStone"
            sortedStone.gameObject.layer = 9;
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

                    GoStone localStone = stonePosHistory.Last().Find(s => s.x == iteratedX &&
                                                                          s.y == iteratedY);


                    if (localStone == null) { localStone = new GoStone(); }

                    if (localStone.stoneColor != StoneColor.None) { continue; }

                    Collider[] stonesInRange = Physics.OverlapSphere(new Vector3(boardCoordinateSeparationX * iteratedX,
                                                                                -boardCoordinateSeparationY * iteratedY,
                                                                                 0),
                                                                     boardCoordinateSeparationX * 19 * 2 * (r / searchIncrement),
                                                                     //9);
                                                                     LayerMask.GetMask("SortingStone"));

                    if (stonesInRange.Length == 0)
                    {
                        continue;
                    }

                    string foundStoneColor = stonesInRange[0].name.Contains("Black") ? "Black" : "White";

                    stonesInRange[0].name = $"{iteratedX}x{iteratedY}x{foundStoneColor}Stone";

                    GoStone sortedStone = SortStone(stonesInRange[0], iteratedX, iteratedY);




                    //newStonePosHistoryEntry.stones.Add(new GoStone { x = sortedStone.x, y = sortedStone.y, stoneColor = sortedStone.stoneColor });
                    //newStonePosHistoryEntry.gameObjects[sortedStone.x, sortedStone.y] = sortedStone.gameObject;
                }
            }
        }

        List<GoStone> alreadyGroupedStones = new List<GoStone>();

        List<StoneColor> StoneColors = new List<StoneColor>() {
                                            currentPlayerColor == StoneColor.White ? StoneColor.Black :StoneColor.White
                                            ,
                                            currentPlayerColor
        };

        //todo improve this?
        foreach (StoneColor iteratedColor in StoneColors)
        {
            for (int iteratedY = 0; iteratedY < 19; iteratedY++)
            {
                for (int iteratedX = 0; iteratedX < 19; iteratedX++)
                {

                    //GoStone localStone = newStonePosHistoryEntry.stones.Find(s => (s.x == iteratedX) &&
                    //                                                              (s.y == iteratedY));

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
                    stonePosHistory.Last(),
                    ref stoneGroup
                    );

                    foreach (GoStoneCoordinates coordinates in stoneGroup.libertyCoordinates)
                    {
                        //print("xy: " + coordinates.x + " " + coordinates.y);
                    }

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
                            groupStonesToKill.Add(stone);
                        }

                        KillGroupStones(stoneGroup.stones);
                    }
                }
            }
        }
        //return newStonePosHistoryEntry;
    }

    public void PlusOneToScore(StoneColor stoneColor)
    {
        if (stoneColor == StoneColor.Black)
        {
            ChangeFloatAndText(ref blackScore, 1, blackTextObject, "Black Captures: ", false);
        }
        if (stoneColor == StoneColor.White)
        {
            ChangeFloatAndText(ref whiteScore, 1, whiteTextObject, "White Captures: ", false);
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadScene("og");
    }

    public enum GameState
    {
        PlaceStone,
        ThrowStone
    }

    private void OgNow()
    {
        currentGameState = GameState.ThrowStone;
        curentGoStoneRotation = Quaternion.identity;
        isOgNowFirstFrame = true;
        ogProb = ogBaseProb;
        ogProbText.GetComponent<Text>().text = "Og prob: " + ogProb + "%";
        mainCamera.GetComponent<Transform>().position = ogCameraReference.GetComponent<Transform>().position;
        mainCamera.GetComponent<Transform>().rotation = ogCameraReference.GetComponent<Transform>().rotation;
        sensorStone.GetComponent<Renderer>().enabled = true;
        DisableButtons();
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

    public void KillStoneWithDelay(GoStone StoneToDestroy, float destroyDelay, float entireDelay = 0)
    {

        stonePosHistory.Last().Remove(stonePosHistory.Last().Find(s => (s.x == StoneToDestroy.x) &&
                                                                       (s.y == StoneToDestroy.y)));

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
            yield return new WaitForSeconds(entireDelay);
            //yield return new WaitForSeconds(entireDelay);

            GameObject exploder = boardExploderGameObjects[StoneToDestroy.y, StoneToDestroy.x];

            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().frame = 7;
            exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Play();

            yield return new WaitForSeconds(0.2f);

            exploder.transform.position = StoneToDestroy.gameObject.transform.position;
            exploder.GetComponent<Renderer>().enabled = true;
            exploder.transform.LookAt(mainCamera.transform, new Vector3(0, 0, 1));
            exploder.transform.rotation = mainCamera.transform.rotation;

            Destroy(StoneToDestroy.gameObject);

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

        StoneColor stoneToSortColor = StoneColor.None;



        stoneToSortColor = stoneToSort.name.Contains("Black") ? StoneColor.Black : StoneColor.White;

        stonePosHistory.Last().Add(new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor, gameObject = stoneToSort.gameObject });

        //changes layer to "Stone"
        stoneToSort.gameObject.layer = 8;

        //todo use this return value
        return new GoStone { x = CoordinateX, y = CoordinateY, stoneColor = stoneToSortColor, gameObject = stoneToSort.gameObject };
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
        public StoneColor stoneColor = StoneColor.None;
        //public List<GoStoneCoordinates> libertyCoordinates = new List<GoStoneCoordinates>();

        public GameObject gameObject;

        public readonly static float diameter = 0.22f;
    }

    public class GoStoneGroup
    {
        public List<GoStone> stones = new List<GoStone>();
        public List<GoStoneCoordinates> libertyCoordinates = new List<GoStoneCoordinates>();
    }

    public enum StoneColor
    {
        None,
        Black,
        White
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
    }

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