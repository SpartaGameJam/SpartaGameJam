using UnityEngine;
using UnityEngine.UI;

public class UI_Coin : MonoBehaviour
{
    public RectTransform ImageRect => imageRect;
    RectTransform imageRect;
    Image image;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        imageRect = image.GetComponent<RectTransform>();
        image.enabled = false;
    }
}
