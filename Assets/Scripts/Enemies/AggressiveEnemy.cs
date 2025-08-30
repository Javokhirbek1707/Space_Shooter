using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;

    [SerializeField]
    private float _distance;
    private float _attackRange = 4.0f;
    private float _ramMultiplier = 2.0f;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL.");
        }
    }

    void Update()
    {
        AggressiveMovement();
    }

    private void AggressiveMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        _distance = Vector3.Distance(_player.transform.position, this.transform.position);
        if (_distance <= _attackRange)
        {
            Vector3 direction = this.transform.position - _player.transform.position;
            direction = direction.normalized;
            this.transform.position -= direction * Time.deltaTime * (_speed * _ramMultiplier);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            OnEnemyDeath();
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
                _player.EnemyKillCount();
            }
            OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.6f);
    }
}
