using Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 dir;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        float angle = (this.transform.rotation.eulerAngles.z-90) * Mathf.Deg2Rad;
        dir = new Vector3(-Mathf.Cos(angle), -Mathf.Sin(angle), 0);
        this.transform.position += dir.normalized;
        Destroy(this.gameObject, 6f);
        Destroy(this, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += dir * 30 * Time.deltaTime;
        this.transform.position += gameManager.GetBackgroundMovement() * Time.deltaTime;
    }
}
