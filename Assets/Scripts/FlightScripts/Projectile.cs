using Control;
using UnityEngine;

namespace FlightScripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        public Vector3 dir;
    
        private void Start()
        {
            this.transform.position -= this.dir * 1.5f;
            Destroy(this.gameObject, 3f);
            Destroy(this, 3f);
        }

        private void FixedUpdate()
        {
            var vector = GameManager.Instance.GetBackgroundMovement() / 60;
            vector += -1 * (GameManager.Instance.ShipScript.Speed + this.speed) / 60 * this.dir;
            this.transform.position += vector;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ship":
                    break;
                case "Projectile":
                    break;
                case "Asteroid":
                    collision.gameObject.GetComponent<AsteroidBehaviour>()?.DestroyByShot();
                    Destroy(this.gameObject);
                    break;
                case "default":
                    Destroy(this.gameObject);
                    break;

            }
        }
    }
}