/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    public Color color;

    [Space(10)]
    public GameObject fadeScreen;
    public GameObject
        homeScreen,
        tutorialsScreen;
    public GameObject[] tutorials;

    [Space(10)]
    public GameObject popupCoin;
    public GameObject popupDiamond;

    [Space(10)]
    public GameObject optionsScreen;
    public Image
        musicCheckBox,
        soundCheckBox;
    public Sprite
        checkSpr,
        uncheckSpr;

    [Space(10)]
    [Header("MAP")]
    public GameObject mapScreen;
    public Text
        totalCoinInMap,
        totalDiamondInMap;
    public Text missionName;
    public GameObject unlockNotify;
    public Text lvlNeedUnlock;
    public Animation[] levelAnim;
    public GameObject[] levelObjectives;
    public Sprite unlockedLevel;

    [Space(10)]
    [Header("HANGAR")]
    public GameObject hangar;
    public Text
        totalCoinInHangar,
        totalDiamondInHangar;
    public Text
        armorPriceTxt,
        mainGunPriceTxt,
        dronesPriceTxt;
    public Image[]
        armorTrail,
        mainGunTrail,
        dronesTrail;
    public GameObject
        helicopper1,
        helicopper2,
        helicopper3,
        mainGun,
        normalSideGun,
        upgradedSideGun,
        drones1,
        drones2;
    public Sprite
        emptySpr,
        upgradedSpr;

    [Space(10)]
    [Header("InGame")]
    public GameObject inGame;
    public GameObject victory;
    public RectTransform hpBar;
    public Text
        rescueText,
        scoreText,
        coinText;

    [Space(10)]
    [Header("OVER")]
    public GameObject overScreen;
    public Text
        totalCoinInOver,
        totalDiamondInOver;
    public GameObject
        notifyLevel_0,
        notifyNormalLevel;
    public Image
        killBossMedal,
        rescueMedal,
        killAllMedal;
    public Sprite
        killBossSpr,
        rescueSpr,
        killAllSpr,
        starSpr;
    public Text
        missionResult,
        enemyKillBonus,
        bossBonus,
        rescueBonus,
        score,
        bestScore;

    public bool ToHome { get; set; }
    public bool ToOptions { get; set; }
    public bool ToTutorials { get; set; }
    public bool ToGame { get; set; }
    public bool ToMap { get; set; }
    public bool ToHangar { get; set; }
    public bool ToOver { get; set; }

    // Behaviour messages
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Behaviour messages
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt(Const.FIRST_TIME_PLAY, 1);
        //PlayerPrefs.SetFloat(Const.COIN, 0.0f);
        //PlayerPrefs.SetFloat(Const.DIAMOND, 0.0f);

        string levelStatus = Const.UNLOCKED_LEVEL + 1;
        PlayerPrefs.SetInt(levelStatus, 1);

        CheckMusicAndSound();
    }

    private void CheckMusicAndSound()
    {
        if (PlayerPrefs.GetInt(Const.MUSIC, 1) == 1)
        {
            musicCheckBox.sprite = checkSpr;
        }
        else
        {
            musicCheckBox.sprite = uncheckSpr;
        }

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            soundCheckBox.sprite = checkSpr;
        }
        else
        {
            soundCheckBox.sprite = uncheckSpr;
        }
    }

    public void TapToStart()
    {
        fadeScreen.SetActive(true);

        if (PlayerPrefs.GetInt(Const.FIRST_TIME_PLAY, 1) == 1)
        {
            ToTutorials = true;
        }
        else
        {
            ToMap = true;
        }
    }

    public void SwitchToTutorials()
    {
        homeScreen.SetActive(false);
        mapScreen.SetActive(false);

        tutorials[0].SetActive(true);
        tutorialsScreen.SetActive(true);
    }

    public void TapTutorial(int number)
    {
        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.nextStep.Play();
        }

        tutorials[number].SetActive(false);

        if (number < tutorials.Length - 1)
            tutorials[number + 1].SetActive(true);
        else
        {
            fadeScreen.SetActive(true);

            if (PlayerPrefs.GetInt(Const.FIRST_TIME_PLAY, 1) == 1)
                ToGame = true;
            else
                ToMap = true;
        }
    }

    public void SwitchToGame()
    {
        if (PlayerPrefs.GetInt(Const.FIRST_TIME_PLAY, 1) == 1)
        {
            PlayerPrefs.SetInt(Const.FIRST_TIME_PLAY, 0);

            tutorialsScreen.SetActive(false);
            inGame.SetActive(true);
            GameController.Instance.mapLevel = 0;
        }
        else
        {
            hangar.SetActive(false);
            inGame.SetActive(true);
            coinText.text = "0";
            scoreText.text = "0";

            PlayerController.Instance.SetUp();

            if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
            {
                SoundManager.Instance.chopGoingLoop.Play();
            }
        }

        GameController.Instance.GetMap();
    }

    public void GameOver()
    {
        // Get coins
        float coin = PlayerPrefs.GetFloat(Const.COIN, 0.0f);
        coin += GameController.Instance.Coin;
        PlayerPrefs.SetFloat(Const.COIN, coin);

        fadeScreen.SetActive(true);
        ToOver = true;
    }

    public void SwitchToGameOver()
    {
        inGame.SetActive(false);
        victory.SetActive(false);
        overScreen.SetActive(true);

        totalCoinInOver.text = PlayerPrefs.GetFloat(Const.COIN, 0.0f) + "";
        totalDiamondInOver.text = PlayerPrefs.GetFloat(Const.DIAMOND, 0.0f) + "";

        if (GameController.Instance.mapLevel == 0)
        {
            notifyLevel_0.SetActive(true);
        }
        else
        {
            enemyKillBonus.text = GameController.Instance.EnemyBonus + "";
            bossBonus.text = GameController.Instance.BossBonus + "";
            rescueBonus.text = GameController.Instance.RescueBonus + "";

            int currentScore = GameController.Instance.EnemyBonus + GameController.Instance.BossBonus + GameController.Instance.RescueBonus;
            score.text = currentScore + "";

            if (currentScore > PlayerPrefs.GetInt(Const.BEST_SCORE, 0))
            {
                PlayerPrefs.SetInt(Const.BEST_SCORE, currentScore);
            }

            bestScore.text = PlayerPrefs.GetInt(Const.BEST_SCORE, 0) + "";

            // Check medal
            CheckMedal();

            notifyLevel_0.SetActive(false);
            notifyNormalLevel.SetActive(true);
        }

        if (PlayerController.Instance.Dead)
        {
            missionResult.text = "MISSION FAILED!";
        }
        else
        {
            missionResult.text = "MISSION SUCCESSFUL!";
        }
    }

    private void CheckMedal()
    {
        string medalStatus1 = Const.KILL_BOSS_MEDAL + GameController.Instance.mapLevel;
        string medalStatus2 = Const.RESCUE_MEDAL + GameController.Instance.mapLevel;
        string medalStatus3 = Const.KILL_ALL_ENEMY_MEDAL + GameController.Instance.mapLevel;

        if (PlayerPrefs.GetInt(medalStatus1, 0) == 1)
        {
            killBossMedal.sprite = killBossSpr;
            killBossMedal.SetNativeSize();
        }
        else
        {
            killBossMedal.sprite = starSpr;
            killBossMedal.SetNativeSize();
        }

        if (PlayerPrefs.GetInt(medalStatus2, 0) == 1)
        {
            rescueMedal.sprite = rescueSpr;
            rescueMedal.SetNativeSize();
        }
        else
        {
            rescueMedal.sprite = starSpr;
            rescueMedal.SetNativeSize();
        }

        if (PlayerPrefs.GetInt(medalStatus3, 0) == 1)
        {
            killAllMedal.sprite = killAllSpr;
            killAllMedal.SetNativeSize();
        }
        else
        {
            killAllMedal.sprite = starSpr;
            killAllMedal.SetNativeSize();
        }
    }

    public void SetPlayerHP(int HP, int originalHP)
    {
        if (HP <= 0)
            hpBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        else
            hpBar.localScale = new Vector3(HP / (float)originalHP, 1.0f, 1.0f);
    }

    public void AddParachutist(int amount)
    {
        rescueText.text = amount + "/5";
    }

    // OK btn is clicked
    public void OKBtn_Onclick()
    {
        fadeScreen.SetActive(true);
        ToMap = true;

        // Move to level 1
        GameController.Instance.mapLevel = 1;
    }

    public void SwitchToMap()
    {
        if (PlayerPrefs.GetInt(Const.MUSIC, 1) == 1)
        {
            if (!SoundManager.Instance.mapTheme.isPlaying)
                SoundManager.Instance.mapTheme.Play();
        }

        homeScreen.SetActive(false);
        tutorialsScreen.SetActive(false);
        overScreen.SetActive(false);
        hangar.SetActive(false);
        mapScreen.SetActive(true);

        // Check map status
        CheckMap();

        totalCoinInMap.text = PlayerPrefs.GetFloat(Const.COIN, 0.0f) + "";
        totalDiamondInMap.text = PlayerPrefs.GetFloat(Const.DIAMOND, 0.0f) + "";

        int lastLvl = PlayerPrefs.GetInt(Const.LAST_LEVEL, 1);
        levelAnim[lastLvl - 1].Play();

        missionName.text = "MISSION " + lastLvl;
        GameController.Instance.mapLevel = lastLvl;
    }

    private void CheckMap()
    {
        for (var i = levelAnim.Length - 1; i >= 0; i--)
        {
            string levelStatus = Const.UNLOCKED_LEVEL + (i + 1);
            if (PlayerPrefs.GetInt(levelStatus, 0) == 1)
            {
                levelAnim[i].GetComponent<Image>().sprite = unlockedLevel;
                levelAnim[i].GetComponentInChildren<Text>().color = color;
                levelObjectives[i].SetActive(true);

                string medalStatus1 = Const.KILL_BOSS_MEDAL + (i + 1);
                string medalStatus2 = Const.RESCUE_MEDAL + (i + 1);
                string medalStatus3 = Const.KILL_ALL_ENEMY_MEDAL + (i + 1);

                Image[] childs = levelObjectives[i].GetComponentsInChildren<Image>();
                if (PlayerPrefs.GetInt(medalStatus1, 0) == 1)
                {
                    childs[0].sprite = killBossSpr;
                    childs[0].SetNativeSize();
                    childs[0].rectTransform.localScale = new Vector3(0.15f, 0.15f, 1.0f);
                }

                if (PlayerPrefs.GetInt(medalStatus2, 0) == 1)
                {
                    childs[1].sprite = rescueSpr;
                    childs[1].SetNativeSize();
                    childs[1].rectTransform.localScale = new Vector3(0.15f, 0.15f, 1.0f);
                }

                if (PlayerPrefs.GetInt(medalStatus3, 0) == 1)
                {
                    childs[2].sprite = killAllSpr;
                    childs[2].SetNativeSize();
                    childs[2].rectTransform.localScale = new Vector3(0.15f, 0.15f, 1.0f);
                }
            }
        }
    }

    // Missions btn is clicked
    public void ChooseMission_Onclick(int level)
    {
        string levelStatus = Const.UNLOCKED_LEVEL + level;
        if (PlayerPrefs.GetInt(levelStatus, 0) == 1)
        {
            for (var i = levelAnim.Length - 1; i >= 0; i--)
            {
                if ((i + 1) == level)
                {
                    levelAnim[i].Play();
                }
                else
                {
                    levelAnim[i].Stop();
                    levelAnim[i].GetComponentsInChildren<Image>()[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                }
            }

            missionName.text = "MISSION " + level;
            PlayerPrefs.SetInt(Const.LAST_LEVEL, level);

            GameController.Instance.mapLevel = level;

            if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
            {
                SoundManager.Instance.levelMapSelect.Play();
            }
        }
        else
        {
            lvlNeedUnlock.text = (level - 1) + "";
            unlockNotify.SetActive(true);

            if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
            {
                SoundManager.Instance.needToUnlock.Play();
            }
        }
    }

    // Home btn is clicked
    public void HomeBtn_Onclick()
    {
        fadeScreen.SetActive(true);
        ToHome = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    public void SwitchToHome()
    {
        mapScreen.SetActive(false);
        optionsScreen.SetActive(false);
        homeScreen.SetActive(true);
    }

    // Tutorial btn is clicked
    public void TutorialBtn_Onclick()
    {
        fadeScreen.SetActive(true);
        ToTutorials = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // OK btn in map is clicked
    public void OKBtn2_Onclick()
    {
        unlockNotify.SetActive(false);

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Play btn is clicked
    public void Play()
    {
        fadeScreen.SetActive(true);
        ToHangar = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    public void SwitchToHangar()
    {
        mapScreen.SetActive(false);
        hangar.SetActive(true);

        totalCoinInHangar.text = PlayerPrefs.GetFloat(Const.COIN, 0.0f) + "";
        totalDiamondInHangar.text = PlayerPrefs.GetFloat(Const.DIAMOND, 0.0f) + "";

        CheckUpgradedArmorLvl();
        CheckUpgradedGunLvl();
        CheckUpgradedDronesLvl();
    }

    private void CheckUpgradedArmorLvl()
    {
        int armorLvl = PlayerPrefs.GetInt(Const.ARMOR_LEVEL, 1);
        float armorPrice = armorLvl * 1000;
        armorPriceTxt.text = armorPrice + "";


        for (var i = armorTrail.Length - 1; i >= 0; i--)
        {
            if (i <= (armorLvl - 2))
            {
                armorTrail[i].sprite = upgradedSpr;
                armorTrail[i].SetNativeSize();
            }
            else
            {
                armorTrail[i].sprite = emptySpr;
                armorTrail[i].SetNativeSize();
            }
        }

        if (armorLvl >= 3 && armorLvl < 6)
        {
            helicopper1.SetActive(true);
        }
        else
        {
            helicopper1.SetActive(false);
        }

        if (armorLvl >= 6 && armorLvl < 10)
        {
            helicopper2.SetActive(true);
        }
        else
        {
            helicopper2.SetActive(false);
        }

        if (armorLvl >= 10)
        {
            helicopper3.SetActive(true);
        }
        else
        {
            helicopper3.SetActive(false);
        }
    }

    private void CheckUpgradedGunLvl()
    {
        int mainGunLvl = PlayerPrefs.GetInt(Const.PLAYER_GUN_LEVEL, 1);
        float mainGunPrice = mainGunLvl * 1000;
        mainGunPriceTxt.text = mainGunPrice + "";


        for (var i = mainGunTrail.Length - 1; i >= 0; i--)
        {
            if (i <= (mainGunLvl - 2))
            {
                mainGunTrail[i].sprite = upgradedSpr;
                mainGunTrail[i].SetNativeSize();
            }
            else
            {
                mainGunTrail[i].sprite = emptySpr;
                mainGunTrail[i].SetNativeSize();
            }
        }

        if (mainGunLvl >= 2 && mainGunLvl <= 6)
        {
            normalSideGun.SetActive(true);
        }
        else
        {
            normalSideGun.SetActive(false);
        }

        if (mainGunLvl > 6)
        {
            upgradedSideGun.SetActive(true);
        }
        else
        {
            upgradedSideGun.SetActive(false);
        }

        if (mainGunLvl >= 8)
        {
            mainGun.SetActive(true);
        }
        else
        {
            mainGun.SetActive(false);
        }
    }

    private void CheckUpgradedDronesLvl()
    {
        int dronesLvl = PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0);
        float dronesPrice = (dronesLvl == 0 ? 1 : dronesLvl * 10);
        dronesPriceTxt.text = dronesPrice + "";

        for (var i = dronesTrail.Length - 1; i >= 0; i--)
        {
            if (i <= (dronesLvl - 1))
            {
                dronesTrail[i].sprite = upgradedSpr;
                dronesTrail[i].SetNativeSize();
            }
            else
            {
                dronesTrail[i].sprite = emptySpr;
                dronesTrail[i].SetNativeSize();
            }
        }

        if (dronesLvl >= 1)
        {
            drones1.SetActive(true);
        }
        else
        {
            drones1.SetActive(false);
        }

        if (dronesLvl == 10)
        {
            drones2.SetActive(true);
        }
        else
        {
            drones2.SetActive(false);
        }
    }

    // Back btn is clicked
    public void BackToMap()
    {
        fadeScreen.SetActive(true);
        ToMap = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Done btn is clicked
    public void Done()
    {
        fadeScreen.SetActive(true);
        ToGame = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Upgrade btn is clicked
    public void UpgradeBtn_Onclick(string type)
    {
        if (type == "ARMOR")
        {
            UpgradeArmor();
        }
        else if (type == "GUN")
        {
            UpgradeGun();
        }
        else if (type == "DRONES")
        {
            UpgradeDrones();
        }

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.hangarUpdate.Play();
        }
    }

    private void UpgradeArmor()
    {
        int armorLvl = PlayerPrefs.GetInt(Const.ARMOR_LEVEL, 1);

        if (armorLvl < PlayerController.Instance.maxUpgradedArmorLvl)
        {
            float armorPrice = armorLvl * 1000;
            float coin = PlayerPrefs.GetFloat(Const.COIN, 0.0f);

            if (coin >= armorPrice)
            {
                coin -= armorPrice;
                PlayerPrefs.SetFloat(Const.COIN, coin);
                totalCoinInHangar.text = coin + "";

                armorLvl++;
                PlayerPrefs.SetInt(Const.ARMOR_LEVEL, armorLvl);

                armorTrail[armorLvl - 2].sprite = upgradedSpr;
                armorTrail[armorLvl - 2].SetNativeSize();

                armorPriceTxt.text = (armorLvl * 1000) + "";
            }

            if (armorLvl >= 4 && armorLvl < 7)
            {
                helicopper1.SetActive(true);
            }

            if (armorLvl >= 7 && armorLvl < 11)
            {
                helicopper1.SetActive(false);
                helicopper2.SetActive(true);
            }
            else
            {
                helicopper2.SetActive(false);
            }

            if (armorLvl >= 11)
            {
                helicopper2.SetActive(false);
                helicopper3.SetActive(true);
            }
        }
    }

    private void UpgradeGun()
    {
        int mainGunLvl = PlayerPrefs.GetInt(Const.PLAYER_GUN_LEVEL, 1);

        if (mainGunLvl < PlayerController.Instance.maxUpgradedGunLvl)
        {
            float mainGunPrice = mainGunLvl * 1000;
            float coin = PlayerPrefs.GetFloat(Const.COIN, 0.0f);

            if (coin >= mainGunPrice)
            {
                coin -= mainGunPrice;
                PlayerPrefs.SetFloat(Const.COIN, coin);
                totalCoinInHangar.text = coin + "";

                mainGunTrail[mainGunLvl - 1].sprite = upgradedSpr;
                mainGunTrail[mainGunLvl - 1].SetNativeSize();

                mainGunLvl++;
                PlayerPrefs.SetInt(Const.PLAYER_GUN_LEVEL, mainGunLvl);

                mainGunPriceTxt.text = (mainGunLvl * 1000) + "";

                if (mainGunLvl == 2)
                {
                    normalSideGun.SetActive(true);
                }
                if (mainGunLvl == 7)
                {
                    upgradedSideGun.SetActive(true);
                    normalSideGun.SetActive(false);
                }
                else if (mainGunLvl == 8)
                {
                    mainGun.SetActive(true);
                }
            }
        }
    }

    private void UpgradeDrones()
    {
        int dronesLvl = PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0);

        if (dronesLvl < PlayerController.Instance.maxUpgradedDronesLvl)
        {
            float dronesPrice = (dronesLvl == 0 ? 1 : dronesLvl * 10);
            float diamond = PlayerPrefs.GetFloat(Const.DIAMOND, 0.0f);

            if (diamond >= dronesPrice)
            {
                diamond -= dronesPrice;
                PlayerPrefs.SetFloat(Const.DIAMOND, diamond);
                totalDiamondInHangar.text = diamond + "";

                dronesLvl++;
                PlayerPrefs.SetInt(Const.DRONES_LEVEL, dronesLvl);

                dronesTrail[dronesLvl - 1].sprite = upgradedSpr;
                dronesTrail[dronesLvl - 1].SetNativeSize();

                dronesPriceTxt.text = (dronesLvl * 10) + "";

                drones1.SetActive(true);

                if (dronesLvl == 10)
                {
                    drones2.SetActive(true);
                }
                else
                {
                    drones2.SetActive(false);
                }
            }
        }
    }

    // Options btn is clicked
    public void OptionBtn_Onclick()
    {
        fadeScreen.SetActive(true);
        ToOptions = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    public void SwitchToOptions()
    {
        homeScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    // Music btn is clicked
    public void MusicBtn_Onclick()
    {
        if (musicCheckBox.sprite == checkSpr)
        {
            musicCheckBox.sprite = uncheckSpr;
            PlayerPrefs.SetInt(Const.MUSIC, 0);

            SoundManager.Instance.menuTheme.Stop();
        }
        else
        {
            musicCheckBox.sprite = checkSpr;
            PlayerPrefs.SetInt(Const.MUSIC, 1);

            SoundManager.Instance.menuTheme.Play();
        }

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Sound btn is clicked
    public void SoundBtn_Onclick()
    {
        if (soundCheckBox.sprite == checkSpr)
        {
            soundCheckBox.sprite = uncheckSpr;
            PlayerPrefs.SetInt(Const.SOUND, 0);
        }
        else
        {
            soundCheckBox.sprite = checkSpr;
            PlayerPrefs.SetInt(Const.SOUND, 1);

            if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
            {
                SoundManager.Instance.click.Play();
            }
        }
    }

    // Back btn in options is clicked
    public void BackBtn2_Onclick()
    {
        fadeScreen.SetActive(true);
        ToHome = true;

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Open Popup
    public void OpenPopup_Onclick(GameObject popup)
    {
        popup.SetActive(true);

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Exit popup
    public void ExitPopup_Onclick(GameObject popup)
    {
        popup.SetActive(false);

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.click.Play();
        }
    }

    // Pause btn is clicked
    public void Pause(GameObject pauseMenu)
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    // Resume btn is clicked
    public void Resume(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Back To Map
    public void ExitMission(GameObject pauseMenu)
    {
        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.chopGoingLoop.Stop();
        }

        if (PlayerPrefs.GetInt(Const.MUSIC, 1) == 1)
        {
            SoundManager.Instance.gameplayTheme.Stop();
        }

        PlayerController.Instance.StopCoroutine("Shoot");
        PlayerController.Instance.rigid2D.constraints = RigidbodyConstraints2D.FreezePosition;
        GameController.Instance.StopAllCoroutines();
        GameController.Instance.GameOver = true;

        pauseMenu.SetActive(false);
        inGame.SetActive(false);
        Time.timeScale = 1.0f;
        BackToMap();
    }

    // Quit btn is clicked
    public void QuitGame()
    {
        Application.Quit();
    }

    // Purchase resources
    public void Purchase(string content)
    {
        string type = System.Text.RegularExpressions.Regex.Match(content, @"\D+").Value;
        float amount = float.Parse(System.Text.RegularExpressions.Regex.Match(content, @"\d+").Value);

        if (type == "C")
        {
            float coin = PlayerPrefs.GetFloat(Const.COIN, 0.0f);
            coin += amount;
            PlayerPrefs.SetFloat(Const.COIN, coin);

            totalCoinInMap.text = totalCoinInHangar.text = totalCoinInOver.text = coin + "";
        }
        else if (type == "D")
        {
            float diamond = PlayerPrefs.GetFloat(Const.DIAMOND, 0.0f);
            diamond += amount;
            PlayerPrefs.SetFloat(Const.DIAMOND, diamond);

            totalDiamondInMap.text = totalDiamondInHangar.text = totalDiamondInOver.text = diamond + "";
        }
    }
}
