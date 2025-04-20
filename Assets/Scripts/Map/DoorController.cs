using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class DoorController : Sounds
{
    [Header("Sprites")]
    public Sprite closedSprite;

    private SpriteRenderer sr;
    private BoxCollider2D bc;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
    }

    public void Close()
    {
        PlaySound(sounds[0], 0.3f);
        sr.sprite = closedSprite;
        sr.color = new Color(1, 1, 1, 1);
        bc.enabled = true;
    }

    public void Open()
    {
        PlaySound(sounds[1], 0.3f);
        sr.color = new Color(1, 1, 1, 0);
        bc.enabled = false;
    }
}
