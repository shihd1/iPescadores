using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    int skysI;
    public Material[] skys;

    // Start is called before the first frame update
    void Start()
    {
        //RenderSettings.skybox = skys[0];
        InvokeRepeating("SwitchDayNight", 5, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        //transform.RotateAround(Vector3.zero, Vector3.right, 10f * Time.deltaTime);
        //transform.LookAt(Vector3.zero);
    }
    void SwitchDayNight()
    {
        Debug.Log("Switch");
        if(skysI == 0)
        {
            RenderSettings.skybox = skys[1];
            skysI = 1;
        }
        else if(skysI == 1)
        {
            RenderSettings.skybox = skys[0];
            skysI = 0;
        }
    }
}
