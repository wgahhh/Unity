using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO SceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public PlayerInputControl inputControl;
    public Vector2 inPutDirection;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private PlayerAnimation playerAnimation;
    private PhysicsCheck physicsCheck;
    private Character character;

    [Header("基本参数")]
    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideDistance;
    public float slideSpeed;
    public int slidePowerCost;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;//是否攻击
    public bool wallJump;
    public bool isSlide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();
        originalOffset = coll.offset;
        originalSize = coll.size;

        inputControl = new PlayerInputControl();
        //跳跃
        inputControl.GamePlay.Jump.started += Jump;

        #region 强制走路
        runSpeed = speed;
        inputControl.GamePlay.WalkButton.performed += ctx => {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.GamePlay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround) speed = runSpeed;
        };
        #endregion

        //攻击
        inputControl.GamePlay.Attack.started += PlayerAttack;

        //滑铲
        inputControl.GamePlay.Slide.started += Slide;

        inputControl.Enable();
    }

    private void OnEnable()
    {
        SceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        SceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }


    private void Update()
    {
        inPutDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
        {
            Move();
        }
    }

    //场景加载过程停止控制
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable();
    }

    //读取游戏进度
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();
    }

    public void Move()
    {
        //人物移动
        if (!isCrouch&& !wallJump)
        {
            rb.velocity = new Vector2(inPutDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
        
        int faceDir = (int)transform.localScale.x;
        if (inPutDirection.x > 0) faceDir = 1;
        if (inPutDirection.x < 0) faceDir = -1;
        //转向
        transform.localScale = new Vector3(faceDir,1,1);
        //下蹲
        isCrouch = inPutDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //修改碰撞体的大小
            coll.offset = new Vector2(-0.05f,0.85f);
            coll.size = new Vector2(0.7f,1.7f);
        }
        else
        {
            //还原之前的碰撞体参数
            coll.offset = originalOffset;
            coll.size = originalSize;
        }
    }

    private void Jump(InputAction.CallbackContext obj) 
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
            //打断滑铲 协程
            isSlide = false;
            StopAllCoroutines();
        }
        else if (physicsCheck.onWall && !wallJump)//蹬墙跳
        {
            rb.AddForce(new Vector2(-inPutDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
        
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    private void Slide(InputAction.CallbackContext obj)
    {
        if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost)
        {
            isSlide = true;
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);

            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if (!physicsCheck.isGround)
                break;
            //滑铲过程中撞墙
            if(physicsCheck .touchLeftWall && transform .localScale.x<0f || physicsCheck.touchRightWall && transform.localScale.x > 0f)
            {
                isSlide = false;
                break;
            }
            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        } while (MathF.Abs(target.x-transform.position.x)>0.1f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

    }


    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x-attacker.position.x),0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

        if (physicsCheck.onWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        if (wallJump && rb.velocity.y<0f)
        {
            wallJump = false;
        }
    }
}
