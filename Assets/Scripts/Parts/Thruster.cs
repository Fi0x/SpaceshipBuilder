using System;
using UnityEngine;

namespace Parts
{
    public class Thruster : MonoBehaviour
    {
        public const int SpeedIncrease = 50;
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