using System.Collections;
using UnityEngine;

namespace NeoCasual.GoingHyper
{
    public class CoroutineHelper : SingletonMono<CoroutineHelper>
    {
        public static Coroutine WaitForSeconds (float time, System.Action callback, bool isRealtime = true)
        {
            return Instance.StartCoroutine (IEnumeratorWaitForSeconds (time, callback, isRealtime));
        }

        private static IEnumerator IEnumeratorWaitForSeconds (float time, System.Action callback, bool isRealtime)
        {
            if (isRealtime)
            {
                yield return new WaitForSecondsRealtime (time);
            }
            else
            {
                yield return new WaitForSeconds (time);
            }

            callback?.Invoke ();
        }
    }
}