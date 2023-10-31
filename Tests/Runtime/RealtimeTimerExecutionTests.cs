using System;
using System.Collections;
using System.Collections.Generic;
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
        public IEnumerator StartTimerMultipleTimes_UpdateTimer()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();
            List<double> timeDifferences = new List<double>();
            List<DateTime> dateTimes = new List<DateTime>();
            
            //Act
            int duration = 2;
            
            DateTime dateTimeBeforeStart = DateTime.Now;
            for (int i = 0; i < 6; i++)
            {
                dateTimeBeforeStart = DateTime.Now;
                _realtimeTimer.OnTimerFinished = null;
                _realtimeTimer.OnTimerFinished += () =>
                {
                    DateTime dateTimeBeforeStartVar = dateTimeBeforeStart;
                    dateTimes.Add(dateTimeBeforeStartVar);
                };
                _realtimeTimer.Start(duration);
            
                yield return new WaitForSeconds(duration);
            }

            for (var index = 0; index < dateTimes.Count - 1; index++)
            {
                DateTime dateTime = dateTimes[index];
                DateTime nextDateTime = dateTimes[index + 1];
                timeDifferences.Add((nextDateTime - dateTime).TotalSeconds);
            }

            foreach (var timeDifference in timeDifferences)
            {
                double difference = Mathf.Abs((float)timeDifference - duration);
                Assert.LessOrEqual(difference, 0.01f);
            }
        }
        
        [UnityTest]
        public IEnumerator StartTimerMultipleTimes_UpdateTimerUntil()
        {
            //Arrange
            _realtimeTimerExecutor.Initialize();
            List<double> timeDifferences = new List<double>();
            List<DateTime> dateTimes = new List<DateTime>();
            List<float> time = new List<float>();
            
            //Act
            int duration = 2;

            int times = 4;
            int passedTimes = 0;

            bool isDone = false;
            
            _realtimeTimer.OnTimerFinished += () =>
            {
                dateTimes.Add(DateTime.Now);
                time.Add(Time.time);

                passedTimes++;

                if (passedTimes < times)
                    _realtimeTimer.Start(duration);
                else
                    isDone = true;
            };
            
            _realtimeTimer.Start(duration);
            
            yield return new WaitUntil(() => isDone);

            for (var index = 0; index < time.Count - 1; index++)
            {
                timeDifferences.Add((time[index + 1] - time[index]));
            }

            foreach (var timeDifference in timeDifferences)
            {

                double difference = Mathf.Abs((float)timeDifference - duration);
                Assert.LessOrEqual(difference, 0.01f);
            }
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

        [UnityTest]
        public IEnumerator CheckElapsedTime()
        {
            yield return null;

            //Arrange
            int duration = 4;
            _realtimeTimer.Start(duration);

            //Act
            yield return new WaitForSeconds(2f);
            
            _realtimeTimer.Pause();
            
            yield return new WaitForSeconds(2f);

            //Assert
            float elapsedTime = _realtimeTimer.ElapsedTime;
            Assert.GreaterOrEqual(elapsedTime, duration / 2f);
        }
    }
}