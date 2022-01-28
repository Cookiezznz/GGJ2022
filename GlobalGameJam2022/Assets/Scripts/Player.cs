using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public gameController gc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(string input)
    {
        switch (input)
        {
            case "up":
                if (transform.position.y < gc.boundY)
                {
                    transform.position += Vector3.up;
                }
                break;
            case "left":
                if (transform.position.x > -gc.boundX)
                {
                    transform.position += Vector3.left;
                }

                break;
            case "down":
                if (transform.position.y > -gc.boundY)
                {
                    transform.position += Vector3.down;
                }
                break;
            case "right":
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
