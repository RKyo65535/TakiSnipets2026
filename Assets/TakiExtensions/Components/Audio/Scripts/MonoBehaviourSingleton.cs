using System;
using UnityEngine;


//まだ未検証です
namespace TakiExtensions.Audio
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;

        // staticなメソッドでは、当然自分自身を取得できない
        // そのため、staticなUnityAPIでなるべく同等となる処理を試みている
        // 具体的には、現在有効なオブジェクトのうち、今回ターゲットとするオブジェクトを対象としている。
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof(T);
                    instance = (T)FindAnyObjectByType(t);
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            Debug.Log("インスタンスチェック: " + instance);

            if(instance == null)
            {
                instance = GetComponent<T>();
                if (instance != null)
                {
                    Debug.Log("Singleton instance created: " + instance);

                    DontDestroyOnLoad(gameObject);
                    LateAwake();
                }
            }
            else if (this == Instance)
            {
                Debug.LogWarning($"Instance of {typeof(T)} already exists. Destroying duplicate.");
                LateAwake();
                return;
            }
            else
            {
                Destroy(this);
            }
        }

        protected abstract void LateAwake();
    }
}