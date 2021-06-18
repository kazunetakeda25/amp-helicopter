/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class InfiniteBackground : BackgroundMove
{

    private float posX = 0.0f;

    [Space(10)]
    public new MeshRenderer renderer;

    [Space(10)]
    public string sortingLayerName;
    public int sortingOrder;

    // Behaviour messages
    void Start()
    {
        renderer.sortingLayerName = sortingLayerName;
        renderer.sortingOrder = sortingOrder;
    }

    // Behaviour messages
    void Update()
    {
        if (!GameController.Instance.GameOver)
        {
            if (Time.timeScale != 0.0f)
            {
                if (GameController.Instance.StartFire)
                {
                    posX += normalSpeed;
                }
                else
                {
                    posX += slowSpeed;
                }

                if (posX > 1.0f)
                {
                    posX -= 1.0f;
                }

                renderer.material.mainTextureOffset = new Vector2(posX, 0.0f);
            }
        }
    }
}
