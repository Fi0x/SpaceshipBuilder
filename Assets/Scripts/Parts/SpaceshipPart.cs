using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        [HideInInspector] public bool drift;
        private GameManager _gameManager;
        public void Start()
        {
            this._gameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
        }

        public void FixedUpdate()
        {
            if (this.drift)
                this.transform.position += this._gameManager.GetBackgroundMovement() * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ship":
                case "Projectile":
                    return;
            }
            if(!this._gameManager.Alive)
                return;
        
            this.drift = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
        
            this.transform.parent = null;
            var asteroid = collision.gameObject.GetComponent<AsteroidBehaviour>();
            if (asteroid != null)
                asteroid.Init();
        }

        private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
        {
            this._gameManager = args.NewInstance;
        }
    }
}