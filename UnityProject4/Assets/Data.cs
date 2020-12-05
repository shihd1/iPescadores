using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    //Player info
    public string id;
    public int level;
    public int coins;

    //Friend info
    public string[] friendID;
    public Point[] friendLocation;

    //Friend request info
    public string[] friendRequestID;

    void Start()
    {

    }
}
public class Point
{
    public float x;
    public float z;
    public Point(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
}
