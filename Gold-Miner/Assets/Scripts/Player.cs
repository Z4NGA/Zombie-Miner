using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Player : MonoBehaviour
{

    public Transform grabbed_object;
    public Vector3 offset_to_grabbed_object;
    private float min_angle = -150;
    private float max_angle = -30;
    private float min_reach = 0.5f;
    private float max_reach = 6;
    private float current_reach = 0.5f;
    private float current_angle = -150;
    private bool angle_min_to_max = true;
    private bool swinging = true;
    private bool reach_min_to_max = true;
    public float angle_speed = 120;
    public float reach_speed = 5;
    private LineRenderer rope;
    private Transform claw;
    //player related
    public static float player_strength = 1;
    public static float stone_knowledge = 1;
    public static float gem_affinity = 1;
    //level related
    public static int current_day=1;
    public static int current_level=1;
    public static float remaining_time=1;
    public static int goal_money, current_money, nr_of_dynamite;
    private level_manager lvl;

    //time
    public static bool in_game = true;
    private bool play_countdown = false;
    private float start_time=0;
    

    public void savePlayer()
    {
        PlayerPrefs.SetFloat("player_strength", player_strength);
        PlayerPrefs.SetFloat("stone_knowledge", stone_knowledge);
        PlayerPrefs.SetFloat("gem_affinity", gem_affinity);
        PlayerPrefs.SetInt("current_money", current_money);
        PlayerPrefs.SetInt("nr_of_dynamite", nr_of_dynamite);

    }
    public static bool new_player = true;
    public void loadPlayer()
    {
        if (new_player)
        {
            in_game = true;
            new_player = false;
            player_strength = 1;
            stone_knowledge = 1;
            gem_affinity = 1;
            current_money = 0;
            nr_of_dynamite = 0;
        }
        else
        {
            in_game = true;
            player_strength = PlayerPrefs.GetFloat("player_strength", player_strength);
            stone_knowledge = PlayerPrefs.GetFloat("stone_knowledge", stone_knowledge);
            gem_affinity = PlayerPrefs.GetFloat("gem_affinity", gem_affinity);
            current_money = PlayerPrefs.GetInt("current_money", current_money);
            nr_of_dynamite = PlayerPrefs.GetInt("nr_of_dynamite", nr_of_dynamite);
        }
    }
    private void Awake()
    {

        current_money = 0;
        nr_of_dynamite = 0;
        loadPlayer();
        load_level_stats();

        grabbed_object = null;
        offset_to_grabbed_object = new Vector3(0,0);
        rope = transform.Find("rope").GetComponent<LineRenderer>();
        claw = transform.Find("claw");
        rope.SetPosition(1, calc_position(rope.GetPosition(0), current_angle,current_reach));
        claw.Rotate(0, 0, current_angle+90);
    }

    private Vector3 calc_position(Vector3 posO,float angle,float reach)
    {
        return new Vector3(reach*Mathf.Cos(Mathf.Deg2Rad * angle)+posO.x, reach*Mathf.Sin(Mathf.Deg2Rad * angle)+posO.y);
    }

    private void Update()
    {
        if (in_game)
        {
            remaining_time = remaining_time - Time.deltaTime < 0 ? 0 : remaining_time - Time.deltaTime;
            if (remaining_time > 10) play_countdown = false;
            if((remaining_time<=10)&&(play_countdown==false))
            {
                play_countdown = true;
                soundEngine.play_sound_timed(soundEngine.sound_type.countdown,10);
            }
            if (remaining_time > 0)
            {
                if (grabbed_object != null) //pulling back an object
                {
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        if (nr_of_dynamite > 0)
                        {
                            nr_of_dynamite--;
                            grabbed_object.GetComponent<grabbed_object>().explode();
                            soundEngine.play_sound(soundEngine.sound_type.energy_explosion);
                            grabbed_object = null;
                            GameObject.Find("level_manager").GetComponent<level_manager>().decrease_loot(current_day);
                            reach_min_to_max = true;
                            current_reach = min_reach;
                            swinging = true;
                            return;
                        }
                    }
                    reach_min_to_max = false;
                    swinging = false;
                    current_reach -= (player_strength / grabbed_object.GetComponent<grabbed_object>().get_weight()) * reach_speed * Time.deltaTime;//change to percent
                    if (current_reach <= min_reach)
                    {
                        reach_min_to_max = true;
                        current_reach = min_reach;
                        swinging = true;
                        //reward player
                        current_money += grabbed_object.GetComponent<grabbed_object>().get_price(stone_knowledge,gem_affinity);
                        soundEngine.play_sound(soundEngine.sound_type.sell);
                        Destroy(grabbed_object.gameObject);
                        grabbed_object = null;
                        GameObject.Find("level_manager").GetComponent<level_manager>().decrease_loot(current_day);
                    }
                }
                else
                {
                    if (swinging)
                    {
                        if (angle_min_to_max)//direction is min angle to max angle
                        {
                            current_angle += angle_speed * Time.deltaTime;
                            claw.Rotate(0, 0, angle_speed * Time.deltaTime);
                            if (current_angle >= max_angle)
                            {
                                angle_min_to_max = false;
                                current_angle = max_angle;
                            }
                        }
                        else
                        {
                            current_angle -= angle_speed * Time.deltaTime;
                            claw.Rotate(0, 0, -angle_speed * Time.deltaTime);
                            if (current_angle <= min_angle)
                            {
                                angle_min_to_max = true;
                                current_angle = min_angle;
                            }
                        }
                        transform.Find("crane").GetComponent<Animator>().SetBool("active", false);
                        transform.Find("character").GetComponent<Animator>().SetBool("active", false);
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            swinging = false;
                            transform.Find("crane").GetComponent<Animator>().SetBool("active", true);
                            transform.Find("character").GetComponent<Animator>().SetBool("active", true);
                            soundEngine.play_sound_with_volume(soundEngine.sound_type.push_crane,0.7f);
                        }
                    }
                    else
                    {
                        //if not swinging then reaching
                        if (reach_min_to_max)//direction is min angle to max angle
                        {
                            current_reach += reach_speed * Time.deltaTime;
                            if (current_reach >= max_reach)
                            {
                                reach_min_to_max = false;
                                current_reach = max_reach;
                            }
                        }
                        else
                        {
                            current_reach -= reach_speed * Time.deltaTime;
                            if (current_reach <= min_reach)
                            {
                                reach_min_to_max = true;
                                current_reach = min_reach;
                                swinging = true;
                            }
                        }

                    }
                }
                rope.SetPosition(1, calc_position(rope.GetPosition(0), current_angle, current_reach));
                claw.position = calc_position(rope.GetPosition(0), current_angle, current_reach);
                if (grabbed_object != null) grabbed_object.position = calc_position(rope.GetPosition(0), current_angle, current_reach) + offset_to_grabbed_object;
            }
            else end_of_day();
        }
    }
    public static void end_of_day()
    {
        in_game = false;
        if (current_money >= goal_money)
        {
            //save player
            if(current_day==5)
            {
                if (current_level == 5) { 
                    GameEngine.won_game = true;
                    return; 
                }
                reset_stats();
            }
            shop.open_shop = true;
        }
        else
        {
            GameEngine.lost_game = true;
        }
    }
    public void start_next_day()
    {
        savePlayer();
        GameObject.Find("level_manager").GetComponent<level_manager>().next_day();
        load_level_stats();
        in_game = true;
    }
    public void load_level_stats()
    {
        lvl = GameObject.Find("level_manager").GetComponent<level_manager>();
        current_level = lvl != null ? lvl.get_level() : 1;
        current_day = lvl != null ? lvl.get_day() : 1;
        goal_money = lvl != null ? lvl.get_goal(current_day) : 999;
        remaining_time = lvl != null ? lvl.get_time(current_day) : 60;
    }
    public static void reset_stats()
    {
        player_strength = 1;
        stone_knowledge = 1;
        gem_affinity = 1;
    }
    public static void consume_strength_potion()
    {
        player_strength *= 2;
    }
    public static void reset_strength()
    {
        player_strength = 1;
    }
    public static void read_stone_book()
    {
        stone_knowledge *= 5;
    }
    public static void reset_stone_knowledge()
    {
        stone_knowledge = 1;
    }
    public static void refine_gems()
    {
        gem_affinity *= 1.5f;
    }
    public static void reset_gem_affinity()
    {
        gem_affinity = 1;
    }

}
