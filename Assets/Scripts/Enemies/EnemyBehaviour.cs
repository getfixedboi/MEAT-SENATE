using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.VFX;
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]

[DisallowMultipleComponent]
public abstract class EnemyBehaviour : MonoBehaviour
{
    #region stats
    [Min(1)] protected float maxHP;
    [NonSerialized] protected float currentHP;
    [SerializeField] protected List<float> movSpeedList;
    [SerializeField] protected List<float> damageList;
    [SerializeField] protected List<float> attackSpeedList;
    [SerializeField] protected List<float> cooldownList;
    [SerializeField] protected List<float> rangeList;
    [SerializeField] protected List<float> clipList;
    [NonSerialized] protected bool isDead;
    [NonSerialized] protected float timer;
    #endregion
    #region components
    [SerializeField] protected List<VisualEffect> vfxList;
    [NonSerialized] protected AudioSource source;
    [NonSerialized] protected Animator animator;
    [NonSerialized] protected NavMeshAgent agent;
    #endregion
    #region other
    [NonSerialized] protected static Transform target;
    [NonSerialized] protected static float distanceToPlayer;
    #endregion
    protected virtual void Awake()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag("Player").transform;
        currentHP = maxHP;
        isDead = false;
        agent.speed=movSpeedList[0];
    }
    protected virtual void Update()
    {
        distanceToPlayer = Vector3.Distance(gameObject.transform.position,target.transform.position);
    }
    public virtual IEnumerator C_TakeDamage(float receivedDamage)
    {
        if (receivedDamage <= 0)
        {
            throw new ArgumentException("damage cannot be null");
        }
        else
        {
            yield return null;
            currentHP -= receivedDamage;
            if (currentHP <= 0)
            {
                StartCoroutine(C_Death());
            }
        }
    }
    protected virtual IEnumerator C_Death()
    {
        yield return null;
        Debug.Log("ded");
        Destroy(gameObject);
    }
}
