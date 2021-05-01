using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shop_ui : MonoBehaviour
{
    private Transform dynamite_container, time_container, money_container;
    private int last_dynamite,last_money,last_day;

    private void Awake()
    {
        time_container = transform.Find("time_container");
        dynamite_container = transform.Find("dynamite_container");
        money_container = transform.Find("money_container");
    }
    private void Start()
    {
        last_money = Player.current_money; last_day = Player.current_day;
        last_dynamite = Player.nr_of_dynamite; 
        display_progress();
    }

    void Update() //shouldn't be used to redisplay sprites each second , only when damage is registered
    {
        update_stats();
    }
    void update_stats()
    {
        //values update
        if (last_money != Player.current_money)
        {
            money_container.Find("money").GetComponent<TextMeshProUGUI>().SetText(Player.current_money.ToString());
            last_money = Player.current_money;
        }

        if (last_dynamite != Player.nr_of_dynamite)
        {
            dynamite_container.Find("quantity").GetComponent<TextMeshProUGUI>().SetText(Player.nr_of_dynamite.ToString());
            last_dynamite = Player.nr_of_dynamite;
        }
        time_container.Find("time").GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(shop.remaining_time).ToString());
    }
    void display_progress()
    {
        time_container.Find("time").GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(shop.remaining_time).ToString());
        dynamite_container.Find("quantity").GetComponent<TextMeshProUGUI>().SetText(Player.nr_of_dynamite.ToString());
        money_container.Find("money").GetComponent<TextMeshProUGUI>().SetText(Player.current_money.ToString());
    }
    public void activate_shop_items(int day)
    {
        transform.Find("items_day_" + day).gameObject.SetActive(true);
    }
    public void deactivate_shop_items(int day)
    {
        transform.Find("items_day_" + day).gameObject.SetActive(false);
    }
    public void buy_item(string type)
    {
        switch (type)
        {
            case "dynamite":
                if (Player.current_money > 200)
                {
                    Player.current_money -= 200;
                    Player.nr_of_dynamite++;
                    transform.Find("items_day_" + Player.current_day).Find("dynamite").gameObject.SetActive(false);
                }
                else Debug.Log("Not enough money to buy the items !! ");
                break;
            case "strength_potion":
                if (Player.current_money > 300)
                {
                    Player.current_money -= 300;
                    Player.consume_strength_potion();
                    transform.Find("items_day_" + Player.current_day).Find("strength_potion").gameObject.SetActive(false);
                }
                else Debug.Log("Not enough money to buy the items !! ");
                break;
            case "stone_book":
                if (Player.current_money > 168)
                {
                    Player.current_money -= 168;
                    Player.read_stone_book();
                    transform.Find("items_day_" + Player.current_day).Find("stone_book").gameObject.SetActive(false);
                }
                else Debug.Log("Not enough money to buy the items !! ");
                break;
            case "gem_liquid":
                if (Player.current_money > 537)
                {
                    Player.current_money -= 537;
                    Player.refine_gems();
                    transform.Find("items_day_" + Player.current_day).Find("gem_liquid").gameObject.SetActive(false);
                }
                else Debug.Log("Not enough money to buy the items !! ");
                break;
        }
    }
    /*
      private void Start()
    {
        
       currency_text.SetText(Player.coins.ToString());
        
        createShopItem(Items.Item_type.armor_lvl1, Items.getName(Items.Item_type.armor_lvl1), Items.getSprite(Items.Item_type.armor_lvl1), Items.getPrice(Items.Item_type.armor_lvl1), 1, 0);
        createShopItem(Items.Item_type.armor_lvl2, Items.getName(Items.Item_type.armor_lvl2), Items.getSprite(Items.Item_type.armor_lvl2), Items.getPrice(Items.Item_type.armor_lvl2), 1, 1);
        createShopItem(Items.Item_type.helmet_lvl1, Items.getName(Items.Item_type.helmet_lvl1), Items.getSprite(Items.Item_type.helmet_lvl1), Items.getPrice(Items.Item_type.helmet_lvl1), 1, 2);
        createShopItem(Items.Item_type.helmet_lvl2, Items.getName(Items.Item_type.helmet_lvl2), Items.getSprite(Items.Item_type.helmet_lvl2), Items.getPrice(Items.Item_type.helmet_lvl2), 1, 3);

        createShopItem(Items.Item_type.health_potion, Items.getName(Items.Item_type.health_potion), Items.getSprite(Items.Item_type.health_potion), Items.getPrice(Items.Item_type.health_potion), 2, 0);
        createShopItem(Items.Item_type.damage_potion, Items.getName(Items.Item_type.damage_potion), Items.getSprite(Items.Item_type.damage_potion), Items.getPrice(Items.Item_type.damage_potion), 2, 1);
        createShopItem(Items.Item_type.speed_potion, Items.getName(Items.Item_type.speed_potion), Items.getSprite(Items.Item_type.speed_potion), Items.getPrice(Items.Item_type.speed_potion), 2, 2);
        createShopItem(Items.Item_type.armor_lvl1, "God", Items.getSprite(Items.Item_type.armor_lvl2), 9999, 2, 3);
        
    } 
     void createShopItem(Items.Item_type type , string name,Sprite sp ,int price , int pagelocation ,int posinpage) //creates an item on the specified location
    {
        
        float item_width = 220;
        Transform newitem = Instantiate(itemTemp, page_containers[pagelocation]);
        RectTransform newitem_rect = newitem.GetComponent<RectTransform>();
        newitem_rect.anchoredPosition = new Vector2(-300 +item_width * posinpage, 0);
        newitem.Find("item_icon").GetComponent<Image>().sprite = sp;
        newitem.Find("item_price").GetComponent<TextMeshProUGUI>().SetText(price.ToString());
        newitem.Find("item_name").GetComponent<TextMeshProUGUI>().SetText(name);
        newitem.gameObject.SetActive(true);
        newitem.GetComponent<Button>().onClick.AddListener(() => BuyItem(type));
    }
    public void loadNextPage()
    {
        page_containers[current_page].gameObject.SetActive(false);
        current_page = current_page<num_pages ? current_page+1 : 1;
        page_containers[current_page].gameObject.SetActive(true);
    }
    public void loadPreviousPage()
    {
        page_containers[current_page].gameObject.SetActive(false);
        current_page = current_page > 1 ? current_page - 1 : num_pages;
        page_containers[current_page].gameObject.SetActive(true);
    }
    public void BuyItem(Items.Item_type i)
    {
        Debug.Log("trying to buy " + i.ToString());
        
        if (pl.canAfford(Items.getPrice(i))){
            //can afford the item
            if (pl.boughtItem(i))//can buy the item
            {
                soundEngine.play_sound(soundEngine.sound_type.buy);
                pl.spendCoins(Items.getPrice(i));
                currency_text.SetText(Player.coins.ToString());
            }
        }
    }
    public void update_currency()
    {
        currency_text.SetText(Player.coins.ToString());
    }*/
}
