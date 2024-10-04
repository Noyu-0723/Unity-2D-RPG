using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuSystem : MonoBehaviour
{
    static MenuSystem Instance = null;

    public static MenuSystem GetInstance(){
        if(Instance == null){
            Instance = FindObjectOfType<MenuSystem>();
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

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] MenuDialog menuDialog;
    [SerializeField] MenuSelectionUI menuSelectionUI;
    [SerializeField] StrengthMenuSelectionUI strengthMenuSelectionUI;
    [SerializeField] StrengthMenuStatusUI strengthMenuStatusUI;
    [SerializeField] AttackSelectionUI strengthMenuAttacksUI;
    [SerializeField] MagicSelectionUI strengthMenuMagicsUI;
    [SerializeField] MenuMagicSelectionUI menuMagicSelectionUI;
    [SerializeField] ItemSelectionUI menuItemSelectionUI;

    enum State{
        Start,
        MenuSelection,
        StrengthMenuSelection,
        StrengthMenuStatus,
        StrengthMenuAttacks,
        StrengthMenuMagics,
        MenuMagicSelection,
        MenuItemSelection,
        End,
    }

    State state;

    public UnityAction OnCloseMenu;

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
        menuSelectionUI.Open();
    }

    void StateForStrengthMenuSelection(){
        state = State.StrengthMenuSelection;
        strengthMenuSelectionUI.Open();
    }

    void StateForStrengthMenuStatus(){
        state = State.StrengthMenuStatus;
        strengthMenuStatusUI.Open();
    }

    void StateForStrengthMenuAttacks(){
        state = State.StrengthMenuAttacks;
        strengthMenuAttacksUI.Open();
    }

    void StateForStrengthMenuMagics(){
        state = State.StrengthMenuMagics;
        strengthMenuMagicsUI.Open();
    }

    void StateForMenuMagicSelection(){
        state = State.MenuMagicSelection;
        menuMagicSelectionUI.Open();
    }

    void StateForMenuItemSelection(){
        state = State.MenuItemSelection;
        menuItemSelectionUI.Open();
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

            }else if(menuSelectionUI.SelectedIndex == 4){

            }else if(menuSelectionUI.SelectedIndex == 5){

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
        }
    }

    void HandleStrengthMenuMagics(){
        strengthMenuMagicsUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            strengthMenuMagicsUI.Close();
            StateForStrengthMenuSelection();
        }
    }

    void HandleMenuMagicSelection(){
        menuMagicSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            menuMagicSelectionUI.Close();
            StateForMenuSelection();
        }
    }

    void HandleMenuItemSelection(){
        menuItemSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space)){
            menuItemSelectionUI.Close();
            StateForMenuSelection();
        }
    }
}