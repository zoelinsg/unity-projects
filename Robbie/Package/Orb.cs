using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int player;
    public GameObject ExplosionVFXPrefab;
    void Start()
    {
        player = LayerMask.NameToLayer("Player");
        GameManager.RegisterOrb(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == player)               //寶珠被主角吃掉
        {
            Instantiate(ExplosionVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayOrAudio();
            GameManager.PlayerGrabbedOrb(this);                 //調用GameManager寶珠被吃掉
        }
    }
}
