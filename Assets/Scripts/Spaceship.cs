using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour
{
    private int zAngle = 0;
    public int AccelerationPerSecond = 100;
    public int MaxSpeed = 100;
    public int MaxAngle = 45;
    public int TurnSpeed = 100;

    private float speed;
    private float horizontalOffset;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.initShip();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.alive)
        {
            //Rotate

            if (zAngle == 360 && zAngle == -360)
            {
                zAngle = 0;
            }
            if (Input.GetKey(KeyCode.A))
            {
                zAngle+= TurnSpeed/100;
            }
            if (Input.GetKey(KeyCode.D))
            {
                zAngle-= TurnSpeed/100;
            }
            Vector2Int threshold = calcAngleThreshold();
            zAngle = Mathf.Clamp(zAngle, threshold.x, threshold.y);
            this.transform.rotation = Quaternion.AngleAxis(zAngle, Vector3.forward);
          
            
            //Debug.Log("Rotation" + zAngle);
            
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.alive)
        {
            //Accelerate & Decelerate

            if (Input.GetKey(KeyCode.W))
            {
                speed += AccelerationPerSecond / 60;
                if (speed > MaxSpeed)
                {
                    speed = MaxSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                speed -= AccelerationPerSecond / 60;
                if (speed < 10)
                {
                    speed = 10;
                }
            }

            //Debug.Log("Speed:" + speed);
            //Debug.Log("Horizontal: " + horizontalOffset);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        if (!collision.gameObject.name.Contains("Projectile") && collision.gameObject.tag != "Ship")
        {
            gameManager.GameOver();
        }
    }

    private Vector2Int calcAngleThreshold()
    {
        float offset = Mathf.Abs(horizontalOffset);
        if(offset > 120 - MaxAngle/2)
        {
            Debug.Log("DangerZone!" + horizontalOffset);
            if (horizontalOffset < 0) {
                return new Vector2Int( -(int)(120 - offset)*2 , MaxAngle );
            }
            return new Vector2Int( -MaxAngle , (int)(120 - offset)*2 );
        }
        return new Vector2Int(-MaxAngle,MaxAngle);
    }
    public float getSpeed()
    {
        return speed;
    }
    public Vector3 getDirection()
    {
        float angle = (zAngle-90) * Mathf.Deg2Rad;
        Vector3 toReturn = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        return toReturn;
        
    }

    public void HorizontalOffset(float offset)
    {
        horizontalOffset = offset;
    }
}
