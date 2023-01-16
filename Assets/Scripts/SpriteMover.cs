using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMover : MonoBehaviour
{
    public Camera camera;
    private Vector3 startPosition;
    private Vector3 cameraStartPosition;
    public float MovementMultiplicator = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        cameraStartPosition = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var cameraPos = camera.transform.position;
        gameObject.transform.position = startPosition + new Vector3(cameraPos.x - cameraStartPosition.x, cameraPos.y-cameraStartPosition.y, 0)*MovementMultiplicator;
    }
}
