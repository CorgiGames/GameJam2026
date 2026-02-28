using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] characterPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseCharacters = 8;
    [SerializeField] private float characterPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
}
