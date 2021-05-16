using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
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
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        WWW www = new WWW(Application.persistentDataPath + "/saves/photo.JPG");
        this.GetComponent<AspectRatioFitter>().aspectRatio = (float)www.texture.width / www.texture.height;
        this.GetComponent<RawImage>().texture = www.texture;
    }
    public void savePicture()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        int i = 0;
        while (File.Exists(Application.persistentDataPath + "/saves/photo" + i + ".JPG"))
        {
            i++;
        }
        File.Copy(Application.persistentDataPath + "/saves/photo.JPG", Application.persistentDataPath + "/saves/photo" + i + ".JPG");
    }
    public void UploadImage()
    {
        StartCoroutine(UploadPNG());
    }
    IEnumerator UploadPNG()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        // We should only read the screen after all rendering is complete
        //yield return new WaitForEndOfFrame();

        var tex = GetTextureCopy();
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);



        /*
        // read a file and add it to the form
        List<IMultipartFormSection> form = new List<IMultipartFormSection>
 {
    new MultipartFormFileSection("image",bytes,"screenShot.png", "image")
 };
        // generate a boundary then convert the form to byte[]
        byte[] boundary = UnityWebRequest.GenerateBoundary();
        byte[] formSections = UnityWebRequest.SerializeFormSections(form, boundary);
        // my termination string consisting of CRLF--{boundary}--
        byte[] terminate = Encoding.UTF8.GetBytes(String.Concat("\r\n--", Encoding.UTF8.GetString(boundary), "--"));
        // Make my complete body from the two byte arrays
        byte[] body = new byte[formSections.Length + terminate.Length];
        Buffer.BlockCopy(formSections, 0, body, 0, formSections.Length);
        Buffer.BlockCopy(terminate, 0, body, formSections.Length, terminate.Length);
        // Set the content type - NO QUOTES around the boundary
        string contentType = String.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary));
        // Make my request object and add the raw body. Set anything else you need here
        UnityWebRequest wr = UnityWebRequest.Post("http://localhost:3000/upload/post",form);
        UploadHandler uploader = new UploadHandlerRaw(body);
        uploader.contentType = contentType;
        yield return wr.SendWebRequest();
        if (wr.isNetworkError || wr.isHttpError)
        {
            print(wr.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }
        */



        
        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());
        form.AddBinaryData("image", bytes, GameObject.Find("Local Data").GetComponent<Data>().id+".png", "image/png");

        // Upload to a cgi script
        Debug.Log("Upload to a cgi script");
        using (var w = UnityWebRequest.Post(ServerService.appserver + "/upload/post", form))
        {
            yield return w.SendWebRequest();
            Debug.Log("-----------------------------");
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                Debug.Log("Finished Uploading Screenshot");
            }
        }
        
    }
    /*IEnumerator DoRequest()
    {
        var request = UnityWebRequest.Get("ptsv2.com/t/1j4xq-1610873654/post");
        
    }*/
    Texture2D GetTextureCopy()
    {
        Texture2D source = (Texture2D)GetComponent<RawImage>().texture;
        RenderTexture rt = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
            );
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        return readableTexture;
    }
}
