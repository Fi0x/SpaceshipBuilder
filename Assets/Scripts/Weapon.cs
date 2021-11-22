using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameManager _gameManager;
    public bool Working { get; set; }
    
    private void Start()
    {
        this._gameManager = GameManager.Instance;
        GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
    }

    private void Update()
    {
        if(this.gameObject.GetComponent<SpaceshipPart>().drift)
            return;
        if (!this.Working)
            return;
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject projectile = Instantiate(this._gameManager.prefabProjectile, this.transform.position, this.transform.rotation);
                projectile.AddComponent(typeof(Projectile));
            }
        }
    }

    private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
    {
        this._gameManager = args.NewInstance;
    }
}
