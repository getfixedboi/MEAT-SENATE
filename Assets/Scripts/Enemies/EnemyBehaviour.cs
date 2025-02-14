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
    [SerializeField][Min(1)] protected float maxHP;
    protected float currentHP;
    [SerializeField] protected List<float> movSpeedList;
    [SerializeField] protected List<float> damageList;
    [SerializeField] protected List<float> attackSpeedList;
    [SerializeField] protected List<float> cooldownList;
    [SerializeField] protected List<float> rangeList;
    [SerializeField] protected List<float> clipList;
    protected bool isDead = false;
    [HideInInspector] public bool InAura = false;
    protected float timer;
    protected float totalCooldownTimer;
    public int SpawnCost;
    #endregion

    #region components
    [SerializeField] protected List<VisualEffect> vfxList;
    protected AudioSource source;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    #endregion

    #region other
    protected Transform target;
    [HideInInspector] public float DistanceToPlayer;

    private GameObject _meatPiece;
    private int _minRangePieceCount = 2;
    private int _maxRangePieceCount = 4;

    private GameObject _dmgCanv;
    private float _dmgCanvLifetime = .75f;
    private float _ydmgCanvasOffset = 4.2f;
    private bool _isRightSide = false;
    private Material _onDamageMaterial;
    private Material _defaultMateral;
    #endregion

    protected virtual void Awake()
    {
        _meatPiece = Resources.Load<GameObject>("Prefabs/meatPiece");
        _dmgCanv = Resources.Load<GameObject>("Prefabs/damageCanvas");

        _onDamageMaterial = Resources.Load<Material>("Materials/red");
        _defaultMateral = GetComponent<Renderer>().material;

        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        target = GameObject.FindWithTag("Player").transform;
        currentHP = maxHP;
        agent.speed = movSpeedList[0];
        totalCooldownTimer = 0f;
    }

    protected virtual void Update()
    {
        DistanceToPlayer = Vector3.Distance(this.gameObject.transform.position, target.transform.position);
        totalCooldownTimer -= Time.deltaTime;
        timer += Time.deltaTime;
    }

    public virtual void TakeDamage(float receivedDamage)
    {
        if (isDead)
        {
            return;
        }
        if (receivedDamage <= 0)
        {
            throw new ArgumentException("damage cannot be null");
        }
        else
        {
            currentHP -= receivedDamage;
            SpawnDamageCanvas(receivedDamage);
            StartCoroutine(ChangeMaterialOnDamage());

            if (currentHP <= 0)
            {
                isDead = true;
                LevelManager.Instance.AmountOfKills++;
                Death();
            }
        }
    }

    private IEnumerator ChangeMaterialOnDamage()
    {
        SetMaterial(_onDamageMaterial);
        yield return new WaitForSeconds(0.1f);
        SetMaterial(_defaultMateral);
    }

    private void SetMaterial(Material material)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    protected virtual void Death()
    {
        SpawnMeatPieces(UnityEngine.Random.Range(_minRangePieceCount, _maxRangePieceCount + 1), 2f);
    }

    private void SpawnMeatPieces(int piecesCount, float spawnRadius)
    {
        for (int i = 0; i < piecesCount; i++)
        {
            Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
            randomPos.y = transform.position.y; // Оставляем ту же высоту, чтобы не улетало вверх или вниз

            GameObject meatPieceClone = Instantiate(_meatPiece, randomPos, Quaternion.identity);

            Rigidbody rb = meatPieceClone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(
                    UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(0.5f, 1.5f),
                    UnityEngine.Random.Range(-1f, 1f)
                );
                rb.AddForce(randomForce * 4f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }

    private void SpawnDamageCanvas(float damage)
    {
        GameObject canvas = GameObject.Instantiate(_dmgCanv);
        Destroy(canvas, _dmgCanvLifetime);

        DamageText dmgText = canvas.GetComponentInChildren<DamageText>();
        dmgText.Duration = _dmgCanvLifetime;
        dmgText.Text = damage.ToString();

        _isRightSide = !_isRightSide;
        dmgText.IsRightSide = _isRightSide;

        canvas.transform.localPosition = new Vector3(transform.position.x, transform.position.y + _ydmgCanvasOffset, transform.position.z);
        //canvas.transform.position = new Vector3(transform.position.x, transform.position.y + _ydmgCanvasOffset, transform.position.z);
    }
}
