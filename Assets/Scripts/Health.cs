using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public Image HealthBar;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (health <=0)
        {
            audioManager.PlaySFX(audioManager.playerdeath);
            Destroy(gameObject);
            SceneManager.LoadScene("GameOverScreen");
        }
    }

    public void TakeDamage (int damage)
    {
        health -= damage;

    }
}
