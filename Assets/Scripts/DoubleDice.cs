using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoubleDice : MonoBehaviour
{
    [Header("UI References")]
    public Image diceImage1;
    public Image diceImage2;
    
    [Header("Dice Configuration")]
    public Sprite[] diceFaces; 
    public float rollDuration = 0.5f;
    
    private bool isRolling = false;

    public void RollBothDice()
    {
        if (!isRolling) 
        {
            StartCoroutine(RollRoutine());
        }
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        float elapsed = 0;

        // Animasyon döngüsü
        while (elapsed < rollDuration)
        {
            diceImage1.sprite = diceFaces[Random.Range(0, diceFaces.Length)];
            diceImage2.sprite = diceFaces[Random.Range(0, diceFaces.Length)];
            
            elapsed += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        // Kesin sonuçların belirlenmesi (0 ile 5 arası indeks döner)
        int finalResult1 = Random.Range(0, diceFaces.Length);
        int finalResult2 = Random.Range(0, diceFaces.Length);

        // Sonuçların UI'a yansıtılması
        diceImage1.sprite = diceFaces[finalResult1];
        diceImage2.sprite = diceFaces[finalResult2];
        
        // Zar değerleri (indeks + 1)
        int dice1Value = finalResult1 + 1;
        int dice2Value = finalResult2 + 1;
        int totalSum = dice1Value + dice2Value;

        Debug.Log($"Zar 1: {dice1Value} | Zar 2: {dice2Value} | Toplam: {totalSum}");

        isRolling = false;
    }
}