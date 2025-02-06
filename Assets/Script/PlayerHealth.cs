using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int maxHealth;
    int currentHealth;
    public HealthManager healthManager;
    void Start()
    {
        currentHealth = maxHealth;
        healthManager.UpdateHealth(currentHealth, maxHealth);
    }
    public void TakeDamage(int minDamage, int maxDamage)
    {
        var damage = Random.Range(minDamage, maxDamage);
        Debug.Log(damage);
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            GameObject.Destroy(gameObject);
        }
        healthManager.UpdateHealth(currentHealth, maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10, 15);
        }
    }
}
