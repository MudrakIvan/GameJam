using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgrounSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public float StartPositionX;
    public float EndPositionX;
   
    private SpriteRenderer background;

    void Awake()
    {
        
        ChangeVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeVisibility();
    }
    
    void ChangeVisibility()
    {
        background = gameObject.GetComponent<SpriteRenderer>();
        if (gameObject.transform.position.x >= StartPositionX && gameObject.transform.position.x <= EndPositionX)
        {
            background.enabled = true;
        }
        else
        {
            background.enabled = false;
        }
    }
}
