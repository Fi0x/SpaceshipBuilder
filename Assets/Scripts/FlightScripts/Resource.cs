using Control;
using UnityEngine;

namespace FlightScripts
{
    public class Resource : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (!GameManager.Running)
                return;

            this.transform.position += GameManager.Instance.GetBackgroundMovement() / 60;
            
            if(this.transform.position.y < -30)
                Destroy(this.gameObject);
        }
    }
}