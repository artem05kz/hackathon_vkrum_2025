using UnityEngine;
using UnityEngine.UI;


public class Quiver : MonoBehaviour
{
    public Image quiverImage;
    public Sprite[] quiverSprites;

    void Start()
    {
        UpdateArrows(5);
    }

    public void UpdateArrows(int currentArrows)
    {
        if (quiverImage != null && quiverSprites != null)
        {
            currentArrows = Mathf.Clamp(currentArrows, 0, 5);
            quiverImage.sprite = quiverSprites[currentArrows];
        }
    }
}