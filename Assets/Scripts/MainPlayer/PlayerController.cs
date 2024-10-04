using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{
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

    [SerializeField] Battler battler;
    
    public UnityAction<Battler> OnEncounts; // Encountしたときに実行したい関数を登録
    public UnityAction OnOpenMenu;
    Animator animator;
    bool isMoving = false;

    public Battler Battler { get => battler; }

    private void Start(){
        battler.Init();
    }

    void Update(){
        if(isMoving == false){
            if(Input.GetKeyDown(KeyCode.X)){
                OnOpenMenu?.Invoke();
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
                StartCoroutine(Move(new Vector2(x, y)));
            }
        }
        animator.SetBool("IsMoving", isMoving);
    }

    IEnumerator Move(Vector3 direction){
        isMoving = true;
        Vector3 targetPos = transform.position + direction;
        if(IsWalkable(targetPos) == false){
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

    // 敵にあうか調べる
    void CheckForEncounts(){
        // 移動した地点に、敵がいるか判断する
        Collider2D encount = Physics2D.OverlapCircle(transform.position, 0.2f, encountLayer);
        if(encount){
            if(Random.Range(0, 100) < 10){
                Battler enemy = encount.GetComponent<EncountArea>().GetRandomBattler();
                OnEncounts?.Invoke(enemy); // もしOnEncountsに関数が登録されていれば実行
            }
        }
    }

    // 今から特定の位置に移動できるか判定する関数
    bool IsWalkable(Vector3 targetPos){
        // targetPosを中心に円形のRayを作る：SolodObjectsLayerにぶつかったらfalse
        return !Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer);
    }

    // 次のエリアに移動するか判断する
    void CheckForGoToOtherArea(){
        Collider2D OtherArea = Physics2D.OverlapCircle(transform.position, 0.2f, goToOtherAreaLayer);
        if(OtherArea){
            string nextArea = OtherArea.GetComponent<GoToOtherArea>().GetNextArea();
            SceneManager.LoadScene(nextArea);
        }
    }

    void CheckForItemArea(){
        Collider2D ItemArea = Physics2D.OverlapCircle(transform.position, 0.2f, itemLayer);
        if(ItemArea){

        }
    }
}
