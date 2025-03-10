using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    private Transform playerPos;
    public GameObject item;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void SpawnItem() {
        Vector3 spawnPosition = playerPos.position + new Vector3(1, 1, 1);
        Instantiate(item, spawnPosition, Quaternion.identity);
    }
}
