using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject _spaceship;
    private Spaceship _spaceshipScript;
    private GameObject _lowerTile;
    private GameObject _upperTile;
    private GameManager _gameManager;

    public GameObject tile1;
    public GameObject tile2;

    private void Start()
    {
        this._spaceship = GameObject.Find("Spaceship(Clone)");
        this._spaceshipScript = this._spaceship.GetComponent<Spaceship>();
        this._gameManager = GameManager.Instance;
        GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
    }

    private void FixedUpdate()
    {
        if (!this._gameManager.Alive)
            return;
        
        if (this.tile1.transform.position.y < this.tile2.transform.position.y)
        {
            this._lowerTile = this.tile1;
            this._upperTile = this.tile2;
        }
        else
        {
            this._lowerTile = this.tile2;
            this._upperTile = this.tile1;
        }

        this._lowerTile.transform.position += this._spaceshipScript.GetDirection() * this._spaceshipScript.Speed * Time.deltaTime;
        var pos = this._lowerTile.transform.position;
        if (pos.y <= -61.46f)
        {
            var buffer = this._lowerTile;
            this._lowerTile = this._upperTile;
            this._upperTile = buffer;
        }
        this._upperTile.transform.position = this._lowerTile.transform.position + Vector3.up * 81.92f;

        this._spaceshipScript.HorizontalOffset(pos.x);
    }

    private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
    {
        this._gameManager = args.NewInstance;
    }
}
