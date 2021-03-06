/*
Auteur : Maxence Weyermann
Date : 17.05.22
Lieu : ETML
Description : Script qui permet de tourner les faces du cubes
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_CubeMap : MonoBehaviour
{
    S_CubeState s_CubeState;
    S_ChangeColor s_ChangeColor;
    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    internal bool ApplyDayanColor = false;

    //Permet d'appliquer les couleurs sur la map
    public void Set()
    {
        s_CubeState = FindObjectOfType<S_CubeState>();
        s_ChangeColor = FindObjectOfType<S_ChangeColor>();
        UpdateMap(s_CubeState.front, front);
        UpdateMap(s_CubeState.back, back);
        UpdateMap(s_CubeState.left, left);
        UpdateMap(s_CubeState.right, right);
        UpdateMap(s_CubeState.up, up);
        UpdateMap(s_CubeState.down, down);
    }

    //Par rapport à notre liste, on va changer la couleur de la map du cube
    internal void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach (Transform map in side)
        {
            if (face[i].name[0] == 'F')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            
            if (face[i].name[0] == 'B')
            {
                face[i].GetComponent<Renderer>().material.color = Color.blue;
                map.GetComponent<Image>().color = Color.blue;

                if (ApplyDayanColor)
                {
                    face[i].GetComponent<Renderer>().material.color = Color.cyan;
                    map.GetComponent<Image>().color = Color.cyan;
                }
            }

            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = Color.white;
            }

            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.yellow;
            }

            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1); //Couleur orange
            }

            if (face[i].name[0] == 'R')
            {
                face[i].GetComponent<Renderer>().material.color = Color.red;
                map.GetComponent<Image>().color = Color.red;

                if (ApplyDayanColor)
                {
                    face[i].GetComponent<Renderer>().material.color = Color.magenta;
                    map.GetComponent<Image>().color = Color.magenta;
                }
            }
            i++;          
        }
    }
}
