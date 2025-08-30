using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private float _magnetSpeed = 1.0f;
    [SerializeField]
    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        if (Player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
        if (Input.GetKey(KeyCode.C))
        {
            Magnet();
        }
    }

    private void Magnet()
    {
        transform.position = Vector3.Lerp(this.transform.position, Player.transform.position, _magnetSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           AudioSource.PlayClipAtPoint(_clip, transform.position);
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                { 
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;    
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoReloadActive();
                        break;
                    case 4:
                        player.NegativePowerup();
                        break;
                    case 5:
                        player.SuperLaserActive();
                        break;
                    case 6:
                        player.AddHealthPowerup();
                        break;
                    default:
                        Debug.Log("Invalid value");
                        break;
                }
                
            }
            Destroy(this.gameObject);
        }
    }
}
