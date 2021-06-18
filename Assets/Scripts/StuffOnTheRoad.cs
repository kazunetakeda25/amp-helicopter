/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;

public class StuffOnTheRoad : MonoBehaviour
{

    private SpriteRenderer spriteRender;

    private SpriteRenderer[] childsList;

    private float
        startPos = 0.0f,
        endPos = 0.0f;

    public float speed = 1.0f;

    // Behaviour messages
    void Awake()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
    }

    // Behaviour messages
    void Start()
    {
        SetStartPosAndEndPos();

        // Get all childs
        childsList = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void SetStartPosAndEndPos()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);

        float offset = spriteRender.bounds.size.x / 2 * (GetComponentsInChildren<SpriteRenderer>().Length);

        endPos = worldPoint.x - offset;
        startPos = transform.position.x;

        transform.position = new Vector3(startPos, transform.position.y, 0.0f);
    }

    // Behaviour messages
    void Update()
    {
        if (!GameController.Instance.GameOver)
        {
            transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);

            if (transform.position.x <= endPos)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    // Behaviour messages
    void OnDisable()
    {
        if (this.tag == Const.OBSTACLE_TAG)
        {
            transform.position = new Vector3(startPos, transform.position.y, 0.0f);
        }
        else
        {
            transform.position = new Vector3(startPos, Random.Range(-3.5f, 3.5f), 0.0f);
        }
    }

    // Behaviour messages
    void OnEnable()
    {
        // Active all coin
        if (this.tag == Const.BONUS_TAG)
        {
            int numberOfCoin = Random.Range(10, 17);
            if (numberOfCoin % 2 != 0)
            {
                numberOfCoin++;
            }

            if (childsList != null)
            {
                for (var i = childsList.Length - 1; i >= 0; i--)
                {
                    if (i < numberOfCoin)
                    {
                        childsList[i].gameObject.SetActive(true);
                    }
                    else
                        childsList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
