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

public class Coin : MonoBehaviour
{

    private Rigidbody2D rigid2D;

    private SpriteRenderer spriteRender;

    private float endPos = 0.0f;

    // Behaviour messages
    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Behaviour messages
    void Start()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);

        float offset = spriteRender.bounds.size.y / 2;
        endPos = worldPoint.y - offset;
    }

    // Behaviour messages
    void OnEnable()
    {

        float velocityUp = Random.Range(5.5f, 10.0f);

        float velocityHorizontal = Random.Range(6.5f, 9.5f);

        int dir = -1;

        rigid2D.velocity = new Vector2(velocityHorizontal * dir, velocityUp);
    }

    // Behaviour messages
    void OnDisable()
    {
        transform.position = Vector3.zero;
    }

    // Behaviour messages
    void Update()
    {
        if (transform.position.y <= endPos)
        {
            gameObject.SetActive(false);
        }
    }
}
