using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMOnload : MonoBehaviour
{
    static CMOnload CM;
    private void Awake()
    {
        if (CM == null)
        {
            CM = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
