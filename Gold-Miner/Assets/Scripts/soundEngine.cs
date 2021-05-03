using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class soundEngine 
{   
    public static float sfx_volume = 0.3f;
    public static float bg_volume = 0.8f;
    //private static float gui_volume = 0.5f;
   
    public enum sound_type
    {
            //ui
        click
        , countdown
        //player
        , pull_crane
        , push_crane
            //attacks effects
        , energy_explosion
        , fireball_explosion
		//gui interactions
        , buy
        , sell
            //level bg music
        , level1_bg
        , level2_bg
        , level3_bg
        , level4_bg
        , level5_bg
    }
    public static AudioClip get_clip (sound_type sound)
    {
        switch(sound)
        {
            //ui
            case sound_type.click:
                return soundAssets.instance.click;
            case sound_type.countdown:
                return soundAssets.instance.countdown;
            //player
            case sound_type.pull_crane:
                return soundAssets.instance.pull_crane;
            case sound_type.push_crane:
                return soundAssets.instance.push_crane;
            //explosions sfx
            case sound_type.energy_explosion:
                return soundAssets.instance.energy_explosion;
            case sound_type.fireball_explosion:
                return soundAssets.instance.fireball_explosion;
			//gui interactions
            case sound_type.buy:
                return soundAssets.instance.buy;
            case sound_type.sell:
                return soundAssets.instance.sell;
                //level bg music
            case sound_type.level1_bg:
                return soundAssets.instance.level1_bg;
            case sound_type.level2_bg:
                return soundAssets.instance.level2_bg;
            case sound_type.level3_bg:
                return soundAssets.instance.level3_bg;
            case sound_type.level4_bg:
                return soundAssets.instance.level4_bg;
            case sound_type.level5_bg:
                return soundAssets.instance.level5_bg;
            default: return null;
        }
    }
    public static float get_volume(sound_type s)
    {
        switch (s)
        {
            default: return sfx_volume;
        }
    }
    public static void play_sound(sound_type s)
    {
        GameObject sound = new GameObject("sound");
        AudioSource source =  sound.AddComponent<AudioSource>();
        source.PlayOneShot(get_clip(s), get_volume(s));
        GameObject.Destroy(sound, 2.5f);
    }
	public static void play_sound(sound_type s,float v)
    {
        GameObject sound = new GameObject("sound");
        AudioSource source = sound.AddComponent<AudioSource>();
        source.PlayOneShot(get_clip(s), sfx_volume);
        GameObject.Destroy(sound, 2.5f);
    }
    public static void play_sound_with_volume(sound_type s, float v)
    {
        GameObject sound = new GameObject("sound");
        AudioSource source = sound.AddComponent<AudioSource>();
        source.PlayOneShot(get_clip(s), v);
        GameObject.Destroy(sound, 2.5f);
    }
    public static void play_sound_timed(sound_type s, float time)
	{
		GameObject sound = new GameObject("sound");
		AudioSource source = sound.AddComponent<AudioSource>();
		source.PlayOneShot(get_clip(s), sfx_volume);
		GameObject.Destroy(sound, time);
	}
	public static void play_looped(sound_type s)
    {
        GameObject sound = new GameObject("sound_looped");
        AudioSource source = sound.AddComponent<AudioSource>();
        source.loop = true;
        source.PlayOneShot(get_clip(s), get_volume(s));
    }
    public static void play_looped(sound_type s,float v)
    {
        GameObject sound = new GameObject("sound_looped");
        AudioSource source = sound.AddComponent<AudioSource>();
        source.loop = true;
        source.PlayOneShot(get_clip(s), sfx_volume);
    }
    public static void play_from_side(sound_type s,float side)
    {
        GameObject sound = new GameObject("sound");
        AudioSource source = sound.AddComponent<AudioSource>();
        source.panStereo = side;
        source.PlayOneShot(get_clip(s), get_volume(s));
        GameObject.Destroy(sound, 2.5f);
    }
    public static void play_3d_sound(sound_type s, Vector3 position)
    {
        GameObject sound = new GameObject("sound");
        sound.transform.localPosition = position;
        AudioSource source = sound.AddComponent<AudioSource>();
        source.spatialBlend =1;
        source.PlayOneShot(get_clip(s), get_volume(s));
        GameObject.Destroy(sound, 2.5f);
    }

}
