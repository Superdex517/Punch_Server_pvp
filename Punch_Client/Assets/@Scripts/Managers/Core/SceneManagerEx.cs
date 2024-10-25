using UnityEngine;
using UnityEngine.SceneManagement;

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
}