using System.Collections;
using UnityEngine;
using Company.NewApp;
using Company.NewApp.Views;
using Company.NewApp.Presenters;

public class MainController : SimpleUnitySingleton<MainController>
{
    // Start is called before the first frame update
    void Start()
    {
        if (AppLaunch.InitState == InitState.Inited)
        {
            Init();
        }
    }

    public void Init()
    {
        StartCoroutine(IEInit());
    }

    private IEnumerator IEInit()
    {
        UpdateManager.Instance.ClearAll();
        UIManager.Instance.ClearViews();
        ResourcesManager.Instance.ReleaseAll();

        yield return new WaitForSeconds(0.1f);

        UIManager.Instance.Open<UISelectLevelView>(ViewType.UISelectLevel);

        yield break;
    }
}
