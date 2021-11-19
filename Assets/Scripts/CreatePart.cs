using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatePart : MonoBehaviour
{
    public GameObject boxPrefab;

    private GameObject inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = this.gameObject.transform.parent.gameObject;

        for (int i = 0; i < 10; i++)
        {
          CreateBox();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateBox();
        }
    }

    private void OnMouseDown()
    {
        CreateBox();
    }

    private void CreateBox()
    {
        GameObject a = Instantiate(boxPrefab, inventory.transform, true) as GameObject;
        a.transform.position = transform.position;
    }
}
