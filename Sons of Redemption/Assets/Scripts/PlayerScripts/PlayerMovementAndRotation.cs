using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAndRotation : MonoBehaviour {

    public float velocitat;

    public float speed = 2;
 
    public Transform directW;
    public Transform directA;
    public Transform directS;
    public Transform directD;
    public float gravityMagnitude;
    public bool apuntando;
    public bool disparando;
    public float smooth = 0;
    //seran privadas
    public float yVelocityW = 0.0f; 
    public float yAngleW;

    public float yVelocityS = 0.0f;
    public float yAngleS;

    public float yVelocityA = 0.0f;
    public float yAngleA;

    public float yVelocityD = 0.0f;
    public float yAngleD;

    public Vector3 moveDirection = Vector3.zero;
    public bool XW;
    public bool XA;
    public bool XS;
    public bool XD;
    public float H;
    public CharacterController characterController;
    public Rigidbody rb;
    public Rigidbody bala;
    public GameObject pistola;
    public Canvas canvas;
    public Vector2 mousePos;
    public Vector3 screenPos;
    public float theta;
    public float dx, dz;
    public Camera camera;
    public float direction;
    Ray ray;
    RaycastHit hit;
    // Use this for initialization
    void Start () {
        apuntando = false;
        disparando = false;

        characterController = GetComponent<CharacterController>();
       

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = directW.TransformDirection(moveDirection * Time.deltaTime);

        yAngleW = Mathf.SmoothDampAngle(transform.eulerAngles.y, directW.eulerAngles.y, ref yVelocityW, smooth);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 13))
        {
            if (Input.GetMouseButton(0))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("hola");
                }
            }
        }
            if (Input.GetMouseButton(1))
            {  // Si disparo o apunto
           // El player rotara hacia donde este mirando el objeto de la variable DirectW
            transform.rotation = Quaternion.Euler(0, yAngleW, 0);
            apuntando = true;
        }
         
        if (Input.GetKey(KeyCode.W) && XS == false)
                {
                    // El player rotara hacia donde este mirando el objeto de la variable DirectW
                    transform.rotation = Quaternion.Euler(0, yAngleW, 0);
                    XW = true;
                }
                else
                {
                    XW = false;
                }

                if (Input.GetKey(KeyCode.W) && XS == false)
                {
                    transform.rotation = Quaternion.Euler(0, yAngleW, 0);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);

                    XW = true;
                }
                else
                {
                    XW = false;
                }
                yAngleS = Mathf.SmoothDampAngle(transform.eulerAngles.y, directS.eulerAngles.y, ref yVelocityS, smooth);
                if (Input.GetKey(KeyCode.S) && XW == false)
                {
                    transform.rotation = Quaternion.Euler(0, yAngleS, 0);
                    transform.Translate(Vector3.back * -speed * Time.deltaTime);
                    XS = true;

                }
                else
                {
                    XS = false;
                }
                yAngleA = Mathf.SmoothDampAngle(transform.eulerAngles.y, directA.eulerAngles.y, ref yVelocityA, smooth);
                if (Input.GetKey(KeyCode.A) && XD == false)
                {
                    transform.rotation = Quaternion.Euler(0, yAngleA, 0);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    XA = true;
                }
                else
                {
                    XA = false;
                }
                yAngleD = Mathf.SmoothDampAngle(transform.eulerAngles.y, directD.eulerAngles.y, ref yVelocityD, smooth);
                if (Input.GetKey(KeyCode.D) && XA == false)
                {
                    transform.rotation = Quaternion.Euler(0, yAngleD, 0);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);

                    XD = true;
                }
                else
                {
                    XD = false;
                }
     }
   


}

