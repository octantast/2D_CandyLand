using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    private float initialLaunch;
    private float loadingtimer = 3;

    public int howManyLevelsDone;
    public float chosenLevel;
    public int levelMax;

    //public Color32 notenableButton;
    //public Color32 enableButton;

    public Sprite starsprite;
    public Sprite nostarsprite;
    public List<Image> buttons; // levels
    public List<ButtonScript> buttonscripts;
    public GameObject leftarrow;
    public GameObject righarrow;


    public GameObject startButtons;
    public GameObject levelButtons;
    public GameObject loadingScreen;
    public GameObject settings;

    // music
    private float volume;
    public AudioSource ambient;
    public AudioSource tapSound;
    public GameObject volumeOn;
    public GameObject volumeOff;

    // currency
    public TMP_Text currencyCount;
    private int coins;

    public List<GameObject> backgrounds;
    public List<string> backgrounddescriptions;
    public int backgroundPrice;
    public TMP_Text pricebacktext;

    // background
    public int background2bought;
    public int background3bought;
    public int background4bought;
    public int currentBackground;
   // public GameObject nextButton;
    public GameObject buy;
    public GameObject set;
    public TMP_Text backgroundDescription;
    public int howManyStars;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
    void Start()
    {
        Time.timeScale = 1;
        asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        initialLaunch = PlayerPrefs.GetFloat("initialLaunch");
        if (initialLaunch == 0)
        {
            PlayerPrefs.SetFloat("initialLaunch", 1);
            volume = 1;
            PlayerPrefs.SetFloat("volume", volume);
            PlayerPrefs.Save();
        }
        else
        {
            volume = PlayerPrefs.GetFloat("volume");
        }

        howManyStars = PlayerPrefs.GetInt("howManyStars");
        background2bought = PlayerPrefs.GetInt("background2bought");
        background3bought = PlayerPrefs.GetInt("background3bought");
        background4bought = PlayerPrefs.GetInt("background4bought");
        currentBackground = PlayerPrefs.GetInt("currentBackground");
        backgroundDescription.text = backgrounddescriptions[currentBackground];

        foreach (GameObject back in backgrounds)
        {
            back.SetActive(false);
        }
        backgrounds[currentBackground].SetActive(true);
        set.SetActive(true);
        buy.SetActive(false);

        ambient.Play();
        if (volume == 1)
        {
            Sound(true);
        }
        else
        {
            Sound(false);
        }

      
        coins = PlayerPrefs.GetInt("coins");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");

        //alreay done levels
        for (int i = 0; i < howManyLevelsDone; i++)
        {
            if (i < buttons.Count)
            {
                foreach(Image star in buttonscripts[i].starSprites)
                {
                    star.sprite = starsprite;
                }
            }
        }
        
        if (howManyLevelsDone != 0)
        {
            foreach (Image star in buttonscripts[howManyLevelsDone].starSprites)
            {
                for (int i = 0; i <= howManyStars; i++)
                {
                    star.sprite = starsprite;
                }

            }
        }
        currencyCount.text = coins.ToString("0");


        pricebacktext.text = backgroundPrice.ToString("0");

        settings.SetActive(false);
        loadingScreen.SetActive(false);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);

        asyncOperation.allowSceneActivation = false;


    }
    public void nextBackground(int number)
    {
        tapSound.Play();
        currentBackground += number;
        if (currentBackground >= backgrounds.Count)
        {
            currentBackground = 0;
        }
        else if (currentBackground < 0)
        {
            currentBackground = backgrounds.Count - 1;
        }
        backgroundDescription.text = backgrounddescriptions[currentBackground];

        if (currentBackground == 0 || (currentBackground == 1 && background2bought == 1) || (currentBackground == 2 && background3bought == 1) || (currentBackground == 3 && background4bought == 1))
        {
            foreach (GameObject back in backgrounds)
            {
                back.SetActive(false);
            }
            backgrounds[currentBackground].SetActive(true);
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
        tapSound.Play();
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
            foreach (GameObject back in backgrounds)
            {
                back.SetActive(false);
            }
            backgrounds[currentBackground].SetActive(true);
            PlayerPrefs.Save();
        }
    }
    private void Update()
    {

        if (loadingScreen.activeSelf == true)
        {
            ambient.volume -= 0.1f;
            tapSound.volume -= 0.1f;
            if (loadingtimer > 0)
            {
                loadingtimer -= Time.deltaTime;
            }
            else
            {
                asyncOperation.allowSceneActivation = true;
            }
        }

    }
    public void StartGame(float mode)
    {
        // cycled
        playSound(tapSound);
        if (mode <= howManyLevelsDone + 1)
        {
            PlayerPrefs.SetInt("levelMax", levelMax);

            PlayerPrefs.SetFloat("chosenLevel", mode);

            // cycled
            for (int j = 1; j <= 10; j++)
            {
                for (int i = j; i <= levelMax; i += 10) // if 11, j = 1
                {
                    if (mode == i)
                    {
                        mode = j;
                    }
                    if (mode == 0)
                    {
                        mode = 1;
                    }
                }
            }

            // unique levels

            loadingScreen.SetActive(true);
            startButtons.SetActive(false);
            levelButtons.SetActive(false);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
        }
    }

    public void ShowLevels()
    {
        playSound(tapSound);
        loadingScreen.SetActive(false);
        startButtons.SetActive(false);
        levelButtons.SetActive(true);

        leftarrow.SetActive(false);
        righarrow.SetActive(true);

        foreach (ButtonScript button in buttonscripts)
        {
            button.thisLevelNumber = buttonscripts.IndexOf(button) + 1;
            buttonCheck(button);
        }
    }
    public void HideLevels()
    {
        playSound(tapSound);
        settings.SetActive(false);
        loadingScreen.SetActive(false);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);
    }
    public void Settings()
    {
        playSound(tapSound);
        if (!settings.activeSelf)
        {
            HideLevels();
            startButtons.SetActive(false);
            settings.SetActive(true);
        }
        else
        {
            HideLevels();
            settings.SetActive(false);
        }
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
        ambient.volume = volume;
        tapSound.volume = volume;

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
    public void playSound(AudioSource sound)
    {
        sound.Play();
    }



    public void leftArrow()
    {
        playSound(tapSound);
        foreach (ButtonScript button in buttonscripts)
        {
            // changes text
            button.thisLevelNumber -= 9;
            buttonCheck(button);
        }
        if (buttonscripts[0].thisLevelNumber <= 1)
        {
            leftarrow.SetActive(false);
        }
        righarrow.SetActive(true);

    }

    public void rightArrow()
    {
        playSound(tapSound);
        foreach (ButtonScript button in buttonscripts)
        {
            // changes text
            button.thisLevelNumber += 9;
            buttonCheck(button);
            if (button.thisLevelNumber > levelMax)
            {
                button.gameObject.SetActive(false);
                righarrow.SetActive(false);
            }

        }
        leftarrow.SetActive(true);

    }

    public void buttonCheck(ButtonScript button)
    {
        button.thisLevelNumberText.text = button.thisLevelNumber.ToString("0");        
        button.gameObject.SetActive(true);
        if (button.thisLevelNumber <= howManyLevelsDone)
        {
            foreach (Image star in button.starSprites)
            {
                star.sprite = starsprite;
            }
        }
        else
        {
            foreach (Image star in button.starSprites)
            {
                star.sprite = nostarsprite;
            }
        }
    }
}
