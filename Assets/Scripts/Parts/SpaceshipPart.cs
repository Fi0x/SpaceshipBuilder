using Control;
using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        [HideInInspector] public bool drift;
        protected GameManager GameManager;
        public void Start()
        {
            this.GameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += (sender, args) => { this.GameManager = args.NewInstance; };
        }

        private void FixedUpdate()
        {
            if (this.drift)
                this.transform.position += this.GameManager.GetBackgroundMovement() * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ship":
                case "Projectile":
                    return;
            }
            if(!this.GameManager.Running)
                return;
        
            this.drift = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
        
            this.transform.parent = null;
            var asteroid = collision.gameObject.GetComponent<AsteroidBehaviour>();
            if (asteroid != null)
                asteroid.Init();
        }
    }
}