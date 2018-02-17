using System;
using System.Collections.Generic;
using UnityEngine;
namespace Tools.Helper
{
    /// <summary>  
    /// 定时任务管理器  
    /// </summary>  
    public class Time2Helper : MonoSingleton<Time2Helper>
    {
        /// <summary>  
        /// 定时任务列表  
        /// </summary>  
        private List<TimeTask> taskList = new List<TimeTask>();

        private Time2Helper() { }

        /// <summary>  
        /// 添加定时任务  
        /// </summary>  
        /// <param name="timeDelay">延时执行时间间隔</param>  
        /// <param name="repeat">是否可以重复执行</param>  
        /// <param name="timeTaskCallback">执行回调</param>  
        public void AddTask(float timeDelay, bool repeat, Action timeTaskCallback)
        {
            AddTask(new TimeTask(timeDelay, repeat, timeTaskCallback));
        }
        public void AddTask(TimeTask taskToAdd)
        {
            if (taskList.Contains(taskToAdd) || taskToAdd == null) return;
            taskList.Add(taskToAdd);
        }

        /// <summary>  
        /// 移除定时任务  
        /// </summary>  
        /// <param name="taskToRemove"></param>  
        /// <returns></returns>  
        public bool RemoveTask(Action taskToRemove)
        {
            if (taskList.Count == 0 || taskToRemove == null) return false;
            foreach (TimeTask item in taskList)
            {
                if (item.TimeTaskCallBack == taskToRemove)
                    return taskList.Remove(item);
            }
            return false;
        }

        void Update()
        {
            Tick();
        }

        /// <summary>  
        /// 执行定时任务  
        /// </summary>  
        private void Tick()
        {
            if (taskList == null) return;
            foreach (TimeTask task in taskList)
            {
                task.TimeDelay -= Time.deltaTime;
                if (task.TimeDelay <= 0)
                {
                    task.TimeTaskCallBack();
                    task.TimeDelay = task.TimeDelayOnly;
                }
                if (!task.Repeat)
                    taskList.Remove(task);
            }
        }
    }

    /// <summary>  
    /// 定时任务封装类  
    /// </summary>  
    public class TimeTask
    {
        /// <summary>  
        /// 延迟时间  
        /// </summary>  
        private float _timeDelay;
        /// <summary>  
        /// 延迟时间  
        /// </summary>  
        private float _timeDelayOnly;
        /// <summary>  
        /// 是否需要重复执行  
        /// </summary>  
        private bool _repeat;
        /// <summary>  
        /// 回调函数  
        /// </summary>  
        private Action _timeTaskCallBack;

        public float TimeDelay { get { return _timeDelay; } set { _timeDelay = value; } }
        public float TimeDelayOnly { get { return _timeDelayOnly; } }
        public bool Repeat { get { return _repeat; } set { _repeat = value; } }
        public Action TimeTaskCallBack { get { return _timeTaskCallBack; } }

        //构造函数  
        public TimeTask(float timeDelay, bool repeat, Action timeTaskCallBack)
        {
            _timeDelay = timeDelay;
            _timeDelayOnly = timeDelay;
            _repeat = repeat;
            _timeTaskCallBack = timeTaskCallBack;
        }

        public TimeTask(float timeDelay, Action timeTaskCallBack) : this(timeDelay, true, timeTaskCallBack) { }
    }
}