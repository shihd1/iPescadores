using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public string id;
    public string[] friendID;
    public point[] friendLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(id);
    }

}
public class point
{
    public int x;
    public int z;

}
