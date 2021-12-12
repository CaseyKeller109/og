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

        if (xy.Length > 0 && other.gameObject.name.Contains("Stone"))
        {
            GameController.StoneColor stoneColor = GameController.StoneColor.Black;
            if (other.gameObject.name.Contains("White"))
            {
                stoneColor = GameController.StoneColor.White;
            }
             else if (other.gameObject.name.Contains("Black"))
            {
                stoneColor = GameController.StoneColor.Black;
            }

            gameController.GetComponent<GameController>().KillStoneWithDelay(
                    new GameController.GoStone(
                        new GameController.BoardCoordinates(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1])), 
                        stoneColor,
                        other.gameObject), 
            0f);
        }
        else
        {
            print("non-stone object trying to be destroyed");
        }
    }
}
