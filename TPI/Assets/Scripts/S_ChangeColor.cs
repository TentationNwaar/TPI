/*
Auteur : Maxence Weyermann
Date : 19.05.22
Lieu : ETML
Description : Script qui permet de gérer le menu
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ChangeColor : MonoBehaviour
{
    public S_CubeMap s_CubeMap;
    public GameObject pauseMenu;
    public void StandardColor()
    {
        s_CubeMap.ApplyDayanColor = false;
        Return();
    }

    public void DayanColor()
    {
        s_CubeMap.ApplyDayanColor = true;
        Return();
    }

    public void Return()
    {
        pauseMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);    
    }
}
