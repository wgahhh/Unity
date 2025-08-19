using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    public Transform playerTrans;
    private Animator anim;
    public GameObject signSprite;
    private IInteractable targetItem;

    private bool canPress;

    private void Awake()
    {
        //anim = GetComponentInChildren<Animator>();
        anim = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }
    private void OnDisable()
    {
        canPress = false;
    }


    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled=canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress) 
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }

    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange==InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("SignKeyBoard");
                    break;
                case DualShockGamepad:
                    anim.Play("SignPS");
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
            canPress = false;

    }
}
