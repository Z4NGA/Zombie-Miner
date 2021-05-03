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
    /*
    public bool isAlive;
    bool is_invincible;
    public GameObject inventory_gui,DeathMenu,bg_pause,counter;
    public static bool inventory_changes = false;
    public static bool in_shop_range = false;
    private string[] pickable_items = { "", "", "", "", "", "", "", "", "", "", "", "" };
    
    public static int current_level,current_xp;
    private static int[] level_xp = { 125, 378, 598, 1356, 2145, 3565, 4445, 6800 };
    public static int coins;
    public static int gems;
    public static int num_health_pot;
    public static int num_speed_pot;
    public static int num_damage_pot;
    private static bool has_armor ;
    public static int current_health;
    public static int initial_health;
    public static int current_armor;
    public static int initial_armor;
    public static float current_speed;
    public static float initial_speed;
    public static float current_damage;
    public static float initial_damage;

    public static int nr_of_quests = 0;

    private static int armor_level;
    private float death_time = 0;
    public int getArmor() { return armor_level; }
    private int helmet_level;
    public int getHelmet() { return helmet_level; }
    public void savePlayer()
    {
        PlayerPrefs.SetInt("initial_health", initial_health);
        PlayerPrefs.SetInt("initial_armor", initial_armor);
        PlayerPrefs.SetInt("armor_level", armor_level);
        PlayerPrefs.SetInt("num_health_pot", num_health_pot);
        PlayerPrefs.SetInt("num_speed_pot", num_speed_pot);
        PlayerPrefs.SetInt("num_damage_pot", num_damage_pot);
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetInt("gems", gems);
        PlayerPrefs.SetInt("current_level", current_level);
        PlayerPrefs.SetInt("current_xp", current_xp);
        PlayerPrefs.SetFloat("initial_damage", initial_damage);
        PlayerPrefs.SetFloat("initial_speed", initial_speed);

    }
    public static void newplayer()
    {
        PlayerPrefs.DeleteKey("initial_health");
        PlayerPrefs.DeleteKey("initial_armor");
        PlayerPrefs.DeleteKey("armor_level");
        PlayerPrefs.DeleteKey("num_health_pot");
        PlayerPrefs.DeleteKey("num_speed_pot");
        PlayerPrefs.DeleteKey("num_damage_pot");
        PlayerPrefs.DeleteKey("coins");
        PlayerPrefs.DeleteKey("gems");
        PlayerPrefs.DeleteKey("current_level");
        PlayerPrefs.DeleteKey("current_xp");
        PlayerPrefs.DeleteKey("initial_damage");
        PlayerPrefs.DeleteKey("initial_speed");
    }
    public void loadPlayer()
    {
        initial_health= PlayerPrefs.GetInt("initial_health",initial_health);
        initial_armor=  PlayerPrefs.GetInt("initial_armor", initial_armor);
        armor_level =   PlayerPrefs.GetInt("armor_level", armor_level);
        num_health_pot= PlayerPrefs.GetInt("num_health_pot", num_health_pot);
        num_speed_pot=  PlayerPrefs.GetInt("num_speed_pot", num_speed_pot);
        num_damage_pot= PlayerPrefs.GetInt("num_damage_pot", num_damage_pot);
        coins=          PlayerPrefs.GetInt("coins", coins);
        gems=           PlayerPrefs.GetInt("gems", gems);
        current_level=  PlayerPrefs.GetInt("current_level", current_level);
        current_xp=     PlayerPrefs.GetInt("current_xp", current_xp);
        initial_damage= PlayerPrefs.GetFloat("initial_damage", initial_damage);
        initial_speed=  PlayerPrefs.GetFloat("initial_speed", initial_speed);
        has_armor = initial_armor > 0 ? true : false;
        current_health = initial_health;
        current_armor = initial_armor;
        current_speed = initial_speed;
        current_damage = initial_damage;

    }*/
    private void Awake()
    {
        /* is_invincible = false;
         isAlive = true;
         initial_health = 500;
         current_health = initial_health; //starts at full hp
         coins = 200; //starts with 200 base coins 
         gems = 0;//starts with 0 gems
         num_health_pot =0 ;
         num_speed_pot=0;
         num_damage_pot=0;
         initial_armor = 0; //starts with 0 armor
         current_armor = initial_armor; // current armor starts same  as initial
         has_armor = false; //player starts without armor
         current_level = 0;
         current_xp = 0;
         initial_speed = 10;
         current_speed = initial_speed;
         initial_damage = 25;
         current_damage = initial_damage;
         armor_level = 0;
         loadPlayer();*/
        current_money = 0;
        nr_of_dynamite = 0;

        load_stats();

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
            shop.open_shop = true;
        }
        else
        {

        }
    }
    public void start_next_day()
    {
        GameObject.Find("level_manager").GetComponent<level_manager>().next_day();
        load_stats();
        in_game = true;
    }
    public void load_stats()
    {
        lvl = GameObject.Find("level_manager").GetComponent<level_manager>();
        current_level = lvl != null ? lvl.get_level() : 1;
        current_day = lvl != null ? lvl.get_day() : 1;
        goal_money = lvl != null ? lvl.get_goal(current_day) : 999;
        remaining_time = lvl != null ? lvl.get_time(current_day) : 60;
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

    /*
    public  void spendCoins(int amount)
    {
        if (coins >= amount)
            coins -= amount;
    }
    public bool canAfford(int amount)
    {
        Debug.Log("trying to buy !:!");
        if (coins >= amount)
        {
            Debug.Log("can be purchased");
            return true;
        }
        Debug.Log("can not be purchased");
        return false;
    }

    public  bool boughtItem(Items.Item_type i)
    {
        switch (i)
        {
            case Items.Item_type.armor_lvl1:
                return(add_armor(1));
            case Items.Item_type.armor_lvl2:
                return(add_armor(2));
            case Items.Item_type.helmet_lvl1:
                helmet_level = 1;
                return true;
            case Items.Item_type.helmet_lvl2:
                helmet_level = 2;
                return false;
            case Items.Item_type.health_potion:
                num_health_pot++;
                return true;
            case Items.Item_type.damage_potion:
                num_damage_pot++;
                return true;
            case Items.Item_type.speed_potion:
                num_speed_pot++;
                return true;
            default:
                return true;
        }
    }

    public void take_damage(int dmg)
    {
        if (!GetComponent<movement>().isDashing) // prevents damage while dashing
        {
            if (!is_invincible)
            {
                if (has_armor)
                {
                    current_armor = current_armor > dmg ? current_armor - dmg : 0;
                    current_health = current_armor > dmg ? current_health : current_health - (dmg - current_armor);
                    has_armor = current_armor > 0 ? true : false;
                }
                else
                {
                    current_health = (current_health - dmg) < 0 ? 0 : current_health - dmg;//stop it from going under 0 hp
                }
                Debug.Log("current armor : " + current_armor + ", current health : " + current_health);
            }
        }
        else Debug.Log("player is invincible in the safe zone thus takes no damage !");
    }
    public void set_invincible(bool i)
    {
        is_invincible = i;
    }
    public void add_coins(int amount)
    {
        coins += amount;
    }
    public static void gain_coins(int amount)
    {
        coins += amount;
    }
    public static void gain_gems(int amount)
    {
        gems += amount;
    }
    public static void sell_item(int price)
    {
        coins += price; 
    }
    public void add_gems(int amount)
    {
        gems += amount;
    }
    public static int current_lvl_xp()
    {
        if(current_level<level_xp.Length)
            return level_xp[current_level];
        return 999999;
    }
    public static void check_level_up()
    {
        if (current_xp > current_lvl_xp())
        {
            soundEngine.play_sound_timed(soundEngine.sound_type.level_up, 4f);
            current_level += 1;
            upgrade_stat(10, "health", true);
            upgrade_stat(10, "damage", true);
            upgrade_stat(10, "speed", true);
            Debug.Log("LEVEL UP !! YOU are now level " + current_level + " !!");
            check_level_up();
        }
        //popup_text.popup("LEVEL UP !!", transform.position, false);
    }
    public static void gain_xp(int amount)
    {
        current_xp += amount;
        check_level_up();
    }
    public static bool use_item(Items.Item_type i)
    {
        switch (i)
        { //case Items.Item_type.xp_potion : 
            case Items.Item_type.health_potion:
                if(num_health_pot>0)
                {
                    if (current_health != initial_health)
                    {
                        soundEngine.play_sound(soundEngine.sound_type.potion);
                        upgrade_stat(15, "health", false);
                        num_health_pot--;
                        inventory_changes = true;
                        return true;
                    }
                    else return false;
                }
                return false;
            case Items.Item_type.damage_potion:
                if (num_damage_pot > 0)
                {
                    soundEngine.play_sound(soundEngine.sound_type.potion);
                    upgrade_stat(20, "damage", false);
                    num_damage_pot--;
                    inventory_changes = true;
                    return true;
                }
                return false;
            case Items.Item_type.speed_potion:
                if (num_speed_pot > 0)
                {
                    soundEngine.play_sound(soundEngine.sound_type.potion);
                    upgrade_stat(25, "speed", false);
                    num_speed_pot--;
                    inventory_changes = true;
                    return true;
                }
                return false;
            case Items.Item_type.armor_lvl1:
                return(add_armor(1));
            case Items.Item_type.armor_lvl2:
                return(add_armor(2));
            case Items.Item_type.weapon_1:
                return false;
            case Items.Item_type.weapon_2:
                return false;
            default: return false;
        }
    }
    public static void upgrade_stat(int percent,string stat_type,bool is_permanant)
    {
        if (!is_permanant)
        {
            switch (stat_type)
            {
                case "health":
                    current_health = current_health + (int)(((float)percent / 100) * current_health) > initial_health ? initial_health : current_health + (int)(((float)percent / 100) * current_health);
                    break;
                case "speed":
                    current_speed = (int)(current_speed + ((float)percent / 100) * current_speed);
                    break;
                case "damage":
                    current_damage = (int)(current_damage + ((float)percent / 100) * current_damage);
                    break;
                default: break;
            }
        }
        else
        {
            switch (stat_type)
            {
                case "health":
                    initial_health = initial_health + (int)(((float)percent / 100) * initial_health);
                    current_health = initial_health;
                    break;
                case "speed":
                    initial_speed = (int)(initial_speed + ((float)percent / 100) * initial_speed);
                    current_speed = initial_speed;
                    break;
                case "damage":
                    initial_damage = (int)(initial_damage + ((float)percent / 100) * initial_damage);
                    current_damage = initial_damage;
                    break;
                default: break;
            }
        }
    }
    public void add_to_inventory(string type  ,int amount ,string discription)
    {
        soundEngine.play_sound(soundEngine.sound_type.pick_item);
        if (inventory_gui.GetComponent<inventory_ui>().items_in_inventory > 0)
        {
            foreach(string t in pickable_items)
            {
                if (t.Equals(type))
                {
                    inventory_gui.GetComponent<inventory_ui>().add_existing_pickable_item(type, amount);
                    return;
                }
            }
        }
        inventory_gui.GetComponent<inventory_ui>().add_pickable_item(type, amount, discription);
        pickable_items[inventory_gui.GetComponent<inventory_ui>().items_in_inventory] = type;
    }
    //add remove armor helmets etc 
    private static bool add_armor(int armor_lvl)
    {
        if(!has_armor)
        {//sets new armor
            current_armor = armor_lvl == 1 ? 250 : 500;
            initial_armor = armor_lvl == 1 ? 250 : 500;
            has_armor = true;
            armor_level = armor_lvl;
            return true;
        }
        else
        {
            if (armor_lvl == armor_level)
            {
                if (current_armor < initial_armor)
                {
                    //replenishes armor
                    current_armor = initial_armor;
                    has_armor = true;
                    return true;
                }
                has_armor = true;
                return false;
            }
            else if (armor_lvl > armor_level)
            {
                //upgrade armor
                armor_level = armor_lvl;
                initial_armor = 500;//temp then *45%
                current_armor = initial_armor;
                has_armor = true;
                return true;
            }
            return false;
        }
    }
*/
}
