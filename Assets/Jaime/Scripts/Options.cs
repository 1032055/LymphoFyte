using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    [SerializeField] public GameObject MainMenu;

    [SerializeField] public GameObject OptionsMenu;

    [SerializeField] public GameObject ControlsPopUp;

    [SerializeField] public GameObject CharacterMenu;
 
    private void Start()
    {
        OptionsMenu.SetActive(false);

        ControlsPopUp.SetActive(false);

        CharacterMenu.SetActive(false);

        MainMenu.SetActive(true);
    }

    public void LoadOptions()
    {
        
        MainMenu.SetActive(false);

        OptionsMenu.SetActive(true);
        
    }

    public void LoadMenu()
    {

        OptionsMenu.SetActive(false);

        MainMenu.SetActive(true);

        ControlsPopUp.SetActive(false);

    }

    public void LoadControls()
    {
        ControlsPopUp.SetActive(true);

        MainMenu.SetActive(false);
    }

    public void LoadCharacters()
    {
        CharacterMenu.SetActive(true);

        MainMenu.SetActive(false);
    }

}
