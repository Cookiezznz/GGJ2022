using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image xpbar;
    public Image healthbar;

    //Variables
    public string input;

    private int turnsTaken;
    private int boundX = 35;
    private int boundY = 27;
    private bool awaitInput = true;
    private Enemy focusedEnemy;


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
                if (Input.GetKeyDown("q"))
                {
                    input = "ability1";
                }
                if (Input.GetKeyDown("e"))
                {
                    input = "ability2";
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

        if (Input.GetMouseButtonDown(0)) //Mouse Selection
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "enemy")
                {
                    SetFocusedEnemy(hit.transform.gameObject.GetComponent<Enemy>());
                }
            }
        }
        


    }

    void Tick(string input)
    {
        turnsTaken++;
        //Player Turn
        player.Tick(input);
        player.CheckXP();

        //Enemy Turn -- Iterative
        enemies = sc.GetComponentsInChildren<Enemy>();
        for(int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            if (!enemy.IsDead()) //If Alive - Do Action
            {
                if(enemy.ShouldAttacked()) //If attacked by player
                {
                    enemy.AttackPlayer(player);
                }
                else //Move then check for attack
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
        sc.Tick(); //Tick spawn controller

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

    public void UpdateUI()
    {
        TextMeshProUGUI[] tmp_componentsArray = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < tmp_componentsArray.Length; i++)
        {
            //HUD Elements
            xpbar.fillAmount = player.GetXP() / 100f;
            healthbar.fillAmount = player.GetHealth() / 100f;

            //Player / Game Stats
            TextMeshProUGUI current = tmp_componentsArray[i];
            if (current.gameObject.name == "TurnsTaken")
            {
                current.text = "Turns Taken: " + turnsTaken;
            }

            if (current.gameObject.name == "PlayerHealth")
            {
                current.text = "Player Health: " + player.GetHealth().ToString();
            }

            if (current.gameObject.name == "PlayerDamage")
            {
                current.text = "Player Damage: " + player.GetDamage();
            }

            if (current.gameObject.name == "PlayerKills")
            {
                current.text = "Kills: " + player.GetKills();
            }

            if (current.gameObject.name == "PlayerLevel")
            {
                current.text = "Level: " + player.GetLevel();
            }

            //Enemy Stats
            if (focusedEnemy)
            {
                if (current.gameObject.name == "EnemyName")
                {
                    current.text = "Enemy Name: " + focusedEnemy.gameObject.name;
                }

                if (current.gameObject.name == "EnemyHealth")
                {
                    current.text = "Enemy Health: " + focusedEnemy.GetHealth();
                }

                if (current.gameObject.name == "EnemyDamage")
                {
                    current.text = "Enemy Damage: " + focusedEnemy.GetDamage();
                }

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

    public void SetFocusedEnemy(Enemy enemy)
    {
        focusedEnemy = enemy;
        UpdateUI();
    }




}
