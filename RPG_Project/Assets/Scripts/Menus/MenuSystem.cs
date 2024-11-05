using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour{

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] MenuDialog menuDialog;
    [SerializeField] MenuSelectionUI menuSelectionUI;
    [SerializeField] StrengthMenuSelectionUI strengthMenuSelectionUI;
    [SerializeField] StrengthMenuStatusUI strengthMenuStatusUI;
    [SerializeField] AttackSelectionUI strengthMenuAttacksUI;
    [SerializeField] MagicSelectionUI strengthMenuMagicsUI;
    [SerializeField] MenuMagicSelectionUI menuMagicSelectionUI;
    [SerializeField] ItemSelectionUI menuItemSelectionUI;
    [SerializeField] CheckSaveUI checkSaveUI;
    [SerializeField] CheckLoadUI checkLoadUI;
    [SerializeField] CheckGoToTitleUI checkGoToTitleUI;

    enum State{
        Start,
        MenuSelection,
        StrengthMenuSelection,
        StrengthMenuStatus,
        StrengthMenuAttacks,
        StrengthMenuMagics,
        MenuMagicSelection,
        MoveRun,
        MenuItemSelection,
        ItemUse,
        CheckFoot,
        CheckSave,
        CheckLoad,
        CheckGoToTitle,
        End,
    }

    State state;
    bool isProcessing = false;

    public UnityAction OnCloseMenu;
    public UnityAction OnSavePlayerData;
    public UnityAction OnLoadPlayerData;

    public void MenuStart(Battler player){
        state = State.Start;

        playerUnit.Setup(player);
        menuSelectionUI.Init();
        strengthMenuSelectionUI.Init();
        strengthMenuStatusUI.Setup(player);
        strengthMenuAttacksUI.Init(player.Attacks);
        strengthMenuMagicsUI.Init(player.Magics);
        menuMagicSelectionUI.Init(player.Magics);
        menuItemSelectionUI.Init(player.Items);
        checkSaveUI.Init();
        checkLoadUI.Init();
        checkGoToTitleUI.Init();

        StateForMenuSelection();
    }

    void MenuEnd(){
        strengthMenuAttacksUI.DeleteMoveTexts();
        strengthMenuMagicsUI.DeleteMoveTexts();
        menuMagicSelectionUI.DeleteMoveTexts();
        menuItemSelectionUI.DeleteItemTexts();
        OnCloseMenu?.Invoke();
    }

    void StateForMenuSelection(){
        state = State.MenuSelection;
        menuDialog.TypeDialogNoDelay("行動を選択してください");
        menuSelectionUI.Open();
    }

    void StateForStrengthMenuSelection(){
        state = State.StrengthMenuSelection;
        menuDialog.TypeDialogNoDelay("行動を選択してください");
        strengthMenuSelectionUI.Open();
    }

    void StateForStrengthMenuStatus(){
        state = State.StrengthMenuStatus;
        menuDialog.TypeDialogNoDelay("ステータスを確認できます");
        strengthMenuStatusUI.Open();
    }

    void StateForStrengthMenuAttacks(){
        state = State.StrengthMenuAttacks;
        menuDialog.TypeDialogNoDelay("使える攻撃の一覧です");
        strengthMenuAttacksUI.Open();
    }

    void StateForStrengthMenuMagics(){
        state = State.StrengthMenuMagics;
        menuDialog.TypeDialogNoDelay("使える魔法の一覧です");
        strengthMenuMagicsUI.Open();
    }

    void StateForMenuMagicSelection(){
        state = State.MenuMagicSelection;
        menuDialog.TypeDialogNoDelay("使いたい魔法を選択してください");
        menuMagicSelectionUI.Open();
    }

    void StateForMenuItemSelection(){
        state = State.MenuItemSelection;
        menuDialog.TypeDialogNoDelay("使いたいアイテムを選択してください");
        menuItemSelectionUI.Open();
    }

    void StateForCheckSave(){
        state = State.CheckSave;
        menuDialog.TypeDialogNoDelay("ここまでの進行状況を保存してもいいですか？");
        checkSaveUI.Open();
    }

    void StateForCheckLoad(){
        state = State.CheckLoad;
        menuDialog.TypeDialogNoDelay("前回のセーブデータをロードしますか？");
        checkLoadUI.Open();
    }

    void StateForCheckGoToTitle(){
        state = State.CheckGoToTitle;
        menuDialog.TypeDialogNoDelay("タイトルに戻りますか？\n※セーブされていないデータは消えてしまいます");
        checkGoToTitleUI.Open();
    }

    IEnumerator StateForMoveRun(Move playerMove){
        state = State.MoveRun;
        yield return RunMove(playerMove, playerUnit);
        StateForMenuMagicSelection();
    }

    IEnumerator StateForItemUse(Item playerItem){
        state = State.ItemUse;
        yield return UseItem(playerItem, playerUnit);
        if(playerItem.Base.UseItemResultMoveScene()){
            MenuEnd();
            yield break;
        }
        StateForMenuItemSelection();
    }

    IEnumerator StateForCheckFoot(){
        state = State.CheckFoot;
        yield return CheckFoot();
        StateForMenuSelection();
    }

    IEnumerator RunMove(Move move, BattleUnit player){
        string resultText;
        resultText = move.Base.RunMoveResultInMenu(player);
        yield return menuDialog.TypeDialog(resultText);
        player.UpdateUI();
    }
    IEnumerator UseItem(Item item, BattleUnit player){
        string resultText;
        resultText = item.Base.UseItemResultInMenu(player);
        yield return menuDialog.TypeDialog(resultText);
        player.UpdateUI();
    }

    IEnumerator CheckFoot(){
        string resultText;
        yield return menuDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は足元を調べた");
        resultText = "しかし何も見つからなかった";
        yield return menuDialog.TypeDialog(resultText);
    }

    private void Update(){
        switch(state){
            case State.Start:
                break;
            case State.MenuSelection:
                HandleMenuSelection();
                break;
            case State.StrengthMenuSelection:
                HandleStrengthMenuSelection();
                break;
            case State.StrengthMenuStatus:
                HandleStrengthMenuStatus();
                break;
            case State.StrengthMenuAttacks:
                HandleStrengthMenuAttacks();
                break;
            case State.StrengthMenuMagics:
                HandleStrengthMenuMagics();
                break;
            case State.MenuMagicSelection:
                HandleMenuMagicSelection();
                break;
            case State.MenuItemSelection:
                HandleMenuItemSelection();
                break;
            case State.CheckFoot:
                break;
            case State.CheckSave:
                HandleCheckSave();
                break;
            case State.CheckLoad:
                HandleCheckLoad();
                break;
            case State.CheckGoToTitle:
                HandleCheckGoToTitle();
                break;
            case State.End:
                break;
        }
    }

    void HandleMenuSelection(){
        menuSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            MenuEnd();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            if(menuSelectionUI.SelectedIndex == 0){
                StateForStrengthMenuSelection();
            }else if(menuSelectionUI.SelectedIndex == 1){
                StateForMenuMagicSelection();
            }else if(menuSelectionUI.SelectedIndex == 2){
                StateForMenuItemSelection();
            }else if(menuSelectionUI.SelectedIndex == 3){
                StartCoroutine(StateForCheckFoot());
            }else if(menuSelectionUI.SelectedIndex == 4){
                StateForCheckSave();
            }else if(menuSelectionUI.SelectedIndex == 5){
                StateForCheckLoad();
            }else if(menuSelectionUI.SelectedIndex == 6){
                StateForCheckGoToTitle();
            }
        }
    }

    void HandleStrengthMenuSelection(){
        strengthMenuSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            strengthMenuSelectionUI.Close();
            StateForMenuSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            if(strengthMenuSelectionUI.SelectedIndex == 0){
                StateForStrengthMenuStatus();
            }else if(strengthMenuSelectionUI.SelectedIndex == 1){
                StateForStrengthMenuAttacks();
            }else if(strengthMenuSelectionUI.SelectedIndex == 2){
                StateForStrengthMenuMagics();
            }
        }
    }

    void HandleStrengthMenuStatus(){
        if(Input.GetKeyDown(KeyCode.Space)){
            strengthMenuStatusUI.Close();
            StateForStrengthMenuSelection();
        }
    }

    void HandleStrengthMenuAttacks(){
        strengthMenuAttacksUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            strengthMenuAttacksUI.Close();
            StateForStrengthMenuSelection();
            return;
        }

        if(playerUnit.Battler.Attacks.Count == 0) return;

        menuDialog.TypeDialogNoDelay(playerUnit.Battler.Attacks[strengthMenuMagicsUI.SelectedIndex].Base.Message.Replace("  ", "\n"));
    }

    void HandleStrengthMenuMagics(){
        strengthMenuMagicsUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            strengthMenuMagicsUI.Close();
            StateForStrengthMenuSelection();
            return;
        }

        if(playerUnit.Battler.Magics.Count == 0) return;

        menuDialog.TypeDialogNoDelay(playerUnit.Battler.Magics[strengthMenuMagicsUI.SelectedIndex].Base.Message.Replace("  ", "\n"));
    }

    void HandleMenuMagicSelection(){
        menuMagicSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            menuMagicSelectionUI.Close();
            StateForMenuSelection();
            return;
        }

        if(menuMagicSelectionUI.useableMoves.Count == 0) return;

        int nowIndex = menuMagicSelectionUI.SelectedIndex;
        menuDialog.TypeDialogNoDelay(menuMagicSelectionUI.useableMoves[nowIndex].Base.Message.Replace("  ", "\n"));
        
        if(Input.GetKeyDown(KeyCode.Return)){
            Move playerMove = menuMagicSelectionUI.useableMoves[nowIndex];
            menuMagicSelectionUI.Close();
            StartCoroutine(StateForMoveRun(playerMove));
        }
    }

    void HandleMenuItemSelection(){
        menuItemSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            menuItemSelectionUI.Close();
            StateForMenuSelection();
            return;
        }

        if(playerUnit.Battler.Items.Count == 0) return;

        int nowIndex = menuItemSelectionUI.SelectedIndex;
        menuDialog.TypeDialogNoDelay(playerUnit.Battler.Items[nowIndex].Base.Message.Replace("  ", "\n"));

        if(Input.GetKeyDown(KeyCode.Return)){
            Item playerItem = playerUnit.Battler.Items[nowIndex];
            if(playerItem.Base.IsSave) OnSavePlayerData?.Invoke();
            if(playerItem.Base.Type == ItemBase.Types.Heal){
                // アイテムの削除
                playerUnit.Battler.Items.Remove(playerItem);
                // アイテムの再生成
                menuItemSelectionUI.DeleteItemTexts();
                menuItemSelectionUI.Init(playerUnit.Battler.Items);
            }else if(playerItem.Base.Type == ItemBase.Types.MoveScene){
                playerUnit.Battler.Items.Remove(playerItem);
                menuItemSelectionUI.DeleteItemTexts();
                menuItemSelectionUI.Init(playerUnit.Battler.Items);
            }
            menuItemSelectionUI.Close();
            // 処理の実行
            StartCoroutine(StateForItemUse(playerItem));
        }
    }

    void HandleCheckSave(){
        if(isProcessing) return;

        checkSaveUI.HandleUpdate();
        
        if(Input.GetKeyDown(KeyCode.Space)){
            checkSaveUI.Close();
            StateForMenuSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            if(checkSaveUI.SelectedIndex == 0){
                OnSavePlayerData?.Invoke();
                StartCoroutine(HandleSaveMessage());
            }else if(checkSaveUI.SelectedIndex == 1){
                checkSaveUI.Close();
                StateForMenuSelection();
            }
        }
    }

    IEnumerator HandleSaveMessage(){
        isProcessing = true;
        yield return StartCoroutine(menuDialog.TypeDialog("正常にセーブが完了しました"));
        checkSaveUI.Close();
        StateForMenuSelection();
        isProcessing = false;
    }

    void HandleCheckLoad(){
        checkLoadUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            checkLoadUI.Close();
            StateForMenuSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            if(checkLoadUI.SelectedIndex == 0){
                checkLoadUI.Close();
                MenuEnd();
                OnLoadPlayerData?.Invoke();
            }else if(checkLoadUI.SelectedIndex == 1){
                checkLoadUI.Close();
                StateForMenuSelection();
            }
        }
    }

    void HandleCheckGoToTitle(){
        checkGoToTitleUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            checkGoToTitleUI.Close();
            StateForMenuSelection();
        }else if(Input.GetKeyDown(KeyCode.Return)){
            if(checkGoToTitleUI.SelectedIndex == 0){
                checkGoToTitleUI.Close();
                MenuEnd();
                SceneManager.LoadScene("Title");
            }else if(checkGoToTitleUI.SelectedIndex == 1){
                checkGoToTitleUI.Close();
                StateForMenuSelection();
            }
        }
    }
}