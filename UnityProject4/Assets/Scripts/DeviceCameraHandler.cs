using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class DeviceCameraHandler : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No camera detected !");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            //if (devices[i].isFrontFacing) //back camera
            if (devices[i].isFrontFacing) // front camera
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);

            }
        }

        if (backCam == null)
        {
            Debug.Log("Unable to find back camera!");
            return;
        }

        backCam.Play();
        background.texture = backCam;
        camAvailable = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!camAvailable)
        {
            return;
        }

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void TakePhoto()  // Start this Coroutine on some button click
    {
        // NOTE - you almost certainly have to do this here:

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 
        /*
        Texture2D photo = new Texture2D(backCam.width, backCam.height);
        photo.SetPixels(backCam.GetPixels());
        photo.Apply();

        //Encode to a PNG
        byte[] bytes = photo.EncodeToJPG();
        */
        //Write out the PNG.
        //File.WriteAllBytes(Application.persistentDataPath + "/saves/" + "photo.JPG", bytes);
        //UnityEngine.Debug.Log(Application.persistentDataPath + "/saves/" + "photo.png");
    }
    public void TakeSamplePhoto()  // Start this Coroutine on some button click
    {/*
        GameObject mg = GameObject.Find("Manager");
        Main mi = mg.GetComponent<Main>();
        int location = mi.currentPlace;
        int species = mi.currentThing;

        System.Random random = new System.Random();
        int num = random.Next(4);

        File.Delete(Application.persistentDataPath + "/saves/" + "photo.JPG");*/

        //UnityEditor.FileUtil.CopyFileOrDirectory(Application.persistentDataPath + "/saves/" + location + "/" + species + "T/" + "photo" + num + ".JPG",
        //    Application.persistentDataPath + "/saves/" + "photo.JPG");
    }
}
