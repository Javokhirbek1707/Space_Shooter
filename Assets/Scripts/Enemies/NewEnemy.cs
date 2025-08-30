using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{

    private float _speed = 3.0f;


    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    

    private Animator _anim;
    private AudioSource _audioSource;


    private float _fireRate = 3.0f;
    private float _canFire = -1;


    private float _frequency = 1.0f;
    private float _amplitude = 5.0f;
    private float _cycleSpeed = 1.0f;


    private Vector3 _pos;
    private Vector3 _axis;


    [SerializeField]
    private GameObject _enemySheild;
    private bool _isEnemySheildActive = true;
    private int _enemySheildLive = 1;


    void Start()
    {
        _pos = transform.position;
        _axis = transform.right;
        
        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

    }

   
    void Update()
    {
        ZigZag();

        if (Time.time > _canFire)
        {
            FireLaser();
        }
    }


    private void ZigZag()
    {
        _pos += Vector3.down * Time.deltaTime * _cycleSpeed;
        transform.position = _pos + _axis * Mathf.Sin(Time.deltaTime * _frequency) * _amplitude;
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
                _player.AddScore(15);
                _player.EnemyKillCount();
            }
            OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        if(_isEnemySheildActive == true)
        {
            _enemySheildLive--;
            if( _enemySheildLive < 1 )
            {
                _isEnemySheildActive=false;
                _enemySheild.SetActive(false);
            }
        }
        else
        {
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _cycleSpeed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }
        
    }
}
