using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject spaceship;
    private Spaceship spaceshipScript;
    private GameObject lowerTile;
    private GameObject upperTile;
    private GameManager gameManager;

    public GameObject Tile1;
    public GameObject Tile2;

    // Start is called before the first frame update
    void Start()
    {
        spaceship = GameObject.Find("Spaceship");
        spaceshipScript = spaceship.GetComponent<Spaceship>();
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.alive)
        {
            if (Tile1.transform.position.y < Tile2.transform.position.y)
            {
                lowerTile = Tile1;
                upperTile = Tile2;
            }
            else
            {
                lowerTile = Tile2;
                upperTile = Tile1;
            }

            lowerTile.transform.position += spaceshipScript.getDirection() * spaceshipScript.getSpeed() * Time.deltaTime;
            Vector3 pos = lowerTile.transform.position;
            float diff = pos.y + 61.46f;
            if (pos.y <= -61.46f)
            {
                GameObject buffer = lowerTile;
                lowerTile = upperTile;
                upperTile = buffer;
            }
            upperTile.transform.position = lowerTile.transform.position + Vector3.up * 81.92f;

            spaceshipScript.HorizontalOffset(pos.x);
        }
    }
}
