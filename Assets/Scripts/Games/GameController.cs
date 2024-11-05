using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] BattleSystem battleSystem;
    private MusicController musicController;
    
    private void Start(){
        musicController = FindObjectOfType<MusicController>();
        musicController.StartMusic(musicController.MapBGM);
        player = PlayerController.GetInstance();
        battleSystem = BattleSystem.GetInstance();
        player.OnEncounts = BattleStart;
        battleSystem.OnBattleOver = BattleEnd;
        battleSystem.OnGameOver = GameOver;
        if(InputNameSystem.playerNameOrigin != ""){
            player.Battler.Base.Init();
        }
        player.gameObject.SetActive(true);
        player.SetMusicController(musicController);
        if(player.MoveSceneSoundType == 0){
            musicController.StartSound(musicController.MoveSceneSound);
        }
    }

    public void BattleStart(Battler enemyBattler){
        musicController.StopMusic();
        musicController.StartMusic(musicController.BattleBGM);
        enemyBattler.Init();
        player.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleUnder(player.Battler, enemyBattler);
    }

    public void BattleEnd(Battler enemy){
        musicController.StopMusic();
        musicController.StartMusic(musicController.MapBGM);
        player.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
    }

    public void GameOver(){
        Destroy(player.gameObject);
    }
}
