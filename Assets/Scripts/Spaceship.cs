using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public int accelerationPerSecond = 100;
    public int maxSpeed = 100;
    public int maxAngle = 45;
    public int turnSpeed = 100;

    private int _zAngle;
    private float _horizontalOffset;

    private GameManager _gameManager;
    private GameManager GameManagerInstance
    {
        get => this._gameManager;
        set
        {
            this._gameManager = value;
            
            if(this._gameManager != null)
                this._gameManager.InitShip(this.gameObject);
        }
    }

    public float Speed { get; private set; }

    private void Start()
    {
        this.GameManagerInstance = GameManager.Instance;
        GameManager.GameManagerInstantiatedEvent += this.GameManagerInstantiatedEventHandler;
    }

    private void FixedUpdate()
    {
        if (!this.GameManagerInstance.Alive)
            return;
        
        //Rotate
        if (this._zAngle == 360 && this._zAngle == -360)
            this._zAngle = 0;

        if (Input.GetKey(KeyCode.A))
            this._zAngle += this.turnSpeed / 60;
        
        if (Input.GetKey(KeyCode.D))
            this._zAngle -= this.turnSpeed / 60;

        var threshold = this.CalcAngleThreshold();
        this._zAngle = Mathf.Clamp(this._zAngle, threshold.x, threshold.y);
        this.transform.rotation = Quaternion.AngleAxis(this._zAngle, Vector3.forward);
        
        //Adjust speed
        if (Input.GetKey(KeyCode.W))
        {
            this.Speed += this.accelerationPerSecond / 60;
            if (this.Speed > this.maxSpeed)
                this.Speed = this.maxSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.Speed -= this.accelerationPerSecond / 60;
            if (this.Speed < 10)
                this.Speed = 10;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Projectile") && !collision.gameObject.tag.Equals("Ship"))
            this.GameManagerInstance.GameOver();
    }

    private Vector2Int CalcAngleThreshold()
    {
        var offset = Mathf.Abs(this._horizontalOffset);
        if (!(offset > 120 - this.maxAngle / 2)) 
            return new Vector2Int(-this.maxAngle, this.maxAngle);
        
        Debug.Log("DangerZone!" + this._horizontalOffset);
        return this._horizontalOffset < 0
            ? new Vector2Int(-(int) (120 - offset) * 2, this.maxAngle)
            : new Vector2Int(-this.maxAngle, (int) (120 - offset) * 2);
    }

    public Vector3 GetDirection()
    {
        var angle = (this._zAngle - 90) * Mathf.Deg2Rad;
        var toReturn = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        return toReturn;
    }

    public void HorizontalOffset(float offset)
    {
        this._horizontalOffset = offset;
    }

    private void GameManagerInstantiatedEventHandler(object sender, GameManager.NewGameManagerEventArgs args)
    {
        this.GameManagerInstance = args.NewInstance;
    }
}