//-----------------------------------------------------------------------
// <copyright file="CameraPointer.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sends messages to gazed GameObject.
/// </summary>
public class CameraPointer : MonoBehaviour
{
    [SerializeField] private float gazeTime = 3;

    [SerializeField] private float maxDistance = 10;
    private GameObject _gazedAtObject = null;

    [SerializeField] private Image gazeClickIndicator;

    private float timeToClick = 0;

    private void Awake()
    {
        gazeClickIndicator.enabled = false;
    }

    private void Update()
    {
        if (timeToClick > 0)
        {
            timeToClick -= Time.deltaTime;

            if (timeToClick <= 0)
            {
                Click();
            }

            gazeClickIndicator.fillAmount = gazeTime - timeToClick;
        }

        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed at.
        if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                if (_gazedAtObject != null)
                    _gazedAtObject.SendMessage("OnPointerExit");
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnter");
                timeToClick = gazeTime;
                gazeClickIndicator.enabled = true;
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            if (_gazedAtObject != null)
                _gazedAtObject.SendMessage("OnPointerExit");
            _gazedAtObject = null;

            timeToClick = 0;
            gazeClickIndicator.enabled = false;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            Click();
        }
    }

    /// <summary>
    /// Click the camera pointer (gaze).
    /// </summary>
    private void Click()
    {
        if (_gazedAtObject != null)
            _gazedAtObject.SendMessage("OnPointerClick");
        timeToClick = 0;
        gazeClickIndicator.enabled = false;

        Debug.Log("Clicked");
    }
}