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
        //��ȿ� �ִ� �÷��̾�� scene ��ȯ
        //���??
        //��� �÷��̾���� ��� ���¿� ����
        //start�� ������ ������ �� ��ȯ -> ���� enterGame?

        Managers.Scene.LoadScene(EScene.GameScene);
    }
}