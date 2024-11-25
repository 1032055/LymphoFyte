using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    [SerializeField] public GameObject MainMenu;

    [SerializeField] public GameObject OptionsMenu;


    public void LoadOptions()
    {
        
        MainMenu.SetActive(false);

        OptionsMenu.SetActive(true);
        
    }

    public void LoadMenu()
    {

        OptionsMenu.SetActive(false);

        MainMenu.SetActive(true);

    }
}
