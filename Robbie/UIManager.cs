using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public TextMeshProUGUI orbText, timeText, deathText, gameOverText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);            //不需要被銷毀
    }
    public static void UpdateOrbUI(int orbCount)                    //寶珠
    {
        instance.orbText.text = orbCount.ToString();
    }
    public static void UpdateDeathUI(int deathCount)                //死亡
    {
        instance.deathText.text = deathCount.ToString();
    }
    public static void UpdateTimeUI(float time)                     //時間
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60;

        instance.timeText.text = minutes.ToString("00")+":"+seconds.ToString("00");
    }
    public static void DisplayGameOver()
    {
        instance.gameOverText.enabled = true;
    }
}
