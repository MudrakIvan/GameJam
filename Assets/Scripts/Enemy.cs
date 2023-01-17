using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    /// <summary>
    /// Time before the enemy will turn around
    /// </summary>
    public float oneDiractionWalkTime = 0.5f;

    /// <summary>
    /// Speed of movement
    /// </summary>
    public float speed = 1.0f;

    private bool diractionLeft = true;

    private float timeOut = 0.0f;

    private BoxCollider2D characterCollider;

    private BoxCollider2D groundCollider;

    // Start is called before the first frame update
    private void Start()
    {
        timeOut = oneDiractionWalkTime / 2;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (timeOut + Time.deltaTime >= oneDiractionWalkTime){
            timeOut = 0.0f;
            diractionLeft = !diractionLeft;
        }

        transform.position = new Vector3(((diractionLeft) ? -1 : 1) * Time.deltaTime * speed, 0.0f, 0.0f) + transform.position;
    }

    private void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.name == ""){
            Destroy(gameObject);
        }
    }
}
