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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeFiver(FiverState.Start, false, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeFiver(FiverState.Ing, true, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeFiver(FiverState.End, false, 1);
        }
    }

    // 사용 ChangeFiver(FiverState, 반복 여부, 재생 시간)
    public void ChangeFiver(FiverState state, bool isLoop, float timeScale)
    {
        skeletonGraphicTimeFiver.Initialize(true);

        fiverState = state;

        skeletonGraphicTimeFiver.AnimationState.SetAnimation((int)state, fiverClip[(int)state]
            , isLoop).TimeScale = timeScale;
        skeletonGraphicTimeFiver.color = Color.white;
        skeletonGraphicTimeFiver.startingLoop = isLoop;
        skeletonGraphicTimeFiver.timeScale = timeScale;
    }

    public void StartTitle(bool isLoop, float timeScale)
    {
        skeletonGraphicTitle.Initialize(true);

        skeletonGraphicTitle.AnimationState.SetAnimation(0, titleClip, isLoop).TimeScale = timeScale;
        skeletonGraphicTitle.color = Color.white;
        skeletonGraphicTitle.startingLoop = isLoop;
        skeletonGraphicTitle.timeScale = timeScale;
    }
 
    public void StartCutScene(bool isLoop, float timeScale)
    {
        skeletonGraphicCutscine.Initialize(true);

        skeletonGraphicCutscine.AnimationState.SetAnimation(0, cutsceneClip, isLoop).TimeScale = timeScale;
        skeletonGraphicCutscine.color = Color.white;
        skeletonGraphicCutscine.startingLoop = isLoop;
        skeletonGraphicCutscine.timeScale = timeScale;
    }
}
