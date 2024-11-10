using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Suicide : EnemyBehaviour
{
    private bool _hasJumped = false;
    private bool _timeToPeak = false;
    [SerializeField] private float _jumpHeightMul;
    [SerializeField] private float _jumpLengthMul;
    [SerializeField] private float _jumpFallMul;
    [SerializeField] private float _peakAfterJumpTime;
    [SerializeField] private GameObject _explosion;

    protected override void Update()
    {
        base.Update();
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
        }
        else if (!_timeToPeak)
        {
            transform.LookAt(target);
        }

        if (DistanceToPlayer <= agent.stoppingDistance && !_hasJumped)
        {
            agent.enabled = false;
            JumpTowardsTarget();
            _hasJumped = true;
        }
        else if (_timeToPeak)
        {
            rb.AddForce(Vector3.down * _jumpFallMul, ForceMode.Acceleration);
        }
    }

    private void JumpTowardsTarget()
    {
        rb.AddForce(transform.forward * _jumpLengthMul, ForceMode.Impulse);
        rb.AddForce(transform.up * _jumpHeightMul, ForceMode.Impulse);

        rb.isKinematic = false;

        rb.AddForce(transform.forward * _jumpLengthMul, ForceMode.Impulse);
        rb.AddForce(transform.up * _jumpHeightMul, ForceMode.Impulse);
        StartCoroutine(TimeToPeak());
    }

    private IEnumerator TimeToPeak()
    {
        yield return new WaitForSeconds(_peakAfterJumpTime);
        _timeToPeak = true;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasJumped) { return; }
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject obj = GameObject.Instantiate(_explosion, transform.position, new Quaternion());
        obj.GetComponent<Explosion>().Damage = damageList[0] * (InAura ? 2 : 1);
        Destroy(gameObject);
    }
}
