using System;
using System.Collections;
using UnityEngine;

public class PlayerDash : PlayerAbstract
{
    [SerializeField] protected float dashSpeed = 20;
    [SerializeField] protected float dashTime = 0.2f;
    [SerializeField] protected float dashCooldown = 0.3f;

    protected float gravity;
    protected bool canDash = true;

    protected virtual void Update()
    {
        this.StartDash();
    }

    protected override void Awake() 
        => this.gravity = playerCtrl.Rigidbody2D.gravityScale;

    protected void StartDash()
    {
        if(Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        StartDashState();
        yield return new WaitForSeconds(dashTime);
        EndDashState();
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void EndDashState()
    {
        playerCtrl.Rigidbody2D.gravityScale = gravity;
        PlayerCtrl.dashing = false;
    }

    private void StartDashState()
    {
        canDash = false;
        PlayerCtrl.dashing = true;
        playerCtrl.Rigidbody2D.gravityScale = 0;
        playerCtrl.Rigidbody2D.linearVelocity = new Vector2(transform.parent.localScale.x * dashSpeed, 0);
    }
}
