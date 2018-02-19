using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour {
    public float counterToDoDamage = 0;
    private CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 targetDirection;

    public int speed;
    public bool nearTarget = false;

    private float gravity;
    public float gravityMagnitude;

   // public PlayFX audio;
    public int maxLife;
    public int life;

    public Transform target;

    public float range;
    public bool targetFound;
    public LayerMask mask;

    public static float explosionRange;
    public LayerMask explosionMask;
    public float explosionDamage;
    private bool boom;

    public GameObject psExplosion;
    bool isDamaged = false;
    // Use this for initialization
    void Start()
    {
        nearTarget = false;
        explosionRange = 2.5f;
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

        gravity = Physics.gravity.y;
       
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
      //  FindTarget();
        //DetectNearObject();
        FollowTarget();
      
    }

    void FollowTarget()
    {
        // target.position = new Vector3(target.position.x - 0.5f, target.position.y, target.position.z - 0.5f);
      
            targetDirection = target.position - transform.position;
             targetDirection = targetDirection.normalized;
        if (!containerItm.playerIsHere)
        {

            if (Vector3.Distance(target.position, transform.position) <= range)
            {
                if (!nearTarget)
                {
                    moveDirection = new Vector3(targetDirection.x * speed, moveDirection.y, targetDirection.z * speed);
                    controller.Move(moveDirection * Time.deltaTime);
                }
            }
            if (Vector3.Distance(target.position, transform.position) <= explosionRange)
            {
                nearTarget = true;
                if (!isDamaged)
                {
                    PlayerColliders.life -= 5;
                    PlayerColliders.toxicity += 10;
                    isDamaged = true;
                }
            }
            else
            {
                nearTarget = false;
                isDamaged = false;
            }

            if (counterToDoDamage > 3)
            {
                isDamaged = false;
                counterToDoDamage = 0;
            }
            else
            {
                counterToDoDamage += Time.deltaTime;
            }
        }

    }

/*    void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, mask);

        if (hitColliders.Length != 0)
        {
            targetFound = true;
            target = hitColliders[0].transform;
        }
        else
        {
            target = null;
            targetFound = false;
        }
    }
  
    */
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
