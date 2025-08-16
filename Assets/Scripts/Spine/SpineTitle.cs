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

        StartCoroutine(PlayTitle());
    }

    private IEnumerator PlayTitle()
    {
        yield return new WaitForSeconds(5f);

        SpineController.Instance.StartTitle(false, 1f);
    }
}
