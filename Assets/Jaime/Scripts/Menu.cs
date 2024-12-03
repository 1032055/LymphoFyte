using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGameKiller()
    {
        SceneManager.LoadScene("FightScene Killer", LoadSceneMode.Single);
    }

    public void PlayGameMemory()
    {
        SceneManager.LoadScene("FightScene Memory", LoadSceneMode.Single);
    }
}
