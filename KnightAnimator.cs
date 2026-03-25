using UnityEngine;
using UnityEngine.UI;

public class KnightAnimator : MonoBehaviour
{
    [SerializeField] private Image knightImage;
    [SerializeField] private Sprite[] idleSprites;  // 10 кадров
    [SerializeField] private float frameRate = 0.15f;  // Время между кадрами

    private int currentFrame = 0;
    private float frameTimer = 0f;

    private void Start()
    {
        if (knightImage == null)
            knightImage = GetComponent<Image>();

        if (idleSprites == null || idleSprites.Length == 0)
        {
            Debug.LogError("Idle sprites не загружены!");
            return;
        }

        knightImage.sprite = idleSprites[0];
    }

    private void Update()
    {
        if (idleSprites == null || idleSprites.Length == 0)
            return;

        frameTimer += Time.deltaTime;

        if (frameTimer >= frameRate)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % idleSprites.Length;
            knightImage.sprite = idleSprites[currentFrame];
        }
    }

    public void SetIdleSprites(Sprite[] newIdleSprites)
    {
        if (newIdleSprites == null || newIdleSprites.Length == 0) return;
        idleSprites = newIdleSprites;
        currentFrame = 0;
        frameTimer = 0f;
        if (knightImage != null)
            knightImage.sprite = idleSprites[0];
    }
}