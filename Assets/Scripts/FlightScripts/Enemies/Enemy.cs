using Control;
using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;

namespace FlightScripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject resourcePrefab;
        [SerializeField] private int speed = 5;
        [SerializeField] private int noticingRange = 50;
        [SerializeField] private int turnSpeed = 100;
        [SerializeField] private float reloadTime = 1.0f;

        public static event EventHandler EnemyDestroyedEvent;

        private Vector3 _distanceToPlayer;
        private float _targetRotation;
        private float _currentRotation;
        private bool _weaponReady = true;
        private float _searchAngle;
        private bool _lostTrack = false;
        
        private void Start()
        {
            Init();
        }

        private void FixedUpdate()
        {
            this._distanceToPlayer = this.transform.position;
            this._distanceToPlayer.y += 10;
            this._distanceToPlayer *= -1;

            this.SetTargetRotation();
            this.TurnToTarget();

            if (this._distanceToPlayer.magnitude < this.noticingRange && this._weaponReady)
                this.Shoot();

            var rotationInRad = (this._currentRotation - 90) * Mathf.Deg2Rad;
            var move = new Vector3(-Mathf.Cos(rotationInRad), -Mathf.Sin(rotationInRad), 0);
            move.Normalize();

            this.transform.rotation = Quaternion.AngleAxis(this._currentRotation, Vector3.forward);
        
            var velocity = GameManager.Instance.GetBackgroundMovement() / 60;
            velocity += move * this.speed / 60;
            this.transform.position += velocity;
        }

        public void Init()
        {
            this._searchAngle = Random.Range(110, 150);
            this._currentRotation = 180;

            if (GameManager.Instance.ShipScript.HorizontalOffset < 0)
                this._targetRotation = this._searchAngle;
            else
                this._targetRotation = -this._searchAngle;

            var newPos = new Vector3(Random.Range(-10, 10), 120, 0);
            this.transform.position = newPos;
            this._distanceToPlayer = new Vector2(newPos.x, newPos.y + 10);
            this._lostTrack = false;

        }

        private void SetTargetRotation()
        {
            if (this._distanceToPlayer.magnitude < this.noticingRange)
            {
                // Noticed behaviour
                float angle;
                if (this._distanceToPlayer.x < 0)
                {
                    angle = Mathf.Acos(this._distanceToPlayer.normalized.x);
                    if (this._distanceToPlayer.y < 0)
                        angle = Mathf.PI * 2 - angle;
                }
                else
                    angle = Mathf.Asin(this._distanceToPlayer.normalized.y);
                
                this._targetRotation = (angle * Mathf.Rad2Deg - 90);   
            }
            else
            {
                //Unnoticed behaviour
                var offset = GameManager.Instance.ShipScript.HorizontalOffset;
                offset += this.transform.position.x;
                if(offset < -80)
                    this._targetRotation = -this._searchAngle;
                else
                {
                    if(offset > 80)
                        this._targetRotation = this._searchAngle;
                }            
            }
            if(_distanceToPlayer.y > 20 && !_lostTrack)
            {
                Invoke(nameof(Init), 2.0f);
                _targetRotation = 180;
                this._lostTrack = true;
                Debug.Log("Enemy Respawn");
            }
        }

        private void TurnToTarget()
        {
            var diff = this._targetRotation - this._currentRotation;
            if(diff > 180)
                diff = 360 - diff;
            if(diff < -180)
                diff = -360 - diff;
            
            var maxTurn = this.turnSpeed * Time.deltaTime;
            if(Mathf.Abs(diff) <= maxTurn)
            {
                this._currentRotation = this._targetRotation;
                return;
            }
            
            if(diff < 0 && diff > -90)
                this._currentRotation -= maxTurn;
            else
                this._currentRotation += maxTurn;
        }

        private void Shoot()
        {
            var ownTransform = this.transform;
            var projectile = Instantiate(this.projectilePrefab, ownTransform.position, ownTransform.rotation);
            projectile.AddComponent(typeof(EnemyProjectile));
            this._weaponReady = false;
            this.Invoke(nameof(this.ResetWeaponFlag), this.reloadTime);
        }

        private void ResetWeaponFlag()
        {
            this._weaponReady = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Asteroid":
                    break;

                case "EnemyProjectile":
                    break;

                default:
                    GameObject resource = Instantiate(resourcePrefab, this.transform.position, new Quaternion());
                    EnemyDestroyedEvent?.Invoke(null, null);
                    Destroy(this.gameObject);
                    break;

            }
        }
    }
}