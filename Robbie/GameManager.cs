using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;              //調用重置


public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader fader;                       //獲得場景Fader
    List<Orb> Orbs;
    Door lockedDoor;                        //門變數

    float gameTime;
    bool gameIsOver;                        //當角色結束

    //public int orbNum;                    //寶珠數量
    public int deatNum;                     //死亡數量

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this; ;
        Orbs = new List<Orb>();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (gameIsOver)
        {
            return;
        }
        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);

    }
    public static void RegisterDoor(Door door)          //註冊門參數調用門
    {
        instance.lockedDoor = door;
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;                           //
    }
    //寶珠判斷
    public static void RegisterOrb(Orb orb)             
    {
        if (instance == null)
        {
            return;
        }
        if (!instance.Orbs.Contains(orb))
        {
            instance.Orbs.Add(orb);                     //計算列表的寶珠數量
        }
        UIManager.UpdateOrbUI(instance.Orbs.Count);     //從UIManager調用函數，將數值同步UI介面上
    }
    public static void PlayerGrabbedOrb(Orb orb)        //角色蒐集寶珠顯示
    {
        if (!instance.Orbs.Contains(orb))
        {
            return;
        }
        instance.Orbs.Remove(orb);                      //刪除寶珠數量
        if (instance.Orbs.Count == 0)                   //當寶珠變數為0
        {
            instance.lockedDoor.Open();                 //撥放開門動畫
        }
        UIManager.UpdateOrbUI(instance.Orbs.Count);     //從UIManager調用函數，將數值同步UI介面上
    }
    public static void PlayerWin()                      //當遊戲角色獲勝
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.PlayerWinAudio();
    }
    public static bool GameOver()
    {
        return instance.gameIsOver;
    }
    //角色死亡
    public static void PlayerDied()                     //角色判定為死亡後
    {
        instance.fader.FadeOut();
        instance.deatNum++;
        UIManager.UpdateDeathUI(instance.deatNum);
        instance.Invoke("RestartScene", 1.5f);          //經過1.5秒後重新加載場景
    }
    void RestartScene()         //重新加載場景
    {
        instance.Orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
