using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTimeManager
{
    float TimeSize { get; set; }
    GameDateTime GetNow();
    float GetNowLight();
    void DoStart();
    void Pause();
    bool IsPlaying();
    void RegisterTimeRepeat(Func<GameDateTime, bool> action, GameDateTime interval);
    bool StepMinute(float stepTime);
    bool IsDayTime();
}
