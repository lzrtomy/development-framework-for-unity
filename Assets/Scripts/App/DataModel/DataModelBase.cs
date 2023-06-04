using Company.Tools;
using System;
using System.Collections;

namespace Company.NewApp.Models
{
    public abstract class DataModelBase
    {
        protected InitState m_InitState = InitState.NotInit;

        public DataModelManager DataModelManager;


        public InitState InitState { get { return m_InitState; } }

        public virtual void Init(params object[] parameters)
        {
            m_InitState = InitState.NotInit;
        }

        protected virtual void StartReadFile(Uri uri, Action<string> onComplete, Action onFailed = null)
        {
            m_InitState = InitState.Inited;
            IEnumerator ie = FileManager.Instance.IEReadFile(
                uri,
                (string text) =>
                {
                    onComplete?.Invoke(text);
                    m_InitState = InitState.Inited;
                },
                onFailed);
            UnityTools.Instance.StartIE(ie);
        }
    }
}