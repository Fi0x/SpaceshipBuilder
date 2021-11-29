using Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ProjectilePrefab;
    public int Speed = 5;
    public int NoticingRange = 50;
    public int turnSpeed = 100;
    public float ReloadTime = 1.0f;


    private GameManager gameManager;
    private Vector3 distanceToPlayer;
    private float targetRotation;
    private float currentRotation;
    private bool weaponReady = true;
    private float searchangle;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        searchangle = Random.Range(110, 150);
        currentRotation = 180;
        if(gameManager.ShipScript.GetHorizontalOffset() < 0)
        {
        targetRotation = searchangle;
        }
        else
        {
            targetRotation = -searchangle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = this.transform.position;
        distanceToPlayer.y += 10;
        distanceToPlayer *= -1;

        Vector3 move = Vector3.zero;
        Debug.Log(targetRotation);

        setTargetRotation();
        turnToTarget();

        if (distanceToPlayer.magnitude < NoticingRange && weaponReady)
        {
            shoot();
        }

            float rotationInRad = (currentRotation - 90) * Mathf.Deg2Rad;
        move = new Vector3(-Mathf.Cos(rotationInRad), -Mathf.Sin(rotationInRad), 0);
        move.Normalize();

        this.transform.rotation = Quaternion.AngleAxis(currentRotation, Vector3.forward);
        
        this.transform.position += gameManager.GetBackgroundMovement() * Time.deltaTime;
        this.transform.position += move * Speed * Time.deltaTime;
       }

    public void Init()
    {
        Vector3 newPos = new Vector3(Random.Range(-120, 120), 120, 0);
        this.transform.position = newPos;
        this.distanceToPlayer = new Vector2(newPos.x, newPos.y + 10);
    }

    private void setTargetRotation()
    {
        float angle;
        if (distanceToPlayer.magnitude < NoticingRange)
        {
            // Noticed behaviour
            if (distanceToPlayer.x < 0)
            {
                angle = Mathf.Acos(distanceToPlayer.normalized.x);
                if (distanceToPlayer.y < 0)
                {
                    angle = Mathf.PI * 2 - angle;
                }
            }
        else
        {
            angle = Mathf.Asin(distanceToPlayer.normalized.y);
        }
        targetRotation = (angle * Mathf.Rad2Deg - 90);   
        }
        else
        {
            //Unnoticed behaviour
            float offset = gameManager.ShipScript.GetHorizontalOffset();
            offset += transform.position.x;
            if(offset < -80)
            {
                targetRotation = -searchangle;
            }
            else
            {
                if(offset > 80)
                {
                targetRotation = searchangle;
                }
            }            
        }
    }

    private void turnToTarget()
    {
        float diff = targetRotation - currentRotation;
        if(diff > 180)
        {
            diff = 360 - diff;
        }
        if(diff < -180)
        {
            diff = -360 - diff;
        }
        float maxTurn = turnSpeed * Time.deltaTime;
        if(Mathf.Abs(diff) <= maxTurn)
        {
            currentRotation = targetRotation;
            return;
        }
        if(diff < 0 && diff > -90)
        {
            currentRotation -= maxTurn;
        }
        else
        {
            currentRotation += maxTurn;
        }
        
    }

    private void shoot()
    {
        //Shoot
        var projectile = Instantiate(this.ProjectilePrefab, this.transform.position, this.transform.rotation);
        projectile.AddComponent(typeof(EnemyProjectile));
        weaponReady = false;
        Invoke("resetWeaponFlag", ReloadTime);
    }

    private void resetWeaponFlag()
    {
        weaponReady = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Destroy(this.gameObject);
        }
    }
}
