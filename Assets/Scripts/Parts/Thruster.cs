using System;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Thruster : SpaceshipPart
    {
        public const int SpeedIncrease = Spaceship.MaxSpeed / 4;
        public event EventHandler ThrusterDestroyedEvent;
        
        protected override void Start()
        {
            base.Start();
            this.OriginalInventory = GameObject.Find("ThrusterInventory");
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            switch (collision.gameObject.tag)
            {
                case "Ship":
                case "Projectile":
                    return;
            }
            
            this.ThrusterDestroyedEvent?.Invoke(this, null);
        }
    }
}