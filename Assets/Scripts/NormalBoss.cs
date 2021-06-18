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

public class NormalBoss : Enemy
{

    private Animation anim;

    private SpriteRenderer spriteRender;

    private Transform rocketsHolder;

    private List<GameObject> rocketList;

    private float
        startPos = 0.0f,
        endPos = 0.0f;

    private bool activeSecondStage = false;

    public GameObject rocketPrefab;
    public float rocketFireRate = 0.1f;
    public int rocketLimit = 1;

    // Behaviour messages
    new void Awake()
    {
        base.Awake();

        anim = GetComponentInParent<Animation>();

        spriteRender = GetComponent<SpriteRenderer>();

        rocketsHolder = GameObject.FindGameObjectWithTag(Const.ENEMY_AMMO_2_TAG).transform;
    }

    // Behaviour messages
    new void Start()
    {
        base.Start();

        rocketList = new List<GameObject>();

        SetStartPosAndEndPos();
    }

    private void SetStartPosAndEndPos()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float offset = spriteRender.bounds.size.x / 2 + 0.5f;

        startPos = worldPoint.x + worldScreenWidth + offset;
        endPos = worldPoint.x + worldScreenWidth - offset;

        firstTargetPos = new Vector3(endPos, transform.position.y, 0.0f);
    }

    // Behaviour messages
    new void Update()
    {
        base.Update();

        if (!firstStage)
        {
            if (!activeSecondStage)
            {
                activeSecondStage = true;
                anim.Play();
            }
        }
    }

    private IEnumerator ShootRocket()
    {
        int count = 0;

        // Get the rockets available
        Transform[] rockets = rocketsHolder.GetComponentsInChildren<Transform>(true);

        for (var i = rockets.Length - 1; i > 0; i--)
        {
            rocketList.Add(rockets[i].gameObject);
        }

        while (!GameController.Instance.GameOver)
        {
            if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
            {
                SoundManager.Instance.enemyShootRocket.Play();
            }

            if (rocketList.Count == 0)
            {
                CreateNewRocket();
            }
            else
            {
                for (var i = rocketList.Count - 1; i >= 0; i--)
                {
                    if (!rocketList[i].activeInHierarchy)
                    {
                        rocketList[i].transform.position = transform.position;
                        rocketList[i].SetActive(true);
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewRocket();
                        }
                    }
                }
            }
            count++;

            if (count >= rocketLimit)
            {
                count = 0;
                yield return new WaitForSeconds(fireRate + 1.5f);
            }
            else
            {
                yield return new WaitForSeconds(rocketFireRate);
            }
        }
    }

    private void CreateNewRocket()
    {
        GameObject newRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);

        newRocket.transform.SetParent(rocketsHolder);
        rocketList.Add(newRocket);
    }

    // Behaviour messages
    void OnEnable()
    {
        firstStage = true;
        firstTargetPos = new Vector3(endPos, transform.position.y, 0.0f);

        if (originalHP != 0)
        {
            HP = originalHP;
            hpBar.localScale = new Vector3(HP / (float)originalHP, 1.0f, 1.0f);
        }
    }

    // Behaviour messages
    void OnDisable()
    {
        transform.position = new Vector3(startPos, transform.position.y, 0.0f);

        activeSecondStage = false;
    }
}
