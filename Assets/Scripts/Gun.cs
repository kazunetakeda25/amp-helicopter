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

public class Gun : MonoBehaviour
{

    protected List<GameObject> bulletList;

    protected bool activateGun = false;

    [Space(20)]
    public Transform firePoint;
    public Transform bulletsHolder;
    public GameObject[] bulletTypes;
    public float[] fireRateTypes;

    // Behaviour messages
    protected void Start()
    {
        // Initialize bullets holder
        bulletList = new List<GameObject>();
    }

    // Behaviour messages
    protected void Update()
    {
        if (!activateGun)
        {
            if (GameController.Instance.StartFire)
            {
                activateGun = true;
                StartCoroutine("Shoot");
            }
        }
    }

    private IEnumerator Shoot()
    {
        while (!GameController.Instance.GameOver)
        {
            if (bulletList.Count == 0)
            {
                if (tag == Const.PLAYER_TAG)
                {
                    if (GameController.Instance.mapLevel == 0)
                    {
                        CreateNewBullet(bulletTypes[PlayerController.Instance.maxUpgradedGunLvl - 1]);
                    }
                    else
                    {
                        CreateNewBullet(bulletTypes[PlayerPrefs.GetInt(Const.PLAYER_GUN_LEVEL, 1) - 1]);
                    }
                }
                else if (tag == Const.DRONES_TAG)
                {
                    if (GameController.Instance.mapLevel == 0)
                    {
                        CreateNewBullet(bulletTypes[PlayerController.Instance.maxUpgradedDronesLvl - 3]);
                    }
                    else
                    {
                        int dronesLvl = PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0);
                        if (dronesLvl != 10)
                            CreateNewBullet(bulletTypes[PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0) - 1]);
                        else
                            CreateNewBullet(bulletTypes[PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0) - 2]);

                    }
                }
            }
            else
            {
                for (var i = bulletList.Count - 1; i >= 0; i--)
                {
                    if (!bulletList[i].activeInHierarchy)
                    {
                        bulletList[i].SetActive(true);
                        break;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (tag == Const.PLAYER_TAG)
                            {
                                if (GameController.Instance.mapLevel == 0)
                                {
                                    CreateNewBullet(bulletTypes[PlayerController.Instance.maxUpgradedGunLvl - 1]);
                                }
                                else
                                {
                                    CreateNewBullet(bulletTypes[PlayerPrefs.GetInt(Const.PLAYER_GUN_LEVEL, 1) - 1]);
                                }
                            }
                            else if (tag == Const.DRONES_TAG)
                            {
                                if (GameController.Instance.mapLevel == 0)
                                {
                                    CreateNewBullet(bulletTypes[PlayerController.Instance.maxUpgradedDronesLvl - 3]);
                                }
                                else
                                {
                                    int dronesLvl = PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0);
                                    if (dronesLvl != 10)
                                        CreateNewBullet(bulletTypes[PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0) - 1]);
                                    else
                                        CreateNewBullet(bulletTypes[PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0) - 2]);

                                }
                            }
                        }
                    }
                }
            }

            float timeWait = 1.0f;

            if (GameController.Instance.mapLevel == 0)
            {
                if (tag == Const.PLAYER_TAG)
                    timeWait = fireRateTypes[PlayerController.Instance.maxUpgradedGunLvl - 1];
                else if (tag == Const.DRONES_TAG)
                    timeWait = fireRateTypes[PlayerController.Instance.maxUpgradedDronesLvl - 2];
            }
            else
            {
                if (tag == Const.PLAYER_TAG)
                    timeWait = fireRateTypes[PlayerPrefs.GetInt(Const.PLAYER_GUN_LEVEL, 1) - 1];
                else if (tag == Const.DRONES_TAG)
                {
                    int dronesLvl = PlayerPrefs.GetInt(Const.DRONES_LEVEL, 0);
                    if (dronesLvl != 10)
                        timeWait = fireRateTypes[dronesLvl - 1];
                    else
                        timeWait = fireRateTypes[dronesLvl - 2];
                }
            }

            yield return new WaitForSeconds(timeWait);
        }
    }

    private void CreateNewBullet(GameObject bulletPrefab)
    {
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        newBullet.GetComponent<BulletMove>().SetStartPosition(firePoint);
        newBullet.transform.position = firePoint.position;
        newBullet.transform.SetParent(bulletsHolder);
        bulletList.Add(newBullet);
    }
}
