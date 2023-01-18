using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2[] Ranges;
    public float fadeIn;
    public float fadeOut;
    public float maxIntesity;
    public GameObject Target;
    
    private Light mLight;
    
    private void Start()
    {
        
    }

    void Awake()
    {

        ChangeVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeVisibility();
    }


    // Update is called once per frame
    void ChangeVisibility()
    {
        mLight = gameObject.GetComponent<Light>();
        foreach (var range in Ranges)
        {
            if (Target.transform.position.x >= range.x && Target.transform.position.x <= range.y)
            {
                var fadeInPos = Target.transform.position.x - range.x;
                var fadeOutPos = range.y - Target.transform.position.x;
                if (fadeInPos < fadeIn && fadeInPos > 0)
                {
                    mLight.intensity = maxIntesity * (fadeInPos/fadeIn);
                }
                if (fadeOutPos < fadeOut && fadeOutPos >0)
                {
                    mLight.intensity = maxIntesity * (fadeOutPos/fadeOut);
                }
                mLight.enabled = true;
                break;
            }
            else
            {
                mLight.enabled = false;
            }
        }
    }
}
