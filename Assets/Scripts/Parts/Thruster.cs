using System;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Thruster : SpaceshipPart
    {
        [SerializeField] private float speedMultiplier;
        public float SpeedIncrease => Spaceship.MaxSpeed * this.speedMultiplier;
        public event EventHandler ThrusterDestroyedEvent;

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