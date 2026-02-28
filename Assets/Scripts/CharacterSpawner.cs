using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] characterPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseCharacters = 8;
    [SerializeField] private float characterPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int charactersAlive;
    private int charactersLeftToSpawn;
    private bool isSpawning = false;

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
        if(timeSinceLastSpawn >= (1f / characterPerSecond))
        {
            Debug.Log("Spawn");
        }
    }

    private void StartWave()
    {
        isSpawning = true;
        charactersLeftToSpawn = baseCharacters;
    }
    private int CharacterPerWave()
    {
        return Mathf.RoundToInt(baseCharacters * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
