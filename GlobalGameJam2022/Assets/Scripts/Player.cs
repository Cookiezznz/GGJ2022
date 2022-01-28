using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public gameController gc;
    public spawnController sc;

    //Variables/Statistics
    private Vector2 scanRange = new Vector2(3, 3);
    private int i_health = 100;
    private int i_damage = 10;
    private int i_xp = 0;
    private bool b_isDead = false;
    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = gc.GetBounds() / 2;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(string input)
    {
        prevPos = transform.position;
        Vector3 bounds = gc.GetBounds();
        switch (input)
        {
            case "up":
                if (transform.position.y < bounds.y)
                {
                    transform.position += Vector3.up;
                }
                break;
            case "left":
                if (transform.position.x > 0)
                {
                    transform.position += Vector3.left;
                }

                break;
            case "down":
                if (transform.position.y > 0)
                {
                    transform.position += Vector3.down;
                }
                break;
            case "right":
                if (transform.position.x < bounds.x)
                {
                    transform.position += Vector3.right;
                }
                break;
            case "wait":
                break;
            default:
                break;
        }
        AttackEnemy(); //Attack any enemy you move onto




    }

    void AttackEnemy()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, scanRange, 0, Vector2.zero);
        for (int i = 0; i < hits.Length && hits.Length > 0; i++)
        {
            Enemy enemy = hits[i].collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (transform.position == enemy.transform.position) //If Enemy overlaps player, move to previous position.
                {
                    enemy.TakeDamage(i_damage);
                    if(enemy.IsDead())
                    {
                        //If Kills enemy > Do something
                    }
                    else
                    {
                        UndoMove();
                    }
                }

            }
        }
        
    }

    void UndoMove()
    {
        transform.position = prevPos;
    }

    public void TakeDamage(int damage)
    {
        i_health -= damage;
        Debug.Log("Took Damage!");
        if(i_health <= 0)
        {
            i_health = 0;
            Die();
        }
    }

    void Die()
    {
        b_isDead = true;
        Debug.Log(this.gameObject + " Died");
    }

    public bool IsDead()
    {
        return b_isDead;
    }

    public int GetHealth()
    {
        return i_health;
    }

}


