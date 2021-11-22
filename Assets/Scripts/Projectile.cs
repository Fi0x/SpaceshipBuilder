using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _dir;
    private GameManager _gameManager;
    private void Start()
    {
        this._gameManager = GameManager.Instance;
        GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
        if(this._gameManager != null)
            this._dir = this._gameManager.ShipScript.GetDirection();
     
        Destroy(this.gameObject, 3f);
        Destroy(this, 3f);
    }

    private void Update()
    {
        this.transform.position += this._gameManager.GetBackgroundMovement() * Time.deltaTime;
        this.transform.position += this._dir* -1 * (this._gameManager.ShipScript.Speed+100) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var incoming = collision.gameObject;
        if(incoming.tag.Equals("Ship"))
            return;
        
        var asteroid = collision.gameObject.GetComponent<AsteroidBehaviour>();
        asteroid.Init();
        Destroy(this.gameObject);
        Destroy(this);
    }

    private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
    {
        this._gameManager = args.NewInstance;
        this._dir = this._gameManager.ShipScript.GetDirection();
    }
}
