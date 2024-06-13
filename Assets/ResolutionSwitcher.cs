using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSwitcher : MonoBehaviour
{
    
    bool small = true;
    bool medium = false;

    private void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeReso();
        }
    }


    public void ChangeReso()
    {
        if (small)
        {
            Screen.SetResolution(768, 768, false);
            small = false;
            medium = true;
        }
        else if (medium)
        {
            small = false;
            medium = false;
            Screen.SetResolution(1024,1024,false);
            
        }
        else
        {
            Screen.SetResolution(512, 512, false);
            small = true;
            medium = false;
        }

    }
}
