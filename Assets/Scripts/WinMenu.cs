using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    // Draft sahnesine (veya oyunun başlangıç sahnesine) döner
    public void PlayAgain()
    {
        Debug.Log("Yeniden başlatılıyor...");
        SceneManager.LoadScene("CardGame"); // Buradaki "Draft" sahne adının Build Settings ile aynı olduğundan emin ol.
    }

    // Credits sahnesini yükler (Eğer henüz yoksa hata verir, oluşturman gerekir)
    public void OpenCredits()
    {
        Debug.Log("Credits sahnesi yükleniyor...");
        // SceneManager.LoadScene("Credits"); // Credits sahnen hazır olduğunda bu satırı açabilirsin.
    }

    // Uygulamadan tamamen çıkar
    public void ExitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit();
        
        // Editor içinde test ederken çalışması için:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}