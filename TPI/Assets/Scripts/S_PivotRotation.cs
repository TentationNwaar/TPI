using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;

    private float sensitivity = 0.3f;
    private float speed = 200f;
    private Vector3 rotation;
    private Quaternion targetQuaternion;
    private S_ReadCube s_ReadCube;

    private S_CubeState s_CubeState;
    //public S_Automate //s_Automate;
    public Quaternion lastRotation;

    // Start is called before the first frame update
    void Start()
    {
        s_ReadCube = FindObjectOfType<S_ReadCube>();
        s_CubeState = FindObjectOfType<S_CubeState>();
        ////s_Automate = FindObjectOfType<S_Automate>();
        lastRotation = this.transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dragging && !autoRotating)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Ouaah");
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

    private void SpinSide(List<GameObject> side)
    {
        //reset la rotation
        rotation = Vector3.zero;

        //La position actuelle de la souris - la dernière position
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);
        float AdditionMouse = mouseOffset.x + mouseOffset.y;

        if (side == s_CubeState.up)
        {
            rotation.y = (AdditionMouse) * sensitivity * 1;
        }

        if (side == s_CubeState.down)
        {
            rotation.y = (AdditionMouse) * sensitivity * -1;
        }

        if (side == s_CubeState.left)
        {
            rotation.z = (AdditionMouse) * sensitivity * 1;
        }

        if (side == s_CubeState.right)
        {
            rotation.z = (AdditionMouse) * sensitivity * -1;
        }

        if (side == s_CubeState.front)
        {
            rotation.x = (AdditionMouse) * sensitivity * -1;
        }

        if (side == s_CubeState.back)
        {
            rotation.x = (AdditionMouse) * sensitivity * 1;
        }
        
        //Rotation
        transform.Rotate(rotation, Space.Self);
        mouseRef = Input.mousePosition;
    }

    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;

        //Crée un vecteur pour tourner autour
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition; 
    }

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
            //S_CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;

            if(( Mathf.Round(lastRotation.eulerAngles.x)!= Mathf.Round(this.transform.localRotation.eulerAngles.x) ||  
                Mathf.Round(lastRotation.eulerAngles.y)!= Mathf.Round(this.transform.localRotation.eulerAngles.y) || 
                Mathf.Round(lastRotation.eulerAngles.z)!= Mathf.Round(this.transform.localRotation.eulerAngles.z) ))
            {
                if (transform.gameObject.name == "FMM")
                {
                    // | 1 | :: 0 == 180
                    //  0 :: 0 == 0
                    // 270
                    // 90
                    
                    // Debug.Log("last : " + Mathf.Round(lastRotation.x));
                    // Debug.Log("now : " + Mathf.Round(transform.localRotation.x));
                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.x/90) * 90 ;
                    if (lastAngle == 0 && Mathf.Round(lastRotation.x)==-1)
                    {   
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.x/90) * 90;
                    if (newAngle== 0 && Mathf.Round(transform.localRotation.x)==-1)
                    {
                        newAngle = 180;
                    }

                    if(lastAngle + 90 == newAngle ||( lastAngle + 90 == 360 && newAngle==0))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.F);
                    }
                    if(lastAngle + 180 == newAngle || (lastAngle+ 180 == 360 && newAngle== 0) || (lastAngle+180 == 450 && newAngle==90) || (lastAngle - 180 == newAngle) || (lastAngle-  180 == -90 && newAngle== 270) || (lastAngle-180 == -180 && newAngle==180))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.F);
                        //s_Automate.AddToHistory(S_Automate.Move.F);
                    }
                    if(lastAngle - 90 == newAngle ||( lastAngle - 90 == -90 && newAngle==270))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.FPrime);
                    }
                }

                if (transform.gameObject.name == "BMM")
                {
                    //move BPrime ou BPrime BPrime
                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.x / 90) * 90;
                    if (lastAngle == 0 && Mathf.Round(lastRotation.x) == -1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.x / 90) * 90;
                    if (newAngle == 0 && Mathf.Round(transform.localRotation.x) == -1)
                    {
                        newAngle = 180;
                    }

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.B);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.B);
                        //s_Automate.AddToHistory(S_Automate.Move.B);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.BPrime);
                    }
                }

                if (transform.gameObject.name == "UMM")
                {
                    //move U ou U U

                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.y/90) * 90 ;
                    if (lastAngle == 0 && Mathf.Round(lastRotation.y)==-1)
                    {   
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.y/90) * 90;
                    if (newAngle== 0 && Mathf.Round(transform.localRotation.y)==-1)
                    {
                        newAngle = 180;
                    }

                    if(lastAngle + 90 == newAngle ||( lastAngle + 90 == 360 && newAngle==0))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.U);
                    }
                    if(lastAngle + 180 == newAngle || (lastAngle+ 180 == 360 && newAngle== 0) || (lastAngle+180 == 450 && newAngle==90) || lastAngle - 180 == newAngle || (lastAngle-  180 == -90 && newAngle== 270) || (lastAngle-180 == -180 && newAngle==180))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.U);
                        //s_Automate.AddToHistory(S_Automate.Move.U);
                    }
                    if(lastAngle - 90 == newAngle ||( lastAngle - 90 == -90 && newAngle==270))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.UPrime);
                    }
                }

                if (transform.gameObject.name == "DMM")
                {
                    //move D ou DPrime

                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.y / 90) * 90;
                    if (lastAngle == 0 && Mathf.Round(lastRotation.y) == -1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.y / 90) * 90;
                    if (newAngle == 0 && Mathf.Round(transform.localRotation.y) == -1)
                    {
                        newAngle = 180;
                    }

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.DPrime);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.D);
                        //s_Automate.AddToHistory(S_Automate.Move.D);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.D);
                    }
                }

                if (transform.gameObject.name == "LMM")
                {
                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.z / 90) * 90;
                    if (lastAngle == 0 && Mathf.Round(lastRotation.z) == -1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.z / 90) * 90;
                    if (newAngle == 0 && Mathf.Round(transform.localRotation.z) == -1)
                    {
                        newAngle = 180;
                    }

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.L);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.L);
                        //s_Automate.AddToHistory(S_Automate.Move.L);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.LPrime);
                    }
                }

                if (transform.gameObject.name == "RMM")
                {
                    //move RPrime ou RPrimeRPrime

                    float lastAngle = Mathf.Round(lastRotation.eulerAngles.z / 90) * 90;
                    if (lastAngle == 0 && Mathf.Round(lastRotation.z) == -1)
                    {
                        lastAngle = 180;
                    }

                    float newAngle = Mathf.Round(transform.localEulerAngles.z / 90) * 90;
                    if (newAngle == 0 && Mathf.Round(transform.localRotation.z) == -1)
                    {
                        newAngle = 180;
                    }
                    Debug.Log(lastAngle + " : : " + newAngle);

                    // 0 90 180 270
                    // 270 180 90 0

                    if (lastAngle + 90 == newAngle || (lastAngle + 90 == 360 && newAngle == 0))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.RPrime);
                    }
                    if (lastAngle + 180 == newAngle || (lastAngle + 180 == 360 && newAngle == 0) || (lastAngle + 180 == 450 && newAngle == 90) || lastAngle - 180 == newAngle || (lastAngle - 180 == -90 && newAngle == 270) || (lastAngle - 180 == -180 && newAngle == 180))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.R);
                        //s_Automate.AddToHistory(S_Automate.Move.R);
                    }
                    if (lastAngle - 90 == newAngle || (lastAngle - 90 == -90 && newAngle == 270))
                    {
                        //s_Automate.AddToHistory(S_Automate.Move.R);
                    }
                }
            }
            //On applique la dernière rotation
            lastRotation = transform.localRotation;
        }
    }

    
}
