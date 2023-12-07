using System.Collections;
using UnityEngine;

namespace TimmyFramework
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines _instance
        {
            get
            {
                if (_coroutines == null)
                {
                    var gameObject = new GameObject("[COROUTINE MANAGER]");
                    _coroutines = gameObject.AddComponent<Coroutines>();
                    DontDestroyOnLoad(gameObject);
                }

                return _coroutines;
            }
        }

        private static Coroutines _coroutines;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return _instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                _instance.StopCoroutine(coroutine);
            }
        }
    }
}