using UnityEngine;

namespace Parts
{
    public class Body : SpaceshipPart
    {
        protected override void Start()
        {
            base.Start();
            this.OriginalInventory = GameObject.Find("BuildingInventory");
        }
    }
}