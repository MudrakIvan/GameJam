using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    public float DecreasePerSecond;
    public float RemoveHealthPerSecond = 1.0f;
    public float MaxIntensity;
    public Vector2 ActiveRange;

    private Light candle;

    private Player mPlayer;

    // Start is called before the first frame update
    void Start()
    {
        candle = gameObject.GetComponent<Light>();
        mPlayer = gameObject.GetComponentInParent<Player>();
    }

    void FixedUpdate()
    {
        if (!(mPlayer.transform.position.x >= ActiveRange[0] && mPlayer.transform.position.x <= ActiveRange[1]))
        {
            candle.enabled = false;
            return;
        }
        
        candle.enabled = true;
        candle.intensity -= Time.fixedDeltaTime * DecreasePerSecond;
        
        if (candle.intensity <= 0.0f)
        {
            candle.intensity = 0.0f;
            mPlayer.RemoveHealth(RemoveHealthPerSecond * Time.fixedDeltaTime);
        }
    }

    public void AddIntensity(float intensity)
    {
        if(candle.intensity + intensity <= MaxIntensity)
        {
            candle.intensity += intensity;
        }
        else
        {
            candle.intensity = MaxIntensity;
        }
        
    }
}
