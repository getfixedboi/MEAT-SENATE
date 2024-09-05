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
    public int MeatPieceCount = 0;
    [SerializeField] private float _hpRegen;
    private void Awake()
    {
        _playerStats = gameObject.GetComponent<PlayerStatictics>();
        _textKeyMeatAbility.text = _meatAbilityBindedKey.ToString();
    }
    private void Update()
    {
        _meatPieceCountOutput.text = MeatPieceCount.ToString() + "/" + _meatPieceReqiureCount.ToString();
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
}

