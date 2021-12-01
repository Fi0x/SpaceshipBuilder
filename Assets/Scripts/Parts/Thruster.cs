using System;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Thruster : MonoBehaviour
    {
        public static readonly int SpeedIncrease = Spaceship.MaxSpeed / 4;
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