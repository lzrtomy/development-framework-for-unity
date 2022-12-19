using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AbstractRawEntityModel<T, P>
    where T : class, new()
    where P : AbstractRawEntity
{
    protected List<P> _list;
    protected Dictionary<int, P> _dic;

    protected string CREATED_RAW_ENTITIES_PATH = "/Scripts/Game/EntityData/AutoCreatedRawEntities/";
    protected string RAW_ENTITY_DATA_PATH = "AutoCreatedRawEntityData/";

    /// <summary>
    /// 配表文件名称
    /// </summary>
    protected abstract string FileName { get; }

    public AbstractRawEntityModel()
    {
        _list = new List<P>();
        _dic = new Dictionary<int, P>();

    }

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    private string StreamingAssetsPath
    {
        get
        {
#if UNITY_EDITOR
            return "file://" + Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID && !UNITY_EDITOR
        return Application.streamingAssetsPath + "/";  //"jar: file://" + Application.persistentDataPath + "!/assets/";
#elif UNITY_IOS && !UNITY_EDITOR
        return "file://" + Application.streamingAssetsPath + "/";
#endif
            return "file://" + Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected abstract P MakeEntity(GameEntityParser parse);

    /// <summary>
    /// 加载实体数据
    /// </summary>
    private void LoadEntityData()
    {
        using (GameEntityParser parse = new GameEntityParser(Application.dataPath + CREATED_RAW_ENTITIES_PATH + FileName))
        {
            while (!parse.Eof)
            {
                //创建实体
                P p = MakeEntity(parse);
                _list.Add(p);
                _dic[p.Id] = p;
                parse.Next();
            }
        }
    }

    public IEnumerator IELoadData()
    {
        string path = StreamingAssetsPath + RAW_ENTITY_DATA_PATH + FileName;
        byte[] bytes;
        using (UnityWebRequest uwb = UnityWebRequest.Get(path)) 
        {
            yield return uwb.SendWebRequest();
            bytes = uwb.downloadHandler.data;
        }
        using (GameEntityParser parse = new GameEntityParser(bytes))
        {
            while (!parse.Eof)
            {
                //创建实体
                P p = MakeEntity(parse);
                _list.Add(p);
                _dic[p.Id] = p;
                parse.Next();
            }
        }
    }


    /// <summary>
    /// 获取集合
    /// </summary>
    /// <returns></returns>
    public List<P> GetList()
    {
        return _list;
    }

    /// <summary>
    /// 根据编号获取实体
    /// </summary>
    /// <returns></returns>
    public P GetEntityById(int id)
    {
        if (_dic.ContainsKey(id))
            return _dic[id];
        return null;
    }
}
