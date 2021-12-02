using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        string[] xy = other.gameObject.name.Split('x');
        //print(other.gameObject.name);

        if (xy.Length > 0 && other.gameObject.name.Contains("Stone"))
        {
            gameController.GetComponent<GameController>().KillStoneWithDelay(new GameController.GoStone { coordinates = { x = Convert.ToInt32(xy[0]), y = Convert.ToInt32(xy[1]) }, gameObject = other.gameObject }, 0f);
        }
        else
        {
            print("non-stone object trying to be destroyed");
        }
    }
}
