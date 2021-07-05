using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeLater
{
    private Queue<Action> actions = new Queue<Action>();
    private float actionsPerSecond;


    public InvokeLater(float actionsPerSecond)
    {
        this.actionsPerSecond = actionsPerSecond;
    }

    public IEnumerator MainCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => actions.Count > 0);

            while (actions.Count > 0)
            {
                actions.Dequeue().Invoke();
                yield return new WaitForSeconds(1 / actionsPerSecond);
            }
        }
    }

    public IEnumerator AddNextFrame(Action action)
    {
        yield return null;
        actions.Enqueue(action);
    }
}