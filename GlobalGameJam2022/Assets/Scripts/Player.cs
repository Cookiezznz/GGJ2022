using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public gameController gc;
    public spawnController sc;
    public LineRenderer lr;

    //Variables/Statistics
    private Vector2 scanRange = new Vector2(3, 3);
    private int i_health = 100;
    private int i_maxHP;
    private int i_damage = 10;
    private int i_xp = 0;
    private int i_killCount = 0;
    private int i_damageTaken = 0;
    private int i_damageDealt = 0;
    private int i_damageHealed = 0;
    private int i_playerLevel = 0;

    private int novaRange = 2; //int range^2 surrounding player.
    private int novaCooldown = 25;
    private int novaCooldownTimer = 0;
    private int boltRange = 10; //int range^2 surrounding player.
    private int boltCooldown = 2;
    private int boltCooldownTimer = 0;



    private bool b_isDead = false;
    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = gc.GetBounds() / 2;
        i_maxHP = i_health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tick(string input)
    {
        UpdateCooldowns();
        lr.positionCount = 2;
        Vector3[] positions = { Vector3.zero, Vector3.zero };
        lr.SetPositions(positions);
        Move(input);
        AttackEnemy(); //Attack any enemy you move onto
    }

    void UpdateCooldowns()
    {
        if (novaCooldownTimer > 0)
        {
            novaCooldownTimer -= 1;
        }
        if (boltCooldownTimer > 0)
        {
            boltCooldownTimer -= 1;
        }
    }

    public void Move(string input) //First action of turn
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
            case "ability1":
                CastAbility(1);
                break;
            case "ability2":
                CastAbility(2);
                break;
            case "wait":
                break;
            default:
                break;
        }
    }

    void AttackEnemy()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, scanRange, 0, Vector2.zero);
        for (int i = 0; i < hits.Length && hits.Length > 0; i++)
        {
            Enemy enemy = hits[i].collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (transform.position == enemy.transform.position) //If Enemy overlaps player
                {
                    enemy.TakeDamage(i_damage, true);
                    i_damageDealt += i_damage;
                    if(enemy.IsDead())
                    {
                        //Add killcount and XP
                        i_killCount++;
                        i_xp += enemy.GetXp();
                        
                    }
                    else
                    {
                        //FOcus enemy and undo move.
                        gc.SetFocusedEnemy(enemy);
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
        if(i_health <= 0)
        {
            i_health = 0;
            Die();
        }
        i_damageTaken += damage;
    }

    public void HealDamage(int value)
    {
        i_health += value;
        if (i_health > i_maxHP)
        {
            i_health = i_maxHP;
        }
        i_damageHealed += value;
    }

    public void IncreaseDamage(int value)
    {
        i_damage += value;
    }

    public void AddXP(int value)
    {
        i_xp += value;
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

    public int GetDamage()
    {
        return i_damage;
    }

    public int GetKills()
    {
        return i_killCount;
    }    

    public int GetLevel()
    {
        return i_playerLevel;
    }

    public int GetXP()
    {
        return i_xp;
    }

    public int GetCooldownBolt()
    {
        return boltCooldownTimer;
    }

    public int GetCooldownNova()
    {
        return novaCooldownTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "healthpickup")
        {
            HealDamage(collision.GetComponent<pickupController>().GetValue());
        }
        if (collision.gameObject.tag == "damagepickup")
        {
            IncreaseDamage(collision.GetComponent<pickupController>().GetValue());
        }
        if (collision.gameObject.tag == "xppickup")
        {
            AddXP(collision.GetComponent<pickupController>().GetValue());
        }
        Destroy(collision.gameObject);
        gc.UpdateUI();
    }

    public void CheckXP()
    {
        if(i_xp > 100) //Level up
        {
            i_xp = 0;
            i_playerLevel++;
            //Full Heal + More Max HP
            i_maxHP += i_maxHP / 100 * 5;
            i_health = i_maxHP;
            //Increase Damage
            i_damage += i_damage / 100 * 5;
        }
    }

    public void CastAbility(int ability)
    {
        if (ability == 1)
        {
            if (novaCooldownTimer == 0)
            {
                CastShadowNova();
                novaCooldownTimer = novaCooldown;
            }
        }
        else
        {
            if (boltCooldownTimer == 0)
            {
                CastShadowBolt();
                boltCooldownTimer = boltCooldown;
            }
        }
    }

    void CastShadowNova()
    {
        //Shadow Nova
        //Hits all enemies in a 3x3 Range!

        Vector2 range_botleft = new Vector2(transform.position.x - novaRange, transform.position.y - novaRange);
        Vector2 range_topright = new Vector2(transform.position.x + novaRange, transform.position.x + novaRange);

        int novaDamage = i_damage * 2;
        Collider2D[] hits = Physics2D.OverlapAreaAll(range_botleft, range_topright);

        lr.positionCount = 8;
        Vector3 pos1 = new Vector3(-novaRange, -novaRange);
        Vector3 pos2 = new Vector3(-novaRange, novaRange);
        Vector3 pos3 = new Vector3(novaRange, novaRange);
        Vector3 pos4 = new Vector3(novaRange, -novaRange);
        Vector3[] positions = { pos1, pos2, pos3, pos4, pos1, pos3, pos2, pos4 };
        lr.SetPositions(positions);


        for (int i = 0; i < hits.Length; i++)
        {
            Enemy enemy = hits[i].gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.tag.Contains("enemy")) ;
                {
                    enemy.TakeDamage(novaDamage);
                    i_damageDealt += novaDamage;
                    if (enemy.IsDead())
                    {
                        //Add killcount and XP
                        i_killCount++;
                        i_xp += enemy.GetXp();
                    }
                    else
                    {
                        //Focus enemy
                        gc.SetFocusedEnemy(enemy);
                    }

                }
            }
        }
        //Draw line to hits
    }

    void CastShadowBolt()
    {
        //Shadow Bolt!
        //Hits one enemy within range
        Vector2 range_botleft = new Vector2(transform.position.x - boltRange, transform.position.y - boltRange);
        Vector2 range_topright = new Vector2(transform.position.x + boltRange, transform.position.y + boltRange);
        Enemy enemy = null;
        int boltDamage = i_damage - i_damage / 10 * 3; ;
        Collider2D[] enemies = Physics2D.OverlapAreaAll(range_botleft, range_topright);
        if (enemies.Length > 0)
        {
            Transform enemyTransform = GetClosestEnemy(enemies);
            if (enemyTransform != null)
            {
                if(enemyTransform.tag == "enemy")
                {
                    enemy = enemyTransform.GetComponent<Enemy>();
                    Vector3[] positions = { Vector3.zero, enemy.transform.position - transform.position };
                    lr.SetPositions(positions);
                    enemy.TakeDamage(boltDamage);
                    if (enemy.IsDead())
                    {
                        //Add killcount and XP
                        i_killCount++;
                        i_xp += enemy.GetXp();
                    }
                    else
                    {
                        //Focus enemy
                        gc.SetFocusedEnemy(enemy);
                    }
                }
            }
        }
    }

    Transform GetClosestEnemy(Collider2D[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider2D potentialTarget in enemies)
        {
            if (potentialTarget.tag != "Player")
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }
        return bestTarget;
    }

}


