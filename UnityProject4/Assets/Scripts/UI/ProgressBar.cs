using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int minimum;
    public int maximum;
    public int current;
    public Image mask;
    public Image fill;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }
    public void setFill()
    {
        Debug.Log(this.ToString() + " " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        int totalXP = GameObject.Find("Local Data").GetComponent<Data>().totalXP;
        int[] xpLevels = new int[] {0, 100, 350, 850, 1600, 2600, 4600, 7600, 11600, 16600, 22600, 29600, 37600, 46600, 56600, 67600, 78600, 92600, 106600, 121600};
        for(int i = xpLevels.Length-1; i>=0; i--)
        {
            if(totalXP >= xpLevels[i])
            {
                minimum = xpLevels[i];
                current = totalXP;
                if (i == xpLevels.Length - 1)
                {
                    maximum = totalXP;
                }
                else
                {
                    maximum = xpLevels[i + 1];
                }
                transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = ""+(i+1);
                GameObject.Find("Local Data").GetComponent<Data>().level = i+1;
                ServerService.updateLevel();
                break;
            }
        }
        GetCurrentFill();
    }
}
