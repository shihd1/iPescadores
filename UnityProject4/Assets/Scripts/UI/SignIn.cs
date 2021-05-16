using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    //Sign in
    public Text username;
    public InputField setUsername;
    public Text password;
    public InputField setPassword;

    //Display Information
    public Text info;
    public Button moveOn;
    public Button redo;

    //Remember me
    public bool rememberMe = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Username") != "")
        {
            Debug.Log("Auto-Login");
            setUsername.text = PlayerPrefs.GetString("Username");
            setPassword.text = PlayerPrefs.GetString("Password");
            signIn();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void signIn()
    {
        Debug.Log(username.text + " " + password.text);
        int value = ServerService.signIn(username.text,password.text);
        if(value == 0)
        {
            info.text = "Login is successful!";
            canMoveOn(true);
            if(rememberMe == true)
            {
                PlayerPrefs.SetString("Username", username.text);
                PlayerPrefs.SetString("Password", password.text);
            }
        }
        else if (value == 1)
        {
            info.text = "Your username is not valid or your password is incorrect";
            canMoveOn(false);
        }
        else if (value == 2)
        {
            info.text = "Please connect to the internet to sign in";
            canMoveOn(false);
        }
        GameObject.Find("Canvas").transform.Find("Popup Tab").gameObject.SetActive(true);
    }
    public void changeRememberMeStatus()
    {
        rememberMe = !rememberMe;
    }
    public void clearSavedLogin()
    {
        PlayerPrefs.SetString("Username", "");
        PlayerPrefs.SetString("Password", "");
    }
    public void canMoveOn(bool yes)
    {
        moveOn.gameObject.SetActive(yes);
        redo.gameObject.SetActive(!yes);
    }
}
