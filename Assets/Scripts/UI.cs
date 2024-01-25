using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private AsyncOperation asyncOperation;

    public TouchController touches;
    public GeneralController general;

    private float volume;
    public List<AudioSource> sounds;

    private bool reloadThis;
    private bool reload;
    private float loadingtimer = 3;

    public GameObject volumeOn;
    public GameObject volumeOff;
    public GameObject loadingScreen;
    public GameObject settingScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    private float mode; // unique level
    public int howManyLevelsDone; // real number of last level
    private int levelMax; // how many levels total
    public float chosenLevel; // real number of level

    // all ui
    public int levelmoneyBonus;
    public TMP_Text levelcoins;
    public int coins;
    public int price1;
    public int price2;
    public int backgroundPrice;
    public TMP_Text price1text;
    public TMP_Text price2text;
    public TMP_Text pricebacktext;
    public List<TMP_Text> coinsText;
    public int howManyStars;
    private int currentstars;

    public int candyScoreIndex;
    public int bestScore;
    public int currentScore;
    public int targetScore;
    public TMP_Text bestScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text targetScoreText;

    public bool bombsAllowed;
    public bool specialssAllowed;
    public float gravityIndex;
    public float normalgravity = 1;
    private float speed;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
   // public GameObject nextButton;
    public GameObject buy;
    public GameObject set;
    public TMP_Text backgroundDescription;

    // skills
    public float a2timer;
    public float a2timerMax;
    public Image a2activeskale;
    public bool a2active;
    public float a1timer;
    public float a1timerMax;
    public Image a1activeskale;
    public bool a1active;

    // tips
    public Animator tipAnimator;

    public int tutorial1;
    public int tutorial2;
    public int tutorial3;

    // background
    public int background2bought;
    public int background3bought;
    public int background4bought;
    public int currentBackground;

  //  public GameObject lightObject;
    //private bool lighton;
    public void Start()
    {
        Time.timeScale = 1;
        asyncOperation = SceneManager.LoadSceneAsync("Preloader");
        asyncOperation.allowSceneActivation = false;

        coins = PlayerPrefs.GetInt("coins");
        mode = PlayerPrefs.GetFloat("mode");
        levelMax = PlayerPrefs.GetInt("levelMax");
        volume = PlayerPrefs.GetFloat("volume");
        chosenLevel = PlayerPrefs.GetFloat("chosenLevel");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");
        bestScore = PlayerPrefs.GetInt("bestScore");

        tutorial1 = PlayerPrefs.GetInt("tutorial1");
        tutorial2 = PlayerPrefs.GetInt("tutorial2");
        tutorial3 = PlayerPrefs.GetInt("tutorial3");

        howManyStars = PlayerPrefs.GetInt("howManyStars");
        background2bought = PlayerPrefs.GetInt("background2bought");
        background3bought = PlayerPrefs.GetInt("background3bought");
        background4bought = PlayerPrefs.GetInt("background4bought");
        currentBackground = PlayerPrefs.GetInt("currentBackground");
        backgroundDescription.text = general.backgrounddescriptions[currentBackground];

        foreach (GameObject back in general.backgrounds)
        {
            back.SetActive(false);
        }
        general.backgrounds[currentBackground].SetActive(true);
        set.SetActive(true);
        buy.SetActive(false);

        a2activeskale.fillAmount = 0;
        a1activeskale.fillAmount = 0;

        sounds[0].Play();
        if (volume == 1)
        {
            Sound(true);
        }
        else
        {
            Sound(false);
        }

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        settingScreen.SetActive(false);
        loadingScreen.SetActive(false);

        tipAnimator.enabled = false;
        price1text.text = price1.ToString("0");
        price2text.text = price2.ToString("0");
        pricebacktext.text = backgroundPrice.ToString("0");


        speed = 0.3f;

        // levels
        if (mode != 0)
        {
            if (mode == 1)
            {
                targetScore = candyScoreIndex * 5;
                levelmoneyBonus = 50;
                bombsAllowed = false;
                specialssAllowed = false;
                speed = 0.3f;
            }
            else if (mode == 2)
            {
                targetScore = candyScoreIndex * 7;
                levelmoneyBonus = 50;
                bombsAllowed = false;
                specialssAllowed = false;
                speed = 0.3f;

            }
            else if (mode == 3)
            {
                targetScore = candyScoreIndex * 10;
                levelmoneyBonus = 50;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.4f;
            }
            else if (mode == 4)
            {
                targetScore = candyScoreIndex * 15;
                levelmoneyBonus = 50;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.4f;

            }
            else if (mode == 5)
            {
                targetScore = candyScoreIndex * 20;
                levelmoneyBonus = 70;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.4f;
            }
            else if (mode == 6)
            {

                targetScore = candyScoreIndex * 25;
                levelmoneyBonus = 100;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.5f;
            }
            else if (mode == 7)
            {
                targetScore = candyScoreIndex * 30;
                levelmoneyBonus = 100;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.5f;

            }
            else if (mode == 8)
            {
                targetScore = candyScoreIndex * 35;
                levelmoneyBonus = 150;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.6f;

            }
            else if (mode == 9)
            {
                targetScore = candyScoreIndex * 40;
                levelmoneyBonus = 200;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.6f;

            }
            else if (mode == 10)
            {

                targetScore = candyScoreIndex * 50;
                levelmoneyBonus = 200;
                bombsAllowed = true;
                specialssAllowed = true;
                speed = 0.6f;
            }

        }
        gravityIndex = normalgravity * speed;

        bestScoreText.text = "best score: " + bestScore.ToString("0");
        currentScoreText.text = "score: " + currentScore.ToString("0");
        targetScoreText.text = "target: " + targetScore.ToString("0");

        
        levelcoins.text = "+" + levelmoneyBonus.ToString("0") + "!";

        if (tutorial1 == 0)
        {
            //tutorial1 = 1;
            PlayerPrefs.SetInt("tutorial1", 1);
            PlayerPrefs.Save();
            tipAnimator.enabled = false;
            tipAnimator.Play("Start");
            tipAnimator.enabled = true;
        }
        else if (tutorial1 != 0 && tutorial2 == 0 && coins >= price1)
        {
            PlayerPrefs.SetInt("tutorial2", 1);
            PlayerPrefs.Save();
            tipAnimator.enabled = false;
            tipAnimator.Play("Bonuses");
            tipAnimator.enabled = true;
        }

        general.SpawnObjects(5);
    }

    public void nextBackground(int number)
    {
        sounds[1].Play();
        currentBackground += number;
        if(currentBackground >= general.backgrounds.Count)
        {
            currentBackground = 0;
        }
        else if(currentBackground < 0)
        {
            currentBackground = general.backgrounds.Count-1;
        }
        backgroundDescription.text = general.backgrounddescriptions[currentBackground];

        if (currentBackground == 0 || (currentBackground == 1 && background2bought == 1) || (currentBackground == 2 && background3bought == 1) || (currentBackground == 3 && background4bought == 1))
        {
            foreach (GameObject back in general.backgrounds)
            {
                back.SetActive(false);
            }
            general.backgrounds[currentBackground].SetActive(true);
            set.SetActive(true);
            buy.SetActive(false);
            PlayerPrefs.SetInt("currentBackground", currentBackground);
        }
        else
        {
            set.SetActive(false);
            buy.SetActive(true);
        }    
        PlayerPrefs.Save();
    }

    public void buyBackground()
    {
        sounds[1].Play();
        if (coins >= backgroundPrice)
        {

            coins -= backgroundPrice;
            PlayerPrefs.SetInt("coins", coins);
            PlayerPrefs.Save();

            if (currentBackground == 1)
            {
                background2bought = 1;
                PlayerPrefs.SetInt("background2bought", background2bought);
            }
            else if (currentBackground == 2)
            {
                background3bought = 1;
                PlayerPrefs.SetInt("background3bought", background3bought);
            }
            else if (currentBackground == 3)
            {
                background4bought = 1;
                PlayerPrefs.SetInt("background4bought", background4bought);
            }
            PlayerPrefs.SetInt("currentBackground", currentBackground);
            set.SetActive(true);
            buy.SetActive(false);
            foreach (GameObject back in general.backgrounds)
            {
                back.SetActive(false);
            }
            general.backgrounds[currentBackground].SetActive(true);
            PlayerPrefs.Save();
        }
    }

    public void win()
    {
       // sounds[2].Play();
        PlayerPrefs.SetInt("bestScore", bestScore);
        general.paused = true;
        Debug.Log("win");
        winScreen.SetActive(true);
        if (chosenLevel > howManyLevelsDone)
        {
            PlayerPrefs.SetInt("howManyLevelsDone", (int)chosenLevel);
        }

        coins += levelmoneyBonus;
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.Save();
    }

    public void lose()
    {
        if (currentScore >= targetScore/3)
        {
            currentstars = 1;
            star1.SetActive(true);
            //nextButton.SetActive(true);
        }
        if (currentScore >= targetScore / 2)
        {
            currentstars = 2;
            star2.SetActive(true);
        }

        if (currentstars > howManyStars)
        {
            // last avaliable
            if (chosenLevel == howManyLevelsDone)
            {
                howManyStars = currentstars;
                PlayerPrefs.SetInt("howManyStars", howManyStars);
            }
        }


        PlayerPrefs.SetInt("bestScore", bestScore);
        general.paused = true;
        loseScreen.SetActive(true);

        PlayerPrefs.Save();
    }

    public void Update()
    {
        //if (!lighton)
        //{
        //    lighton = true;
        //    GameObject lightObj = Instantiate(lightObject, transform.position, Quaternion.Euler(60, -30, 0));
        //    lightObj.transform.position = Vector3.zero;
        //}
       

            foreach (TMP_Text text in coinsText)
        {
            text.text = coins.ToString("0");
        }

        if(currentScore > bestScore)
        {
            bestScore = currentScore;
        }

        if (currentScore >= targetScore && !winScreen.activeSelf)
        {
            win();
        }

        bestScoreText.text = "best score: " + bestScore.ToString("0");
        currentScoreText.text = "score: " + currentScore.ToString("0");
        targetScoreText.text = "target: " + targetScore.ToString("0");


        if (loadingScreen.activeSelf == true)
        {
            foreach (AudioSource audio in sounds)
            {
                audio.volume = 0;
            }

            if (loadingtimer > 0)
            {
                loadingtimer -= Time.deltaTime;
            }
            else
            {
                if (!reload)
                {
                    reload = true;
                    if (reloadThis)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    else
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
           }
        }
        if (!loadingScreen.activeSelf)
        {
            foreach (AudioSource audio in sounds)
            {
                audio.volume = volume;
            }
        }
    }

    public void ExitMenu()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        general.paused = false;
        asyncOperation.allowSceneActivation = true;
        loadingScreen.SetActive(true);
    }
    public void reloadScene()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        //general.paused = false;
        reloadThis = true;
        loadingScreen.SetActive(true);
    }
    public void Sound(bool volumeBool)
    {
        if (volumeBool)
        {
            volumeOn.SetActive(true);
            volumeOff.SetActive(false);
            volume = 1;
        }
        else
        {
            volume = 0;
            volumeOn.SetActive(false);
            volumeOff.SetActive(true);
        }

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    public void closeIt()
    {
        sounds[1].Play();
        touches.blocked = true;
        general.paused = false;
        settingScreen.SetActive(false);
        gravityIndex = normalgravity * speed;
    }

    public void Settings()
    {
        sounds[1].Play();
        touches.blocked = true;
        general.paused = true;
        settingScreen.SetActive(true);
        gravityIndex = 0;
    }

    public void a1()
    {
        sounds[1].Play();
        touches.blocked = true;

        if (coins >= price1)
        {
            if (!a1active)
            {
                coins -= price1;
                PlayerPrefs.SetInt("coins", coins);
                PlayerPrefs.Save();
               // a1active = true;
               // a1timer = a1timerMax;

                foreach(CandyScript candyscript in general.allcandies)
                {if (candyscript.thisrenderer != null)
                    {
                        candyscript.thisrenderer.sprite = general.universalCandy;
                    }
                    candyscript.gameObject.tag = "Universal";
                }
            }
        }
        else
        {
            tipAnimator.enabled = false;
            tipAnimator.Play("Warning");
            tipAnimator.enabled = true;

        }
    }

    public void a2()
    {
        sounds[1].Play();
        touches.blocked = true;

        if (coins >= price2)
        {
            if (!a2active)
            {
                coins -= price2;
                PlayerPrefs.SetInt("coins", coins);
                PlayerPrefs.Save();
                a2active = true;
                a2timer = a2timerMax;
            }
        }
        else
        {

            tipAnimator.enabled = false;
            tipAnimator.Play("Warning");
            tipAnimator.enabled = true;

        }
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        if (chosenLevel <= howManyLevelsDone + 1 && chosenLevel != levelMax)
        {
            chosenLevel += 1;
            mode += 1;
            if (mode > 10)
            {
                mode = 1;
            }


            PlayerPrefs.SetFloat("chosenLevel", chosenLevel);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
            reloadScene();
        }
    }
}
