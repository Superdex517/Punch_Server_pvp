using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.EScene type, Transform parents = null)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    private string GetSceneName(Define.EScene type)
    {
        string sceneName = System.Enum.GetName(typeof(Define.EScene), type);
        return sceneName;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    public void LoadGameScene()
    {
        //룸안에 있는 플레이어들 scene 전환
        //방법??
        //모든 플레이어들을 대기 상태에 놓고
        //start를 누르면 일제히 씬 전환 -> 각자 enterGame?

        Managers.Scene.LoadScene(EScene.GameScene);
    }
}