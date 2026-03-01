using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FastDice : MonoBehaviour
{
    public Image diceImage; // UI'daki zar resmi
    public Sprite[] diceFaces; // 6 tane zar yüzü resmi (Inspector'dan sürükle)
    public float rollDuration = 0.5f; // Yarım saniye dönsün (Hızlı olsun)
    
    private bool isRolling = false;

    public void RollDice()
    {
        if (!isRolling) StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        float elapsed = 0;
        int lastIndex = -1;

        while (elapsed < rollDuration)
        {
            // Rastgele bir yüz göster (Animasyon hissi)
            int randomIndex = Random.Range(0, diceFaces.Length);
            diceImage.sprite = diceFaces[randomIndex];
            
            elapsed += 0.05f; // Her 0.05 saniyede bir resim değişsin
            yield return new WaitForSeconds(0.05f);
        }

        // SONUÇ: Gerçek sonucu belirle
        int finalResult = Random.Range(0, diceFaces.Length);
        diceImage.sprite = diceFaces[finalResult];
        
        Debug.Log("Zar Atıldı: " + (finalResult + 1));
        
        // BURADA: Sonuca göre ne olacağını tetikleyebilirsin
        // Örn: if(finalResult + 1 == 6) { BonusVer(); }

        isRolling = false;
    }
}