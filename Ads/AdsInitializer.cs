using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameID = "5432546";
    [SerializeField] string iOSGameID = "5432547";
    [SerializeField] bool testMode = true;
    private string gameID;

    private void Awake()
    {
        gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameID : androidGameID;
        Advertisement.Initialize(gameID, testMode, this);
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Инициализация прошла успешно.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Ошибка инициализации: {error.ToString()} - {message}");
    }

}