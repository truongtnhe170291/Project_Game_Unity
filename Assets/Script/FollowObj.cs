using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = new Vector3(obj.transform.position.x, obj.transform.position.y, -10f);
        transform.position = position;
    }
}
