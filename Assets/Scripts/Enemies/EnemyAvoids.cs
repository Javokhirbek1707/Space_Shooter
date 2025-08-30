using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoids : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    [SerializeField]
    private float _rayDistance = 6.0f;
    [SerializeField]
    private float _rayCastRad = 0.25f;

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
        CalculateMovement();
        EnemyAvoid();
        if (Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void EnemyAvoid()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayCastRad, Vector2.down, _rayDistance, LayerMask.GetMask("Laser"));

        float x = Random.Range(-5.0f, 5.0f);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Laser"))
            {
                transform.position = new Vector3(x, transform.position.y, 0);
            }
        }

    }

    private void FireLaser()
    {
        _fireRate = Random.Range(3f, 7f);

        _canFire = Time.time + _fireRate;

        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(new Vector3(0, 0, 0) * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
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
                _player.AddScore(25);
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
