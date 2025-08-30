using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private float _speed = 1.0f;

    [SerializeField]
    private GameObject _laser;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    [SerializeField]
    private BossLifeBar _bossLifebar;
    [SerializeField]
    private int _currentLives = 10;

    private bool _canTakeDamage = false;
    private bool _isEnemyAlive = true;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    void Start()
    {
        _bossLifebar = GetComponentInChildren<BossLifeBar>();
        _bossLifebar.gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _anim = GetComponent<Animator>();

        if (_bossLifebar == null)
        {
            Debug.LogError("Boss Life Bar is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= 4.0f)
        {
            _canTakeDamage = true;
            _bossLifebar.gameObject.SetActive(true);
            _speed = 0.0f;
        }
    }

    private void FireLaser()
    {
        _fireRate = Random.Range(2f, 4f);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laser, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    public void BossDamage()
    {
        if (_canTakeDamage == false)
        {
            return;
        }

        _currentLives--;
        _bossLifebar.UpdateBossLifeBar(_currentLives);

        if (_currentLives == 0)
        {
            _isEnemyAlive = false;
            _bossLifebar.gameObject.SetActive(false);
            _uiManager.GameWinner();
            _spawnManager.BossDied();
            if (_player != null)
            {
                _player.AddScore(100);
                _player.EnemyKillCount();
            }
            OnEnemyDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            BossDamage();
            Destroy(other.gameObject);
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
