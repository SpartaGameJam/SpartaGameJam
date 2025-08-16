using Spine.Unity;
using UnityEngine;

public class SpineCutScene : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    private void Start()
    {
        SpineController.Instance.skeletonGraphicCutscine = skeletonGraphic;
        SpineController.Instance.StartCutScene(false, 1f);
    }
}
