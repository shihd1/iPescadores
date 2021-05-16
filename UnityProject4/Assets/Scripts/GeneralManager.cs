using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralManager : MonoBehaviour
{
    public Sprite UIMask;
    public Sprite UISprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject particleSystem = FindInActiveObjectByName("Particle System");
        if (particleSystem.activeSelf == true && !particleSystem.GetComponent<ParticleSystem>().isPlaying)
        {
            particleSystem.SetActive(false);
        }
    }
    public void changeCoin(int amount)
    {
        //Update LocalData coins
        GameObject.Find("Local Data").GetComponent<Data>().coins += amount;
        
        //Update UI display
        GameObject.Find("Canvas")
                .transform.Find("Main Page")
                .transform.Find("Horizontal Panel")
                .transform.Find("Text (TMP)")
                .GetComponent<CoinPanel>().updateCoinCount();

        ////Gain xp
        //changeXP(amount);

        //Update server data - coins, numObjects
        ServerService.updateCoins();
    }
    public void changeXP(int amount)
    {
        //Update LocalData xp
        GameObject.Find("Local Data").GetComponent<Data>().totalXP += amount;

        int currentLevel = GameObject.Find("Local Data").GetComponent<Data>().level;

        //Convert xp into level and Update UI display
        GameObject.Find("Canvas")
            .transform.Find("Main Page")
            .transform.Find("Radial Progress bar")
            .GetComponent<ProgressBar>().setFill();

        //Show animation
        int newLevel = GameObject.Find("Local Data").GetComponent<Data>().level;
        if (currentLevel != newLevel)
        {
            FindInActiveObjectByName("Particle System").SetActive(true);
            GameObject.Find("Manager").GetComponent<ModelManager>().showMyModel();
            GameObject.Find("Manager").GetComponent<ModelManager>().removePrevModel();
        }

        //Update level restrictions
        GameObject.Find("Canvas")
            .transform.Find("Shop")
            .GetComponent<ShopScript>().removeLevelRestrictions();

        //Update server data - totalXP, level
        ServerService.updateLevel();
        ServerService.updateTotalXP();

    }
    public void changeSensitivity(float x)
    {
        GameObject.Find("Local Data").GetComponent<Data>().sensitivity = x;
        GameObject.Find("DollyCart1").GetComponent<CartHandler>().sensitivity = x * 40f + 10;
    }
    public void changeVolume(float x)
    {
        GameObject.Find("Background Music").GetComponent<AudioSource>().volume = x;
    }
    public void setLanguage(string language)
    {
        GameObject.Find("Local Data").GetComponent<Data>().language = language;
    }
    public void showLanguage()
    {
        string language = GameObject.Find("Local Data").GetComponent<Data>().language;
        //FindInActiveObjectByName("English").GetComponent<CanvasRenderer>().SetColor(new Color(0.6698113f, 0.6698113f, 0.6698113f));
        //FindInActiveObjectByName("English").GetComponent<CanvasRenderer>().SetTexture(UIMask)
        //Sprite UIMaskSprite = //Resources.Load<Sprite>("Textures/Knob.psd");
        //Instantiate(UISprite);
            //Sprite.Create(UISprite, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f),10,0,SpriteMeshType.FullRect,new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
        //UIMaskSprite.border.Set(10, 10, 10, 10);
        //FindInActiveObjectByName("Chinese").GetComponent<Image>().sprite = UIMaskSprite;
        //FindInActiveObjectByName("Chinese").GetComponent<Image>().type = Image.Type.Sliced;
        
        if (language.Equals("English"))
        {
            //Debug.Log("English");
            FindInActiveObjectByName("English").GetComponent<Image>().sprite = Instantiate(UISprite);
            FindInActiveObjectByName("Chinese").GetComponent<Image>().sprite = Instantiate(UIMask);
        }
        else if (language.Equals("Chinese"))
        {
            //Debug.Log("Chinese");
            FindInActiveObjectByName("Chinese").GetComponent<Image>().sprite = Instantiate(UISprite);
            FindInActiveObjectByName("English").GetComponent<Image>().sprite = Instantiate(UIMask);
        }
        else
        {
            Debug.Log("Error - Unable to show UI in settings");
        }
        
    }
    public void showSlider()
    {
        FindInActiveObjectByName("Sensitivity").GetComponent<Slider>().value = GameObject.Find("Local Data").GetComponent<Data>().sensitivity;
        FindInActiveObjectByName("Music Volume").GetComponent<Slider>().value = GameObject.Find("Background Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void saveSettings()
    {
        PlayerPrefs.SetFloat("Sensitivity", GameObject.Find("Local Data").GetComponent<Data>().sensitivity);
        PlayerPrefs.SetString("Language", GameObject.Find("Local Data").GetComponent<Data>().language);
        PlayerPrefs.SetFloat("MusicVolume",GameObject.Find("Background Music").GetComponent<AudioSource>().volume);
    }
    public void startChecking()
    {
        InvokeRepeating("checkServerPoints", 5, 10);
    }
    public void checkServerPoints()
    {
        int points = ServerService.getPoints(GameObject.Find("Local Data").GetComponent<Data>().id);
        if (points != 0)
        {
            changeCoin(points * 50);
            changeXP(points * 100);
            if (!ServerService.erasePoints())
            {
                Debug.Log("Error! Could not erase points.");
            }
        }
    }
    public static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
