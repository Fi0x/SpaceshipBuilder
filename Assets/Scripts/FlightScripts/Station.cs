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
            
            if(this.transform.position.y < -50)
                Destroy(this.gameObject);
        }

        private void Move()
        {
            this.transform.position += GameManager.Instance.GetBackgroundMovement() / 60;
        }
    }
}