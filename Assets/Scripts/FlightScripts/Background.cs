using Control;
using UnityEngine;

namespace FlightScripts
{
    public class Background : MonoBehaviour
    {
        private GameObject _spaceship;
        private Spaceship _spaceshipScript;
        private GameObject _lowerTile;
        private GameObject _upperTile;

        public GameObject tile1;
        public GameObject tile2;

        private void Start()
        {
            this._spaceship = GameObject.Find("Spaceship(Clone)");
            this._spaceshipScript = this._spaceship.GetComponent<Spaceship>();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Running)
                return;
        
            if (this.tile1.transform.position.y < this.tile2.transform.position.y)
            {
                this._lowerTile = this.tile1;
                this._upperTile = this.tile2;
            }
            else
            {
                this._lowerTile = this.tile2;
                this._upperTile = this.tile1;
            }

            var movement = this._spaceshipScript.GetDirection() * this._spaceshipScript.Speed / 60;
            GameManager.Instance.DistanceToNextStation += movement.y;
            
            this._lowerTile.transform.position += movement;
            var pos = this._lowerTile.transform.position;
            if (pos.y <= -61.46f)
                (this._lowerTile, this._upperTile) = (this._upperTile, this._lowerTile);
        
            this._upperTile.transform.position = this._lowerTile.transform.position + Vector3.up * 81.92f;

            this._spaceshipScript.HorizontalOffset = pos.x;
        }
    }
}