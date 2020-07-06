using System;
using System.Collections;
using UnityEngine;

public static class StateEnumerators
{
    public static Func<IEnumerator> WaitForSeconds(float seconds)
    {
        return () => Enumerator();

        IEnumerator Enumerator()
        {
            for (float timer = 0f; timer < seconds; timer += Time.deltaTime)
            {
                yield return null;
            }
        }
    }
    public static Func<IEnumerator> WaitForSecondsRealtime(float seconds)
    {
        return () => Enumerator();

        IEnumerator Enumerator()
        {
            for (float timer = 0f; timer < seconds; timer += Time.unscaledDeltaTime)
            {
                yield return null;
            }
        }
    }
    public static Func<IEnumerator> WaitUntil(Func<bool> condition)
    {
        return () => Enumerator();

        IEnumerator Enumerator()
        {
            while (!condition.Invoke())
            {
                yield return null;
            }
        }
    }
    public static Func<IEnumerator> WaitWhile(Func<bool> condition)
    {
        return () => Enumerator();

        IEnumerator Enumerator()
        {
            while (condition.Invoke())
            {
                yield return null;
            }
        }
    }
}
