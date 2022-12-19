using System.Collections;
using UnityEngine;
using Company.NewApp;
using Company.NewApp.Views;

public class MainController : SimpleUnitySingleton<MainController>
{
    [Header("»­²¼")]
    [SerializeField] RectTransform m_CanvasRect;

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

        UISelectLevelView view = UIManager.Instance.Open<UISelectLevelView>(ViewType.UISelectLevel, typeof(UISelectLevelView).Name, m_CanvasRect);

        yield break;
    }
}
