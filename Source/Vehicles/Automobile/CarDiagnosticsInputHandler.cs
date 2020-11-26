﻿using UnityEngine;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
        {
            diagnosticsPanel.SetActive(!diagnosticsPanel.activeSelf);

            if (!diagnosticsPanel.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.0f;
                // Cursor lock recovery handled in UnityInput.cs LateUpdate()
            }
        }

        if (Time.frameCount % 10 == 0 && diagnosticsPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }