using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    [SerializeField] public ObjectList[] models;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showMyModel()
    {
        int level = GameObject.Find("Local Data").GetComponent<Data>().level;
        GameObject.Find("Manager").GetComponent<FriendManager>().showFriend(level, new Vector3(0, 0, 0));
    }
    public void showModel(int index)
    {
        int i = 0;
        while (models[index].list[i].gameObject.activeSelf)
        {
            i++;
        }
        models[index].list[i].gameObject.SetActive(true);
    }
}
[System.Serializable]
public class ObjectList
{
    public GameObject[] list;
}