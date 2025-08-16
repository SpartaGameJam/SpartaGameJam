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
    public AnimationReferenceAsset[] fiverClip;

    private FiverState fiverState;

    private string CurrentAnimation;
    

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
}
