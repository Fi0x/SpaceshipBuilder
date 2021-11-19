using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameManager gameManager;
    private bool working = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameObject.GetComponent<SpaceshipPart>().drift && working) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject projectile = Instantiate(gameManager.PrefabProjectile, this.transform.position, this.transform.rotation);
                projectile.AddComponent(typeof(Projectile));
            }
        }
    }

    public void Disable()
    {
        working = false;
    }
    public void Enable()
    {
        working = true;
    }
}
