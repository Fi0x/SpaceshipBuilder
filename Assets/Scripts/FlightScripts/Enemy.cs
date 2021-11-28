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
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        currentRotation = 180;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = this.transform.position;
        distanceToPlayer.y += 10;
        distanceToPlayer *= -1;

        Vector3 move = Vector3.zero;
        setTargetRotation();
        Debug.Log(targetRotation);

        if(distanceToPlayer.magnitude < NoticingRange)
        {
            // Noticed behaviour
            //Wrong Behaviour, but works as intended
            turnToTarget();
            float rotationInRad = (currentRotation-90) * Mathf.Deg2Rad;
            move = new Vector3(-Mathf.Cos(rotationInRad), -Mathf.Sin(rotationInRad),0);
            if (weaponReady)
            {
                shoot();
            }
        }
        else
        {
            //Unnoticed behaviour
        }
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
        if(diff < 0)
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
}
