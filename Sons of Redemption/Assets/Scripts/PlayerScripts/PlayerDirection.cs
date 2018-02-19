using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour {
    public float rotation;
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        rotation = GameObject.Find("PuntoCam").GetComponent<PlayerCamara>().yRotation;
        transform.rotation = Quaternion.Euler(0, rotation, 0);

       
    }
}
