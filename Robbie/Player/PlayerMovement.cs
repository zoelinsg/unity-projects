using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;              //調用重置
public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移動參數")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 1f;
    [Header("跳躍參數")]
    public float jumpForce = 10.0f;             //跳躍的力
    public float jumpHoldForce = 2.2f;          //跳躍額外加乘的力
    public float jumpHoldDuration = 0.1f;       //跳躍時間
    public float crouchJumpBoost = 2.5f;        //下蹲時跳躍額外加乘
    public float hangingJumpForce = 5f;        //懸掛跳躍的力

    float jumpTime;
    [Header("狀態")]
    public bool isCrouch, isOnGround, isJump, isHeadBlocked, isHanging;                       //下蹲的狀態
                //正在下蹲,正在站在地上,正在跳躍,頭上射線判斷,正在懸掛時
    [Header("環境檢測")]
    public float footOffset = 0.4f;             //Collider的一半
    public float headClearance = 0.5f;          //檢測頭頂之間距離
    public float groundDistance = 0.2f;         //檢測地面之間距離
    float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;
    public LayerMask groundLayer;               //判斷圖層
    public float xVelocity;

    //按鍵設置
    bool jumpPressed;                   //單次跳躍
    bool jumpHeld;                      //長按跳躍
    bool crouchPressed;                 //單次下蹲
    bool crouchHeld;                    //長按下蹲

    //碰撞體尺寸
    Vector2 colliderStandSize;          //站立時的尺寸
    Vector2 colliderStandOffset;        //站立時的座標
    Vector2 colliderCrouchSize;         //下蹲時的尺寸
    Vector2 colliderCrouchOffset;       //下蹲時的座標

    //死亡機制
    int trapLayer;                      //Trap圖層編號
    public GameObject deathVFXPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        trapLayer = LayerMask.NameToLayer("Traps");     //取得Trap圖層編號

        playerHeight = coll.size.y;             //遊戲角色高度
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);                //下蹲狀態尺寸
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);          //下蹲狀態座標
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameOver())           //當gameover時 跳出函數
        {
            return;
        }
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchPressed = Input.GetButton("Crouch");
        crouchHeld = Input.GetButton("Crouch");
    }
    private void FixedUpdate()
    {
        if (GameManager.GameOver())           //當gameover時 跳出函數
        {
            return;
        }
        PlayerIf();
    }
    private void OnTriggerEnter2D(Collider2D collision)         //角色死亡機制
    {
        if (collision.gameObject.layer == trapLayer)
        {
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();                      //死亡音效
            GameManager.PlayerDied();                           //調用GameManager內的PlayerDied
        }
    }
    void PlayerIf()             //角色判斷
    {
        TotalRays();            //調用射線
        if (isHanging)                   //當懸掛時型態轉換
        {
           
            if (isOnGround)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
                return;
            }
            if (jumpPressed)            //按下跳躍->靜態轉動態->懸掛狀態false
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            if (crouchPressed)          //按下下蹲->靜態轉動態
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }
        //下蹲狀態判斷
        if (crouchHeld && !isCrouch && isOnGround)                         //當按下蹲 且 狀態不是下蹲 且 在地板上->進行下蹲
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouch && !isHeadBlocked)       //當沒按下蹲 且 狀態為下蹲 且 頭上射線無東西->狀態恢復
        {
            Standup();
        }
        //跳躍型態
        if (jumpPressed && isOnGround && !isJump && !isHeadBlocked)               //當按下跳躍 且 在地板上 且 不是跳躍狀態 且 頭上射線無東西
        {
            Jump();
        }
        else if (isJump)
        {
            if (jumpHeld)
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            if (jumpTime < Time.time)
                isJump = false;
        }
        GroundMovement();
    }
    void GroundMovement()                       //在地上移動+左右轉向
    {
        xVelocity = Input.GetAxis("Horizontal");     //-1f 1f
        if (isHanging)
        {
            return;
        }
        //下蹲影響速度
        if (isCrouch)
        {
            rb.velocity = new Vector2(xVelocity * crouchSpeedDivisor, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        }
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1, 1,1);
        }
        if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1,1);
        }
    }
    void Jump()
    {
        isOnGround = false;
        isJump = true;                                      //不再地面->狀態跳躍
        AudioManager.PlayJumpAudio();                       //跳躍音效
        jumpTime = Time.time + jumpHoldDuration;            //計算跳躍時間
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }       //跳躍
    void Crouch()       //下蹲
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    void Standup()      //站立
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    void TotalRays()    //所有射線函數
    {
        RaycastHit2D leftCheck, rightCheck, headCheck, blockedCheck, wallCheck, ledgeCheck;
        float direction;
        leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);          //左腳
        rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);          //右腳
        headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);             //頭頂
        //懸掛
        direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);
        blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);      //角色頭頂
        wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);            //角色眼睛
        ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);  //中間直線
        if (!isOnGround && rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position; ;
            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
        //左右腳射線判斷
        if (leftCheck || rightCheck) //當站在GroundLayer圖層
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        if (headCheck)//頭頂射線判斷
        {
            isHeadBlocked = true;
        }
        else
        {
            isHeadBlocked = false;
        }
    }
    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction,float length,LayerMask layer)      //射線函數
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction * length,color);
        return hit;
    }
}
