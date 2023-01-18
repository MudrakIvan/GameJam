using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrounSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2[] Ranges;
   
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
        foreach (var range in Ranges)
        {
            if (gameObject.transform.position.x >= range.x && gameObject.transform.position.x <= range.y)
            {
                background.enabled = true;
                break;
            }
            else
            {
                background.enabled = false;
            }
        }
    }
}
