using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSourse : MonoBehaviour
{
    public static MusicSourse instance;
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = GetComponent<MusicSourse>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
