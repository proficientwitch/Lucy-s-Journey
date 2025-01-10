using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DmgReceiver : GameMonoBehaviour
{
    public event Action<LayerMask> OnHurt; //event collider
    public event Action OnDead;
    
    [SerializeField] Collider2D _collider;
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHp = 10;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCollider();
    }

    private void LoadCollider()
    {
        if (_collider != null) return;
        _collider = transform.GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": LoadCollider", gameObject);
    }

    protected virtual void Dead()
    {
        _collider.enabled = false;
        OnDead?.Invoke();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        this.Reborn();
    }

    public virtual void Reborn()
    {
        hp = maxHp;
    }

    public virtual void Add(int add)
    {
        if (CheckDead()) return;
        hp += add;
        if (hp > maxHp) hp = maxHp;
    }

    public virtual void Deduct(int deduct)
    {
        if (CheckDead()) return;
        Debug.Log("Deduct: " + deduct);
        hp -= deduct;
        if (hp < deduct) hp = 0;
        this.IsDead();
    }
    
    protected virtual void IsDead()
    {
        if (!this.CheckDead()) return;
        this.Dead();
    }

    public virtual void SetMaxHp(int maxHp)
    {
        this.maxHp = maxHp;
    }
    public virtual int GetMaxHp()
    {
        return this.maxHp;
    }

    public virtual int GetHp()
    {
        return this.hp;
    }

    protected virtual bool CheckDead()
    {
        return hp <= 0;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        OnHurt?.Invoke(collision.gameObject.layer);//detect collider
    }
}
