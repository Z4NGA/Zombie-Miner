using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_manager : MonoBehaviour
{
    [SerializeField] private int[] day_goals = { 500, 1500, 2500, 3500, 4500 } ;
    [SerializeField] private float[] day_times = { 30, 45, 60, 75, 90 };
    [SerializeField] private int level = 1;
    [SerializeField] private int current_day = 1;
    [SerializeField] private GameObject[] day_objects;
    [SerializeField] private int[] day_loot = { 3, 3, 3, 3, 3 }; 
    public int get_goal(int day)
    {
        if (day > day_goals.Length + 1) return 0;
        if (day == 0) day = 1;
        return day_goals[day-1];
    }
    public int get_level()
    {
        return level;
    }
    public int get_day()
    {
        return current_day;
    }
    public float get_time(int day)
    {
        if (day > day_times.Length + 1) return 0;
        if (day == 0) day = 1;
        return day_times[day - 1];
    }
    public void next_day()
    {
        current_day = current_day + 1;
        if (current_day > 5) next_level();
        else
        {
            load_day_objects(current_day);
            remove_day_objects(current_day - 1);
        }
        //load next scene
    }
    public void next_level()
    {
        current_day = 1;
        level++;
        GameObject.Find("GameEngine").GetComponent<GameEngine>().nextLevel();
    }
    public void load_day_objects(int day)
    {
        if (day > day_objects.Length + 1) return;
        if (day == 0) day = 1;
        day_objects[day-1].SetActive(true);
    }
    public void remove_day_objects(int day)
    {
        if (day > day_objects.Length + 1) return;
        if (day == 0) day = 1;
        day_objects[day-1].SetActive(false);
    }
    public void decrease_loot(int day)
    {
        if (day > day_loot.Length + 1) return;
        if (day == 0) day = 1;
        day_loot[day - 1]--;
        if (day_loot[day - 1] <= 0) Player.end_of_day();
    }
}
