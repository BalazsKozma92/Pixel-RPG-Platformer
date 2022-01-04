using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    Transform player;
    SpriteRenderer mySprite;
    SpriteRenderer playerSprite;
    Color color;
    float activeTime = .1f;
    float timeActivated;
    float alpha;
    float alphaSet = .8f;
    float alphaDecay = 8f;

    void OnEnable()
    {
        mySprite = GetComponent<SpriteRenderer>();
        player = Player.Instance.transform;
        playerSprite = player.GetComponentInChildren<SpriteRenderer>();

        alpha = alphaSet;
        mySprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;
        color = new Color(1f, 1f, 1f, alpha);
        mySprite.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }   
    }
}
