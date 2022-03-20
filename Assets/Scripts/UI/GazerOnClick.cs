using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GazerOnClick : MonoBehaviour
{
    [SerializeField] private UnityEvent onGazerClick;

    private void OnPointerClick()
    {
        onGazerClick?.Invoke();
    }
}