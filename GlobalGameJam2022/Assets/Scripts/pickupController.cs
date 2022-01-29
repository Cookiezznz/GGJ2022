using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    public gameController gc;
    private int value;
    private int healVal = 10;
    private int damageVal = 2;
    private int xpVal = 15;


    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<Player>().gameObject.GetComponent<gameController>();
        int spawnX = Random.Range(0, gc.GetBounds().x);
        int spawnY = Random.Range(0, gc.GetBounds().y);
        transform.position = new Vector3Int(spawnX, spawnY, 0);
        if(gameObject.tag == "healthpickup")
        {
            value = healVal;
        }
        if (gameObject.tag == "damagepickup")
        {
            value = damageVal;

        }
        if (gameObject.tag == "xppickup")
        {
            value = xpVal;

        }
    }

    public int GetValue()
    {
        return value;
    }
}
