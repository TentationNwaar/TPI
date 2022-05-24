/*
Auteur : Maxence Weyermann
Date : 24.05.22
Lieu : ETML
Description : Script qui permet de résoudre le rubik's cube et de le mélanger
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

    public class S_Automate : MonoBehaviour
    {
        public static List<Move> moveList = new List<Move>() { };
        private S_CubeState s_CubeState;
        private S_ReadCube s_ReadCube;
        //private S_SolveTwoPhase s_solveTwoPhase;
        public List<Move> history;
        //public S_mouvement s_Mouvement;

        //Liste des mouvements possible du cube
        public enum Move
        {
            U, UPrime,
            D, DPrime,
            L, LPrime,
            R, RPrime, 
            F, FPrime, 
            B, BPrime,
        }

        // Start is called before the first frame update
        void Start()
        {
            s_CubeState = FindObjectOfType<S_CubeState>();
            s_ReadCube = FindObjectOfType<S_ReadCube>();
            //s_solveTwoPhase = FindObjectOfType<S_SolveTwoPhase>();
        }

        // Update is called once per frame
        void Update()
        {

            //Si l'utilisateur appuye sur ces touches, le mélange se lance
            if (Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.A))
            {
                Debug.Log("OKK");
                //méthode de mélange
                Shuffle();
            }

            if(Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.R))
            {
                Debug.Log("On résout !");
                //méthode de résolution
            }

            if (moveList.Count > 0 && !S_CubeState.autoRotating && S_CubeState.started)
            {
                //Faire le mouvement au premier index
                DoMove(moveList[0]);

                //Annuler le mouvement au premier index
                moveList.Remove(moveList[0]);

                // // Si on est en mode solve
                // if (s_solveTwoPhase.solveCount>0)
                // {
                //     s_solveTwoPhase.solveCount--;
                //     if (s_solveTwoPhase.solveCount == 0)
                //     {
                //         StartCoroutine(ClearAfterSolve());
                //     }
                // }
            }
        }
        IEnumerator ClearAfterSolve()
        {
            yield return new WaitForSeconds(1);
            history.Clear();

            //Si il bug
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Shuffle()
        {
            Debug.Log("Melannnge");
            //s_Mouvement.target.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            StartCoroutine(WaitBeforeShuffle());
        }

        float SetVectorDirection(Func<List<GameObject>, float> deleg, List<GameObject> rotatingFace)
        {
            return 0f - deleg(rotatingFace);
        }

        internal void DoMove(Move move)
        {
            //Permet de connaître l'état du cube avant d'effectuer les mouvements automatisée
            s_ReadCube.ReadState();
            S_CubeState.autoRotating = true;

            // Délégué : petite fonction rapide à laquelle on lui fournit un objet (ici List<GameObject>)
            // et retourne ce que l'on veut à partir de cet objet (ici, une propriété float)
            // Appel : xDelegate(liste) -> retourne la propriété "position.x"
            Func<List<GameObject>, float> xDelegate = x => x[4].transform.parent.transform.position.x;
            Func<List<GameObject>, float> yDelegate = x => x[4].transform.parent.transform.position.y;
            Func<List<GameObject>, float> zDelegate = x => x[4].transform.parent.transform.position.z;

            List<GameObject> rotatingFace = null;
            Vector3 directionVector = new Vector3 (0,0,0);
            int angle = 0;

            switch (move)
            {
                //Si on bouge vers le haut, la rotation sera de -90C
                case Move.U:
                    rotatingFace = s_CubeState.up;
                    angle = -90;
                    directionVector = new Vector3(0f, SetVectorDirection(yDelegate, rotatingFace), 0f);
                    break;
                case Move.UPrime:
                    rotatingFace = s_CubeState.up;
                    angle = 90;
                    directionVector = new Vector3(0f, SetVectorDirection(yDelegate, rotatingFace), 0f);
                    break;
                case Move.D:
                    rotatingFace = s_CubeState.down;
                    angle = -90;
                    directionVector = new Vector3(0f, SetVectorDirection(yDelegate, rotatingFace), 0f);
                    break;
                case Move.DPrime:
                    rotatingFace = s_CubeState.down;
                    angle = 90;
                    directionVector = new Vector3(0f, SetVectorDirection(yDelegate, rotatingFace), 0f);
                    break;
                case Move.L:
                    rotatingFace = s_CubeState.left;
                    angle = -90;
                    directionVector = new Vector3(0f, 0f, SetVectorDirection(zDelegate, rotatingFace));
                    break;
                case Move.LPrime:
                    rotatingFace = s_CubeState.left;
                    angle = 90;
                    directionVector = new Vector3(0f, 0f, SetVectorDirection(zDelegate, rotatingFace));
                    break;
                case Move.R:
                    rotatingFace = s_CubeState.right;
                    angle = -90;
                    directionVector = new Vector3(0f, 0f, SetVectorDirection(zDelegate, rotatingFace));
                    break;
                case Move.RPrime:
                    rotatingFace = s_CubeState.right;
                    angle = 90;
                    directionVector = new Vector3(0f, 0f, SetVectorDirection(zDelegate, rotatingFace));
                    break;
                case Move.F:
                    rotatingFace = s_CubeState.front;
                    angle = -90;
                    directionVector = new Vector3(SetVectorDirection(xDelegate, rotatingFace), 0f, 0f);
                    break;
                case Move.FPrime:
                    rotatingFace = s_CubeState.front;
                    angle = 90;
                    directionVector = new Vector3(SetVectorDirection(xDelegate, rotatingFace), 0f, 0f);
                    break;
                case Move.B:
                    rotatingFace = s_CubeState.back;
                    angle = -90;
                    directionVector = new Vector3(SetVectorDirection(xDelegate, rotatingFace), 0f, 0f);
                    break;
                case Move.BPrime:
                    rotatingFace = s_CubeState.back;
                    angle = 90;
                    directionVector = new Vector3(SetVectorDirection(xDelegate, rotatingFace), 0f, 0f);
                    break;
            }
            RotateSide(rotatingFace, angle, directionVector);            
        }

        public void AddToHistory(Move move)
        {
            history.Add(move);
        }

        void RotateSide(List<GameObject> side, float angle, Vector3 localForward)
        {
            foreach(var sides in side)
            {
                Debug.Log(side.Count);
            }
            //On tourne automatiquement les côtés par les angles
            S_PivotRotation pr = side[4].transform.parent.GetComponent<S_PivotRotation>();
            pr.StartAutoRotate(side, angle, localForward);
        }
        
        IEnumerator WaitBeforeShuffle()
        {
            yield return new WaitForSeconds(0f);
            //yield return new WaitForSeconds(1.3f);
            List<Move> randomMoves = new List<Move>();
            int shuffleLength = Random.Range(10, 30);
            for (int i = 0; i < shuffleLength; i++)
            {
                //Tableau contenant toutes les valeurs de l'enum Move
                Array moveValues = Enum.GetValues(typeof(Move));

                //Valeur tirée au sort
                object randomMoveValue = moveValues.GetValue(Random.Range(0, moveValues.Length));

                //On retransforme la valeur en un type Move à travers le cast
                randomMoves.Add((Move)randomMoveValue);
            }
            moveList = randomMoves;
        }
    }
