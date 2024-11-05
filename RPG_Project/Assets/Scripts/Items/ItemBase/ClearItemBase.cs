using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ClearItemBase : ItemBase
{
    [SerializeField] string nextArea;

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\nしかし何も起こらなかった…";
    }
    public override string UseItemResultInMenu(BattleUnit player){
        
        return $"{player.Battler.Base.Name}は「{Name}」を使った\nなんと、ゲームをクリアした！？";
    }

    public override bool UseItemResultMoveScene(){
        SceneManager.LoadScene(nextArea);
        return true;
    }
}