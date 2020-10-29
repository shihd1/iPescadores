using UnityEngine;
using UnityEngine.UI;

public class CreateAccount : MonoBehaviour
{
    //Create Account
    public Text firstName;
    public Text lastName;
    public Text username;
    public Text password;
    public Text confirmPassword;

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

    public void createAccount()
    {

        if (password.text != confirmPassword.text)
        {
            info.text = "Password does not match!";
            canMoveOn(false);
        }
        else
        {
            string value = serverService.usernameExist(username.text);
            if (value.Equals("True"))
            {
                info.text = "Username is not valid!";
                canMoveOn(false);
            }
            else if (value.Equals("False"))
            {
                string val = serverService.createUser(username.text, firstName.text, lastName.text, password.text);
                if (!val.Equals("ERROR"))
                {
                    info.text = "Account Created!";
                    canMoveOn(true);
                }
                else
                {
                    info.text = "Please try again in 5 minutes!";
                    canMoveOn(false);
                }
            }
            else if (value.Equals("ERROR"))
            {
                info.text = "Please try again in 5 minutes!";
                canMoveOn(false);
            }
        }
    }
    public void canMoveOn(bool yes)
    {
        moveOn.gameObject.SetActive(yes);
        redo.gameObject.SetActive(!yes);
    }
}
