using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShelf : MonoBehaviour
{

    public GameObject item;
    public GeneralItem itemScript;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setScript()
    {
        itemScript = item.GetComponent<GeneralItem>();
        Debug.Log("SetScript");
    }

    public void OnClick()
    {
        itemScript.OnBuy(); 
    }
}