using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaDeFuego : MonoBehaviour {

    public Rigidbody rb;
    public float speed = 13;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(this.gameObject, 3);
	}
    public void SetDirection(Vector3 dir)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = dir * speed;
    }
}
