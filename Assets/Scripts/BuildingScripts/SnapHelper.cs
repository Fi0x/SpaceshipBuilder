
using System.Collections.Generic;
using System.Linq;
using Parts;
using UnityEngine;

namespace BuildingScripts
{
    public static class SnapHelper
    {
        public static bool Snap(Transform originalTransform, GameObject spaceship, SpaceshipPart partType, List<GameObject> possibleDocks)
        {
            (var dockingObject, var dockingTransform) = GetClosestDockingPoint(originalTransform, possibleDocks);
            if (dockingObject == null || dockingTransform == null)
                return false;
            var position = dockingObject.transform.position;
            var localPosition = dockingTransform.localPosition;
            var localPosRot= originalTransform.rotation * localPosition;
            originalTransform.position = position - localPosRot;
            originalTransform.SetParent(spaceship.transform);
            originalTransform.tag = "Ship";
            return true;
        }

        public static (GameObject, Transform) GetClosestDockingPoint(Transform originalTransform, List<GameObject> possibleDocks)
        {
            Transform dockingPoint = null;
            
            var closestDistanceSqr = Mathf.Infinity;
            GameObject closestPart = null;
            foreach (Transform child in originalTransform)
            {
                foreach (var possibleDock in possibleDocks)
                {
                    var locChildPoss =child.localPosition.normalized;
                    if (child.name.Contains("."))
                    {
                        locChildPoss.x = 0;
                        locChildPoss = locChildPoss.normalized;
                    }
                    var localTransPoss = possibleDock.transform.localPosition.normalized;

                    if (possibleDock.name.Contains("."))
                    {
                        localTransPoss.x = 0;
                        localTransPoss = localTransPoss.normalized;
                    }
                    
                    var localRotVec = - (originalTransform.rotation * locChildPoss ).normalized;
                    var otherpartVec = (possibleDock.transform.parent.rotation *localTransPoss).normalized;
                    if  (otherpartVec!= localRotVec)
                        continue;
                    
                    possibleDock.GetComponent<SpriteRenderer>().enabled = true;

                    var directionToTarget = possibleDock.transform.position - child.position;
                    var dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget >= closestDistanceSqr)
                        continue;
                    
                    closestDistanceSqr = dSqrToTarget;
                    closestPart = possibleDock;
                    dockingPoint = child;
                }
            }
            return (closestPart, dockingPoint);
        }

        public static List<GameObject> GetPossibleDockingPoints()
        {
            var shipDocks = new List<GameObject>();
            
            GameObject.Find("Spaceship(Clone)").GetComponentsInChildren<CircleCollider2D>()
                .ToList()
                .ForEach(c => shipDocks.Add(c.gameObject));

            var possibleDocks = shipDocks.Where(dock => dock.tag.Equals("DockEmpty")).ToList();
            possibleDocks = possibleDocks.Where(dock => dock.transform.parent.CompareTag("Ship"))
                .ToList();
            possibleDocks.ForEach(d => SetRendererColor(d.GetComponent<SpriteRenderer>(), 0, 1, 0, 0.5f));

            return possibleDocks;
        }

        public static void MakeDockingPointsInvisible()
        {
            var collider = GameObject.Find("Spaceship(Clone)").GetComponentsInChildren<CircleCollider2D>();
            collider.ToList().ForEach(c => SetRendererColor(c.gameObject.GetComponent<SpriteRenderer>(), 1, 1, 1, 0));
        }

        private static void SetRendererColor(SpriteRenderer renderer, float r, float g, float b, float alpha)
        {
            renderer.enabled = true;
            renderer.color = new Color(r, g, b, alpha);
        }
        
    }
}