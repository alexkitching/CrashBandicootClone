using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int crashLives;
    public int CrashLives
    {
        get { return crashLives; }
    }
    private int crashWumpas;
    public int CrashWumpas
    {
        get { return crashWumpas; }
    }
    
	void Awake ()
    {
		if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        crashLives = 3;
        crashWumpas = 0;
	}

    public void IncreaseWumpaCount()
    {
        ++crashWumpas;
        if(crashWumpas >= 100)
        {
            IncreaseLifeCount();
            crashWumpas = 0;
        }
    }

    public void IncreaseLifeCount()
    {
        ++crashLives;
    }
}
