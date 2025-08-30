using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
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
    private float _rayDistance = 10.0f;
    [SerializeField]
    private float _raycastRad = 2f;

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
        if (Time.time > _canFire)
        {
            FireLaser();
        }

        BackAttack();
    }

    private void CalculateMovement()
    {
        transform.position += Vector3.down * (_speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            float _randX = Random.Range(-9.3f, 9.3f);
            transform.position = new Vector3(_randX, 7f, 0f);
        }
    }

    private void FireLaser()
    {
        _fireRate = Random.Range(2f, 4f);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    public void BackAttack()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _raycastRad, Vector2.up, _rayDistance, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, Vector3.up * _raycastRad * _rayDistance, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("The Collider isn't null for the back Attack!");
            if (hit.collider.CompareTag("Player") && Time.time > _canFire)
            {
                Debug.Log("Player Detected");
                FireLaserBack();
            }
        }
    }

    void FireLaserBack()
    {
        _fireRate = 1f;
        _canFire = Time.time + _fireRate;
        GameObject enemeyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180.0f));
        Laser[] lasers = enemeyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
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
                _player.AddScore(20);
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
