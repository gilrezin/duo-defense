using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostTextLayer : MonoBehaviour
{

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<Renderer>().sortingLayerName = "Items";
        text.GetComponent<Renderer>().sortingOrder = -1;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
