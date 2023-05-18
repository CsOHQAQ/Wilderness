using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using UnityEngine.SceneManagement;
using System;
using QxFramework.Utilities;

public class LoadSceneManager : Singleton<LoadSceneManager>, ISystemModule
{

    //加载进度
    public float LoadProgress
    {
        get
        {
            if (_asyncOperation == null)
            {
                return _asyncOperation.progress;
            }
            else
            {
                return 1f;
            }
        }
    }

    public string CurrentLevel
    {
        get
        {
            return _currentLevel;
        }

        set
        {
            _currentLevel = value;
        }
    }

    public bool FirstEnter
    {
        get
        {
            return firstEnter;
        }
    }

    private string _currentLevel = string.Empty;

    private Action<string> _onCompleted = null;

    private AsyncOperation _asyncOperation = null;

    private bool firstEnter = true;

    public void OpenLevel(string levelName, Action<string> onCompleted)
    {
        if (_onCompleted != null)
        {
            Debug.LogError("[LevelManager] 已经正在加载一个关卡中......");
        }

        if (string.IsNullOrEmpty(levelName))
        {
            levelName = CurrentLevel;
            CloseLevel();
        }
        //Debug.Log("Open "+levelName);
        CurrentLevel = levelName;
        _onCompleted = onCompleted;
        _asyncOperation = SceneManager.LoadSceneAsync(CurrentLevel, LoadSceneMode.Additive);


        if (firstEnter)
        {
            Debug.Log("#Procedure第一次进入"+levelName);
        }
        else
        {
            
        }
    }

    public void CloseLevel()
    {
        if (!string.IsNullOrEmpty(CurrentLevel))
        {
            Debug.Log("#ProcedureClose "+CurrentLevel);
            SceneManager.UnloadSceneAsync(CurrentLevel);
            CurrentLevel = string.Empty;
        }
    }

    public override void Initialize()
    {
        SceneManager.sceneLoaded += LoadCompleted;
    }

    private void LoadCompleted(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("正将场景" + scene.name + "激活");
        SceneManager.SetActiveScene(scene);
        if (_onCompleted != null)
        {
            _onCompleted(scene.name);
            _onCompleted = null;
        }
    }

    public void Update(float deltaTime)
    {

    }

    public void FixedUpdate(float deltaTime)
    {

    }

    public void Dispose()
    {
        SceneManager.sceneLoaded -= LoadCompleted;
    }

}

