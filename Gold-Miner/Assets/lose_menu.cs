using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class lose_menu : MonoBehaviour
{
    float counter = 10;

    // Update is called once per frame
    void Update()
    {
        if (GameEngine.lost_game)
        {
            counter = counter - Time.deltaTime > 0 ? counter - Time.deltaTime : 0;
            transform.Find("countdown").Find("counter").Find("counter_txt").GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(counter).ToString());
            if (counter == 0)
            {
                GameObject.Find("GameEngine").GetComponent<GameEngine>().selectMain();
            }
        }
    }
}
