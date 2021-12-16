using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Assets.Scripts.GameController;
using static Assets.Scripts.GoFunctions;

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
        Assets.Scripts.GoFunctions goFunctions = new GoFunctions();
        GameController gameController = new GameController();
        string[] xy = other.gameObject.name.Split('x');

        if (xy.Length > 0 && other.gameObject.name.Contains("Stone"))
        {
            Assets.Scripts.GoFunctions.StoneColor stoneColor = Assets.Scripts.GoFunctions.StoneColor.Black;
            if (other.gameObject.name.Contains("White"))
            {
                stoneColor = Assets.Scripts.GoFunctions.StoneColor.White;
            }
             else if (other.gameObject.name.Contains("Black"))
            {
                stoneColor = Assets.Scripts.GoFunctions.StoneColor.Black;
            }

            gameController.KillStoneWithDelayUnity(
                    new Assets.Scripts.GoFunctions.GoStone(
                        new Assets.Scripts.GoFunctions.BoardCoordinates(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1])), 
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
