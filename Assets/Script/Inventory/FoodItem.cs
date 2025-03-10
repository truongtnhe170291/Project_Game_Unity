using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour {

    private PlayerStats player;
    public GameObject explosionEffect;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void Use() {
        Instantiate(explosionEffect, player.transform.position, Quaternion.identity);
        player.IncreaseStat("speed", 1);
        Destroy(gameObject);
    }

}
