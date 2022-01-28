using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnController : MonoBehaviour
{

    public gameController gc;
    public GameObject p_enemy;
    private int spawnTimer;
    private int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<gameController>();

    }

    // Update is called once per frame
    public void Tick()
    {
        if(gc.GetTurns() > spawnTimer)
        {
            spawnTimer += 5;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Enemy enemy = Instantiate(p_enemy, transform).GetComponent<Enemy>();
        enemy.enemyNum = enemyCount++;
    }
}
