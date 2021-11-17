using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPart : MonoBehaviour
{
    [HideInInspector]
    public bool drift;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (drift)
        {
            this.transform.position += gameManager.getBackgroundMovement() * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ship" && collision.gameObject.tag != "Projectile")
        {
            this.drift = true;
            foreach(Weapon script in this.gameObject.GetComponentsInChildren<Weapon>())
            {
                script.Disable();
            }
            this.transform.parent = null;
            AsteroidBehaviour asteroid = collision.gameObject.GetComponent<AsteroidBehaviour>() as AsteroidBehaviour;
            if (asteroid != null)
            {
                asteroid.Init();
            }
        }
    }

}
