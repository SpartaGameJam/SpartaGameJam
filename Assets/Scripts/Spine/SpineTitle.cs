using System.Collections;
using Spine.Unity;
using UnityEngine;

public class SpineTitle : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    private void Start()
    {
        SpineController.Instance.skeletonGraphicTitle = skeletonGraphic;
        SpineController.Instance.StartTitle(false, 1f);
    }
}
