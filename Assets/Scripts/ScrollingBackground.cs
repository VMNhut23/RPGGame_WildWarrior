using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public RectTransform[] images; 
    public float scrollSpeed = 50f;
    private float imageWidth;

    void Start()
    {
        if (images.Length == 0) return;

        imageWidth = images[0].rect.width;
    }

    void Update()
    {
        foreach (var image in images)
        {
            image.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;
            if (image.anchoredPosition.x <= -imageWidth)
            {
                image.anchoredPosition += new Vector2(imageWidth * images.Length, 0);
            }
        }
    }
}
