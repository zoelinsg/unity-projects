using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerhealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    GameManager gameMan;

    int trapsLayers;

    // Start is called before the first frame update
    void Start()
    {
        trapsLayers = LayerMask.NameToLayer("Traps");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果主角採到陷阱即會觸發死亡
        if (collision.gameObject.layer == trapsLayers) 
        {
            //Instantiate(deathVFXPrefab, transform.position, transform.rotation);  //遊戲主角死亡會化作一團煙消失
            Instantiate(deathVFXPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(-45,90)));  //死亡殘影隨機旋轉
            gameObject.SetActive(false);  //遊戲主角啟用關閉
            AudioManager.PlayDeathAudio();  //調用死亡音效
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  //死亡後重置回當時的場景
            GameManager.PlayerDied();  //原先的死亡重置回遊戲場景改在GameManager,後在這裡調用
        }
    }
}
