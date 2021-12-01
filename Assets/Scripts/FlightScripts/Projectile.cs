using Control;
using UnityEngine;

namespace FlightScripts
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 _dir;
    
        private void Start()
        {
            this._dir = GameManager.Instance.ShipScript.GetDirection();
     
            Destroy(this.gameObject, 3f);
            Destroy(this, 3f);
        }

        private void FixedUpdate()
        {
            var vector = GameManager.Instance.GetBackgroundMovement() / 60;
            vector += -1 * (GameManager.Instance.ShipScript.Speed + 100) / 60 * this._dir;
            this.transform.position += vector;
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
    }
}