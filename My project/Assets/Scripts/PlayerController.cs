using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用

public class PlayerController : MonoBehaviour
{
    [Header("移動・ジャンプの設定")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f; // ジャンプする力

    [Header("地面判定の設定")]
    [SerializeField] private Transform groundCheck; // 足元のオブジェクト(GroundCheck)
    [SerializeField] private Vector2 checkSize = new Vector2(0.5f, 0.1f); // 判定エリアのサイズ
    [SerializeField] private LayerMask groundLayer; // 地面レイヤー(Ground)

    private Rigidbody2D rb;
    private bool isGrounded; // 地面にいるかどうかのフラグ
    private float horizontalInput; // 左右の入力値

    void Start()
    {
        // 物理演算コンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. 毎フレーム、地面に接しているかをチェック
        isGrounded = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        // 2. キーボードの入力を取得
        GetInput();

        // 3. ジャンプの入力検知（地面にいる時だけジャンプできる）
        if (isGrounded && Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame ||
                Keyboard.current.wKey.wasPressedThisFrame ||
                Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // 物理演算（移動）は FixedUpdate で行うと挙動が安定します
        Move();
    }

    private void GetInput()
    {
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            // 左右の入力だけを取得（A/D または 左右矢印キー）
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1f;
        }
    }

    private void Move()
    {
        // 左右はキー入力、上下（Y軸）は Rigidbody の落下速度（重力）をそのまま維持する
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        // 上方向へ力を加える
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // エディタの画面に判定エリアを赤い箱で表示する（デバッグ用）
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }
    }
}