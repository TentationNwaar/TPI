/*
Auteur : Maxence Weyermann
Date : 17.05.22
Lieu : ETML
Description : Script qui permet de voir l'état du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CubeState : MonoBehaviour
{
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();
    public static bool autoRotating = false;
    public static bool started = false;

    //Permet de grouper les pièces entre elles pour
    //Faire la rotation de la face
    public void Pickup(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            //On attache le parent de chaque face (petit cube) 
            //au parent de l'index (cube du milieu), sauf si c'est déjà le cas
            if (face != cubeSide[16])
            {
                face.transform.parent.transform.parent = cubeSide[16].transform;
            }
        }
    }

    //Permet de relâcher les petites pièces 
    //Pour ne pas créer de bugs et les réutiliser
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[16])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }
}
