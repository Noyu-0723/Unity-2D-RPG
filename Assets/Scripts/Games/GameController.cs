using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] MenuSystem menuSystem;
    
    private void Start(){
        player = PlayerController.GetInstance();
        battleSystem = BattleSystem.GetInstance();
        menuSystem = MenuSystem.GetInstance();
        player.OnEncounts = BattleStart;
        player.OnOpenMenu = MenuOpen;
        battleSystem.OnBattleOver = BattleEnd;
        menuSystem.OnCloseMenu = MenuClose;
        if(InputNameSystem.playerNameOrigin != ""){
            player.Battler.Base.Init();
        }
    }

    public void BattleStart(Battler enemyBattler){
        enemyBattler.Init();
        player.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleUnder(player.Battler, enemyBattler);
    }

    public void BattleEnd(){
        player.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
    }

    public void MenuOpen(){
        player.gameObject.SetActive(false);
        menuSystem.gameObject.SetActive(true);
        menuSystem.MenuStart(player.Battler);
    }

    public void MenuClose(){
        player.gameObject.SetActive(true);
        menuSystem.gameObject.SetActive(false);
    }
}