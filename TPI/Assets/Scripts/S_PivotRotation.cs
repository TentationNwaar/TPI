/*
Auteur : Maxence Weyermann
Date : 19.05.22
Lieu : ETML
Description : Script qui permet de gérer le menu
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class S_PivotRotation : MonoBehaviour
{
    //Côté actif du cube
    private List<GameObject> activeSide;

    //Position de départ de la souris
    private Vector3 mouseRef;

    //Détermine si le mouvement est en rotation ou non
    private bool dragging = false;

    //Détermine si il faut ajuster automatiquement la rotation ou non
    private bool autoRotating = false;

    //Sensibilité du cube
    private float sensitivity = 0.3f;

    //Vitesse de rotation
    private float speed = 200f;

    //Vecteur de rotation
    private Vector3 rotation;

    //Quaternion ciblé
    private Quaternion targetQuaternion;

    //Pièce centrale à pivoter
    public GameObject turnCenter;

    //Appel au script mentionné
    private S_ReadCube s_ReadCube;
    private S_CubeState s_CubeState;
    public S_Automate s_Automate;

    //Fait référence à la dernière rotation
    internal Quaternion lastRotation;

    // Start is called before the first frame update
    void Start()
    {
        s_ReadCube = FindObjectOfType<S_ReadCube>();
        s_CubeState = FindObjectOfType<S_CubeState>();
        s_Automate = FindObjectOfType<S_Automate>();
        //Permet de définir la dernière rotation
        lastRotation = this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging && !autoRotating)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(1))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }
        if (autoRotating)
        {
            //Corrige la rotation, evite qu'elle bug
            AutoRotate();
        }
    }

    //Méthode pour déterminer l'axe du côté à tourner
    private void SpinSide(List<GameObject> side)
    {
        //reset la rotation
        rotation = Vector3.zero;

        //La position actuelle de la souris - la dernière position
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);
        float additionMouse = mouseOffset.x + mouseOffset.y;

        if (side == s_CubeState.up)
        {
            rotation.y = (additionMouse) * sensitivity * 1;
        }

        if (side == s_CubeState.down)
        {
            rotation.y = (additionMouse) * sensitivity * -1;
        }

        if (side == s_CubeState.left)
        {
            rotation.x = (additionMouse) * sensitivity * 1;
        }

        if (side == s_CubeState.right)
        {
            rotation.x = (additionMouse) * sensitivity * -1;
        }

        if (side == s_CubeState.front)
        {          
            rotation.z = (additionMouse) * sensitivity * -1;
        }

        if (side == s_CubeState.back)
        {
            rotation.z = (additionMouse) * sensitivity * 1;
        }
        
        //Rotation
        transform.Rotate(rotation, Space.Self);
        mouseRef = Input.mousePosition;
    }

    //Permet de faire tourner le côté
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
    }

    //Ajuste l'angle de rotation
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;

        //On arrondi le vecteur autour de 90 degrés
        vec.x = Mathf.Round(vec.x/90) * 90;
        vec.y = Mathf.Round(vec.y/90) * 90;
        vec.z = Mathf.Round(vec.z/90) * 90;

        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }
    // C'est le petit mouvement qui termine le mouvement manuel
    private void AutoRotate()
    {
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);  

        //si on est autour de 1 degrée, on défini l'angle comme étant l'angle ciblé et on termine la rotation
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            //On déparente les petits cubes
            s_CubeState.PutDown(activeSide, transform.parent);
            s_ReadCube.ReadState();
            S_CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;

            if(( Mathf.Round(lastRotation.eulerAngles.x)!= Mathf.Round(this.transform.localRotation.eulerAngles.x) ||  
                Mathf.Round(lastRotation.eulerAngles.y)!= Mathf.Round(this.transform.localRotation.eulerAngles.y) || 
                Mathf.Round(lastRotation.eulerAngles.z)!= Mathf.Round(this.transform.localRotation.eulerAngles.z) ))
            {
                if (transform.gameObject.name == "FCenter")
                {
                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.z/90) * 90 ;
                    if (lastAngle == 0 && Math.Abs(Mathf.Round(lastRotation.z))== 1)
                    {   
                        lastAngle = 180;          
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.z/90) * 90;
                    if (newAngle== 0 && Math.Abs(Mathf.Round(transform.localRotation.z))==1)
                    {
                        newAngle = 180;
                    }
                    
                    if (lastAngle + 90 == newAngle ||( lastAngle + 90 == 360 && newAngle==0))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.F);
                    }
                    if(lastAngle + 180 == newAngle || (lastAngle+ 180 == 360 && newAngle== 0) || (lastAngle+180 == 450 && newAngle==90) || (lastAngle - 180 == newAngle) || (lastAngle-  180 == -90 && newAngle== 270) || (lastAngle-180 == -180 && newAngle==180))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.F);
                        s_Automate.AddToHistory(S_Automate.Move.F);
                    }
                    if(lastAngle - 90 == newAngle ||( lastAngle - 90 == -90 && newAngle==270))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.FPrime);
                    }
                }

                if (transform.gameObject.name == "BCenter")
                {
                    //move BPrime ou BPrime BPrime
                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.z / 90) * 90;
                    if (lastAngle == 0 && Math.Abs(Mathf.Round(lastRotation.x)) == 1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.z / 90) * 90;
                    if (newAngle == 0 && Math.Abs(Mathf.Round(transform.localRotation.z)) == 1)
                    {
                        newAngle = 180;
                    }

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.BPrime);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.B);
                        s_Automate.AddToHistory(S_Automate.Move.B);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.B);
                    }
                }

                if (transform.gameObject.name == "UCenter")
                {
                    //move U ou U U

                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.y/90) * 90 ;
                    if (lastAngle == 0 && Math.Abs(Mathf.Round(lastRotation.y))==1)
                    {   
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.y/90) * 90;
                    if (newAngle== 0 && Math.Abs(Mathf.Round(transform.localRotation.y))==1)
                    {
                        newAngle = 180;
                    }

                    if(lastAngle + 90 == newAngle ||( lastAngle + 90 == 360 && newAngle==0))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.U);
                    }
                    if(lastAngle + 180 == newAngle || (lastAngle+ 180 == 360 && newAngle== 0) || (lastAngle+180 == 450 && newAngle==90) || lastAngle - 180 == newAngle || (lastAngle-  180 == -90 && newAngle== 270) || (lastAngle-180 == -180 && newAngle==180))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.U);
                        s_Automate.AddToHistory(S_Automate.Move.U);
                    }
                    if(lastAngle - 90 == newAngle ||( lastAngle - 90 == -90 && newAngle==270))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.UPrime);
                    }
                }

                if (transform.gameObject.name == "DCenter")
                {
                    //move D ou DPrime

                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.y / 90) * 90;
                    if (lastAngle == 0 && Math.Abs(Mathf.Round(lastRotation.y)) == 1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.y / 90) * 90;
                    if (newAngle == 0 && Math.Abs(Mathf.Round(transform.localRotation.y)) == 1)
                    {
                        newAngle = 180;
                    }

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.DPrime);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.D);
                        s_Automate.AddToHistory(S_Automate.Move.D);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.D);
                    }
                }

                if (transform.gameObject.name == "LCenter")
                {
                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.x / 90) * 90;
                    if (lastAngle == 0 && Math.Abs(Mathf.Round(lastRotation.x)) == 1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.x / 90) * 90;
                    if (newAngle == 0 && Math.Abs(Mathf.Round(transform.localRotation.x)) == 1)
                    {
                        newAngle = 180;
                    }

                    //Debug.Log(lastAngle + " : : " + newAngle);

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.L);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.L);
                        s_Automate.AddToHistory(S_Automate.Move.L);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.LPrime);
                    }
                }

                if (transform.gameObject.name == "RCenter")
                {
                    //move RPrime ou RPrimeRPrime

                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.x / 90) * 90;
                    if (lastAngle == 0 && Math.Abs(Mathf.Round(lastRotation.x)) == 1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.x / 90) * 90;
                    if (newAngle == 0 && Math.Abs(Mathf.Round(transform.localRotation.x)) == 1)
                    {
                        newAngle = 180;
                    }

                    //Debug.Log(lastAngle + " : : " + newAngle);

                    // 0 90 180 270
                    // 270 180 90 0

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0) )
                    {
                        s_Automate.AddToHistory(S_Automate.Move.RPrime);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.R);
                        s_Automate.AddToHistory(S_Automate.Move.R);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        s_Automate.AddToHistory(S_Automate.Move.R);
                    }
                }
            }
            //On applique la dernière rotation
            lastRotation = transform.localRotation;
        }
    }

    // Mouvement quand c'est automatique seulement
    public void StartAutoRotate(List<GameObject> side, float angle, Vector3 localForward)
    {
        s_CubeState.Pickup(side);
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.rotation;
        activeSide = side;
        autoRotating = true;
    }
}
