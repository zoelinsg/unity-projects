using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("環境聲音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;
    [Header("Fx音效")]
    public AudioClip deathFXClip;       //死亡音效
    public AudioClip orbFXClip;         //寶珠音效
    public AudioClip doorFXClip;        //開門音效
    public AudioClip StartLevelClip;
    public AudioClip WinClip;

    [Header("Robbie音效")]
    public AudioClip[] walkSetClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deachClip;

    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    public AudioClip oubVoiceClip;

    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource voiceSource;

    public AudioMixerGroup ambientGroup, musicGroup, FXGroup, playerGroup, voiceGroup;
    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        fxSource.outputAudioMixerGroup = FXGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();
    }
    void StartLevelAudio()          //進入場景開始撥放
    {
        //環境音效->執行迴圈->撥放
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();
        //背景音效->執行迴圈->撥放
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();

        current.fxSource.clip = current.StartLevelClip;
        current.fxSource.Play();
    }
    public static void PlayerWinAudio()
    {
        current.fxSource.clip = current.WinClip;
        current.fxSource.Play();

        current.playerSource.Stop();
    }
    public static void PlayDoorOpenAudio()      //打開門音效
    {
        current.fxSource.clip = current.doorFXClip;
        current.fxSource.PlayDelayed(1.1f);     //延遲效果1.1秒撥放音效
    }
    public static void PlayFootstepAudio()
    {
        int index = Random.Range(0, current.walkSetClips.Length);

        current.playerSource.clip = current.walkSetClips[index];
        current.playerSource.Play();
    }
    public static void PlayCrouchFootstepAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);

        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }
    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.jumpVoiceClip;
        current.playerSource.Play();
    }
    public static void PlayDeathAudio()     //死亡時音效
    {
        current.playerSource.clip = current.deachClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }
    public static void PlayOrAudio()        //收集寶珠音效
    {
        current.fxSource.clip = current.oubVoiceClip;
        current.fxSource.Play();

        current.voiceSource.clip = current.oubVoiceClip;
        current.voiceSource.Play();
    }
}