using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    //Player info
    public string id;
    public int level;

    //Friend info
    public string[] friendID;
    public point[] friendLocation;

    //Friend request info
    public string[] friendRequestID;

}
public class point
{
    public int x;
    public int z;
}
