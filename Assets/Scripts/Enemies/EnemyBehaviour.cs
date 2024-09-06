using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.VFX;
using Unity.VisualScripting.Antlr3.Runtime;
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
    protected bool isDead;
    protected float timer;
    protected float totalCooldownTimer;
    #endregion
    #region components
    [SerializeField] protected List<VisualEffect> vfxList;
    protected AudioSource source;
    protected Animator animator;
    protected NavMeshAgent agent;
    #endregion
    #region other
    protected static Transform target;
    protected static float distanceToPlayer;

    private GameObject _meatPiece;
    private int _minRangePieceCount = 2;
    private int _maxRangePieceCount = 4;

    private GameObject _dmgCanv;
    private float _dmgCanvLifetime = .75f;
    private float _ydmgCanvasOffset = 4.8f;
    private bool _isRightSide=false;

    #endregion
    protected virtual void Awake()
    {
        _meatPiece = Resources.Load<GameObject>("Prefabs/meatPiece");
        _dmgCanv = Resources.Load<GameObject>("Prefabs/damageCanvas");

        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag("Player").transform;
        currentHP = maxHP;
        isDead = false;
        agent.speed = movSpeedList[0];
        totalCooldownTimer = 0f;
    }
    protected virtual void Update()
    {
        distanceToPlayer = Vector3.Distance(gameObject.transform.position, target.transform.position);
        totalCooldownTimer -= Time.deltaTime;
        timer += Time.deltaTime;
    }
    public virtual void TakeDamage(float receivedDamage)
    {
        if (receivedDamage <= 0)
        {
            throw new ArgumentException("damage cannot be null");
        }
        else
        {
            currentHP -= receivedDamage;
            SpawnDamageCanvas(receivedDamage);
            if (currentHP <= 0)
            {
                StartCoroutine(C_Death());
            }
        }
    }
    protected virtual IEnumerator C_Death()
    {
        // Прежде чем уничтожить объект, раскидаем префабы мяса
        SpawnMeatPieces(UnityEngine.Random.Range(_minRangePieceCount, _maxRangePieceCount + 1), 2f); // Спавним 5 кусочков мяса в радиусе 2 единицы

        yield return null;
        Destroy(gameObject); // Уничтожаем объект
    }

    private void SpawnMeatPieces(int piecesCount, float spawnRadius)
    {
        for (int i = 0; i < piecesCount; i++)
        {
            // Вычисляем случайную позицию в радиусе вокруг врага
            Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
            randomPos.y = transform.position.y; // Оставляем ту же высоту, чтобы не улетало вверх или вниз

            // Создаем клон префаба
            GameObject meatPieceClone = Instantiate(_meatPiece, randomPos, Quaternion.identity);

            // Добавляем немного физики, чтобы кусочки разлетались
            Rigidbody rb = meatPieceClone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(
                    UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(0.5f, 1.5f), // Чуть больше вверх для естественного "разлета"
                    UnityEngine.Random.Range(-1f, 1f)
                );
                rb.AddForce(randomForce * 4f, ForceMode.Impulse); // Применяем силу для разлета
            }
        }
    }
    private void SpawnDamageCanvas(float damage)
    {
        GameObject canvas = GameObject.Instantiate(_dmgCanv);
        Destroy(canvas, _dmgCanvLifetime);

        DamageText dmgText = canvas.GetComponentInChildren<DamageText>();
        dmgText.Duration = _dmgCanvLifetime;
        dmgText.Text = damage.ToString();

        _isRightSide=!_isRightSide;
        dmgText.IsRightSide = _isRightSide;

        canvas.transform.SetParent(transform);
        canvas.transform.localPosition = new Vector3(0, _ydmgCanvasOffset, 0);
    }

}
