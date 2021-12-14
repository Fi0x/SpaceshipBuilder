using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonCheck : MonoBehaviour
{
    public bool _invalid;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Ship"))
            this.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 0.4f);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.transform.CompareTag("Ship"))
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
    }
}
