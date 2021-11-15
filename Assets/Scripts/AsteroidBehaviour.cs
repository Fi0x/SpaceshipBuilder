using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    private Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        /* Position
         * Velocity
         * Rotation
         */
        Vector3 newPos = new Vector3(Random.Range(-120,120), 50, 0);
        this.transform.position = newPos;
        vel = new Vector3(Random.Range(-20,20), Random.Range(-20,0), 0);
    }

    public bool Move(float time)
    {
        /* Position
         * Velocity
         * Rotation
         */
        if (gameManager.alive)
        {
            this.transform.position += gameManager.getBackgroundMovement() * time;
            this.transform.position += vel * time;
            if (this.transform.position.y < -30)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
