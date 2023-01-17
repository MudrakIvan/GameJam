using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    /// <summary>
    /// Time before the enemy will turn around
    /// </summary>
    public float OneDiractionWalkTime = 0.5f;

    /// <summary>
    /// Speed of movement
    /// </summary>
    public float Speed = 1.0f;

    private bool DiractionLeft = true;

    private float TimeOut = 0.0f;

    private BoxCollider2D CharacterCollider;

    private BoxCollider2D GroundCollider;

    // Start is called before the first frame update
    private void Start()
    {
        TimeOut = OneDiractionWalkTime / 2;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        TimeOut += Time.deltaTime;
        if (TimeOut >= OneDiractionWalkTime){
            TimeOut = 0.0f;
            DiractionLeft = !DiractionLeft;
        }

        transform.position = new Vector3(((DiractionLeft) ? -1 : 1) * Time.deltaTime * Speed, 0.0f, 0.0f) + transform.position;
    }

    private void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.name == ""){
            Destroy(gameObject);
        }
    }
}
