using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject terrain = GameObject.Find("Mesh Generator");
        float playerYPos = GameObject.Find("Player").GetComponent<Transform>().position.y;
        float z = GameObject.Find("Mesh Generator").GetComponent<Transform>().position.z;
        float x = GameObject.Find("Mesh Generator").GetComponent<Transform>().position.x;
        float y = GameObject.Find("Mesh Generator").GetComponent<Transform>().position.y;
        GameObject.Find("Player").transform.position = new Vector3(x, y, z);    //(x, y, z)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
