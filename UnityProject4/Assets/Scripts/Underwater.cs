using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour
{
    public float waterlevel;
    private bool isUnderwater;
    public Color normalColor;
    public Color underwaterColor;

    // Start is called before the first frame update
    void Start()
    {
        //normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        //underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.y < waterlevel) != isUnderwater)
        {
            isUnderwater = transform.position.y < waterlevel;
            if (isUnderwater)
                SetUnderwater();
            if (!isUnderwater)
                SetNormal();
        }
    }
    void SetUnderwater()
    {
        Debug.Log("SetUnderwater");
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.1f;
    }
    void SetNormal()
    {
        Debug.Log("SetNormal");
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.01f;
    }
}
