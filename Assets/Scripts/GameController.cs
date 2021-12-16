using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using static Assets.Scripts.GoFunctions;
using PV = Assets.Scripts.GoFunctions.PlayValidity;


namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
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

        //2todo use sensorstone rotation instead of this?
        //sensorStone.gameObject.GetComponent<Transform>().rotation
        public static Quaternion curentGoStoneRotation;



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





        private void Awake()
        {

            GoFunctions goFunctions = new GoFunctions();
            genericStoneObject = Resources.Load("Stone") as GameObject;

            previousMouseCoordinates = new BoardCoordinates(-1, -1);

            sensorStone = new GoStone(null, StoneColor.Black, sensorStoneObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            Console.Write("hello world");

            Application.targetFrameRate = 60;
            resetButton.onClick.AddListener(ResetGame);

            //sensorStone = new GoStone(null, StoneColor.Black, sensorStoneObject);

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

            //1todo set these in editor?
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
                    RenderSensorStone(false);
                }

                else
                {
                    //3todo can improve by not checking every time
                    ValidPlayData validPlayData = GoFunctions.ValidPlayCheck(possibleStoneCoordinates, Currents.currentPlayerColor);

                    if (!previousMouseCoordinates.SameCoordinatesAs(possibleStoneCoordinates))
                    {
                        Currents.playValidity = PV.NotYetSet;
                    }

                    if (Currents.playValidity == PV.NotYetSet)
                    {
                        Currents.playValidity = validPlayData.playValidityLocal;
                    }

                    possibleStoneCoordinates.CopyCoordinatesTo(previousMouseCoordinates);

                    if (Currents.playValidity == PV.Valid)
                    {
                        RenderSensorStone(true);
                    }
                    else if (Currents.playValidity == PV.Invalid)
                    {
                        RenderSensorStone(false);
                    }

                    sensorStone.gameObject.GetComponent<Transform>().position =
                            new Vector3((float)(possibleStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
                                        (float)(-possibleStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
                                        -GoStone.ZHeightValue);
                    sensorStone.gameObject.transform.rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

                    if (Input.GetMouseButtonUp(0) && Currents.playValidity == PV.Valid)
                    {
                        PlaceGoStoneUnity(possibleStoneCoordinates, validPlayData.groupStonesToKill);


                        //GameObject newStoneObject = GameObject.Instantiate(genericStoneObject,
                        //          new Vector3((float)(possibleStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
                        //                      (float)(-possibleStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
                        //                      -GoStone.ZHeightValue),
                        //          Quaternion.identity);
                        //if (Currents.currentPlayerColor == StoneColor.Black)
                        //{
                        //    newStoneObject.GetComponent<MeshRenderer>().material = blackMaterial;
                        //    sensorStone.gameObject.GetComponent<MeshRenderer>().material = whiteMaterialTransparent;
                        //    sensorStone.gameObject.name = "WhiteSensorStone";
                        //}
                        //else if (Currents.currentPlayerColor == StoneColor.White)
                        //{
                        //    newStoneObject.GetComponent<MeshRenderer>().material = whiteMaterial;
                        //    sensorStone.gameObject.GetComponent<MeshRenderer>().material = blackMaterialTransparent;
                        //    sensorStone.gameObject.name = "BlackSensorStone";
                        //}

                        //newStoneObject.name = $"{possibleStoneCoordinates.xCoord}x" +
                        //                      $"{possibleStoneCoordinates.yCoord}x" +
                        //                      $"{Currents.currentPlayerColor}Stone";
                        //newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);



                        //GoFunctions.PlaceGoStone(possibleStoneCoordinates, validPlayData.groupStonesToKill, BoardHistory, newStoneObject);


                        ////PlaceGoStone Game




                        //if (validPlayData.groupStonesToKill != null)
                        //{
                        //    KillGroupStones(validPlayData.groupStonesToKill);
                        //}


                        ////GameObject newStoneObject = GameObject.Instantiate(genericStoneObject,
                        ////                                  new Vector3((float)(possibleStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
                        ////                                              (float)(-possibleStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
                        ////                                              -GoStone.ZHeightValue),
                        ////                                  Quaternion.identity);

                        ////newStoneObject.name = $"{possibleStoneCoordinates.xCoord}x" +
                        ////                      $"{possibleStoneCoordinates.yCoord}x" +
                        ////                      $"{Currents.currentPlayerColor}Stone";
                        ////newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);










                        RenderSensorStone(false);


                        if (Currents.currentPlayerColor == StoneColor.Black)
                        { Currents.currentPlayerColor = StoneColor.White; }
                        else if (Currents.currentPlayerColor == StoneColor.White)
                        { Currents.currentPlayerColor = StoneColor.Black; }

                        if (UnityEngine.Random.Range(0, 100) < ThrowingData.ogProb)
                        {
                            //to og play
                            Currents.currentGameState = GameState.CanThrowStone;
                            curentGoStoneRotation = Quaternion.identity;
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
                RenderSensorStone(true);
                sensorStone.gameObject.transform.position = mainCamera.GetComponent<Transform>().position + 1 * mainCamera.GetComponent<Transform>().forward;
                sensorStone.gameObject.transform.rotation = mainCamera.transform.rotation * curentGoStoneRotation;

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
                Quaternion rotateSpeed = Quaternion.Euler(5, 0, 0);
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
                    curentGoStoneRotation *= rotateSpeed;
                }
                if (Input.GetKey("q"))
                {
                    curentGoStoneRotation *= Quaternion.Inverse(rotateSpeed);
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
                                 mainCamera.transform.rotation * curentGoStoneRotation,
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
                    GetNewBoardLayout(BoardHistory);

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
                        curentGoStoneRotation = Quaternion.identity;

                        ThrowingData.ogProb = ThrowingData.ogBaseProb;
                        ogProbText.GetComponent<Text>().text = "Og Prob: " + ThrowingData.ogProb + "%";
                        mainCamera.GetComponent<Transform>().position = defaultThrowingCamera.GetComponent<Transform>().position;
                        mainCamera.GetComponent<Transform>().rotation = defaultThrowingCamera.GetComponent<Transform>().rotation;
                        DisableButtons();

                        RenderSensorStone(true);
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


        public void PlaceGoStoneUnity(BoardCoordinates possibleStoneCoordinates, List<BoardCoordinates> groupStonesToKill)
        {

            GameObject newStoneObject = GameObject.Instantiate(genericStoneObject,
                      new Vector3((float)(possibleStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
                                  (float)(-possibleStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
                                  -GoStone.ZHeightValue),
                      Quaternion.identity);
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

            newStoneObject.name = $"{possibleStoneCoordinates.xCoord}x" +
                                  $"{possibleStoneCoordinates.yCoord}x" +
                                  $"{Currents.currentPlayerColor}Stone";
            newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);



            GoFunctions.PlaceGoStone(possibleStoneCoordinates, groupStonesToKill, BoardHistory, newStoneObject);


            //PlaceGoStone Game




            if (groupStonesToKill != null)
            {
                KillGroupStonesUnity(groupStonesToKill, LatestBoardStones());
            }


            //GameObject newStoneObject = GameObject.Instantiate(genericStoneObject,
            //                                  new Vector3((float)(possibleStoneCoordinates.xCoord * GoBoard.boardCoordinateSeparationX),
            //                                              (float)(-possibleStoneCoordinates.yCoord * GoBoard.boardCoordinateSeparationY),
            //                                              -GoStone.ZHeightValue),
            //                                  Quaternion.identity);

            //newStoneObject.name = $"{possibleStoneCoordinates.xCoord}x" +
            //                      $"{possibleStoneCoordinates.yCoord}x" +
            //                      $"{Currents.currentPlayerColor}Stone";
            //newStoneObject.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);







        }


        //2todo make unit tests for ThrowGoStone?
        public void ThrowGoStone(Vector3 StonePosition,
                                  Quaternion StoneRotation,
                                  Vector3 StoneVelocity)
        {
            GameObject thrownStoneObject = GameObject.Instantiate(genericStoneObject, new Vector3(0, 0, 0), Quaternion.identity);

            Transform thrownTransform = thrownStoneObject.GetComponent<Transform>();
            Rigidbody thrownRigidbody = thrownStoneObject.GetComponent<Rigidbody>();
            Renderer sensorRenderer = sensorStone.gameObject.GetComponent<Renderer>();

            thrownTransform.position = StonePosition;
            thrownTransform.rotation = StoneRotation;
            thrownRigidbody.velocity = StoneVelocity;
            sensorRenderer.enabled = false;

            thrownStone = new GoStone(null, Currents.currentPlayerColor, thrownStoneObject);

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

        public void GetNewBoardLayout(List<GoBoard> BoardHistory)
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
            KillSortedStones( LatestBoardStones());
            //print(BoardHistory.Last().boardStones.Count);
        }

        public static void FindAndSortAllStones()
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
                        LatestBoardStones().Add(sortedStone);
                    }
                }
            }
        }


        public void KillSortedStones(List<GoStone> latestBoardStones)
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
                        GoStone localStone = latestBoardStones.Find(latestStone => (latestStone.Coordinates.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))));

                        if (localStone == null)
                        {
                            continue;
                        }

                        if (localStone.stoneColor != iteratedColor ||
                            alreadyGroupedStones.Find(groupedStone => groupedStone.SameCoordinatesAs(new BoardCoordinates(iteratedX, iteratedY))) != null)
                        {
                            continue;
                        }

                        GoStoneGroupData stoneGroupData = new GoStoneGroupData();

                        List<GoStoneLite> LatestBoardStonesLite = new List<GoStoneLite>();
                        foreach (GoStone stone in latestBoardStones)
                        {
                            LatestBoardStonesLite.Add(ToLite(stone));
                        }

                        //return groupstonestokill?
                        //3todo make sure all FindGroupAndLibertyCoordinates are passing color in?
                        stoneGroupData = FindGroupAndLibertyCoordinates(new GoStoneLite
                            (iteratedX, iteratedY, localStone.stoneColor),
                            LatestBoardStonesLite,
                            stoneGroupData
                        );

                        foreach (BoardCoordinates newStone in stoneGroupData.stonesCoord)
                        {
                            if (!alreadyGroupedStones.Any(groupedStone => groupedStone.SameCoordinatesAs(newStone)))
                            {
                                alreadyGroupedStones.Add(newStone);
                            }
                        }

                        if (stoneGroupData.libertyCoordinates.Count == 0)
                        {
                            KillGroupStonesUnity(stoneGroupData.stonesCoord, LatestBoardStones());
                        }
                    }
                }
            }
        }



        public void KillGroupStonesUnity(List<BoardCoordinates> groupStonesToKill, List<GoStone> latestBoardStones)
        {
            //Assets.Scripts.GoFunctions.KillGroupStones(groupStonesToKill);

            foreach (BoardCoordinates killCoord in groupStonesToKill)
            {

                GoStone stoneToDestroy = LatestBoardStones().Find(latestStone => (latestStone.SameCoordinatesAs(killCoord)));

                if (stoneToDestroy == null) { continue; }

                KillStoneWithDelayUnity(
                              stoneToDestroy,
                             0.2f,
                             0.2f * groupStonesToKill.IndexOf(killCoord));
            }
        }

        public void KillStoneWithDelayUnity(GoStone StoneToDestroy,
                                       float destroyDelay,
                                       float totalDelay = 0)
        {

            LatestBoardStones().Remove(LatestBoardStones().Find(latestStone => (latestStone.SameCoordinatesAs(StoneToDestroy))));

            if (StoneToDestroy.stoneColor == StoneColor.Black)
            {
                PlusOneToScoreUnity(StoneColor.White);
            }
            else if (StoneToDestroy.stoneColor == StoneColor.White)
            {
                PlusOneToScoreUnity(StoneColor.Black);
            }

            //Assets.Scripts.GoFunctions.KillStone(StoneToDestroy);

            StoneToDestroy.gameObject.layer = 8;

            //1todo use something other than Text?
            Text coroutineHandler = (new GameObject("_coroutineHandler")).AddComponent<Text>();
            coroutineHandler.StartCoroutine(DelayDestroyCoroutine());

            IEnumerator DelayDestroyCoroutine()
            {
                yield return new WaitForSeconds(totalDelay);
                //yield return new WaitForSeconds(entireDelay);

                GameObject exploder = StoneToDestroy.exploderGameObject;
                exploder = GameObject.Instantiate(explosion, new Vector3(0, 0, 0), Quaternion.identity);

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

                GameController.Destroy(StoneToDestroy.gameObject);

                Currents.playValidity = PV.NotYetSet;

                yield return new WaitForSeconds(7);
                exploder.GetComponent<Renderer>().enabled = false;
                exploder.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
            }
        }

        public void PlusOneToScoreUnity(StoneColor stoneColor)
        {
            if (stoneColor == StoneColor.Black)
            {
                PlusOneToScore(StoneColor.Black);

                blackTextObject.GetComponent<Text>().text = "Black Captures: " + PlayerScore.blackScore;
            }
            if (stoneColor == StoneColor.White)
            {
                PlusOneToScore(StoneColor.White);

                whiteTextObject.GetComponent<Text>().text = "White Captures: " + PlayerScore.whiteScore;
            }
        }





        private void OgNow()
        {
            Currents.currentGameState = GameState.CanThrowStone;
            curentGoStoneRotation = Quaternion.identity;
            isOgNowFirstFrame = true;
            ThrowingData.ogProb = ThrowingData.ogBaseProb;
            ogProbText.GetComponent<Text>().text = "Og prob: " + ThrowingData.ogProb + "%";
            mainCamera.GetComponent<Transform>().position = defaultThrowingCamera.GetComponent<Transform>().position;
            mainCamera.GetComponent<Transform>().rotation = defaultThrowingCamera.GetComponent<Transform>().rotation;
            RenderSensorStone(true);
            DisableButtons();
        }

        public void RenderSensorStone(bool isRender)
        {
            sensorStone.gameObject.GetComponent<Renderer>().enabled = isRender;
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

        public void ChangeFloatAndText(ref float valueToChange, int valueAdded, GameObject textObject, string text, bool isPercent)
        {
            valueToChange += valueAdded;

            textObject.GetComponent<Text>().text = text + valueToChange + (isPercent ? "%" : "");
        }
    }
}


//Explosion effect and sound used under Creative Commons(https://creativecommons.org/licenses/by/3.0/)
//From Youtube user Timothy (https://www.youtube.com/channel/UCrxGMPla5PpIdeSyGQFaXhg) (https://www.youtube.com/watch?v=Q7KmAe8_jZE)

//Go board image used under Public Domain from Wikipedia (https://en.wikipedia.org/wiki/File:Blank_Go_board.svg)

//Sky background used from (sketchuptexture.com/2013/02/panoramic-ski-360.html)

//Original concept from Nicholas Abentroth

//Copyright 2021, Casey Keller, All rights reserved.