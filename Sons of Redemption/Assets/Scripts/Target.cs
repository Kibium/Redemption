
using UnityEngine;

public class Target : MonoBehaviour {

    public float health;
	
    public void takeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
