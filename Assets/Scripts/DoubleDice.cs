using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System; 

public class DoubleDice : MonoBehaviour
{
    [Header("UI References")]
    public Image diceImage1;
    public Image diceImage2;
    
    [Header("Dice Configuration")]
    public Sprite[] diceFaces; 
    public float rollDuration = 0.5f;
    
    private bool isRolling = false;


    public void RollBothDice(Action<int> onRollComplete)
    {
        if (!isRolling) 
        {
           
            gameObject.SetActive(true); 
            StartCoroutine(RollRoutine(onRollComplete));
        }
    }

    private IEnumerator RollRoutine(Action<int> onRollComplete)
    {
        isRolling = true;
        float elapsed = 0;

        while (elapsed < rollDuration)
        {
            diceImage1.sprite = diceFaces[UnityEngine.Random.Range(0, diceFaces.Length)];
            diceImage2.sprite = diceFaces[UnityEngine.Random.Range(0, diceFaces.Length)];
            
            elapsed += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        int finalResult1 = UnityEngine.Random.Range(0, diceFaces.Length);
        int finalResult2 = UnityEngine.Random.Range(0, diceFaces.Length);

        diceImage1.sprite = diceFaces[finalResult1];
        diceImage2.sprite = diceFaces[finalResult2];
        
        int totalSum = (finalResult1 + 1) + (finalResult2 + 1);

        
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

        isRolling = false;
        
       
        onRollComplete?.Invoke(totalSum); 
    }
}