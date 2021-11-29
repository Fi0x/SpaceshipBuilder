
using System.Collections.Generic;
using System.Linq;
using Parts;
using UnityEngine;

namespace BuildingScripts
{
    public static class SnapHelper
    {
        public static bool Snap(Transform originalTransform, GameObject spaceship, SpaceshipPart partType, IEnumerable<GameObject> possibleDocks)
        {
            (var dockingObject, var dockingTransform) = GetClosestDockingPoint(originalTransform, possibleDocks);
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

        public static (GameObject, Transform) GetClosestDockingPoint(Transform originalTransform, IEnumerable<GameObject> possibleDocks)
        {
            Transform dockingPoint = null;
            
            var closestDistanceSqr = Mathf.Infinity;
            GameObject closestPart = null;
            foreach (Transform child in originalTransform)
            {
                foreach (var possibleDock in possibleDocks)
                {
                    var localRotVec = - (originalTransform.rotation * child.localPosition);
                    if(possibleDock.transform.parent.rotation * possibleDock.transform.localPosition != localRotVec)
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

        public static IEnumerable<GameObject> GetPossibleDockingPoints(GameObject dragObject)
        {
            var exceptions = new List<GameObject>();
            dragObject.GetComponentsInChildren<CircleCollider2D>().ToList().ForEach(child => exceptions.Add(child.gameObject));

            var emptyDocks = GameObject.FindGameObjectsWithTag("DockEmpty");
            var possibleDocks = emptyDocks
                .Where(dock => dock.transform.parent.tag.Equals("Ship"))
                .Except(exceptions)
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