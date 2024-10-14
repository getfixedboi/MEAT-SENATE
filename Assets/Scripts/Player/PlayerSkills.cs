using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private PlayerStatictics _playerStats;

    [Header("Regen by meat piece")]
    [SerializeField] private UnityEngine.UI.Image _cdMeatAbilityImage;
    [SerializeField] private UnityEngine.UI.Text _textCdMeatAbility;
    [SerializeField] private UnityEngine.UI.Text _textKeyMeatAbility;
    [SerializeField] private UnityEngine.UI.Text _meatPieceCountOutput;
    [SerializeField] private KeyCode _meatAbilityBindedKey;
    [SerializeField] private float _meatAbilityCooldown;
    private float _meatAbilityTimer = 0;
    [SerializeField] private int _meatPieceReqiureCount;
    public static int MeatPieceCount = 40;
    [SerializeField] private float _hpRegen;
    [Header("Shotgun shoot")]
    [SerializeField] private GameObject _playerProjectile;
    [SerializeField] private int _numberOfProjectiles = 5; // Количество снарядов
    [SerializeField] private float _spreadAngle = 15f; // Угол разброса
    [SerializeField] private float _projectileForce = 10f; // Сила выстрела
    [SerializeField] private float _skillUsageCooldown = 1f; // Время, которое нужно подождать перед повторным использованием навыка
    private float _nextSkillUseTime = 0f; // Время, когда навык будет доступен для следующего использования
    private float _damage = 12;
    private Camera _playerCamera; // Ссылка на камеру игрока



    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _playerStats = gameObject.GetComponent<PlayerStatictics>();
        _textKeyMeatAbility.text = _meatAbilityBindedKey.ToString();
    }
    private void Update()
    {
        if(PauseMenu.IsPaused) return;
        _meatPieceCountOutput.text = MeatPieceCount.ToString() + "/" + _meatPieceReqiureCount.ToString();
        _nextSkillUseTime -= Time.deltaTime;

        if (_nextSkillUseTime < 0)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!InteractRaycaster.InTabMode)
                {
                    ShootProjectiles();
                }
            }
        }

        if (_meatAbilityTimer > 0)
        {
            _meatAbilityTimer -= Time.deltaTime;
            _textCdMeatAbility.text = Mathf.Round(_meatAbilityTimer).ToString();
            _cdMeatAbilityImage.fillAmount = _meatAbilityTimer / _meatAbilityCooldown;
        }
        else
        {
            _textCdMeatAbility.text = "";
            _cdMeatAbilityImage.fillAmount = 0;
            _meatAbilityTimer = 0;

            if (Input.GetKeyDown(_meatAbilityBindedKey))
            {
                if (_meatPieceReqiureCount <= MeatPieceCount)
                {
                    MeatPieceCount -= _meatPieceReqiureCount;
                    _meatAbilityTimer = _meatAbilityCooldown;
                    _playerStats.Heal(_hpRegen);
                }
            }
        }
    }
    private void ShootProjectiles()
    {
        if (_playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned.");
            return;
        }

        _nextSkillUseTime = _skillUsageCooldown;

        for (int i = 0; i < _numberOfProjectiles; i++)
        {
            // Создаем снаряд
            GameObject projectile = Instantiate(_playerProjectile, _playerCamera.transform.position, Quaternion.identity);
            projectile.GetComponent<PlayerProjectile>().Damage = _damage;

            // Рассчитываем направление стрельбы с разбросом
            Vector3 baseDirection = _playerCamera.transform.forward;
            float angle = Random.Range(-_spreadAngle, _spreadAngle);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            Vector3 direction = rotation * baseDirection;

            // Добавляем силу к снаряду
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction * _projectileForce, ForceMode.Impulse);
            }
        }
    }


}

