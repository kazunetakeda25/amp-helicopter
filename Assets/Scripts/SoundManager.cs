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

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    public AudioSource
        menuTheme,
        mapTheme,
        click,
        gameplayTheme,
        playerExplode,
        chopGoingLoop,
        enemyShootSinger,
        enemyShootSpread,
        enemyShootRocket,
        rescue,
        getCoin,
        explodeTower,
        enemySmallExplode,
        enemyMediumExplode,
        shootHitEnemy,
        victory,
        levelMapSelect,
        needToUnlock,
        nextStep,
        hangarUpdate,
        takeDamage;

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
        if (PlayerPrefs.GetInt(Const.MUSIC, 1) == 1)
        {
            menuTheme.Play();
        }
    }
}
