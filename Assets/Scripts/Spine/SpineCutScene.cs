using System.Collections;
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
        StartCoroutine(PlayTitle());
    }

    private IEnumerator PlayTitle()
    {
        SpineController.Instance.StartCutScene(false, 1f);

        yield return new WaitForSeconds(7.5f);

        gameObject.SetActive(false);
    }
}
