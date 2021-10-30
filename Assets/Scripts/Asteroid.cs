using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private GameObject spaceship;
    private Spaceship spaceshipScript;

    public Vector3 Direction;
    public int Speed;
    // Start is called before the first frame update
    void Start()
    {
        spaceship = GameObject.Find("Spaceship");
        spaceshipScript = spaceship.GetComponent<Spaceship>();

        init();
}

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.alive)
        {
            Vector3 pos = this.transform.position;
            if (pos.x > 40 || pos.x < -40 || pos.y > 20 || pos.y < -20)
            {
                init();
            }



            this.transform.position += spaceshipScript.getDirection() * spaceshipScript.getSpeed();
            this.transform.position += Direction * Speed / 100;
        }
    }
    private void init()
    {
        float decider = Random.Range(0, 4);
        float rnd = Random.Range(-40, 40);
        float angle = 0;
        
            if (decider >= 0 && decider <1)
            {
                this.transform.position = new Vector3(rnd, 20, 0);
                angle = Random.Range(91, 270);
            }
            else if( decider >=1 && decider <2)
            {
                this.transform.position = new Vector3(rnd, -20, 0);
                angle = Random.Range(-89, 89);
            }
            else if (decider >= 2 && decider <3)
            {
                this.transform.position = new Vector3(40, rnd / 2, 0);
                angle = Random.Range(-1, -179);
            }
        else if (decider >= 3 && decider <= 4)
        {
                this.transform.position = new Vector3(-40, rnd / 2, 0);
                angle = Random.Range(1, 179);
            }
        
        angle = (angle - 90) * Mathf.Deg2Rad;
        Direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        float scale = Random.Range(0.8f, 4);
        this.transform.localScale = new Vector3(scale, scale, 0);
}
}
