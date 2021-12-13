using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuildingScripts
{
    public class MouseEventSender : MonoBehaviour
    {
        [SerializeField] private List<CreatePart> childrenToReceiveEvents;
        
        private void OnMouseDown()
        {
            this.childrenToReceiveEvents.ForEach(c => c.CurrentChild.OnMouseDown());
        }

        private void OnMouseDrag()
        {
            this.childrenToReceiveEvents.ForEach(c => c.CurrentChild.OnMouseDrag());
        }

        private void OnMouseUp()
        {
            this.childrenToReceiveEvents.ForEach(c => c.CurrentChild.OnMouseUp());
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