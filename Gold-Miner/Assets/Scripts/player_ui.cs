using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class player_ui : MonoBehaviour
{
    //sprites 
    private Transform day_prog_bar;
    //containers
    private Transform time_container,level_prog_container,day_prog_container,dynamite_container;

    //days
    private Transform day1_frame, day2_frame, day3_frame, day4_frame, day5_frame;
    private Transform day1_bg, day2_bg, day3_bg, day4_bg, day5_bg;
    // Update is called once per frame
    //timer
    private int last_dynamite, last_money,last_day,last_level,last_goal;//last registered values
    private void Awake()
    {
        time_container = transform.Find("time_container");
        level_prog_container = transform.Find("level_prog_container");
        day_prog_container = transform.Find("day_prog_container");
        dynamite_container = transform.Find("dynamite_container");
        day_prog_bar = day_prog_container.Find("progress").Find("bar").Find("fill");

        day1_frame = level_prog_container.Find("bar").Find("foreground").Find("fill").Find("1");
        day1_bg = level_prog_container.Find("bar").Find("background").Find("fill").Find("1");
        day2_frame = level_prog_container.Find("bar").Find("foreground").Find("fill").Find("2");
        day2_bg = level_prog_container.Find("bar").Find("background").Find("fill").Find("2");
        day3_frame = level_prog_container.Find("bar").Find("foreground").Find("fill").Find("3");
        day3_bg = level_prog_container.Find("bar").Find("background").Find("fill").Find("3");
        day4_frame = level_prog_container.Find("bar").Find("foreground").Find("fill").Find("4");
        day4_bg = level_prog_container.Find("bar").Find("background").Find("fill").Find("4");
        day5_frame = level_prog_container.Find("bar").Find("foreground").Find("fill").Find("5");
        day5_bg = level_prog_container.Find("bar").Find("background").Find("fill").Find("5");
        display_level_progress(1);
    }
    private void Start()
    {
        last_money =Player.current_money; last_goal = Player.goal_money; last_day = Player.current_day;
        last_dynamite = Player.nr_of_dynamite;last_level = Player.current_level;
        display_progress();
    }
    
    void Update() //shouldn't be used to redisplay sprites each second , only when damage is registered
    {
        update_stats();
    }
    void display_level_progress(int day)
    {
        day1_frame.gameObject.SetActive(false); day1_bg.gameObject.SetActive(false);
        day2_frame.gameObject.SetActive(false); day2_bg.gameObject.SetActive(false);
        day3_frame.gameObject.SetActive(false); day3_bg.gameObject.SetActive(false);
        day4_frame.gameObject.SetActive(false); day4_bg.gameObject.SetActive(false);
        day5_frame.gameObject.SetActive(false); day5_bg.gameObject.SetActive(false);

        if (day >= 1)
        {
            day1_frame.gameObject.SetActive(true); day1_bg.gameObject.SetActive(true);
        }
        if (day >= 2)
        {
            day2_frame.gameObject.SetActive(true); day2_bg.gameObject.SetActive(true);
        }
        if (day >= 3)
        {
            day3_frame.gameObject.SetActive(true); day3_bg.gameObject.SetActive(true);
        }
        if (day >= 4)
        {
            day4_frame.gameObject.SetActive(true); day4_bg.gameObject.SetActive(true);
        }
        if (day >= 5)
        {
            day5_frame.gameObject.SetActive(true); day5_bg.gameObject.SetActive(true);
        }
    }
    void update_stats()
    {
        if (last_day != Player.current_day)
        {
            last_day = Player.current_day;
            display_level_progress(last_day);
        }
        //progress bar and positions update
        float prog = (float)Player.current_money / Player.goal_money ;
        day_prog_bar.localScale = prog <= 1 ? new Vector3(1, prog) : new Vector3(1, 1);
        if (prog <= 0.85)
        {
            day_prog_container.Find("progress").Find("current").localPosition = new Vector3(day_prog_container.Find("progress").Find("current").localPosition.x, day_prog_container.Find("progress").Find("goal").localPosition.y * prog);
            day_prog_container.Find("progress").Find("goal").gameObject.SetActive(true);
        }
        else
        {
            day_prog_container.Find("progress").Find("goal").gameObject.SetActive(false);
            day_prog_container.Find("progress").Find("current").localPosition = day_prog_container.Find("progress").Find("goal").localPosition;
        }
        //values update
        if (last_money != Player.current_money)
        {
            day_prog_container.Find("progress").Find("current").GetComponent<TextMeshProUGUI>().SetText(Player.current_money.ToString()+" $");
            last_money = Player.current_money;
        }

        if (last_goal != Player.goal_money)
        {
            day_prog_container.Find("progress").Find("goal").GetComponent<TextMeshProUGUI>().SetText(Player.goal_money.ToString()+" $");
            last_goal = Player.goal_money;
        }

        if (last_dynamite != Player.nr_of_dynamite)
        {
            dynamite_container.Find("quantity").GetComponent<TextMeshProUGUI>().SetText(Player.nr_of_dynamite.ToString());
            last_dynamite = Player.nr_of_dynamite;
        }

        time_container.Find("time").GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(Player.remaining_time).ToString());
    }
    void display_progress()
    {
        day_prog_bar.localScale = (float)Player.current_money / Player.goal_money <= 1 ? new Vector3(1, (float)Player.current_money / Player.goal_money) : new Vector3(1, 1);
        day_prog_container.Find("progress").Find("current").GetComponent<TextMeshProUGUI>().SetText(Player.current_money.ToString()+" $");
        day_prog_container.Find("progress").Find("goal").GetComponent<TextMeshProUGUI>().SetText(Player.goal_money.ToString()+" $");
        time_container.Find("time").GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(Player.remaining_time).ToString());
        dynamite_container.Find("quantity").GetComponent<TextMeshProUGUI>().SetText(Player.nr_of_dynamite.ToString());
    }

}
