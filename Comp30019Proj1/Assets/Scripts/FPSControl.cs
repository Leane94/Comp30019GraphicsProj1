using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Set fps to 30 and display current fps on screen
/// </summary>
// Created by Chao
public class FPSControl : MonoBehaviour {

    private int targetFPS = 30;
    private float deltaTime = 0.0f;

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float currFPS = 1.0f / deltaTime;
        string text = string.Format("{0:0.} fps", currFPS);
        this.GetComponent<Text>().text = text;

	}
}
