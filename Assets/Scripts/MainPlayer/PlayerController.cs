using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// 音楽
public class PlayerController : MonoBehaviour{

    private SaveLoadManager saveLoadManager;
    private SaveData saveData;

    public static List<string>[] playerProgress = {
        new List<string>(),
        new List<string>(),
        new List<string>()
    };
    /*
    playerProgress[0] : Door
    playerProgress[1] : ItemBox
    playerProgress[2] : Character
    */

    private void Start(){
        battler.Init();

        saveLoadManager = GetComponent<SaveLoadManager>();
        LoadPlayerData();

        menuSystem.OnCloseMenu = MenuClose;
        menuSystem.OnSavePlayerData = SavePlayerData;
        menuSystem.OnLoadPlayerData = LoadPlayerData;
        MenuClose();
    }

    public void SavePlayerData(){
        UpdateNowInventory();
        saveData = new SaveData{
            playerPosition = transform.position,
            doorProgress = playerProgress[0],
            itemBoxProgress = playerProgress[1],
            characterProgress = playerProgress[2],
            level = battler.Level,
            hp = battler.HP,
            mp = battler.MP,
            hasExp = battler.HasExp,
            walkCount = battler.walkCount,
            inventory = battler.Inventory,
            map = battler.map
        };
        saveLoadManager.SaveGame(saveData);
    }

    public void LoadPlayerData(){
        saveData = saveLoadManager.LoadGame();
        // セーブデータが存在する場合は復元
        if(saveData != null){
            transform.position = saveData.playerPosition;
            playerProgress[0] = saveData.doorProgress;
            playerProgress[1] = saveData.itemBoxProgress;
            playerProgress[2] = saveData.characterProgress;
            battler.Level = saveData.level;
            battler.HasExp = saveData.hasExp;
            battler.walkCount = saveData.walkCount;
            battler.Inventory = saveData.inventory;
            battler.Init();
            battler.map = saveData.map;
            battler.HP = saveData.hp;
            battler.MP = saveData.mp;
            SceneManager.LoadScene(saveData.map);
        }
    }

    public void UpdateNowInventory(){
        battler.Inventory.Clear();
        foreach(Item item in battler.Items){
            battler.Inventory.Add(item.Base);
        }
    }
    
    static PlayerController Instance = null;

    public static PlayerController GetInstance(){
        if(Instance == null){
            Instance = FindObjectOfType<PlayerController>();
        }
        return Instance;
    }

    private void Awake(){
        if(Instance == null){
            Instance = this; // インスタンスを自身に設定
            DontDestroyOnLoad(this.gameObject); // シーン移動時にオブジェクトを破棄しない
        } else if(Instance != this){
            Destroy(gameObject); // すでに別のインスタンスが存在している場合、破棄する
        }
        animator = GetComponent<Animator>();
    }

    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask encountLayer;
    [SerializeField] LayerMask goToOtherAreaLayer;
    [SerializeField] LayerMask itemLayer;
    [SerializeField] LayerMask doorLayer;
    [SerializeField] LayerMask characterLayer;
    [SerializeField] MenuSystem menuSystem;
    [SerializeField] Dialog dialog;
    [SerializeField] Battler battler;
    public Battler Battler { get => battler; }
    public global::System.Int32 MoveSceneSoundType { get => moveSceneSoundType;}

    public UnityAction<Battler> OnEncounts; // Encountしたときに実行したい関数を登録
    Animator animator;
    Vector3 targetPos;

    bool isMoving = false;
    bool isWalkable = true;
    bool isMenuOpen;
    bool isDialogOpen;
    bool isTalking = false;
    Coroutine dialogCoroutine;
    int moveSceneSoundType = -1;
    private MusicController musicController;

    void Update(){
        if(isMoving == false && isMenuOpen == false && isTalking == false){
            if(isDialogOpen){
                if(Input.GetKeyDown(KeyCode.Return)){
                    DialogClose();
                    return;
                }else{
                    return;
                }
            }
            if(Input.GetKeyDown(KeyCode.X)){
                MenuOpen();
                return;
            }
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            // 斜め移動無効化
            if(y != 0){
                x = 0;
            }
            // アニメーション制御(前の入力保存)
            if(x != 0 || y != 0){
                animator.SetFloat("InputX", x);
                animator.SetFloat("InputY", y);
                targetPos = transform.position + new Vector3(x, y);
                StartCoroutine(Move(targetPos));
            }
            animator.SetBool("IsMoving", isMoving);
            CheckForItemBox(targetPos);
            CheckForOpenDoors(targetPos);
            StartCoroutine(CheckForCharacters(targetPos));
        }
    }

    IEnumerator Move(Vector3 targetPos){
        isMoving = true;
        IsWalkableUpdate(targetPos);
        if(isWalkable == false){
            isMoving = false;
            yield break;
        }
        // 現在とターゲットの場所が違うなら近づけ続ける
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
            // 近づける
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounts();
        CheckForGoToOtherArea();
    }

    // キャラクターを識別する関数
    IEnumerator CheckForCharacters(Vector3 targetPos){
        Collider2D characterArea = Physics2D.OverlapCircle(targetPos, 0.2f, characterLayer);
        if(characterArea && Input.GetKeyDown(KeyCode.Return)){
            if(isTalking) yield break;
            DialogOpen();
            isTalking = true;
            Character character = characterArea.GetComponent<Character>();
            foreach(string line in character.Lines){
                yield return dialog.TypeDialog(line.Replace("  ", "\n"));
            }
            isTalking = false;
            DialogClose();
            if(character.Type == Character.Types.Battler){
                Battler enemy = character.GetBattler();
                enemy.Base.isRunable = false;
                OnEncounts?.Invoke(enemy);
                playerProgress[2].Add(characterArea.gameObject.name);
                character.gameObject.SetActive(false);
            }
        }
    }

    // 今から特定の位置に移動できるか判定する関数
    void IsWalkableUpdate(Vector3 targetPos){
        // targetPosを中心に円形のRayを作る：SolodObjectsLayerにぶつかったらfalse
        isWalkable = !Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | itemLayer | doorLayer | characterLayer);
    }

    // 次のエリアに移動するか判断する
    void CheckForGoToOtherArea(){
        Collider2D otherArea = Physics2D.OverlapCircle(transform.position, 0.2f, goToOtherAreaLayer);
        if(otherArea){
            moveSceneSoundType = 0;
            string nextArea = otherArea.GetComponent<GoToOtherArea>().GetNextArea();
            SceneManager.LoadScene(nextArea);
            battler.map = nextArea;
        }
    }

    // アイテムボックスからアイテムを取得
    void CheckForItemBox(Vector3 targetPos){
        Collider2D Box = Physics2D.OverlapCircle(targetPos, 0.2f, itemLayer);
        if(Box && Input.GetKeyDown(KeyCode.Return)){
            Item item = Box.GetComponent<ItemBox>().GetItem();
            DialogOpen(); // ダイアログを表示
            dialogCoroutine = StartCoroutine(dialog.TypeDialog($"{battler.Base.Name}は{item.Base.Name}を手に入れた！"));
            battler.Items.Add(item);
            playerProgress[1].Add(Box.gameObject.name);
        }
    }

    // ドアを識別する関数
    void CheckForOpenDoors(Vector3 targetPos){
        Collider2D Door = Physics2D.OverlapCircle(targetPos, 0.2f, doorLayer);
        if(Door && Input.GetKeyDown(KeyCode.Return)){
            for(int i = battler.Items.Count - 1; i >= 0; i--){
                Item item = battler.Items[i];
                if(Door.GetComponent<Door>().isOpenDoor(item)){
                    DialogOpen();
                    musicController.StartSound(musicController.DoorOpenSound);
                    dialogCoroutine = StartCoroutine(dialog.TypeDialog($"{battler.Base.Name}は{item.Base.Name}を使った\nなんと、扉が開いた！"));
                    battler.Items.Remove(item);
                    playerProgress[0].Add(Door.gameObject.name);
                    break;
                }else if(i == 0){
                    DialogOpen();
                    dialogCoroutine = StartCoroutine(dialog.TypeDialog($"{battler.Base.Name}はこの扉の鍵を持っていないようだ…"));
                }
            }
        }
    }

    // 敵にあうか調べる
    void CheckForEncounts(){
        // 移動した地点に、敵がいるか判断する
        battler.walkCount += 1;
        Collider2D encount = Physics2D.OverlapCircle(transform.position, 0.2f, encountLayer);
        Collider2D otherArea = Physics2D.OverlapCircle(transform.position, 0.2f, goToOtherAreaLayer);
        if(!otherArea && encount && battler.walkCount > 10){
            if(Random.Range(0, 100) < 15){
                battler.walkCount = 0;
                Battler enemy = encount.GetComponent<EncountArea>().GetRandomBattler();
                enemy.Base.isRunable = true;
                OnEncounts?.Invoke(enemy); // もしOnEncountsに関数が登録されていれば実行
            }
        }
    }

    public void MenuOpen(){
        isMenuOpen = true;
        menuSystem.gameObject.SetActive(true);
        menuSystem.MenuStart(battler);
    }

    public void MenuClose(){
        isMenuOpen = false;
        menuSystem.gameObject.SetActive(false);
    }

    public void DialogOpen(){
        isDialogOpen = true;
        dialog.gameObject.SetActive(true);
    }

    public void DialogClose(){
        isDialogOpen = false;
        dialog.gameObject.SetActive(false);
        if(dialogCoroutine != null){
            StopCoroutine(dialogCoroutine);
        }
    }

    public void SetMusicController(MusicController audio){
        musicController = audio;
    }
}
