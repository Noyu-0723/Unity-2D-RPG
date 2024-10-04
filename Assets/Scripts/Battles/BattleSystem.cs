using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    static BattleSystem Instance = null;

    public static BattleSystem GetInstance(){
        if(Instance == null){
            Instance = FindObjectOfType<BattleSystem>();
        }
        return Instance;
    }

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if(Instance != this){
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    enum State{
        Start,
        ActionSelection,
        SecondActionSelection,
        AttackSelection,
        MagicSelection,
        ItemSelection,
        RunTurns,
        BattleOver,
    }

    State state;

    [SerializeField] ActionSelectionUI actionSelectionUI;
    [SerializeField] SecondActionSelectionUI secondActionSelectionUI;
    [SerializeField] AttackSelectionUI attackSelectionUI;
    [SerializeField] MagicSelectionUI magicSelectionUI;
    [SerializeField] ItemSelectionUI itemSelectionUI;
    [SerializeField] BattleDialog battleDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    public UnityAction OnBattleOver;

    public void BattleUnder(Battler player, Battler enemy){
        state = State.Start;
        
        actionSelectionUI.Init();
        secondActionSelectionUI.Init();
        attackSelectionUI.Init(player.Attacks);
        magicSelectionUI.Init(player.Magics);
        itemSelectionUI.Init(player.Items);

        StartCoroutine(SetupBattle(player, enemy));
    }

    IEnumerator SetupBattle(Battler player, Battler enemy){
        playerUnit.Setup(player);
        enemyUnit.Setup(enemy);
        yield return battleDialog.TypeDialog($"{enemy.Base.Name}が現れた！\nどうする？");
        StateForActionSelection();
    }

    void BattleOver(){
        attackSelectionUI.DeleteMoveTexts();
        magicSelectionUI.DeleteMoveTexts();
        itemSelectionUI.DeleteItemTexts();
        OnBattleOver?.Invoke();
    }

    void StateForActionSelection(){
        state = State.ActionSelection;
        actionSelectionUI.Open();
    }

    void StateForSecondActionSelection(){
        state = State.SecondActionSelection;
        secondActionSelectionUI.Open();
    }

    void StateForAttackSelection(){
        state = State.AttackSelection;
        attackSelectionUI.Open();
    }

    void StateForMagicSelection(){
        state = State.MagicSelection;
        magicSelectionUI.Open();
    }

    void StateForItemSelection(){
        state = State.ItemSelection;
        itemSelectionUI.Open();
    }

    IEnumerator StateForMoveRunTurns(Move playerMove){
        state = State.RunTurns;
        yield return RunMove(playerMove, playerUnit, enemyUnit);
        yield return BattleWin();
        
        Move ememyMove = enemyUnit.Battler.GetRandomAttack();
        yield return RunMove(ememyMove, enemyUnit, playerUnit);
        yield return BattleLose();
        
        yield return battleDialog.TypeDialog("どうする？");
        StateForActionSelection();
    }

    IEnumerator StateForItemUseTurns(Item playerItem){
        state = State.RunTurns;
        yield return UseItem(playerItem, playerUnit, enemyUnit);
        yield return BattleWin();
        
        Move ememyMove = enemyUnit.Battler.GetRandomAttack();
        yield return RunMove(ememyMove, enemyUnit, playerUnit);
        yield return BattleLose();
        
        yield return battleDialog.TypeDialog("どうする？");
        StateForActionSelection();
    }

    IEnumerator RunMove(Move move, BattleUnit sourceUnit, BattleUnit targetUnit){
        string resultText;
        if(sourceUnit.Battler.NoMoveTurn > 0){
            resultText = $"{sourceUnit.Battler.Base.Name}は動けない…!";
        }else{
            resultText = move.Base.RunMoveResult(sourceUnit, targetUnit);
        }
        yield return battleDialog.TypeDialog(resultText, auto: false);
        sourceUnit.UpdateUI();
        targetUnit.UpdateUI();

        if(targetUnit.Battler.HP <= 0){
            state = State.BattleOver;
        }
    }

    IEnumerator UseItem(Item item, BattleUnit sourceUnit, BattleUnit targetUnit){
        string resultText;
        if(sourceUnit.Battler.NoMoveTurn > 0){
            resultText = $"{sourceUnit.Battler.Base.Name}は動けない…!";
        }else{
            resultText = item.Base.UseItemResult(sourceUnit, targetUnit);
        }
        yield return battleDialog.TypeDialog(resultText, auto: false);
        sourceUnit.UpdateUI();
        targetUnit.UpdateUI();

        if(targetUnit.Battler.HP <= 0){
            state = State.BattleOver;
        }
    }

    IEnumerator BattleWin(){
        if(state == State.BattleOver){
            yield return battleDialog.TypeDialog($"{enemyUnit.Battler.Base.Name}を倒した", auto: false);
            // 経験値を得る
            playerUnit.Battler.HasExp += enemyUnit.Battler.Base.Exp;
            yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は{enemyUnit.Battler.Base.Exp}の経験値を得た", auto: false);
            // レベルアップの判定
            while(playerUnit.Battler.IsLevelUp()){
                playerUnit.Battler.UpdateStatus();
                playerUnit.UpdateUI();
                yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}はレベル{playerUnit.Battler.Level}になった", auto: false);
                // 特定のレベルなら技を習得
                Move learnedMove = playerUnit.Battler.LearnedMove();
                if(learnedMove != null){
                    yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は{learnedMove.Base.Name}を覚えた", auto: false);
                }
            }
            // アイテムの判定
            string itemGetString = playerUnit.Battler.GetRandomItem(enemyUnit);
            if(itemGetString != null){
                yield return battleDialog.TypeDialog(itemGetString, auto: false);
            }
            BattleOver();
            yield break;
        }
    }

    IEnumerator BattleLose(){
        if(state == State.BattleOver){
            yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は力尽きてしまった", auto: false);
            BattleOver();
            yield break;
        }
    }

    private void Update(){
        switch(state){
            case State.Start:
                break;
            case State.ActionSelection:
                HandleActionSelection();
                break;
            case State.SecondActionSelection:
                HandleSecondActionSelection();
                break;
            case State.AttackSelection:
                HandleAttackSelection();
                break;
            case State.MagicSelection:
                HandleMagicSelection();
                break;
            case State.ItemSelection:
                HandleItemSelection();
                break;
            case State.BattleOver:
                break;
        }
    }

    void HandleActionSelection(){
        // 色の変更
        actionSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Return)){
            if(actionSelectionUI.SelectedIndex == 0){
                StateForSecondActionSelection();
            }else if(actionSelectionUI.SelectedIndex == 1){
                // 逃げる
                actionSelectionUI.Close();
                BattleOver();
            }
        }
    }

    void HandleSecondActionSelection(){
        // 色の変更
        secondActionSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            // 戻る
            secondActionSelectionUI.Close();
            StateForActionSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            if(secondActionSelectionUI.SelectedIndex == 0){
                StateForAttackSelection();
            }else if(secondActionSelectionUI.SelectedIndex == 1){
                StateForMagicSelection();
            }else if(secondActionSelectionUI.SelectedIndex == 2){
                StateForItemSelection();
            }
        }
    }

    void HandleAttackSelection(){
        // 色の変更
        attackSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            // 戻る
            attackSelectionUI.Close();
            StateForSecondActionSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            // 技を習得しているか判断
            if(playerUnit.Battler.Attacks.Count == 0){
                return;
            }

            Move playerMove = playerUnit.Battler.Attacks[attackSelectionUI.SelectedIndex]; // 選択した技の取得

            actionSelectionUI.Close();
            secondActionSelectionUI.Close();
            attackSelectionUI.Close();
            // 技の実行
            StartCoroutine(StateForMoveRunTurns(playerMove));
        }
    }

    void HandleMagicSelection(){
        // 色の変更
        magicSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            // 戻る
            magicSelectionUI.Close();
            StateForSecondActionSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            // 技を習得しているかどうか判断
            if(playerUnit.Battler.Magics.Count == 0){
                return;
            }

            Move playerMove = playerUnit.Battler.Magics[magicSelectionUI.SelectedIndex]; // 選択した技の取得

            actionSelectionUI.Close();
            secondActionSelectionUI.Close();
            magicSelectionUI.Close();
            // 技の実行
            StartCoroutine(StateForMoveRunTurns(playerMove));
        }
    }

    void HandleItemSelection(){
        // 色の変更
        itemSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            // 戻る
            itemSelectionUI.Close();
            StateForSecondActionSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            // アイテムを持っているかどうか判断
            if(playerUnit.Battler.Items.Count == 0){
                return;
            }

            Item playerItem = playerUnit.Battler.Items[itemSelectionUI.SelectedIndex]; // 選択したアイテムの取得

            if(playerItem.Base.Type == 0){
                // アイテムの削除
                playerUnit.Battler.Items.Remove(playerItem);
                // アイテムの再生成     
                itemSelectionUI.DeleteItemTexts();
                itemSelectionUI.Init(playerUnit.Battler.Items);
            }
            
            actionSelectionUI.Close();
            secondActionSelectionUI.Close();
            itemSelectionUI.Close();
            // 処理の実行
            StartCoroutine(StateForItemUseTurns(playerItem));
        }
    }
}
