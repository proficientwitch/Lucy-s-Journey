using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : PlayerAbstract
{
    [Header("Combo attack setting")]
    [SerializeField] private bool isComboActive;
    [SerializeField] private float maxComboTime = 1.5f;
    [SerializeField] private int comboIndex = 0;
    [SerializeField] private float comboTimer = 0;
    public bool canNextCombo = true;// check on animation

    [Header("Setting send dmg")]
    [SerializeField] protected int dmg;

    protected override void Start()
    {
        base.Start();
        dmg = playerCtrl.PlayerSO.dmg;
    }
    protected virtual void Update()
    {
        HandleComboAttack();
        UpdateComboTimer();
    }

    private void HandleComboAttack()
    {
        if (InputManager.Instance.Attack() && canNextCombo)
        {
            if (comboIndex > 2)
            {
                ResetCombo();
                return;
            }
            PerformAttack();
        }
    }

    private void PerformAttack()
    {

        // update combo state
        comboTimer = maxComboTime;
        isComboActive = true;

        // Start attack combo
        Attack();
        comboIndex++;

    }

    private void Attack()
    {
        switch (comboIndex)
        {
            case 0:
                playerCtrl.PlayerAnim.TriggerAttack(comboIndex);
                break;
            case 1:
                dmg += dmg;
                playerCtrl.PlayerAnim.TriggerAttack(comboIndex);
                
                break;
            case 2:
                dmg += dmg;
                playerCtrl.PlayerAnim.TriggerAttack(comboIndex);
                break;
        }
    }

    private void UpdateComboTimer()
    {
        if (isComboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    private void ResetCombo()
    {
        dmg = playerCtrl.PlayerSO.dmg;
        isComboActive = false;
        comboIndex = 0;
        comboTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<DmgReceiver>(out DmgReceiver component))
        {
            component.Deduct(dmg);
        }
    }

}
