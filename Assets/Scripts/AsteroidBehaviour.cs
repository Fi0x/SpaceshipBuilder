using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10;

    private GameManager _gameManager;
    private Vector3 _vel;

    private void Start()
    {
        this._gameManager = GameManager.Instance;
        GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
    }

    public void Init()
    {
        /* Position
         * Velocity
         * Rotation
         */
        var newPos = new Vector3(Random.Range(-120, 120), 50, 0);
        this.transform.position = newPos;
        this._vel = new Vector3(Random.Range(-this.maxSpeed, this.maxSpeed), Random.Range(-this.maxSpeed, 0), 0);
    }

    public bool Move(float time)
    {
        /* Position
         * Velocity
         * Rotation
         */
        this.transform.position += this._vel * time;
        if (!this._gameManager.Alive)
            return false;

        this.transform.position += this._gameManager.GetBackgroundMovement() * time;

        return this.transform.position.y < -30;
    }

    private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
    {
        this._gameManager = args.NewInstance;
    }
}