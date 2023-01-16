using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject Entity;
    public Vector2[] Positions;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Positions.Length; i++)
        {
            var clone = Instantiate(Entity);
            clone.transform.parent = gameObject.transform;
            clone.transform.position = Positions[i];
        }
        Destroy(Entity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
