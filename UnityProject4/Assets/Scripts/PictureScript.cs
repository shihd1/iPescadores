using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showPicture()
    {
        WWW www = new WWW(Application.persistentDataPath + "/saves/photo.JPG");
        Debug.Log(Application.persistentDataPath + "/saves/photo.JPG");
        this.GetComponent<AspectRatioFitter>().aspectRatio = (float)www.texture.width / www.texture.height;
    }
}
