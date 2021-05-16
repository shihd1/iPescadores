using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgscript : MonoBehaviour
{
    public static bgscript BgInstance;

    private void Awake()
    {
        if (BgInstance != null && BgInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        BgInstance = this;
        DontDestroyOnLoad(this);
    }

}
