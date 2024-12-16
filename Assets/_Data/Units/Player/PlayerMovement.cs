using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerAbstract
{
    [Header("Moving horizontal")]
    [SerializeField] protected float movingSpeed = 7f;

    protected virtual void Update()
    {
        if (playerCtrl.PlayerState.Dashing) return;
        this.Moving();
    }

    protected virtual void Moving()
    {
        float move = InputManager.Instance.Move();
        playerCtrl.PlayerState.Moving = move;
        //Moving
        playerCtrl.Rigidbody2D.linearVelocityX = movingSpeed * move;
        //Flip
        if (move < 0) transform.localScale = new Vector3(-1f, 1, 1);
        if (move > 0) transform.localScale = new Vector3(1f, 1, 1);
    }
}
