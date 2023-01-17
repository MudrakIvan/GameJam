using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject Target;
    private Vector3 startPosition;
    private Vector3 cameraStartPosition;
    public float MovementMultiplicator = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        cameraStartPosition = Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }
        
        var cameraPos = Target.transform.position;
        gameObject.transform.position = startPosition + new Vector3(cameraPos.x - cameraStartPosition.x, cameraPos.y-cameraStartPosition.y, 0)*MovementMultiplicator;
    }
}
