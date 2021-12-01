using Control;
using UnityEngine;

namespace FlightScripts.Enemies
{
    public class EnemyProjectile : MonoBehaviour
    {
        private Vector3 _dir;

        private void Start()
        {
            var angle = (this.transform.rotation.eulerAngles.z-90) * Mathf.Deg2Rad;
            this._dir = new Vector3(-Mathf.Cos(angle), -Mathf.Sin(angle), 0);
            this.transform.position += this._dir.normalized;
            Destroy(this.gameObject, 6f);
            Destroy(this, 6f);
        }

        private void FixedUpdate()
        {
            this.transform.position += this._dir * 30 / 60 + GameManager.Instance.GetBackgroundMovement() / 60;
        }
    }
}