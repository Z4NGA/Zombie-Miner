using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEngine : MonoBehaviour
{   
    static int currentLevel = 0;//at main menu , will be static to all levels
    public static bool lost_game = false;
    public static bool won_game = false;
    private void Awake()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;    
    }

    private void Update()
    {
        if (lost_game == true) GameObject.Find("menues").transform.Find("LoseMenu").gameObject.SetActive(true);
        if (won_game == true) GameObject.Find("menues").transform.Find("WinMenu").gameObject.SetActive(true);
    }
    void reset_gamestats()
    {
        lost_game = false;won_game = false;
    }
    public void selectMain()
    {
        reset_gamestats();
        SceneManager.LoadScene(0);
        currentLevel = 0;
    }
    public void startGame()
    {
        Player.new_player = true;
        reset_gamestats();
        string username = GameObject.Find("newgameMenu").transform.Find("name").transform.Find("name_area").transform.Find("name_field").transform.Find("text_area").transform.Find("text").GetComponent<TextMeshProUGUI>().text;
        if (username.Length>1) PlayerPrefs.SetString("user_name", username);
        else PlayerPrefs.SetString("user_name", "new player");
        SceneManager.LoadScene(1); //assuming scene 1 will be level 1
        currentLevel = 1;
    }
    public void quitGame()
    {
        Application.Quit();
    }

    public void selectLevel(int lvl_index)
    {
        if(lvl_index==1)
            Player.new_player = true;
        reset_gamestats();
        if (SceneManager.GetSceneByBuildIndex(lvl_index) != null)
        {
            SceneManager.LoadScene(lvl_index);
            currentLevel = lvl_index;
        }
        else Debug.Log("Error !! Scene"+ lvl_index + "doesn't exist !!");

    }

    public void pause()
    {
        Player.in_game = false;
    }
    public void resume()
    {
        Player.in_game = true;
    }
    public void restart() //for retrying a level
    {
        if(SceneManager.GetActiveScene().buildIndex==1)
            Player.new_player = true;
        reset_gamestats();
        Player.in_game = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void nextLevel()
    {
        reset_gamestats();
        if (currentLevel + 1 <= 5)
        {
            currentLevel++;
            Debug.Log("Loading Level " + currentLevel+" !!");
            SceneManager.LoadScene(currentLevel);
        }
        else selectMain();
    }
}
