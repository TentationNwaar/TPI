/*
Auteur : Maxence Weyermann
Date : 17.05.22
Lieu : ETML
Description : Script qui permet de lire les faces du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ReadCube : MonoBehaviour
{ 
    public Transform tUp;   
    public Transform tDown;  
    public Transform tLeft;
    public Transform tRight;    
    public Transform tFront;
    public Transform tBack;
    public S_CubeState s_CubeState;
    public S_CubeMap s_CubeMap;
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();
    private int layerMask = 1 << 3; //Pour les faces
    public GameObject emptyGo;

    // Start is called before the first frame update
    void Start()
    {
        s_CubeState =  FindObjectOfType<S_CubeState>();
        SetRayTransforms();
        ReadState();    
    }

    // Update is called once per frame
    void Update()
    {
        ReadState();
    }

    //Méthode qui permet de "lire" l'état du cube
    public void ReadState()
    {
        //On défini l'état de chaque position de la liste des côtés pour connaître la couleur et la position
        s_CubeState.up = ReadFace(upRays, tUp);
        s_CubeState.down = ReadFace(downRays, tDown);
        s_CubeState.left = ReadFace(leftRays, tLeft);
        s_CubeState.right = ReadFace(rightRays, tRight);
        s_CubeState.front = ReadFace(frontRays, tFront);
        s_CubeState.back = ReadFace(backRays, tBack);

        //On met à jour la map avec les positions trouvés
        s_CubeMap.Set();
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();
        
        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            //On vérifie si le raycast est en intersection avec un autre objet du layer
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {     
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 5, Color.green);
            }
        }    
        return facesHit;
    }

    void SetRayTransforms()
    {
        //Augmente la ray liste avec des raycasts venant du transform, dans l'angle du cube
        upRays = BuildRays(tUp, new Vector3(90,90,0));
        downRays = BuildRays(tDown, new Vector3(270,90,0));
        leftRays = BuildRays(tLeft, new Vector3(0,270,0));
        rightRays = BuildRays(tRight, new Vector3(0,90,0));
        frontRays = BuildRays(tFront, new Vector3(0,180,0));
        backRays = BuildRays(tBack, new Vector3(0,360,0));
    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        //On utilise le raycount pour nommer les différents ray pour être sûr qu'ils sont dans le bon ordre
        int rayCount = 0;

        //On crée une liste de 16 ray pour la forme des côtés du cube, 0 ray pour le haut gauche, 15 pour le bas droite
        /*
        |0||1||2||3|
        |4||5||6||7|
        |8||9||10||11|
        |12||13||14||15|
        */
        List<GameObject> rays = new List<GameObject>();
        for (int y = 1; y > -3; y--)
        {
            //Position du 0
            for (int x = -1; x < 3; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.position.x + x, 
                                               rayTransform.position.y + y, 
                                               rayTransform.position.z);
                GameObject rayStart = Instantiate(emptyGo, startPos, Quaternion.identity, rayTransform);
                
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }    
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }
    
}
