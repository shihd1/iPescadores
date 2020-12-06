using UnityEngine;

public class CoinPanel: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCoinCount()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = ""+GameObject.Find("Local Data").GetComponent<Data>().coins;
    }
}
