using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public gameController gc;

    void Start()
    {
        int randX = Random.Range(-gc.boundX, gc.boundX);
        int randY = Random.Range(-gc.boundY, gc.boundY);
        transform.position = new Vector3(randX, randY, 0);
    }

    public void Move()
    {
        int direction = Random.Range(0, 4);
        switch(direction)
        {
            case 0:
                if (transform.position.y < gc.boundY)
                {
                    transform.position += Vector3.up;
                }
                break;
            case 1:
                if (transform.position.x > -gc.boundX)
                {
                    transform.position += Vector3.left;
                }
                break;
            case 2:
                if (transform.position.y > -gc.boundY)
                {
                    transform.position += Vector3.down;
                }
                break;
            case 3:
                if (transform.position.x < gc.boundX)
                {
                    transform.position += Vector3.right;
                }
                break;
            default:
                break;
        }
    }
}
