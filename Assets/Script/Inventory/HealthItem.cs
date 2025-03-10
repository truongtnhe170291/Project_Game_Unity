using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour {

    private PlayerStats player;
    public GameObject healthEffect;
    public int healthBoost;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void Use() {
        Instantiate(healthEffect, player.transform.position, Quaternion.identity);
        player.IncreaseStat("health", 10);
        Destroy(gameObject);
    }
}
 