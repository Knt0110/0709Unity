using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用

public class PlayerController : MonoBehaviour
{
    // インスペクターから移動速度を調整できるようにする
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // 1. キーボードの入力を取得（矢印キーやWASD）
        Vector2 inputVector = Vector2.zero;

        if (Keyboard.current != null)
        {
            // 押されているキーに応じて方向を決定
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputVector.y = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputVector.y = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputVector.x = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputVector.x = 1f;
        }

        // 斜め移動の時に移動速度が速くならないように正規化する
        inputVector = inputVector.normalized;

        // 2. 移動速度とフレームレート（Time.deltaTime）を考慮して実際に動かす
        transform.position += new Vector3(inputVector.x, inputVector.y, 0f) * moveSpeed * Time.deltaTime;
    }
}