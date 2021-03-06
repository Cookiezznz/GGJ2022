using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public audioController ac;
    public gameController gc;
    public Collider2D col;
    public popupController damagePopup;
    public int enemyNum;

    private int i_health = 20;
    private int i_damage = 3;
    private int i_xp = 5;
    private bool b_isDead = false;
    private bool b_attack = false;
    public int moveAttemptCount;
    private Vector3 prevPos;

    void Start()
    {
        gameObject.name = "Warrior of Light";
        ac = GameObject.Find("AudioController").GetComponent<audioController>();
        col = GetComponent<Collider2D>();
        gc = FindObjectOfType<Player>().GetComponent<gameController>();
        damagePopup = GameObject.Find("popupController").GetComponent<popupController>();

        

        //Create spawn location
        int spawnX = 0;
        int spawnY = 0;
        int spawnWall = Random.Range(0, 4); //Top/Left/Right/Down walls -- Choose a random wall to spawn on
        switch(spawnWall)
        {
            case 0: //Spawn on top wall
                spawnX = Random.Range(0, gc.GetBounds().x);
                spawnY = gc.GetBounds().y - 1;
                break;
            case 1: //Spawn on Left wall
                spawnX = 0;
                spawnY = Random.Range(0, gc.GetBounds().y);
                break;
            case 2: //Spawn on Right wall
                spawnX = gc.GetBounds().x - 1;
                spawnY = Random.Range(0, gc.GetBounds().y);
                break;
            case 3: //Spawn on Bottom wall
                spawnX = Random.Range(0, gc.GetBounds().x);
                spawnY = 0;
                break;
            default:
                break;
        }
        transform.position = new Vector3(spawnX, spawnY, 0);

        //Set Stats
        i_health = i_health + (gc.player.GetLevel() * 2);
        i_damage = i_damage + (gc.player.GetLevel() * 2);
    }

    public void Move(bool random = false, int direction = 0)
    {
        moveAttemptCount++;
        gc = FindObjectOfType<Player>().GetComponent<gameController>();
        prevPos = transform.position;
        Vector3Int bounds = gc.GetBounds();
        if (!random)
        {
            direction = GetMoveDirection();
        }
        switch (direction)
        {
            case 0:
                if (transform.position.y < bounds.y)
                {
                    transform.position += Vector3.up;
                }
                break;
            case 1:
                if (transform.position.x > 0)
                {
                    transform.position += Vector3.left;
                }
                break;
            case 2:
                if (transform.position.y > 0)
                {
                    transform.position += Vector3.down;
                }
                break;
            case 3:
                if (transform.position.x < bounds.x)
                {
                    transform.position += Vector3.right;
                }
                break;
            default:
                break;
        }
        if(transform.position == prevPos && moveAttemptCount < 4) //If did not move
        {
            Move(true, direction++);
        }
        //If Enemy overlaps player, move to previous position & flag for Attack
        if (transform.position == gc.player.transform.position) 
        {
            b_attack = true;
            transform.position = prevPos;
        }

        //If Enemy overlaps Enemy, move to previous position.
        for (int i = 0; i < gc.enemies.Length; i++)
        {
            if (gc.enemies[i].enemyNum != enemyNum) //Doesnt look at itself.
            {
                if (transform.position == gc.enemies[i].transform.position) //If it has moved onto another enemy
                {
                    transform.position = prevPos;
                    if(moveAttemptCount < 4)
                    {
                        Move(true, direction++);

                    }
                }
            }
        }
        
        

    }

    public int GetMoveDirection()
    {
        //Player Position and Target Position
        Vector3 playerPos = gc.player.transform.position;
        
        //  0/1/2/3 = Up/Left/Down/Right
        List<int> validDirections = new List<int>();
        int direction = 0;

        if(playerPos.x > transform.position.x) //Player is right of Enemy
        {
            validDirections.Add(3);
        }
        if (playerPos.x < transform.position.x) //Player is left of Enemy
        {
            validDirections.Add(1);

        }
        if (playerPos.y > transform.position.y) //Player is Above Enemy
        {
            validDirections.Add(0);

        }
        if (playerPos.y < transform.position.y) //Player is Below Enemy
        {
            validDirections.Add(2);
        }

        if (validDirections.Count > 0) //Choose a random valid direction
        {
            int randIndex = Random.Range(0, validDirections.Count);
            direction = validDirections[randIndex];
        }
        else //Random save
        {
            direction = Random.Range(0, 4);
        }

        return direction;
    }

    public void AttackPlayer(Player player)
    {
        b_attack = false;
        player.TakeDamage(i_damage);

    }

    public bool ShouldAttacked()
    {
        return b_attack;
    }

    public void TakeDamage(int damage, bool melee=false)
    {
        i_health -= damage;
        if (melee)
        {
            b_attack = true;
        }
        if (i_health <= 0)
        {
            Die();
        }

        //Damage Popup
        damagePopup.Create(transform.position, damage, Color.yellow);

        if(Random.Range(0, 10) < 2) //Play sound 20% of the time
        {
            ac.source.volume = 0.3f;
            ac.PlaySound(ac.enemyHit);
            ac.source.volume = 0.5f;

        }    


    }

    void Die()
    {
        i_health = 0;
        b_isDead = true;
        ac.PlaySound(ac.enemyDie);
    }

    public bool IsDead()
    {
        return b_isDead;
    }

    public int GetHealth()
    {
        return i_health;
    }

    public int GetDamage()
    {
        return i_damage;
    }
    
    public int GetXp()
    {
        return i_xp;
    }
}
