using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfirmFriendRequest : MonoBehaviour
{
    public Text id;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate
        {
            OnClick();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        GameObject.Find("Manager").GetComponent<FriendManager>().confirmFriendRequest(id);
        transform.parent.gameObject.SetActive(false);
        Transform infoCanvas = GameObject.Find("Canvas").transform.Find("Popup Tab");
        infoCanvas.transform.Find("Display Info").Find("Text").GetComponent<Text>().text = "Confirmed!";
        infoCanvas.transform.Find("Display Info").Find("MoveOn").gameObject.SetActive(true);
        infoCanvas.transform.Find("Display Info").Find("Redo").gameObject.SetActive(false);
        infoCanvas.gameObject.SetActive(true);
    }
}
