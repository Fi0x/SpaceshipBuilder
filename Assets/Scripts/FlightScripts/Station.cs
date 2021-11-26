using System;
using Control;
using UnityEngine;

namespace FlightScripts
{
    public class Station : MonoBehaviour
    {
        public void SpawnStation()
        {
            this.transform.position = new Vector3(0, 50, 0);
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.Running)
                return;
            
            this.Move();
        }

        private void Move()
        {
            this.transform.position += GameManager.Instance.GetBackgroundMovement() / 60;
        }
    }
}