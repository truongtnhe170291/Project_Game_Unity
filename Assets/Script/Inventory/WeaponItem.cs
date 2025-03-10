using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour {

    public GameObject sword;
    private PlayerStats player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void Use() {

        // Instantiate(healthEffect, player.transform.position, Quaternion.identity);
        player.IncreaseStat("attack", 10);
        Destroy(gameObject);
    }
}
