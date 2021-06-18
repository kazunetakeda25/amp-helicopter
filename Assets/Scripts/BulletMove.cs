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

public class BulletMove : MonoBehaviour
{

    private Transform startPoint;

    // Use this variable if this is enemy bullet
    private Vector3 playerDirect;

    private float endPos = 0.0f;

    public SpriteRenderer spriteRender;

    public float speed = 1.0f;

    public int damage = 10;

    // Behaviour messages
    void Start()
    {
        SetEndPosition();
    }

    private void SetEndPosition()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float offset = spriteRender.bounds.size.x / 2;

        if (tag == Const.ENEMY_BULLET_TAG || tag == Const.ENEMY_ROCKET_TAG)
        {
            endPos = worldPoint.x - offset;
        }
        else
        {
            endPos = worldPoint.x + worldScreenWidth + offset;
        }
    }

    // Behaviour messages
    void Update()
    {
        if (tag == Const.ENEMY_BULLET_TAG || tag == Const.ENEMY_ROCKET_TAG)
        {
            transform.position += playerDirect * Time.deltaTime * speed;

            // Make bullet move toward player
            float angle = Mathf.Atan2(playerDirect.y, playerDirect.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1.0f);

            if (transform.position.x <= endPos)
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            transform.position += new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);

            if (transform.position.x >= endPos)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    // Behaviour messages
    void OnEnable()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }

        if (tag == Const.ENEMY_BULLET_TAG || tag == Const.ENEMY_ROCKET_TAG)
        {
            float min = -1.4f, max = 1.4f;

            if (tag == Const.ENEMY_ROCKET_TAG)
            {
                min = -5.0f;
                max = 5.0f;
            }

            Vector3 PlayerPos = new Vector3(
                PlayerController.Instance.transform.position.x,
                PlayerController.Instance.transform.position.y + Random.Range(min, max), 0.0f);
            playerDirect = (PlayerPos - transform.position).normalized;
        }
    }

    public void SetStartPosition(Transform value)
    {
        startPoint = value;
    }
}
