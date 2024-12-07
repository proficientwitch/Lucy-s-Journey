using System;
using System.Collections;
using UnityEngine;

public class BanditDmgReceiver : DmgReceiver
{
    [SerializeField] Animator anim;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] protected float cdToDespawn = 3f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAnim();
        LoadCollider();
    }

    private void LoadCollider()
    {
        if (boxCollider != null) return;
        boxCollider = transform.GetComponent<BoxCollider2D>();
        Debug.LogWarning(transform.name + ": LoadCollider", gameObject);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        this.maxHp = 8;
    }

    private void LoadAnim()
    {
        if (this.anim != null) return;
        this.anim = transform.parent.GetComponent<Animator>();
        Debug.LogWarning(transform.name + ": LoadAnim", gameObject);
    }

    protected override void OnDead()
    {
        boxCollider.enabled = false;
        anim.SetTrigger(AnimStrings.isDead);
        StartCoroutine(DespawnAfterTime());
    }

    private IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(cdToDespawn);
        transform.parent.gameObject.SetActive(false);
        boxCollider.enabled = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerCombat>(out _)) anim.SetTrigger(AnimStrings.isHit);
        }
    }

}