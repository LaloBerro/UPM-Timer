using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Timer.Runtime.Realtime.Domain;
using UnityEngine;
using UnityEngine.TestTools;

namespace Timer.Editor.Tests
{
    public class RealtimeTimerEditorTests
    {
        private IRealtimeTimerExecutor _realtimeTimerExecutor;
        private RealtimeTimer _realtimeTimer;
        
        [SetUp]
        protected void SetUp()
        {
            _realtimeTimerExecutor = Substitute.For<IRealtimeTimerExecutor>();
            _realtimeTimer = new RealtimeTimer(_realtimeTimerExecutor);
        }

        [TearDown]
        protected void TearDown()
        {
            _realtimeTimerExecutor = null;
            _realtimeTimer = null;
        }
        
        [Test]
        public void SetElapsedTime_CorrectValue()
        {
            //Act
            var elapsedTime = 30;
            _realtimeTimer.SetElapsedTime(elapsedTime);
            
            //Assert
            Assert.AreEqual(_realtimeTimer.ElapsedTime, elapsedTime);
        }
        
        [Test]
        public void StartTimer_CorrectDurationValue()
        {
            //Act
            float duration = 45;
            _realtimeTimer.Start(duration);
            
            //Assert
            Assert.AreEqual(_realtimeTimer.Duration, duration);
        }
        
        [Test]
        public void FinishTimer_InvokeEvent()
        {
            //Arrange
            float testNumber = 0;
            _realtimeTimer.OnTimerFinished += () => { testNumber = 1; };
            
            //Act
            _realtimeTimer.FinishTimer();
            
            //Assert
            Assert.Greater(testNumber, 0);
        }
        
        [Test]
        public void PauseAllTimers_CheckIfMethodIsCalled()
        {
            //Act
            _realtimeTimerExecutor.PauseAll();
            
            //Assert
            _realtimeTimer.ReceivedWithAnyArgs().Pause();
        }
        
        [Test]
        public void ResumeAllTimers_CheckIfMethodIsCalled()
        {
            //Act
            _realtimeTimerExecutor.ResumeAll();
            
            //Assert
            _realtimeTimer.ReceivedWithAnyArgs().Resume();
        }
    }   
}