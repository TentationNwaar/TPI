/*
Auteur : Maxence Weyermann
Date : 19.05.22
Lieu : ETML
Description : Script qui permet de gérer le menu
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PauseMenu : MonoBehaviour
{
    //Permet d'utiliser le script en question
    public S_SelectFace s_SelectFace;

    public GameObject colorMenu;

    //Défini si le jeu est en pause ou non
    internal bool GameIsPaused = false;

    //Permet d'insérer le Game Object du menu
    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        s_SelectFace.gameObject.SetActive(true);
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        s_SelectFace.gameObject.SetActive(false);
        GameIsPaused = true;
    }

    public void ChangeColor()
    {
        colorMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
