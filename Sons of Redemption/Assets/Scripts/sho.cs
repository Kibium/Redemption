using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sho : MonoBehaviour {
    public GameObject fireBall;
    public Transform shotPosition;
    public int maxMunicion = 0;
    public int municion = 0;
    public bool recargar;
    public float counter;

    public int almacenBalas;
    public float pistola;
    public int direction;
    public Canvas canvas;

    public Camera fpsCam;
    public float range = 100f;
    public float damage = 20;

    public BalasUI balasUI;
    public GameObject impactEffect;
    public GameManager gm;
    public bool firstTime = true;
    // Use this for initialization
    void Start () {
        municion = maxMunicion;
        almacenBalas = 0;
    }
	
	// Update is called once per frame
	void Update () {
        balasUI.almacenBalas(almacenBalas);
        if (GameManager.playerGetPistola && firstTime)
        {
            almacenBalas += 6;
            firstTime = false;
        }
        if (GameManager.playerGetPistola)
        {
            if (counter > 1f)
            {
                if (municion > 0)
                {
                    if (AimBehaviourBasic.isAiming)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Shoot();
                            municion--;
                            counter = 0;

                        }
                    }
                }
            }
            else
            {
                counter += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R))
            {
                if (almacenBalas > 0)
                {
                    if (municion > 0)
                    {
                        maxMunicion -= municion;
                        almacenBalas -= maxMunicion;
                        municion =6;
                    }
                    else
                    {
                        municion = maxMunicion;
                        almacenBalas -= maxMunicion;
                    }
                }
                maxMunicion = 6;
            }

        }

    }
  /*  void Shot()
    {
        //audio.Play3D(0, 1, 1);
        //anim.SetTrigger("shot");
 
        GameObject ball = Instantiate(fireBall, shotPosition.position,
                                     Quaternion.identity) as GameObject;
        ball.GetComponent<BolaDeFuego>().SetDirection(transform.forward);
    }
  */
    void Shoot()
    {
   
        RaycastHit hit;

        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit,range))
            {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.takeDamage(damage);
            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.1f);
        }
    }
}


