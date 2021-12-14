using Control;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlightScripts
{
    public class Resource : MonoBehaviour
    {
        [SerializeReference, Range(0.5f, 1f)] private float SuperiorPartProbability = 0.6f;
        [SerializeReference, Range(0.75f, 1f)] private float MaxedPartProbability = 0.9f;
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
            var inventory = GameManager.Instance.GetComponentInChildren<InventoryTracker>();
            if (Valuable)
            {
                if(Random.value > 0.5f)
                {
                    int gun;
                    float rnd = Random.value;
                    if(rnd > SuperiorPartProbability)
                    {
                        if (rnd > MaxedPartProbability)
                            gun = 2;
                        else
                            gun = 1;
                    }
                    else
                    {
                        gun = 1;
                    }
                    inventory.AddGun(gun);
                }
                else
                {
                    int thruster;
                    float rnd = Random.value;
                    if (rnd > SuperiorPartProbability)
                    {
                        if (rnd > MaxedPartProbability)
                            thruster = 2;
                        else
                            thruster = 1;
                    }
                    else
                    {
                        thruster = 0;
                    }
                    inventory.AddThruster(thruster);
                }
            }
            else
            {
                inventory.AddBodyPart(Random.Range(0, 6));
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Ship":
                    ResourceCollectedEvent?.Invoke(null, null);
                    collected();
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}