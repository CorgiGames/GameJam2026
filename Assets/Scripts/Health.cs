using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHitPoints = 2;
    private int currentHitPoints;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip deathSfx;

    [Header("Death Settings")]
    [SerializeField] private float destroyDelay = 0.25f;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private Vector3 hpBarOffset = new Vector3(0f, 1.0f, 0f);

    private GameObject hpBarInstance;

    private bool isDead = false;
    private bool isInvincible = false;

    public int CurrentHP => currentHitPoints;
    public int MaxHP => maxHitPoints;

    private void Awake()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        currentHitPoints = maxHitPoints;
    }

    private void Start()
    {
        if (hpBarPrefab == null) return;

        hpBarInstance = Instantiate(hpBarPrefab, transform.position + hpBarOffset, Quaternion.identity);

        HealthBar hb = hpBarInstance.GetComponent<HealthBar>();
        if (hb != null)
        {
            hb.health = this;
            hb.offset = hpBarOffset;
        }
    }

    public void Heal(int amount)
{
    if (isDead) return;

    currentHitPoints += amount;

    if (currentHitPoints > maxHitPoints)
        currentHitPoints = maxHitPoints;
}

    public void TakeDamage(int dmg)
    {
        if (isDead) return;
        if (isInvincible) return;

        currentHitPoints -= dmg;

        if (sfxSource != null && hitSfx != null)
            sfxSource.PlayOneShot(hitSfx);

        if (currentHitPoints <= 0)
        {
            isDead = true;

            if (sfxSource != null && deathSfx != null)
                sfxSource.PlayOneShot(deathSfx);

            if (hpBarInstance != null)
                Destroy(hpBarInstance, destroyDelay);

            // Notify the spawner that this unit is dead
            CharacterSpawner.onCharacterDestroy?.Invoke();

            Destroy(gameObject, destroyDelay);
        }
    }
    public void SetInvincible(bool status)
    {
        isInvincible = status;
        // Optional: Change color to gold or add a shield effect here
    }

    public void FullHeal()
    {
        if (isDead) return;
        currentHitPoints = maxHitPoints;
    }
}