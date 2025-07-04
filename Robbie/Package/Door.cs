using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;                              //獲得動畫anim
    int openID;                                 //獲得動畫參數

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");         //獲得字符型變化變為整數型
        GameManager.RegisterDoor(this);                 //從GameManager調用門函數
    }
    public void Open()
    {
        anim.SetTrigger(openID);                        //撥放開門動畫
        AudioManager.PlayDoorOpenAudio();               //從AudioManager調用音效
        //play audio
    }
}
