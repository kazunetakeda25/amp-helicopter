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

public class GameController : MonoBehaviour
{

    private List<GameObject>
        enemyListT1,
        enemyListT2,
        enemyListT3,
        explosionList,
        coinList,
        effectCoinList;

    private int enemyAmount = 0;

    public static GameController Instance { get; private set; }

    [Space(10)]
    public GameObject enemyPrefabT1;
    public Transform enemyHolderT1;

    [Space(10)]
    public GameObject enemyPrefabT2;
    public Transform enemyHolderT2;

    [Space(10)]
    public GameObject enemyPrefabT3;
    public Transform enemyHolderT3;

    [Space(10)]
    public GameObject bigBoss;
    public float
        startPosBigBoss = 0.0f,
        endPosBigBoss = 0.0f;

    [Space(5)]
    public GameObject normalBoss;
    public GameObject normalBoss2;

    [Space(10)]
    public GameObject explosionPrefab;
    public Transform explosionHolder;
    public GameObject coinPrefab;
    public Transform coinHolder;
    public GameObject effectCoinPrefab;
    public Transform effectCoinHolder;

    [Space(5)]
    public GameObject parachutist;

    [Space(10)]
    public int mapLevel = 0;
    public GameObject[] mapLevels;

    #region LEVELS

    [Space(10)]
    [Header("LEVEL 0")]
    [Space(10)]
    public float enemyRespawnT1_level0 = 1.0f;
    public float enemyRespawnT2_level0 = 1.0f;

    [Space(10)]
    [Header("LEVEL 1")]
    [Space(10)]
    public Texture jungleGround;
    public Texture jungleForeground;
    public MeshRenderer
        bgMesh_level1,
        frMesh_level1;

    [Space(5)]
    public float enemyRespawnT1_level1 = 1.0f;
    public float enemyRespawnT2_level1 = 1.0f;
    public GameObject[]
        bonusCoin_level1,
        obstacles_level1;

    [Space(10)]
    [Header("LEVEL 2")]
    [Space(10)]
    public Texture desertGround;
    public Texture desertForeground;
    public MeshRenderer
        bgMesh_level2,
        frMesh_level2;

    [Space(5)]
    public float enemyRespawnT1_level2 = 1.0f;
    public float enemyRespawnT2_level2 = 1.0f;
    public GameObject[]
        bonusCoin_level2,
        obstacles_level2,
        belvedere_level2;

    [Space(10)]
    [Header("LEVEL 3")]
    [Space(10)]
    public Texture jungleGround2;
    public Texture jungleForeground2;
    public MeshRenderer
        bgMesh_level3,
        frMesh_level3;

    [Space(5)]
    public float enemyRespawnT1_level3 = 1.0f;
    public float enemyRespawnT2_level3 = 1.0f;
    public GameObject[]
        bonusCoin_level3,
        obstacles_level3,
        belvedere_level3;

    [Space(10)]
    [Header("LEVEL 4")]
    [Space(10)]
    public Texture snowGround;
    public Texture snowForeground;
    public MeshRenderer
        bgMesh_level4,
        frMesh_level4;

    [Space(5)]
    public float enemyRespawnT1_level4 = 1.0f;
    public float enemyRespawnT3_level4 = 1.0f;
    public GameObject[]
        bonusCoin_level4,
        obstacles_level4,
        belvedere_level4;

    [Space(10)]
    [Header("LEVEL 5")]
    [Space(10)]
    public Texture cityForeground;
    public MeshRenderer frMesh_level5;

    [Space(5)]
    public float enemyRespawnT1_level5 = 1.0f;
    public float enemyRespawnT3_level5 = 1.0f;
    public GameObject[]
        bonusCoin_level5,
        obstacles_level5,
        belvedere_level5;

    [Space(10)]
    [Header("LEVEL 6")]
    [Space(5)]
    public float enemyRespawnT1_level6 = 1.0f;
    public float enemyRespawnT3_level6 = 1.0f;
    public GameObject[]
        bonusCoin_level6,
        obstacles_level6,
        belvedere_level6;

    #endregion

    public int parachutistAmount { get; set; }

    public float Coin { get; set; }

    public int KilledEnemy { get; set; }
    public int EnemyBonus { get; set; }
    public int BossBonus { get; set; }
    public int RescueBonus { get; set; }

    public bool StartFire { get; set; }
    public bool BossDead_1 { get; set; }
    public bool BossDead_2 { get; set; }
    public bool GameOver { get; set; }
    public bool Win { get; set; }

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
        SetUp();
    }

    private void SetUp()
    {
        enemyListT1 = new List<GameObject>();

        // Create first enemy in enemyListT1
        CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);

        enemyListT2 = new List<GameObject>();

        // Create first enemy in enemyListT2
        CreateNewEnemy(enemyPrefabT2, enemyHolderT2, enemyListT2);

        enemyListT3 = new List<GameObject>();

        // Create first enemy in enemyListT3
        CreateNewEnemy(enemyPrefabT3, enemyHolderT3, enemyListT3);

        coinList = new List<GameObject>();

        // Create first coin
        CreateNewStuff(coinPrefab, coinList, coinHolder);
        coinList[0].SetActive(false);

        explosionList = new List<GameObject>();

        effectCoinList = new List<GameObject>();
    }

    public void GetMap()
    {
        for (var i = mapLevels.Length - 1; i >= 0; i--)
        {
            mapLevels[i].SetActive(false);
        }

        UIManager.Instance.rescueText.text = "0/5";

        if (mapLevel == 0 || mapLevel == 6)
        {
            if (mapLevel == 0)
            {
                PlayerController.Instance.ActiveOrInActiveDrones(true);
                PlayerController.Instance.normalSideGun.SetActive(true);
                bigBoss.GetComponent<Enemy>().SetHP(10000);
                UIManager.Instance.rescueText.text = "0/0";
            }

            mapLevels[mapLevels.Length - 1].SetActive(true);
        }
        else
        {
            switch (mapLevel)
            {
                case 1:
                    bgMesh_level1.material.mainTexture = jungleGround;
                    frMesh_level1.material.mainTexture = jungleForeground;
                    break;
                case 2:
                    bgMesh_level2.material.mainTexture = desertGround;
                    frMesh_level2.material.mainTexture = desertForeground;
                    break;
                case 3:
                    bgMesh_level3.material.mainTexture = jungleGround2;
                    frMesh_level3.material.mainTexture = jungleForeground2;
                    break;
                case 4:
                    bgMesh_level4.material.mainTexture = snowGround;
                    frMesh_level4.material.mainTexture = snowForeground;
                    break;
                case 5:
                    frMesh_level5.material.mainTexture = cityForeground;
                    break;
            }

            mapLevels[mapLevel - 1].SetActive(true);
        }
    }

    public void StartLevel()
    {
        if (mapLevel == 0)
        {
            StartCoroutine("SetUpLevel_0");
        }
        else if (mapLevel == 1)
        {
            StartCoroutine("SetUpLevel_1");
        }
        else if (mapLevel == 2)
        {
            StartCoroutine("SetUpLevel_2");
        }
        else if (mapLevel == 3)
        {
            StartCoroutine("SetUpLevel_3");
        }
        else if (mapLevel == 4)
        {
            StartCoroutine("SetUpLevel_4");
        }
        else if (mapLevel == 5)
        {
            StartCoroutine("SetUpLevel_5");
        }
        else if (mapLevel == 6)
        {
            StartCoroutine("SetUpLevel_6");
        }
    }

    private IEnumerator SetUpLevel_0()
    {
        int phaseLimit = 6;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // Phase 1 of level
            if (phase == 1)
            {
                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        enemyCount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                        }
                    }
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                    phaseLimit = 2;
                    enemyCount = 0;
                    yield return new WaitForSeconds(enemyRespawnT2_level0);
                    enemyListT2[0].SetActive(true);
                }

                yield return new WaitForSeconds(enemyRespawnT1_level0);
            }
            else if (phase == 2)
            {
                yield return new WaitForSeconds(enemyRespawnT1_level0);

                if (enemyCount < phaseLimit)
                {
                    for (var i = enemyListT1.Count - 1; i >= 0; i--)
                    {
                        if (!enemyListT1[i].activeInHierarchy)
                        {
                            enemyListT1[i].SetActive(true);
                            enemyCount++;
                            break;
                        }
                    }
                }
                else
                {
                    for (var i = bonusCoin_level6.Length - 1; i >= 0; i--)
                    {
                        bonusCoin_level6[i].SetActive(true);
                    }

                    phase++;
                    phaseLimit = 2;
                    enemyCount = 0;
                    yield return new WaitForSeconds(7.5f);
                }
            }
            else if (phase == 3)
            {
                if (enemyCount < phaseLimit)
                {
                    enemyListT2[0].SetActive(true);
                }

                enemyCount++;
                yield return new WaitForSeconds(enemyRespawnT1_level0);

                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        break;
                    }
                }

                if (enemyCount == phaseLimit)
                {
                    bigBoss.SetActive(true);
                }
            }
        }
    }

    private IEnumerator SetUpLevel_1()
    {
        int phaseLimit = 21;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // The stages of level
            if (phase == 1)
            {
                for (var i = enemyListT2.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT2[i].activeInHierarchy)
                    {
                        enemyListT2[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT2, enemyHolderT2, enemyListT2);
                        }
                    }
                }

                if (enemyCount == 3)
                {
                    parachutist.SetActive(true);
                }
                else if (enemyCount == 8)
                {
                    parachutist.SetActive(true);

                    for (var i = bonusCoin_level1.Length - 1; i >= 1; i--)
                    {
                        bonusCoin_level1[i].SetActive(true);
                    }
                }
                else if (enemyCount == 13)
                {
                    parachutist.SetActive(true);

                    yield return new WaitForSeconds(enemyRespawnT1_level1);
                    enemyListT1[0].SetActive(true);
                    enemyAmount++;
                    yield return new WaitForSeconds(1.0f);
                    bonusCoin_level1[0].SetActive(true);
                }
                else if (enemyCount == 15)
                {
                    parachutist.SetActive(true);

                    for (var i = obstacles_level1.Length - 1; i >= 0; i--)
                    {
                        obstacles_level1[i].SetActive(true);
                    }
                }
                else if (enemyCount == 16)
                {
                    yield return new WaitForSeconds(enemyRespawnT1_level1);
                    enemyListT1[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 19)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level1[0].SetActive(true);
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                }

                yield return new WaitForSeconds(enemyRespawnT2_level1);
            }
            else if (phase == 2)
            {
                for (var i = bonusCoin_level1.Length - 1; i >= 0; i--)
                {
                    bonusCoin_level1[i].SetActive(true);
                }

                phase++;
                yield return new WaitForSeconds(4.5f);
            }
            else if (phase == 3)
            {
                normalBoss.SetActive(true);
                enemyAmount++;
                break;
            }
        }
    }

    private IEnumerator SetUpLevel_2()
    {
        int phaseLimit = 25;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // The stages of level
            if (phase == 1)
            {
                for (var i = enemyListT2.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT2[i].activeInHierarchy)
                    {
                        enemyListT2[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT2, enemyHolderT2, enemyListT2);
                        }
                    }
                }

                if (enemyCount == 4)
                {
                    parachutist.SetActive(true);
                    bonusCoin_level2[0].SetActive(true);
                }
                else if (enemyCount == 5 || enemyCount == 14 || enemyCount == 18)
                {
                    yield return new WaitForSeconds(enemyRespawnT1_level2);

                    int count = 0;
                    while (count < 3)
                    {
                        for (var i = enemyListT1.Count - 1; i >= 0; i--)
                        {
                            if (!enemyListT1[i].activeInHierarchy)
                            {
                                enemyListT1[i].SetActive(true);
                                count++;
                                enemyAmount++;
                                break;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                                }
                            }
                        }
                        yield return new WaitForSeconds(enemyRespawnT1_level2);
                    }
                }
                else if (enemyCount == 9)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level2[0].SetActive(true);
                    bonusCoin_level2[1].SetActive(true);
                }
                else if (enemyCount == 11)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level2[2].SetActive(true);
                    bonusCoin_level2[3].SetActive(true);
                }
                else if (enemyCount == 16)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level2[0].SetActive(true);

                    for (var i = belvedere_level2.Length - 1; i >= 0; i--)
                    {
                        belvedere_level2[i].SetActive(true);
                        belvedere_level2[i].GetComponent<EnemySoldier>().SetStartPosAndEndPos();
                        enemyAmount++;
                    }
                }
                else if (enemyCount == 19)
                {
                    parachutist.SetActive(true);

                    for (var i = obstacles_level2.Length - 1; i >= 0; i--)
                    {
                        obstacles_level2[i].SetActive(true);
                    }
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                }

                yield return new WaitForSeconds(enemyRespawnT2_level2);
            }
            else if (phase == 2)
            {
                for (var i = bonusCoin_level2.Length - 1; i >= 0; i--)
                {
                    bonusCoin_level2[i].SetActive(true);
                }

                phase++;
                yield return new WaitForSeconds(6.5f);
            }
            else if (phase == 3)
            {
                normalBoss.SetActive(true);
                enemyAmount++;
                break;
            }
        }
    }

    private IEnumerator SetUpLevel_3()
    {
        int phaseLimit = 26;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // The stages of level
            if (phase == 1)
            {
                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                        }
                    }
                }

                if (enemyCount == 4)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level3[0].SetActive(true);
                }
                else if (enemyCount == 5 || enemyCount == 14)
                {
                    yield return new WaitForSeconds(enemyRespawnT2_level3);

                    int count = 0;
                    while (count < 3)
                    {
                        for (var i = enemyListT2.Count - 1; i >= 0; i--)
                        {
                            if (!enemyListT2[i].activeInHierarchy)
                            {
                                enemyListT2[i].SetActive(true);
                                count++;
                                enemyAmount++;
                                break;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    CreateNewEnemy(enemyPrefabT2, enemyHolderT2, enemyListT2);
                                }
                            }
                        }
                        yield return new WaitForSeconds(enemyRespawnT2_level3);
                    }
                }
                else if (enemyCount == 8)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level3[0].SetActive(true);
                }
                else if (enemyCount == 11)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level3[1].SetActive(true);
                    bonusCoin_level3[2].SetActive(true);
                }
                else if (enemyCount == 16)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level3[0].SetActive(true);

                    for (var i = belvedere_level3.Length - 1; i >= 0; i--)
                    {
                        belvedere_level3[i].SetActive(true);
                        belvedere_level3[i].GetComponent<EnemySoldier>().SetStartPosAndEndPos();
                        enemyAmount++;
                    }
                }
                else if (enemyCount == 18)
                {
                    parachutist.SetActive(true);

                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                    yield return new WaitForSeconds(enemyRespawnT2_level3);
                    enemyListT2[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 19)
                {
                    for (var i = obstacles_level3.Length - 1; i >= 0; i--)
                    {
                        obstacles_level3[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }
                else if (enemyCount == 24)
                {
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                }

                yield return new WaitForSeconds(enemyRespawnT1_level3);
            }
            else if (phase == 2)
            {
                for (var i = bonusCoin_level3.Length - 1; i >= 0; i--)
                {
                    bonusCoin_level3[i].SetActive(true);
                }

                phase++;
                yield return new WaitForSeconds(6.5f);
            }
            else if (phase == 3)
            {
                normalBoss2.SetActive(true);
                enemyAmount++;
                break;
            }
        }
    }

    private IEnumerator SetUpLevel_4()
    {
        int phaseLimit = 35;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // The stages of level
            if (phase == 1)
            {
                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                        }
                    }
                }

                if (enemyCount == 4)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level4[0].SetActive(true);
                    bonusCoin_level4[1].SetActive(true);
                }
                else if (enemyCount == 5 || enemyCount == 14)
                {
                    yield return new WaitForSeconds(enemyRespawnT3_level4);

                    int count = 0;
                    while (count < 2)
                    {
                        for (var i = enemyListT3.Count - 1; i >= 0; i--)
                        {
                            if (!enemyListT3[i].activeInHierarchy)
                            {
                                enemyListT3[i].SetActive(true);
                                count++;
                                enemyAmount++;
                                break;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    CreateNewEnemy(enemyPrefabT3, enemyHolderT3, enemyListT3);
                                }
                            }
                        }
                        yield return new WaitForSeconds(enemyRespawnT3_level4);
                    }
                }
                else if (enemyCount == 8)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level4[0].SetActive(true);
                }
                else if (enemyCount == 11)
                {
                    parachutist.SetActive(true);

                    bonusCoin_level4[0].SetActive(true);

                    for (var i = belvedere_level4.Length - 1; i >= 0; i--)
                    {
                        belvedere_level4[i].SetActive(true);
                        belvedere_level4[i].GetComponent<EnemySoldier>().SetStartPosAndEndPos();
                        enemyAmount++;
                    }
                }
                else if (enemyCount == 16)
                {
                    parachutist.SetActive(true);

                    for (var i = obstacles_level4.Length - 1; i >= 0; i--)
                    {
                        obstacles_level4[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }
                else if (enemyCount == 18)
                {
                    parachutist.SetActive(true);

                    enemyListT2[0].SetActive(true);
                    enemyAmount++;
                    yield return new WaitForSeconds(enemyRespawnT3_level4);
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 19)
                {
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 24)
                {
                    for (var i = obstacles_level4.Length - 1; i >= 0; i--)
                    {
                        obstacles_level4[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                }

                yield return new WaitForSeconds(enemyRespawnT1_level4);
            }
            else if (phase == 2)
            {
                for (var i = bonusCoin_level4.Length - 1; i >= 0; i--)
                {
                    bonusCoin_level4[i].SetActive(true);
                }

                phase++;
                yield return new WaitForSeconds(6.5f);
            }
            else if (phase == 3)
            {
                normalBoss2.SetActive(true);
                enemyAmount++;
                break;
            }
        }
    }

    private IEnumerator SetUpLevel_5()
    {
        int phaseLimit = 26;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // The stages of level
            if (phase == 1)
            {
                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                        }
                    }
                }

                if (enemyCount == 3)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level5[0].SetActive(true);
                }
                else if (enemyCount == 5 || enemyCount == 14)
                {
                    yield return new WaitForSeconds(enemyRespawnT3_level5);

                    int count = 0;
                    while (count < 3)
                    {
                        for (var i = enemyListT3.Count - 1; i >= 0; i--)
                        {
                            if (!enemyListT3[i].activeInHierarchy)
                            {
                                enemyListT3[i].SetActive(true);
                                count++;
                                enemyAmount++;
                                break;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    CreateNewEnemy(enemyPrefabT3, enemyHolderT3, enemyListT3);
                                }
                            }
                        }
                        yield return new WaitForSeconds(enemyRespawnT3_level5);
                    }
                }
                else if (enemyCount == 7)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level5[0].SetActive(true);
                }
                else if (enemyCount == 11)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level5[0].SetActive(true);

                    for (var i = belvedere_level5.Length - 1; i >= 0; i--)
                    {
                        belvedere_level5[i].SetActive(true);
                        belvedere_level5[i].GetComponent<EnemySoldier>().SetStartPosAndEndPos();
                        enemyAmount++;
                    }
                }
                else if (enemyCount == 16)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    for (var i = obstacles_level5.Length - 1; i >= 0; i--)
                    {
                        obstacles_level5[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }
                else if (enemyCount == 18)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    enemyListT2[0].SetActive(true);
                    enemyAmount++;
                    yield return new WaitForSeconds(enemyRespawnT3_level5);
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 19)
                {
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 24)
                {
                    for (var i = obstacles_level5.Length - 1; i >= 0; i--)
                    {
                        obstacles_level5[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                }

                yield return new WaitForSeconds(enemyRespawnT1_level5);
            }
            else if (phase == 2)
            {
                for (var i = bonusCoin_level5.Length - 1; i >= 0; i--)
                {
                    bonusCoin_level5[i].SetActive(true);
                }

                phase++;
                yield return new WaitForSeconds(6.5f);
            }
            else if (phase == 3)
            {
                if (!BossDead_1)
                {
                    normalBoss.SetActive(true);
                    enemyAmount++;
                    yield return new WaitForSeconds(2.0f);
                    if (BossDead_1)
                    {
                        phase = 1;
                        phaseLimit = 26;
                        enemyCount = 0;
                    }
                }
                else
                {
                    normalBoss2.SetActive(true);
                    enemyAmount++;
                    break;
                }
            }
        }
    }

    private IEnumerator SetUpLevel_6()
    {
        int phaseLimit = 26;
        int enemyCount = 0;
        int phase = 1;

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            // The stages of level
            if (phase == 1)
            {
                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                        }
                    }
                }

                if (enemyCount == 4)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level6[0].SetActive(true);
                }
                else if (enemyCount == 5 || enemyCount == 14)
                {
                    yield return new WaitForSeconds(enemyRespawnT3_level6);

                    int count = 0;
                    while (count < 4)
                    {
                        for (var i = enemyListT3.Count - 1; i >= 0; i--)
                        {
                            if (!enemyListT3[i].activeInHierarchy)
                            {
                                enemyListT3[i].SetActive(true);
                                count++;
                                enemyAmount++;
                                break;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    CreateNewEnemy(enemyPrefabT3, enemyHolderT3, enemyListT3);
                                }
                            }
                        }
                        yield return new WaitForSeconds(enemyRespawnT3_level6);
                    }
                }
                else if (enemyCount == 8)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level6[0].SetActive(true);
                }
                else if (enemyCount == 11)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level6[0].SetActive(true);

                    for (var i = belvedere_level6.Length - 1; i >= 0; i--)
                    {
                        belvedere_level6[i].SetActive(true);
                        belvedere_level6[i].GetComponent<EnemySoldier>().SetStartPosAndEndPos();
                        enemyAmount++;
                    }
                }
                else if (enemyCount == 16)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    for (var i = obstacles_level6.Length - 1; i >= 0; i--)
                    {
                        obstacles_level6[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }
                else if (enemyCount == 18)
                {
                    if (parachutistAmount < 5)
                    {
                        parachutist.SetActive(true);
                    }

                    bonusCoin_level6[0].SetActive(true);
                    yield return new WaitForSeconds(enemyRespawnT3_level6);
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                    yield return new WaitForSeconds(enemyRespawnT3_level6);
                    enemyListT3[0].SetActive(true);
                    enemyAmount++;
                }
                else if (enemyCount == 19)
                {
                    if (!BossDead_1)
                    {
                        normalBoss.SetActive(true);
                        enemyAmount++;
                    }
                    else
                    {
                        normalBoss2.SetActive(true);
                        enemyAmount++;
                    }

                    yield return new WaitForSeconds(3.0f);
                }
                else if (enemyCount == 24)
                {
                    for (var i = obstacles_level6.Length - 1; i >= 0; i--)
                    {
                        obstacles_level6[i].SetActive(true);
                    }

                    yield return new WaitForSeconds(2.0f);
                }

                if (enemyCount == phaseLimit)
                {
                    phase++;
                }

                yield return new WaitForSeconds(enemyRespawnT1_level6);
            }
            else if (phase == 2)
            {
                for (var i = bonusCoin_level6.Length - 1; i >= 0; i--)
                {
                    bonusCoin_level6[i].SetActive(true);
                }

                if (BossDead_1 && BossDead_2)
                {
                    BossDead_1 = BossDead_2 = false;
                    yield return new WaitForSeconds(6.5f);
                    phase++;
                    normalBoss.SetActive(true);
                    enemyAmount++;
                    normalBoss2.SetActive(true);
                    enemyAmount++;
                }
                else
                {
                    phase = 1;
                    enemyCount = 0;
                }
            }
            else if (phase == 3)
            {
                yield return new WaitForSeconds(2.0f);
                if (BossDead_1 && BossDead_2)
                {
                    bigBoss.SetActive(true);
                    enemyAmount++;
                    phase++;
                }
            }
            else if (phase == 4)
            {
                for (var i = enemyListT1.Count - 1; i >= 0; i--)
                {
                    if (!enemyListT1[i].activeInHierarchy)
                    {
                        enemyListT1[i].SetActive(true);
                        enemyCount++;
                        enemyAmount++;
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewEnemy(enemyPrefabT1, enemyHolderT1, enemyListT1);
                        }
                    }
                }

                yield return new WaitForSeconds(enemyRespawnT1_level6);
            }
        }
    }

    private void CreateNewEnemy(GameObject prefab, Transform enemyHolder, List<GameObject> enemyList)
    {
        GameObject newEnemy = Instantiate(prefab, prefab.transform.position, Quaternion.identity);

        newEnemy.GetComponent<EnemySoldier>().SetStartPosAndEndPos();
        newEnemy.transform.SetParent(enemyHolder);
        newEnemy.SetActive(false);
        enemyList.Add(newEnemy);
    }

    public void CreateEffect(string typeEffect, GameObject prefab, Transform holder, Vector3 position)
    {
        List<GameObject> list = null;

        if (typeEffect == Const.EXPLOSION_EFFECT) { list = explosionList; }
        else if (typeEffect == Const.COIN_EFFECT) { list = effectCoinList; }

        if (list.Count == 0)
        {
            CreateNewStuff(prefab, list, holder);
            list[list.Count - 1].transform.position = position;
        }
        else
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (!list[i].activeInHierarchy)
                {
                    list[i].transform.position = position;
                    list[i].SetActive(true);
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        CreateNewStuff(prefab, list, holder);
                        list[list.Count - 1].transform.position = position;
                    }
                }
            }
        }
    }

    public void SpawnBounty(int amount, Vector3 position)
    {
        int count = 0;

        do
        {
            for (var i = coinList.Count - 1; i >= 0; i--)
            {
                if (!coinList[i].activeInHierarchy)
                {
                    coinList[i].transform.position = position;
                    coinList[i].SetActive(true);
                    count++;
                }
                else
                {
                    if (i == 0)
                    {
                        CreateNewStuff(coinPrefab, coinList, coinHolder);
                        coinList[coinList.Count - 1].transform.position = position;
                        count++;
                    }
                }

                if (count >= amount)
                {
                    break;
                }
            }
        } while (count < amount);
    }

    private void CreateNewStuff(GameObject prefab, List<GameObject> list, Transform holder)
    {
        GameObject newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        newObj.transform.SetParent(holder);
        list.Add(newObj);
    }

    public void Rescue()
    {
        parachutistAmount++;
        UIManager.Instance.AddParachutist(parachutistAmount);

        RescueBonus += 10;
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (mapLevel != 0)
        {
            int score = EnemyBonus + BossBonus + RescueBonus;
            UIManager.Instance.scoreText.text = score + "";
        }
    }

    public void UpdateCoin()
    {
        Coin += 5.0f;
        UIManager.Instance.coinText.text = Coin + "";
    }

    public void Reset()
    {
        StartFire = GameOver = Win = false;

        bigBoss.SetActive(false);
        normalBoss.SetActive(false);
        normalBoss2.SetActive(false);

        Coin = 0.0f;
        EnemyBonus = BossBonus = RescueBonus = parachutistAmount = enemyAmount = KilledEnemy = 0;

        ResetMap();
    }

    private void ResetMap()
    {
        switch (mapLevel)
        {
            case 1:
                Inactive(bonusCoin_level1);
                Inactive(obstacles_level1);
                break;
            case 2:
                Inactive(bonusCoin_level2);
                Inactive(obstacles_level2);
                Inactive(belvedere_level2);
                break;
            case 3:
                Inactive(bonusCoin_level3);
                Inactive(obstacles_level3);
                Inactive(belvedere_level3);
                break;
            case 4:
                Inactive(bonusCoin_level4);
                Inactive(obstacles_level4);
                Inactive(belvedere_level4);
                break;
            case 5:
                Inactive(bonusCoin_level5);
                Inactive(obstacles_level5);
                Inactive(belvedere_level5);
                break;
            case 6:
                Inactive(bonusCoin_level6);
                Inactive(obstacles_level6);
                Inactive(belvedere_level6);
                break;
        }
    }

    private void Inactive(GameObject[] list)
    {
        for (var i = list.Length - 1; i >= 0; i--)
            list[i].SetActive(false);
    }

    public void Victory()
    {
        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            SoundManager.Instance.gameplayTheme.Stop();
            SoundManager.Instance.chopGoingLoop.Stop();
            SoundManager.Instance.victory.Play();
        }

        Win = true;

        StopAllCoroutines();

        string medalStatus1 = Const.KILL_BOSS_MEDAL + mapLevel;
        PlayerPrefs.SetInt(medalStatus1, 1);

        if (parachutistAmount == 5)
        {
            string medalStatus2 = Const.RESCUE_MEDAL + mapLevel;
            PlayerPrefs.SetInt(medalStatus2, 1);
        }

        if (KilledEnemy == enemyAmount)
        {
            string medalStatus3 = Const.KILL_ALL_ENEMY_MEDAL + mapLevel;
            PlayerPrefs.SetInt(medalStatus3, 1);
        }

        // Unlock next level
        string levelStatus = Const.UNLOCKED_LEVEL + (mapLevel + 1);
        PlayerPrefs.SetInt(levelStatus, 1);

        StartCoroutine("EndLevel");
    }

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1.5f);

        PlayerController.Instance.StopCoroutine("Shoot");
        PlayerController.Instance.rigid2D.constraints = RigidbodyConstraints2D.FreezePosition;

        UIManager.Instance.victory.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        GameOver = true;
		AdsControl.Instance.showAds ();
        UIManager.Instance.GameOver();
    }
}
