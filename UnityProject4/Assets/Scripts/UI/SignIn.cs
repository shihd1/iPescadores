using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    //Sign in
    public Text username;
    public Text password;

    //Display Information
    public Text info;
    public Button moveOn;
    public Button redo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void signIn()
    {
        bool value = ServerService.signIn(username.text,password.text);
        if(value == true)
        {
            info.text = "Login is successful!";
            canMoveOn(true);
        }
        else
        {
            info.text = "Username is not valid or your password is incorrect!";
            canMoveOn(false);
        }
    }
    public void canMoveOn(bool yes)
    {
        moveOn.gameObject.SetActive(yes);
        redo.gameObject.SetActive(!yes);
    }
}
