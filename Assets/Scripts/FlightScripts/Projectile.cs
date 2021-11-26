using Control;
using UnityEngine;

namespace FlightScripts
{
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

        private void FixedUpdate()
        {
            this.transform.position += this._gameManager.GetBackgroundMovement() / 60;
            this.transform.position += this._dir* -1 * (this._gameManager.ShipScript.Speed+100) / 60;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ship":
                    return;
                case "Asteroid":
                    collision.gameObject.GetComponent<AsteroidBehaviour>()?.DestroyByShot();
                    break;
            }
        
            Destroy(this.gameObject);
            Destroy(this);
        }

        private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
        {
            this._gameManager = args.NewInstance;
            this._dir = this._gameManager.ShipScript.GetDirection();
        }
    }
}