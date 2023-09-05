using System;
using System.Collections;
using NUnit.Framework;
using Timer.Runtime.Realtime.Domain;
using UnityEngine;
using UnityEngine.TestTools;

namespace Timer.Runtime.Tests
{
    public class RealtimeTimerExecutionTests
    {
        private IRealtimeTimerExecutor _realtimeTimerExecutor;
        private RealtimeTimer _realtimeTimer;
        
        [SetUp]
        protected void SetUp()
        {
            _realtimeTimerExecutor = new GameObject("RealtimeTimerExecution").AddComponent<RealtimeTimerExecutor>();
            _realtimeTimer = new RealtimeTimer(_realtimeTimerExecutor);
        }

        [TearDown]
        protected void TearDown()
        {
            _realtimeTimerExecutor = null;
            _realtimeTimer = null;
        }
        
        [UnityTest]
        public IEnumerator Initialize_DontThrowErrors()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();
            
            yield return null;
            
            //Assert and Act
            Assert.DoesNotThrow(() => { _realtimeTimerExecutor.StartTimer(_realtimeTimer);});
        }
        
        [UnityTest]
        public IEnumerator StartTimer_UpdateTimer()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();
            DateTime dateTimeBeforeStart = DateTime.Now;
            double timeDifference = 0;
            _realtimeTimer.OnTimerFinished += () =>
            {
                DateTime dateTimeAfterStart = DateTime.Now;
                
                timeDifference = (dateTimeAfterStart - dateTimeBeforeStart).TotalSeconds;
            };
            
            //Act
            int duration = 2;
            _realtimeTimer.Start(duration);
            
            yield return new WaitForSeconds(duration + 1 );
            
            Assert.GreaterOrEqual(timeDifference, duration - 0.01f);
        }
        
        [UnityTest]
        public IEnumerator StartTimer_ThrowErrorForMultipleTimer()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();

            //Act
            int duration = 2;
            _realtimeTimer.Start(duration);
            
            yield return null;
            
            Assert.Throws<Exception>(() => { _realtimeTimer.Start(duration);});
        }
        
        [UnityTest]
        public IEnumerator Pause_TimerIsPaused()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();
            bool isCalled = false;
            _realtimeTimer.OnTimerFinished += () =>
            {
                isCalled = true;
            };
            
            //Act
            int duration = 2;
            _realtimeTimer.Start(duration);
            
            yield return new WaitForSeconds(duration / 2f);
            
            _realtimeTimer.Pause();
            
            yield return new WaitForSeconds(duration + 1.5f);
            
            Assert.AreEqual(false, isCalled);
        }
        
        [UnityTest]
        public IEnumerator Resume_TimerIsResume()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();
            DateTime dateTimeBeforeStart = default;
            double timeDifference = 0;
            _realtimeTimer.OnTimerFinished += () =>
            {
                DateTime dateTimeAfterStart = DateTime.Now;
                
                timeDifference = (dateTimeAfterStart - dateTimeBeforeStart).TotalSeconds;
            };
            
            //Act
            int duration = 4;
            _realtimeTimer.Start(duration);

            var durationDivided = duration / 4f;
            yield return new WaitForSeconds(durationDivided);
            
            _realtimeTimer.Pause();
            
            yield return new WaitForSeconds(durationDivided);
            
            _realtimeTimer.Resume();
            dateTimeBeforeStart = DateTime.Now;
            
            yield return new WaitForSeconds(duration);
            
            //Assert
            Assert.GreaterOrEqual(timeDifference, duration - durationDivided - 0.01f);
        }
    }
}