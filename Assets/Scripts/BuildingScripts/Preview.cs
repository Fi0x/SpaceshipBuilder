using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts
{
    public static class Preview
    {
        public static GameObject InitShadow(GameObject original, Transform inventoryTransform)
        {
            var shadow = Object.Instantiate(original, inventoryTransform, true);
            foreach (var comp in shadow.GetComponents<Component>())
            {
                if (!(comp is Transform || comp is SpriteRenderer || comp is BoxCollider2D))
                    Object.Destroy(comp);
            }
            for (var i = 0; i < shadow.transform.childCount; i++)
                Object.Destroy(shadow.transform.GetChild(i).gameObject);
            shadow.gameObject.AddComponent<CollisonCheck>();
            shadow.tag = "Untagged";
            var tmp = shadow.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.4f;
            shadow.GetComponent<SpriteRenderer>().color = tmp;

            return shadow;
        }
        
        public static void RenderShadow(GameObject shadow, Quaternion rotation, Transform objectTransform, List<GameObject> possibleDocks)
        {
            shadow.transform.rotation = rotation;
            (GameObject obj, Transform tf) dock = SnapHelper.GetClosestDockingPoint(objectTransform, possibleDocks);
            
            if(dock.obj == null || dock.tf == null)
                return;
            
            var spriteRenderer = shadow.gameObject.GetComponent<SpriteRenderer>();
            var position = dock.obj.transform.position;
            var localPosition = dock.tf.localPosition;
            var localPosRot= objectTransform.rotation * localPosition;
            spriteRenderer.transform.position = position-localPosRot;
        }
    }
}