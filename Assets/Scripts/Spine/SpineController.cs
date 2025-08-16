using Spine.Unity;
using UnityEngine;

public enum FiverState
{ 
    Start, Ing, End
}

public class SpineController : MonoBehaviour
{
    public static SpineController Instance { get; private set; }

    public SkeletonGraphic skeletonGraphicTimeFiver;
    public SkeletonGraphic skeletonGraphicTitle;
    public SkeletonGraphic skeletonGraphicCutscine;

    public AnimationReferenceAsset[] fiverClip;
    public AnimationReferenceAsset titleClip;
    public AnimationReferenceAsset cutsceneClip;

    private FiverState fiverState;


    private void Awake()
    {
        if(Instance == null) Instance = this;
    }


    // 사용 ChangeFiver(FiverState, 반복 여부, 재생 시간)
    public void ChangeFiver(FiverState state, bool isLoop, float timeScale)
    {
        fiverState = state;

        skeletonGraphicTimeFiver.AnimationState.SetAnimation((int)state, fiverClip[(int)state]
            , isLoop).TimeScale = timeScale;
        skeletonGraphicTimeFiver.color = Color.white;
        skeletonGraphicTimeFiver.startingLoop = isLoop;
        skeletonGraphicTimeFiver.timeScale = timeScale;
    }

    public void StartTitle(bool isLoop, float timeScale)
    {
        skeletonGraphicTitle.AnimationState.SetAnimation(0, titleClip, isLoop).TimeScale = timeScale;
        skeletonGraphicTitle.color = Color.white;
        skeletonGraphicTitle.startingLoop = isLoop;
        skeletonGraphicTitle.timeScale = timeScale;
    }
 
    public void StartCutScene(bool isLoop, float timeScale)
    {
        skeletonGraphicCutscine.AnimationState.SetAnimation(0, cutsceneClip, isLoop).TimeScale = timeScale;
        skeletonGraphicCutscine.color = Color.white;
        skeletonGraphicCutscine.startingLoop = isLoop;
        skeletonGraphicCutscine.timeScale = timeScale;
    }
}
