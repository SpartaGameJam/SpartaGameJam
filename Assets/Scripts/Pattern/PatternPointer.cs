using UnityEngine;
using UnityEngine.UI;

public class PatternPointer : MonoBehaviour
{
    public int id;
    public Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
}
