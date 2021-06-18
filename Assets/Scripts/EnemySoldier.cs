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

public class EnemySoldier : Enemy
{

    private SpriteRenderer spriteRender;

    private float
        startPos = 0.0f,
        endPos = 0.0f;

    private bool
        secondStage = false,
        activeSecondStage = false;

    public Transform firePoint;

    [Space(10)]
    public float maxPosSpawn = 3.0f;
    public float
        minPosSpawn = -4.0f,
        firstPosition1 = 8.0f,
        firstPosition2 = 9.7f;

    [Space(10)]
    public float speed = 15.0f;
    public float waitTimeBeforeMove = 0.5f;

    // Behaviour messages
    new void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();

        bulletsHolder = GameObject.FindGameObjectWithTag(Const.ENEMY_AMMO_TAG).transform;
    }

    public void SetStartPosAndEndPos()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float offset = spriteRender.bounds.size.x / 2 + 0.5f;

        if (tag != Const.BELVEDERE_TAG)
        {
            startPos = worldPoint.x + worldScreenWidth + offset;
        }
        else
        {
            startPos = transform.position.x;
        }

        endPos = worldPoint.x - offset;
    }

    // Behaviour messages
    new void Update()
    {
        if (tag != Const.BELVEDERE_TAG && tag != Const.ENEMY_CAR_TAG)
        {
            base.Update();

            if (!firstStage)
            {
                if (!activeSecondStage)
                {
                    activeSecondStage = true;
                    StartCoroutine("StartSecondStage");
                }
                if (secondStage)
                {
                    transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);

                    if (transform.position.x <= endPos)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (!activeSecondStage)
            {
                activeSecondStage = true;
                StartCoroutine("Shoot");
            }

            smoothTime = waitTimeBeforeMove = maxPosSpawn = minPosSpawn = firstPosition1 = firstPosition2 = Mathf.Infinity;

            if (tag == Const.BELVEDERE_TAG)
            {
                if (!GameController.Instance.GameOver)
                    transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);
            }
            else
            {
                transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);
            }

            if (transform.position.x <= endPos)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator Shoot()
    {

        if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
        {
            if (bulletLimit == 1)
            {
                SoundManager.Instance.enemyShootSinger.Play();
            }
            else
            {
                SoundManager.Instance.enemyShootSpread.Play();
            }
        }

        int count = 0;

        // Get the bullets available
        Transform[] bullets = bulletsHolder.GetComponentsInChildren<Transform>(true);

        for (var i = bullets.Length - 1; i > 0; i--)
        {
            bulletList.Add(bullets[i].gameObject);
        }

        while (!GameController.Instance.GameOver)
        {
            if (bulletList.Count == 0)
            {
                CreateNewBullet();
            }
            else
            {
                for (var i = bulletList.Count - 1; i >= 0; i--)
                {
                    if (!bulletList[i].activeInHierarchy)
                    {
                        bulletList[i].transform.position = firePoint.position;
                        bulletList[i].SetActive(true);
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            CreateNewBullet();
                        }
                    }
                }
            }
            count++;

            if (count >= bulletLimit)
            {
                break;
            }

            yield return new WaitForSeconds(fireRate);
        }
    }

    private void CreateNewBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        newBullet.transform.SetParent(bulletsHolder);
        bulletList.Add(newBullet);
    }

    private IEnumerator StartSecondStage()
    {
        yield return new WaitForSeconds(waitTimeBeforeMove);
        secondStage = true;

        yield break;
    }

    // Behaviour messages
    void OnDisable()
    {
        if (tag != Const.BELVEDERE_TAG && tag != Const.ENEMY_CAR_TAG)
        {
            transform.position = new Vector3(startPos, Random.Range(minPosSpawn, maxPosSpawn), 0.0f);

            firstTargetPos = new Vector3(Random.Range(firstPosition1, firstPosition2), transform.position.y, 0.0f);

            firstStage = true;
            secondStage = false;
        }
        else
        {
            transform.position = new Vector3(startPos, transform.position.y, 0.0f);
        }

        activeSecondStage = false;
    }

    // Behaviour messages
    void OnEnable()
    {
        if (originalHP != 0)
        {
            HP = originalHP;
            hpBar.localScale = new Vector3(HP / (float)originalHP, 1.0f, 1.0f);
        }
    }
}
