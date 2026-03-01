using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Draft sahnesine (veya oyunun başlangıç sahnesine) döner
    public void Play()
    {
        Debug.Log("Oyun başlatılıyor...");
        SceneManager.LoadScene("CardGame"); // Buradaki "Draft" sahne adının Build Settings ile aynı olduğundan emin ol.
    }

    // Credits sahnesini yükler (Eğer henüz yoksa hata verir, oluşturman gerekir)
    public void Tutorial()
    {
        Debug.Log("Tutorial sahnesi yükleniyor...");
        SceneManager.LoadScene("Tutorial"); 
    }

    // Uygulamadan tamamen çıkar
    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit();
        
        // Editor içinde test ederken çalışması için:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}