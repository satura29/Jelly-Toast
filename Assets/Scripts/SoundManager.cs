using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;

    [Header("Sound Resources")]
    public AudioSource backgroundMusic;
    public AudioSource factoryMusic;
    public AudioSource bossMusic;
    public AudioSource agressiveMusic;
    public AudioSource dangerousMusic;

    public GameObject enemyHitSoundPrefab;
    public AudioClip playerHitSound;

    public GameObject coinSoundPrafab;
    public GameObject breadSoundPrefab;
    public GameObject deathSoundPrefab;
    public GameObject parrySoundPrefab;
    public GameObject checkpointSoundPrefab;

    private AudioSource thisAudio;

    private void Awake()
    {
        // check for an existing singleton
        if (Steve)
        {
            // a singleton already exists
            Destroy(this.gameObject);
        }
        else
        {
            // no singleton exists
            Steve = this; // singleton definition
            DontDestroyOnLoad(this.gameObject); // keep this alive in a pocket scene
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
        backgroundMusic.Stop(); // temporary stop
    }

    public void MakeCoinSound()
    {
        GameObject thisSound = Instantiate(coinSoundPrafab, transform);
        Destroy(thisSound, 1.5f);
    }

    public void MakeEnemyHitSound()
    {
        // instantiate the prefab
        GameObject thisSound = Instantiate(enemyHitSoundPrefab, transform);
        Destroy(thisSound, 1.5f);
    }

    public void MakeBreadSound()
    {
        StopTheMusic();
        GameObject thisSound = Instantiate(breadSoundPrefab, transform);
        Destroy(thisSound, 1.5f);
    }

    public void MakeDeathSound()
    {
        StopTheMusic();
        GameObject thisSound = Instantiate(deathSoundPrefab, transform);
        Destroy(thisSound, 2.5f);
    }

    public void MakeParrySound()
    {
        GameObject thisSound = Instantiate(parrySoundPrefab, transform);
        Destroy(thisSound, 2.5f);
    }

    public void MakeCheckpointSound()
    {
        GameObject thisSound = Instantiate(checkpointSoundPrefab, transform);
        Destroy(thisSound, 2.5f);
    }

    public void StartTheMusic()
    {
        backgroundMusic.Play();
    }

    public void StartTheFactory()
    {
        factoryMusic.Play();
    }

    public void StartTheBossMusic()
    {
        bossMusic.Play();
    }

    public void BossIsAngry()
    {
        bossMusic.Stop();
        agressiveMusic.Play();
    }

    public void BossIsPissed()
    {
        agressiveMusic.Stop();
        dangerousMusic.Play();
    }

    public void StopTheMusic()
    {
        backgroundMusic.Stop();
        factoryMusic.Stop();
        bossMusic.Stop();
        agressiveMusic.Stop();
        dangerousMusic.Stop();
    }
}

