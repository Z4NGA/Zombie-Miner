using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shop : MonoBehaviour
{
    private const float shop_time = 45;
    public GameObject shop_gui;
    public static bool open_shop = false;
    private bool shop_on = false;
    public static float remaining_time=0;

    private void Update()
    {
        if (!shop_on)
        {
            if (open_shop)
            {
                remaining_time = shop_time;
                shop_on = true;
                if (shop_gui != null)
                {
                    shop_gui.SetActive(true);
                    shop_gui.GetComponent<shop_ui>().activate_shop_items(Player.current_day);
                }
                else Debug.Log("no shop interface found !!");
            }
        }
        else
        {
            remaining_time = remaining_time - Time.deltaTime <= 0 ? 0 : remaining_time - Time.deltaTime;
            if (remaining_time <= 0)
            {
                close_shop();
            }
        }
    }
    public void close_shop()
    {
        shop_on = false;
        open_shop = false;
        if (shop_gui != null)
        {
            shop_gui.SetActive(false);
            shop_gui.GetComponent<shop_ui>().deactivate_shop_items(Player.current_day);
        }
        else Debug.Log("no shop interface found !!");
        GameObject.Find("player").GetComponent<Player>().start_next_day();
    }
}
