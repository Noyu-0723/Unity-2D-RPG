using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour{
    public AudioSource MapBGM;
    public AudioSource BattleBGM;
    public AudioSource DoorOpenSound;
    public AudioSource MoveSceneSound;
    public AudioSource NowBGM;
    public AudioSource NowSound;

    public void StartMusic(AudioSource audio){
        // BGMを再生
        NowBGM = audio;
        NowBGM.volume = 0.2f;
        NowBGM.loop = true;
        NowBGM.Play();
    }

    public void StopMusic(){
        // BGMをストップ
        if(NowBGM != null){
            NowBGM.Stop();
        }
    }

    public void StartSound(AudioSource audio){
        // 効果音を再生
        NowSound = audio;
        NowSound.Play();
    }
}
