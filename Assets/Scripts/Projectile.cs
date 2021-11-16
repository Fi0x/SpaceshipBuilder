using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 dir;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        dir = gameManager.getShipDirection();
     
        Destroy(this.gameObject, 3f);
        Destroy(this, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += gameManager.getBackgroundMovement() * Time.deltaTime;
        this.transform.position += dir* -1 * (gameManager.getShipSpeed()+100) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject incoming = collision.gameObject;
        if (incoming.name != "Spaceship")
        {
            Destroy(incoming);
            Destroy(this.gameObject);
            Destroy(this);

        }
    }
}
