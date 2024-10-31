using System.Collections;
using UnityEngine;

namespace Assets
{
    public class NewMonoBehaviour : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            bool test = false;
            for (int i = 0; i < 9; i++)
            {
                test = !test;
            }


        }       
    }
}