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

public class Enemy : MonoBehaviour
{

    private Transform
        firePoint1,
        firePoint2,
        firePoint3;

    protected Transform bulletsHolder;

    protected List<GameObject> bulletList;

    protected Vector3
        currentVelocity,
        firstTargetPos;

    protected int originalHP = 0;

    protected bool firstStage = false;

    public float smoothTime = 0.4f;

    [Space(10)]
    public int HP = 100;
    public int bonus = 1;
    public Transform hpBar;
    public int amountOfCoin;

    [Space(10)]
    public GameObject bulletPrefab;
    public float fireRate = 0.1f;
    public int bulletLimit = 1;

    // Behaviour messages
    protected void Awake()
    {
        firePoint1 = GameObject.FindGameObjectWithTag(Const.FIRE_POINT_1_TAG).transform;
        firePoint2 = GameObject.FindGameObjectWithTag(Const.FIRE_POINT_2_TAG).transform;
        firePoint3 = GameObject.FindGameObjectWithTag(Const.FIRE_POINT_3_TAG).transform;

        bulletsHolder = GameObject.FindGameObjectWithTag(Const.ENEMY_AMMO_TAG).transform;
    }

    // Behaviour messages
    protected void Start()
    {
        // Initialize bullets holder
        bulletList = new List<GameObject>();

        originalHP = HP;
    }

    public void SetHP(int value)
    {
        originalHP = HP = value;
    }

    // Behaviour messages
    protected void Update()
    {
        if (firstStage)
        {
            transform.position = Vector3.SmoothDamp(transform.position, firstTargetPos, ref currentVelocity, smoothTime);

            // Check if have reached the destination position
            if ((transform.position.x - firstTargetPos.x) < 0.1f)
            {
                firstStage = false;
                if (name == "Big Boss" || name == "Boss")
                {
                    StartCoroutine(Shoot(firePoint1));
                    StartCoroutine(Shoot(firePoint2));
                    StartCoroutine(Shoot(firePoint3));

                    if (name == "Boss")
                    {
                        StartCoroutine("ShootRocket");
                    }
                }
                else
                {
                    StartCoroutine("Shoot");
                }
            }
        }
    }

    private IEnumerator Shoot(Transform firePoint)
    {
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
                CreateNewBullet(firePoint);
            }
            else
            {
                if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
                {
                    SoundManager.Instance.enemyShootSpread.Play();
                }

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
                            CreateNewBullet(firePoint);
                        }
                    }
                }
            }
            count++;

            if (count >= bulletLimit)
            {
                count = 0;
                yield return new WaitForSeconds(2.0f);
            }
            else
            {
                yield return new WaitForSeconds(fireRate);
            }
        }
    }

    private void CreateNewBullet(Transform firePoint)
    {
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        newBullet.transform.SetParent(bulletsHolder);
        bulletList.Add(newBullet);
    }

    // Behaviour messages
    void OnEnable()
    {
        firstStage = true;
        firstTargetPos = new Vector3(GameController.Instance.endPosBigBoss, transform.position.y, 0.0f);

        if (originalHP != 0)
        {
            HP = originalHP;
            hpBar.localScale = new Vector3(HP / (float)originalHP, 1.0f, 1.0f);
        }
    }

    // Behaviour messages
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == Const.PLAYER_BULLET_TAG || col.tag == Const.PLAYER_MINI_BULLET_TAG)
        {
            if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
            {
                SoundManager.Instance.shootHitEnemy.Play();
            }

            HP -= col.GetComponent<BulletMove>().damage;

            if (HP <= 0)
                hpBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            else
                hpBar.localScale = new Vector3(HP / (float)originalHP, 1.0f, 1.0f);

            if (HP <= 0)
            {
                if (PlayerPrefs.GetInt(Const.SOUND, 1) == 1)
                {
                    if (this.tag == Const.BELVEDERE_TAG)
                        SoundManager.Instance.explodeTower.Play();
                    else
                    {
                        if (name != "Boss" && name != "Big Boss")
                        {
                            SoundManager.Instance.enemySmallExplode.Play();
                        }
                        else
                        {
                            SoundManager.Instance.enemyMediumExplode.Play();
                        }
                    }
                }

                GameController.Instance.KilledEnemy++;

                GameController.Instance.CreateEffect(
                    Const.EXPLOSION_EFFECT,
                    GameController.Instance.explosionPrefab,
                    GameController.Instance.explosionHolder,
                    transform.position);

                GameController.Instance.SpawnBounty(amountOfCoin, transform.position);

                if (name == "Big Boss")
                {
                    GameController.Instance.BossBonus += bonus;
                    GameController.Instance.UpdateScore();
                    GameController.Instance.Victory();
                }
                else if (name == "Boss")
                {
                    if (GameController.Instance.mapLevel != 5 && GameController.Instance.mapLevel != 6)
                    {
                        GameController.Instance.BossBonus += bonus;
                        GameController.Instance.UpdateScore();
                        GameController.Instance.Victory();
                    }
                    else
                    {
                        if (!GameController.Instance.BossDead_1)
                        {
                            GameController.Instance.BossDead_1 = true;
                        }
                        else
                        {
                            if (GameController.Instance.mapLevel == 5)
                            {
                                GameController.Instance.BossBonus += bonus;
                                GameController.Instance.UpdateScore();
                                GameController.Instance.Victory();
                            }
                            else
                            {
                                GameController.Instance.BossDead_2 = true;
                            }
                        }
                    }
                }
                else
                {
                    GameController.Instance.EnemyBonus += bonus;
                    GameController.Instance.UpdateScore();
                }

                if (name != "Boss")
                    gameObject.SetActive(false);
                else
                    gameObject.transform.parent.gameObject.SetActive(false);
            }

            col.gameObject.SetActive(false);
        }
    }
}
