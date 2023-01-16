using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    public float DecreasePerSecond;
    private Light candle;
    // Start is called before the first frame update
    void Start()
    {
        candle = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        candle.intensity -= Time.deltaTime * DecreasePerSecond;
        
        if (candle.intensity < 0 )
        {
            candle.intensity = 0;
        }
    }
}
