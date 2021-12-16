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
            this.transform.position += this._dir * 20 / 60 + GameManager.Instance.GetBackgroundMovement() / 60;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Enemy":
                    break;

                default:
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}