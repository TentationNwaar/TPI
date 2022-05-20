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
    //Appel au script mentionné
    public S_CubeMap s_CubeMap;

    //Permet d'activer le menu précédent
    public GameObject pauseMenu;

    //Applique les couleurs standards au cube
    public void StandardColor()
    {
        s_CubeMap.ApplyDayanColor = false;
        Return();
    }

    //Applique la couleur Dayan au cube
    public void DayanColor()
    {
        s_CubeMap.ApplyDayanColor = true;
        Return();
    }

    //Permet à l’utilisateur de retourner au menu précédent
    public void Return()
    {
        pauseMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);    
    }
}
