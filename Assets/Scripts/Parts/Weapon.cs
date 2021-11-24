using Control;
using UnityEngine;

namespace Parts
{
    public class Weapon : MonoBehaviour
    {
        private GameManager _gameManager;
        public bool Working { get; set; }

        private int _weaponDelay;
    
        private void Start()
        {
            this._gameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
        }

        private void FixedUpdate()
        {
            if(this.gameObject.GetComponent<SpaceshipPart>().drift)
                return;
            if (!this.Working)
                return;
            if (this._weaponDelay > 0)
            {
                this._weaponDelay--;
                return;
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                var projectile = Instantiate(this._gameManager.prefabProjectile, this.transform.position, this.transform.rotation);
                projectile.AddComponent(typeof(Projectile));
                this._weaponDelay = 15;
            }
        }

        private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
        {
            this._gameManager = args.NewInstance;
        }
    }
}