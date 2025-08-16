using Spine.Unity;
using UnityEngine;

public class SpineFiverTime : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    private void Start()
    {
        SpineController.Instance.skeletonGraphicTimeFiver = skeletonGraphic;
    }
}
