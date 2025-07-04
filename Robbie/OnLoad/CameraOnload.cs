using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnload : MonoBehaviour
{
    static CameraOnload CR;
    private void Awake()
    {
        if (CR == null)
        {
            CR = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
