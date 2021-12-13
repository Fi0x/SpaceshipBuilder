using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuildingScripts
{
    public class MouseEventSender : MonoBehaviour
    {
        private DragAndDrop _correctPart;
        
        private void OnMouseDown()
        {
            this._correctPart = this.GetRaycastChild();
            this._correctPart.OnMouseDown();
        }

        private void OnMouseDrag()
        {
            if(this._correctPart == null)
                this._correctPart = this.GetRaycastChild();
            
            this._correctPart.OnMouseDrag();
        }

        private void OnMouseUp()
        {
            if(this._correctPart == null)
                this._correctPart = this.GetRaycastChild();
            
            this._correctPart.OnMouseUp();

            this._correctPart = null;
        }

        private DragAndDrop GetRaycastChild()
        {
            var hitTransforms = new List<Transform>();
            Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, 0, 1))
                .ToList()
                .ForEach(hit => hitTransforms.Add(hit.transform));

            var childComponents = this.transform.GetComponentsInChildren<DragAndDrop>();
            var validChild = childComponents.ToList().Where(child => hitTransforms.Contains(child.transform));
            return validChild.First();
        }
    }
}