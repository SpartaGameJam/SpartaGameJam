#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditPlayScene
{

    [MenuItem("StartScene/게임 실행 %[")] // 단축키 등록: ctrl + [
    public static void StartScene()
    {
        var firstScene = EditorBuildSettings.scenes[0].path;
        // AssetDatabase: 프로젝트에 포함된 에셋 접근
        // SceneAsset: 에디터에서 씬 참조
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(firstScene);

        // 플레이 모드의 시작 씬 설정
        EditorSceneManager.playModeStartScene = sceneAsset;
        EditorApplication.isPlaying = true;
    }

    [MenuItem("StartScene/현재 실행 %]")] // 단축키 등록: ctrl + ]
    public static void CurrentScene()
    {
        EditorSceneManager.playModeStartScene = null;
        EditorApplication.isPlaying = true;
    }
}
#endif