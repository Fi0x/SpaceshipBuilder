using Control;
using UnityEngine;

namespace FlightScripts
{
    public class Station : MonoBehaviour
    {
        public void SpawnStation()
        {
            this.transform.position = new Vector3(0, 40, 0);
        }

        private void FixedUpdate()
        {
            if (!GameManager.Running)
                return;
            
            this.Move();
        }

        private void Move()
        {
            this.transform.position += GameManager.Instance.GetBackgroundMovement() / 60;
        }
    }
}