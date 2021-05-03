using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg_soundtrack_manager : MonoBehaviour
{
    
    private AudioSource bg_soundtrack; 

    private void Awake()
    {
        bg_soundtrack = transform.GetComponent<AudioSource>();
        bg_soundtrack.volume = soundEngine.bg_volume;
    }
    public void set_bg_volume(float v)
    {
        bg_soundtrack.volume = v;
        soundEngine.bg_volume = v;
    }
    public void set_sfx_volume(float v)
    {
        soundEngine.sfx_volume = v;
    }
    public void play_click_sound()
    {
        soundEngine.play_sound_with_volume(soundEngine.sound_type.click,0.5f);
    }
   
}
