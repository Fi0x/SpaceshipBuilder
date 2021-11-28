
using System.Linq;
using Parts;
using UnityEngine;

namespace BuildingScripts
{
    public class SnapHelper
    {
        public static bool Snap(Transform originalTransform, GameObject spaceship, SpaceshipPart partType, out GameObject[] parts)
        {
            (var dockingObject, var dockingTransform) = GetClosestDockingPoint(originalTransform, out parts);
            if (dockingObject == null || dockingTransform == null)
                return false;
            
            var transform1 = originalTransform;
            var position = dockingObject.transform.position;
            var localPosition = dockingTransform.localPosition;
            var localPosRot= transform1.rotation * localPosition;
            transform1.position = position-localPosRot;
            originalTransform.SetParent(spaceship.transform);

            partType.SpawnInInventory();
            return true;
        }

        public static (GameObject, Transform) GetClosestDockingPoint(Transform originalTransform, out GameObject[] parts)
        {
            Transform dockingPoint = null;
            var children = new GameObject[originalTransform.childCount];
            for (var i = 0; i < originalTransform.childCount; i++)
                children[i] = originalTransform.GetChild(i).gameObject;
            
            parts = GameObject.FindGameObjectsWithTag("DockEmpty");
            var possibleDocks = parts.Except(children);
            var closestDistanceSqr = Mathf.Infinity;
            GameObject closestPart = null;
            foreach (Transform child in originalTransform)
            {
                foreach (var temp in possibleDocks)
                {
                    var localRotVec = - (originalTransform.rotation * child.localPosition);
                    if((temp.transform.parent.rotation*temp.transform.localPosition)!=localRotVec)
                        continue;
                    if(!temp.transform.parent.CompareTag("Ship"))
                        continue;
                    
                    var directionToTarget = temp.transform.position - child.position;
                    var dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget >= closestDistanceSqr)
                        continue;
                    
                    closestDistanceSqr = dSqrToTarget;
                    closestPart = temp;
                    dockingPoint = child;
                }
            }
            return (closestPart, dockingPoint);
        }
    }
}