using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    //Board Coordinates are used as follows
    //   0 1 2 .. 16 17 18 
    // 0
    // 1
    // 2
    // ..
    // 16
    // 17
    // 18

    // upper left coordinates (0,0) are at (0,0) in real-world space
    // coordinates increment in real-world space by 0.2211 in x, and 0.2366 in y.

    // center of board coordinates (9,9) in real-world space are
    // (1.9899, -2.1294)

    //lower-right corner coordinates (18,18) in real-world space are
    // (3.9798, -4.2588)

    public GameObject gameController;

    public bool isAfterStonesSettled = false;
    public bool isAfterWallsDoneMoving = false;
    public Vector3 defaultPos;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAfterStonesSettled)
        {
            gameObject.GetComponent<Collider>().isTrigger = false;

            if (transform.position.x < -0.2211)
            {
                transform.position += new Vector3(0.02f, 0, 0);
            }
            if (transform.position.x > 3.9798 + 0.2211)
            {
                transform.position += new Vector3(-0.02f, 0, 0);
            }
            if (transform.position.y < -4.2588 - 0.2211)
            {
                transform.position += new Vector3(0, 0.02f, 0);
            }
            if (transform.position.y > 0.2211)
            {
                transform.position += new Vector3(0, -0.02f, 0);
            }
            if (!(transform.position.y > 0.2211))
            {
                isAfterWallsDoneMoving = true;
                Physics.gravity = new Vector3(0,0,(9.8f*15));
            }
        }

        else if (!isAfterStonesSettled)
        {
            Physics.gravity = new Vector3(0, 0, 9.8f);
            gameObject.GetComponent<Collider>().isTrigger = true;
            transform.position = defaultPos;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!isAfterStonesSettled)
    //    {
    //        if (other.GetComponent<MeshRenderer>().material.name == "Black Stone (Instance)")
    //        {
    //            gameController.GetComponent<GameController>().PlusOneToScore(GameController.StoneColor.white);
    //        }
    //        if (other.GetComponent<MeshRenderer>().material.name == "White Stone (Instance)")
    //        {
    //            gameController.GetComponent<GameController>().PlusOneToScore(GameController.StoneColor.black);
    //        }

    //        string[] xy = other.gameObject.name.Split('x');

    //        gameController.GetComponent<GameController>().KillStoneWithDelay(new GameController.GoStone { x = Convert.ToInt32(xy[0]), y = Convert.ToInt32(xy[1]), gameObject = other.gameObject }, 0f);
    //        //here.
    //        //gameController.GetComponent<GameController>().KillStoneWithDelay(other.gameObject, new GameController.GoStone { x= Convert.ToInt32(xy[0]), y= Convert.ToInt32(xy[1]) }, 0f);
    //    }
    //}
}
