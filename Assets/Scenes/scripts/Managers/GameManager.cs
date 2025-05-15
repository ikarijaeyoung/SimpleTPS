using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;

public class GameManager : Singleton<GameManager>
{
    public AudioManager Audio { get; private set; }
    private void Awake() => Init();
    

    private void Init()
    {
        base.SingletonInit();
        Audio = GetComponent<AudioManager>();
    }
    
}
