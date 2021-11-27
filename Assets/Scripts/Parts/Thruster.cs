using System;
using UnityEngine;

namespace Parts
{
    public class Thruster : SpaceshipPart
    {
        public const int SpeedIncrease = Spaceship.MaxSpeed / 4;
        public event EventHandler ThrusterDestroyedEvent;

        private void OnCollisionEnter2D(Collision2D collision)
        {
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