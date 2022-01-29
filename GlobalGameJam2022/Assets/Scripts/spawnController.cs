using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnController : MonoBehaviour
{

    public gameController gc;

    //Pickups
    public GameObject p_PickupDamage;
    public GameObject p_PickupHealth;
    public GameObject p_PickupXp;


    //Enemies
    public GameObject p_enemy;
    [SerializeField] private int i_spawnOnTurn; //Initial spawn turn
    [SerializeField] private int enemyCount;
    [SerializeField] private int spawnCooldown; //Initial delay for spawn speeds
    [SerializeField] private int spawnSpeed; //Every spawnSpeed turns, reduce spawnCooldown by 1. (ramping difficulty)

    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<gameController>();
    }

    // Update is called once per frame
    public void Tick()
    {

        //Spawn Enemies
        if (gc.GetTurns() >= i_spawnOnTurn) //If spawn turn reached then spawn
        {
            SpawnEnemy();
            i_spawnOnTurn += spawnCooldown;
        }
        if (gc.GetTurns() % spawnSpeed == 0 && spawnCooldown > 1) //Every spawnSpeed turns, reduce spawn cooldown
        {
            spawnCooldown -= 1;
        }
        if (spawnCooldown == 1)
        {
            spawnCooldown = 10;
        }

        //Spawn Pickup
        int i = Random.Range(0, 10);
        if (i == 0) //10% Chance per turn
        {
            SpawnPickup();
        }
    }

    void SpawnEnemy()
    {
        Enemy enemy = Instantiate(p_enemy, transform).GetComponent<Enemy>();
        enemy.enemyNum = enemyCount++;
    }

    void SpawnPickup() //Spawns a random pickup
    {
        int pickup = Random.Range(0, 3); //Health, DamageUp, Xp
        switch(pickup)
        {
            case 0:
                Instantiate(p_PickupHealth, transform);
                break;
            case 1:
                Instantiate(p_PickupDamage, transform);
                break;
            case 2:
                Instantiate(p_PickupXp, transform);
                break;
            default:
                break;
        }    
    }
}
