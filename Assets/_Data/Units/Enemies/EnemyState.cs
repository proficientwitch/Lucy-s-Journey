﻿using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class EnemyState : GameMonoBehaviour
{
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl => enemyCtrl;

    public StateMachine<EnemyState> StateMachine { get; private set; }

    [Header("Chase state")]
    public GameObject posPlayer;
    public float detectPlayerRange = 10f;
    public float distanceToPlayer;
    public float delayChase = 0.5f;
    public float distanceToAttack = 1f;
    [Header("Combat state")]
    public float delayHit = 1f;
    public int maxCombo = 1;
    public int dmg = 1;
    [Header("Received dmg state")]
    public float delayWhenReceivedDmg = 0.5f;
    [Header("Dead state")]
    public float cdToDespawn = 3f;
    public int dropItemCnt = 1;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyCtrl();
    }

    protected virtual void LoadEnemyCtrl()
    {
        if (enemyCtrl != null) return;
        enemyCtrl = GetComponent<EnemyCtrl>();
        Debug.LogWarning($"Load{GetType().Name}Ctrl", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        posPlayer = GameObject.FindGameObjectWithTag("Player");
        StateMachine = new StateMachine<EnemyState>(this);

        InitializeEnemy();
        SubscribeEvents();

        //Start state
        StateMachine.ChangeState(GetStartState());
    }

    protected virtual void InitializeEnemy()
    {
        //set maxHp
        enemyCtrl.DmgReceiver.SetMaxHp(enemyCtrl.EnemiesSO.hpMax);
        enemyCtrl.DmgReceiver.Reborn();

        //set dmg
        dmg = enemyCtrl.EnemiesSO.dmg;
        enemyCtrl.DmgSender.SetDmg(dmg);
    }

    protected virtual void SubscribeEvents()
    {
        enemyCtrl.DmgReceiver.OnHurt += HandleHurt;
        enemyCtrl.DmgReceiver.OnDead += HandleDead;
    }

    protected virtual void UnsubscribeEvents()
    {
        enemyCtrl.DmgReceiver.OnHurt -= HandleHurt;
        enemyCtrl.DmgReceiver.OnDead -= HandleDead;
    }

    protected virtual void Update()
    {
        StateMachine.ExecuteState();
        if (!enemyCtrl.dead && !enemyCtrl.hit)
        {
            DetectPlayerInRange();
        }
    }

    protected override void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected virtual void HandleHurt(LayerMask layer)
    {
        if (layer == LayerMask.NameToLayer("Player"))
        {
            StateMachine.ChangeState(GetReceivedDmgState());
        }
    }

    protected virtual void HandleDead()
    {
        Debug.Log("Dead");
        StateMachine.ChangeState(GetDeadState());
    }

    protected virtual void DetectPlayerInRange()
    {
        enemyCtrl.detectPlayer = enemyCtrl.DetectPlayer.DetectPlayerForGround(detectPlayerRange);
        distanceToPlayer = Vector2.Distance(posPlayer.transform.position, transform.position);

        //if (enemyCtrl.detectPlayer && distanceToPlayer < distanceToAttack
        //    && !(StateMachine.currentState is CombatState<Enemy>))
        //{
        //    StateMachine.ChangeState(GetCombatState());
        //}
    }

    // Abstract methods for states
    public abstract State<EnemyState> GetStartState();
    public abstract State<EnemyState> GetReceivedDmgState();
    public abstract State<EnemyState> GetDeadState();
    public abstract State<EnemyState> GetCombatState();
}