using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatePart : MonoBehaviour
{
    public GameObject boxPrefab;

    private GameObject _inventory;
    private void Start()
    {
        this._inventory = this.gameObject.transform.parent.gameObject;

        for (var i = 0; i < 10; i++)
        {
          this.CreateBox();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.CreateBox();
        }
    }

    private void OnMouseDown()
    {
        this.CreateBox();
    }

    private void CreateBox()
    {
        var a = Instantiate(this.boxPrefab, this._inventory.transform, true);
        a.transform.position = this.transform.position;
    }
}
