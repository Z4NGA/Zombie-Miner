using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundAssets : MonoBehaviour
{
    private static soundAssets _instance;
    public static soundAssets instance
    {
        get
        {
            if (_instance == null) _instance = (Instantiate(Resources.Load("soundAssets")) as GameObject).GetComponent<soundAssets>();
            return _instance;
        } //in case instance doesn't exist , it will be loaded to the scene as a clone
    }
    //UI sounds
    public AudioClip click;
    public AudioClip countdown;
    //player sounds
    public AudioClip pull_crane;
    public AudioClip push_crane;
    //explosions sfx
    public AudioClip energy_explosion;
    public AudioClip fireball_explosion;
    //gui interactions
    public AudioClip buy;
    public AudioClip sell;
    //bg music
    public AudioClip level1_bg;
    public AudioClip level2_bg;
    public AudioClip level3_bg;
    public AudioClip level4_bg;
    public AudioClip level5_bg;
}
