using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI, orbUI;
    private int uiXOffset = 5000; 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            //inventoryUI.SetActive(!inventoryUI.activeSelf);
            //orbUI.SetActive(!orbUI.activeSelf);

            inventoryUI.transform.position += new Vector3(uiXOffset,0,0);
            orbUI.transform.position += new Vector3(uiXOffset, 0, 0);
            uiXOffset = -uiXOffset;
        }
    }
}
