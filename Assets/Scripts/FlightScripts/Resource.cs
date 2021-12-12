using Control;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlightScripts
{
    public class Resource : MonoBehaviour
    {
        [SerializeReference, Range(0.5f, 1f)] private float SuperiorPartProbability;
        public bool Valuable { private get; set; }
        public static event EventHandler ResourceCollectedEvent;
        public void FixedUpdate()
        {
            if (!GameManager.Running)
                return;

            this.transform.position += GameManager.Instance.GetBackgroundMovement() / 60;
            
            if(this.transform.position.y < -30)
                Destroy(this.gameObject);
        }
        private void collected()
        {
            if (Valuable)
            {
                if(Random.value > 0.5f)
                {
                    Vector2Int gun;
                    if(Random.value > SuperiorPartProbability)
                    {
                        gun = Vector2Int.right;
                    }
                    else
                    {
                        gun = Vector2Int.up;
                    }
                    PartInventory.Instance.AddGun(gun);
                }
                else
                {
                    Vector2Int thruster;
                    if (Random.value > SuperiorPartProbability)
                    {
                        thruster = Vector2Int.right;
                    }
                    else
                    {
                        thruster = Vector2Int.up;
                    }
                    PartInventory.Instance.AddThruster(thruster);
                }
            }
            else
            {
                //TODO: Body Part Selection
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ship":
                    ResourceCollectedEvent?.Invoke(null, null);
                    collected();
                    break;
            }
        }
    }
}