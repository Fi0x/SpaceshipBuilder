using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BuildingScripts
{
    public class MouseEventSender : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var child = this.GetValidChild();
            child.ToList().ForEach(c => c.OnMouseDown());
        }

        private void OnMouseDrag()
        {
            var child = this.GetValidChild();
            child.ToList().ForEach(c => c.OnMouseDrag());
        }

        private void OnMouseUp()
        {
            var child = this.GetValidChild();
            child.ToList().ForEach(c => c.OnMouseUp());
        }

        private IEnumerable<DragAndDrop> GetValidChild()
        {
            var hitTransforms = new List<Transform>();
            Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, 0, 1))
                .ToList()
                .ForEach(hit => hitTransforms.Add(hit.transform));
            
            var childComponents = this.transform.GetComponentsInChildren<DragAndDrop>();
            var validChild = childComponents.ToList().Where(child => hitTransforms.Contains(child.transform));
            return validChild;
        }
    }
}