using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitNow()
    {
        Manager.Instance.LoadPreviousScene();
    }
}