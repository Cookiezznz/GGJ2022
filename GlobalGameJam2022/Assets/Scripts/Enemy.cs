using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public gameController gc;
    public Collider2D col;
    public int enemyNum;

    private int i_health = 20;
    private int i_damage = 3;
    private bool b_isDead = false;
    private bool b_attack = false;

    void Start()
    {
        col = GetComponent<Collider2D>();
        gc = FindObjectOfType<Player>().GetComponent<gameController>();

        bool valid = false;
        int randX;
        int randY;
        do
        {
            randX = Random.Range(0, gc.GetBounds().x);
            randY = Random.Range(0, gc.GetBounds().y);
            if (randX < gc.player.transform.position.x - 10 && randX > gc.player.transform.position.x + 10)
            {
                if (randY < gc.player.transform.position.y - 10 && randY > gc.player.transform.position.y + 10)
                {
                    valid = true;
                }
            }

            

        } while (!valid);
        transform.position = new Vector3(randX, randY, 0);
    }

    public void Move()
    {
        gc = FindObjectOfType<Player>().GetComponent<gameController>();
        Vector3 prevPos = transform.position;
        Vector3Int bounds = gc.GetBounds();
        int direction = GetMoveDirection();
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

    public void TakeDamage(int damage)
    {
        i_health -= damage;
        b_attack = true;
        if (i_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        i_health = 0;
        b_isDead = true;
    }

    public bool IsDead()
    {
        return b_isDead;
    }
    
}
