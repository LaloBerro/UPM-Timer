using System;
using System.Collections.Generic;
using UnityEngine;

namespace Timer.Runtime.Realtime.Domain
{
    public class RealtimeTimerExecutor : MonoBehaviour, IRealtimeTimerExecutor
    {
        private Dictionary<string, RealtimeTimer> _timers;
        private List<string> _finishedTimersToRemove;

        private bool _isInitialized;

        public void Initialize()
        {
            _isInitialized = true;
            _timers = new Dictionary<string, RealtimeTimer>();
            _finishedTimersToRemove = new List<string>();
        }

        public void StartTimer(RealtimeTimer realtimeTimer)
        {
            if(!_isInitialized)
                Initialize();
            
            var realtimeTimerId = realtimeTimer.Id;
            if (_timers.ContainsKey(realtimeTimerId))
                throw new Exception("You are trying to start a timer that is already started");
            
            realtimeTimer.SetElapsedTime(0);
            _timers.Add(realtimeTimerId, realtimeTimer);
        }

        private void Update()
        {
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            if (ReferenceEquals(_timers, null) || _timers.Count <= 0)
                return;
            
            foreach (var (key, realtimeTimer) in _timers)
            {
                if(realtimeTimer.IsPaused)
                    continue;
                
                bool isFinished = UpdateTimer(realtimeTimer);
                if(!isFinished)
                    continue;
                
                _finishedTimersToRemove.Add(key);
            }
            
            if(_finishedTimersToRemove.Count <= 0)
                return;

            FinishTimers();
        }
        
        private bool UpdateTimer(RealtimeTimer realtimeTimer)
        {
            float elapsedTime = realtimeTimer.ElapsedTime;
            elapsedTime += Time.deltaTime;
            realtimeTimer.SetElapsedTime(elapsedTime);
            
            float seconds = elapsedTime % 60f;
            
            if (seconds < realtimeTimer.Duration)
                return false;
            
            return true;
        }
        
        private void FinishTimers()
        {
            foreach (var key in _finishedTimersToRemove)
            {
                RealtimeTimer realtimeTimer = _timers[key];
                _timers.Remove(key);
                realtimeTimer.FinishTimer();
            }

            _finishedTimersToRemove.Clear();
        }

        public void PauseAll()
        {
            if (ReferenceEquals(_timers, null) || _timers.Count <= 0)
                return;
            
            foreach (var (key, realtimeTimer) in _timers)
            {
                realtimeTimer.Pause();
            }
        }

        public void ResumeAll()
        {
            if (ReferenceEquals(_timers, null) || _timers.Count <= 0)
                return;

            foreach (var (key, realtimeTimer) in _timers)
            {
                realtimeTimer.Resume();
            }
        }

        public float GetElapsedTime(string id)
        {
            bool isContained = _timers.ContainsKey(id);
            if (!isContained)
                throw new Exception("Error trying to Get Elapsed Time because the timer is not contained");

            float elapsedTime = _timers[id].ElapsedTime;
            return elapsedTime;
        }
    }
}