using UnityEngine;
using UnityEngine.Events;

public class CharacterSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] characterPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseCharacters = 8;
    [SerializeField] private float characterPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onCharacterDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int charactersAlive;
    private int charactersLeftToSpawn;
    private bool isSpawning = false;


    private void Awake()
    {
        onCharacterDestroy.AddListener(CharacterDestroyed);
    }
    private void Start()
    {
        StartWave();
    }
    private void Update()
    {
        if (!isSpawning) return;
        {
            
        }
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= (1f / characterPerSecond) && charactersLeftToSpawn > 0)
        {
            SpawnCharacter();
            charactersLeftToSpawn --;
            charactersAlive ++;
            timeSinceLastSpawn = 0f;
        }
    }

    private void CharacterDestroyed() 
    {
        charactersAlive --;
    }
    private void StartWave()
    {
        isSpawning = true;
        charactersLeftToSpawn = baseCharacters;
    }

    private void SpawnCharacter()
    {
        GameObject prefabToSpawn = characterPrefabs[0];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }
    private int CharacterPerWave()
    {
        return Mathf.RoundToInt(baseCharacters * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
