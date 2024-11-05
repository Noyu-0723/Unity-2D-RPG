using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private PlayerController playerController;
    private Transform player;  // 主人公のTransform
    public Vector2 minPosition;  // カメラの最小位置（背景の左端、下端）
    public Vector2 maxPosition;  // カメラの最大位置（背景の右端、上端）

    public float smoothSpeed = 0.125f;  // カメラの追従速度
    public Vector3 offset;  // カメラのオフセット

    void Start(){
        if (playerController == null) {
            playerController = PlayerController.GetInstance();
            if (playerController == null) {
                Debug.LogError("PlayerController instance is not found. Make sure PlayerController is present in the scene.");
                return;
            }
        }
        player = playerController.transform;
    }

    void LateUpdate(){
        // プレイヤーの位置にカメラを追従させる
        Vector3 desiredPosition = player.position + offset;

        // カメラの位置を制約する
        float clampedX = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);

        // カメラのスムーズな移動を実現
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, transform.position.z), smoothSpeed);
        
        // カメラの位置を更新
        transform.position = smoothedPosition;
    }
}