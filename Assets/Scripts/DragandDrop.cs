using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragandDrop : MonoBehaviour{
    // Start is called before the first frame update
    private Vector3 spawnPos;
    private Vector3 size;
    private bool Colide;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        Colide = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
        Colide = true;
    }
    

    private void OnMouseDown()
    {
        spawnPos = transform.position;
    }

    
    void OnMouseDrag()
    {
        transform.position = GetMousePos();
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("END");
        if (Colide)
        {
            transform.position = spawnPos;
        }
    }

    // Update is called once per frame
    Vector3 GetMousePos()
    {
        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = 0;
        return mousPos;
    }
}
