using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerColliders : MonoBehaviour {

    public bool isDamaged;
    public LifeBarUI lifeUI;
    public ToxicityUI toxicityUI;
    public static int life;
    public int maxLife;
    public static int toxicity;
    public int maxToxicity;
    public Transform zombie;
    bool zombieDamage;
    void Start()
    {
        maxLife = 100;
        maxToxicity = 100;
        life = maxLife;
        toxicity = 0;
      
    }

   void Update()
    {
        lifeUI.UpdateLifeBarUI(life);
        toxicityUI.UpdateToxicityUI(toxicity);
        if(life < 0)
        {
            life = 0;
            Application.LoadLevel(2);
        }

        if (Input.GetKey(KeyCode.T))
        {
            life += 5;
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Bomba")
        {
            isDamaged = false;
            if (!isDamaged)
            {
                life -= 5; //25 es la mitad, hay 50!
                toxicity += 3;
            }
        }
        if (other.tag == "Trap")
        {
            isDamaged = false;
            if (!isDamaged)
            {
                life -= 5; //25 es la mitad, hay 50!
                toxicity += 3;
            }
        }
        if (other.tag == "Curacion")
        {
            life += 2;

        }
      
    }
    void OnTriggerExit(Collider other)
    {
        isDamaged = true;
    }
}
