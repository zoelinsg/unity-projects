using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    int playerLayer;            //player圖層
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");          //playerLayer為Player
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)      //當gameObject圖層碰到PlayerLayer時
        {
            Debug.Log("Player Win!");
        }
        GameManager.PlayerWin();
    }
}
