using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSpawner : MonoBehaviour
{

    public static CharacterSpawner main;

    private void Awake()
    {
        main = this;
    }

    public void SpawnCharacter(GameObject characterPrefab)
    {
        if (characterPrefab == null)
        {
            Debug.LogError("No prefab assigned to this button!");
            return;
        }
        Instantiate(characterPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
        Debug.Log($"Spawned: {characterPrefab.name}");
    }
}
