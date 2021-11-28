using UnityEngine;

namespace Parts
{
    public class Body : SpaceshipPart
    {
        private void Start()
        {
            this.OriginalInventory = GameObject.Find("BuildingInventory");
        }
    }
}