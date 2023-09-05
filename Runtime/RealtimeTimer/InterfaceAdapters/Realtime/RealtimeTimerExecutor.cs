using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Timer.Runtime.Realtime.Domain
{
    public class RealtimeTimerExecutor : MonoBehaviour, IRealtimeTimerExecutor
    {
        private Dictionary<string, RealtimeTimer> _timers;
        private IDisposable _updateDisposable;
        private List<string> _finishedTimersToRemove;

        public void Initialize()
        {
            _timers = new Dictionary<string, RealtimeTimer>();
            _finishedTimersToRemove = new List<string>();
        }

        public void StartTimer(RealtimeTimer realtimeTimer)
        {
            var realtimeTimerId = realtimeTimer.Id;
            if (_timers.ContainsKey(realtimeTimerId))
                throw new Exception("You are trying to start a timer that is already started");
            
            _timers.Add(realtimeTimerId, realtimeTimer);
            
            StartTimersExecution();
        }

        private void StartTimersExecution()
        {
            _updateDisposable = this.UpdateAsObservable().Subscribe(_ => { UpdateTimers(); });
        }

        private void UpdateTimers()
        {
            foreach (var realtimeTimerKV in _timers)
            {
                bool isFinished = UpdateTimer(realtimeTimerKV.Value, realtimeTimerKV.Key);
                if(!isFinished)
                    continue;
                
                _finishedTimersToRemove.Add(realtimeTimerKV.Key);
            }
            
            if(_finishedTimersToRemove.Count <= 0)
                return;

            foreach (var key in _finishedTimersToRemove)
            {
                _timers.Remove(key);
            }
            
            _timers.Clear();
        }

        private bool UpdateTimer(RealtimeTimer realtimeTimer, string key)
        {
            float elapsedTime = realtimeTimer.ElapsedTime;
            elapsedTime += Time.deltaTime;
            realtimeTimer.SetElapsedTime(elapsedTime);

            if (elapsedTime < realtimeTimer.Duration)
                return false;

            realtimeTimer.FinishTimer();
            return true;
        }

        public void Pause(string id)
        {
            bool isContained = _timers.TryGetValue(id, out RealtimeTimer realtimeTimer);
            if (!isContained)
                throw new Exception("Error trying to pause a timer that is not contained");
            
            _updateDisposable.Dispose();
        }

        public void Resume(string id)
        {
            bool isContained = _timers.TryGetValue(id, out RealtimeTimer realtimeTimer);
            if (!isContained)
                throw new Exception("Error trying to resume a timer that is not contained");
            
            StartTimersExecution();
        }
    }
}