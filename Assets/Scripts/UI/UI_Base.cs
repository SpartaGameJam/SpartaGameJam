using UnityEngine;

public abstract class UI_Base : MonoBehaviour
{
    protected virtual void Awake()
    {
        gameObject.name = GetType().Name;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}