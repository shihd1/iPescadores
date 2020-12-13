using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRejectFriendRequest : MonoBehaviour
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
        GameObject.Find("Manager").GetComponent<FriendManager>().rejectFriendRequest(id);
        transform.parent.gameObject.SetActive(false);
    }
}
