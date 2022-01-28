using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    /// <summary>
    /// Contains all of the Internal Game Systems
    /// </summary>
    
    //Components & Objects
    public Player player;
    public Enemy enemy;

    //Variables
    public int boundY = 13;
    public int boundX = 17;
    public string input;

    private bool awaitInput = true;


    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        input = null;
        if (awaitInput)
        {
            if (Input.GetKeyDown("w"))
            {
                input = "up";
            }
            if (Input.GetKeyDown("a"))
            {
                input = "left";
            }
            if (Input.GetKeyDown("s"))
            {
                input = "down";
            }
            if (Input.GetKeyDown("d"))
            {
                input = "right";
            }
            if (input != null)
            {
                awaitInput = false;
                Tick(input);
            }
        }


    }

    void Tick(string input)
    {
        player.Move(input);
        enemy.Move();
        awaitInput = true;

    }




}
