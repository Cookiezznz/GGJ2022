using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameController : MonoBehaviour
{
    /// <summary>
    /// Contains all of the Internal Game Systems
    /// </summary>
    
    //Components & Objects
    public Player player;
    public spawnController sc;
    public Canvas canvas;
    public Enemy[] enemies;

    //Variables
    public string input;

    private int turnsTaken;
    private int boundX = 35;
    private int boundY = 27;
    private bool awaitInput = true;


    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if(!player.IsDead())
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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    input = "wait";
                }
                if (input != null)
                {
                    awaitInput = false;
                    Tick(input);
                }
            }
        }
        else
        {
            EndGame();
        }


    }

    void Tick(string input)
    {
        turnsTaken++;
        sc.Tick();
        //Player Turn
        player.Move(input);

        //Enemy Turn -- Iterative
        enemies = sc.GetComponentsInChildren<Enemy>();
        for(int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            if (!enemy.IsDead()) //If Alive - Do Action
            {
                if(enemy.ShouldAttacked())
                {
                    enemy.AttackPlayer(player);
                }
                else
                {
                    enemy.Move();
                    if (enemy.ShouldAttacked())
                    {
                        enemy.AttackPlayer(player);
                    }
                }
            }
            else //Else - Destroy it! Cast it into the fire!
            {
                Destroy(enemy.gameObject);
            }
            
            
        }

        //Resolve Conflicts


        //Next Turn

        awaitInput = true;
        UpdateUI();

    }

    public Vector3Int GetBounds()
    {
        Vector3Int bounds = new Vector3Int(boundX, boundY, 0);
        return bounds;
    }

    void UpdateUI()
    {
        string UI_turnsTaken = "Turns Taken: " + turnsTaken;
        TextMeshProUGUI[] tmp_componentsArray = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < tmp_componentsArray.Length; i++)
        {
            TextMeshProUGUI current = tmp_componentsArray[i];
            if (current.gameObject.name == "TurnsTaken")
            {
                current.text = UI_turnsTaken;
            }

            if (current.gameObject.name == "PlayerHealth")
            {
                current.text = "Player Health: " + player.GetHealth().ToString();
            }
        }
    }

    public int GetTurns()
    {
        return turnsTaken;
    }

    private void EndGame()
    {
        TextMeshProUGUI[] tmpA_youDied = canvas.GetComponentsInChildren<TextMeshProUGUI>(true);
        for(int i = 0; i < tmpA_youDied.Length; i++)
        {
            Debug.Log(tmpA_youDied[i].gameObject.name);
            if(tmpA_youDied[i].gameObject.name == "YouDied")
            {
                tmpA_youDied[i].gameObject.SetActive(true);
            }
        }
        
    }




}
