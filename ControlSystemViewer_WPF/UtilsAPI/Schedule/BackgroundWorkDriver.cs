//*********************************************************************************
//      BackgroundWorkDriver.cs
//*********************************************************************************
// File Name: BackgroundWorkDriver.cs
// Description: 背景執行驅動器
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
// 2014/07/06    Hayes Chen     N/A            N/A     Initial Release
// 2014/08/04    Hayes Chen     N/A            A0.01   增加License Key的檢查
// 2018/03/14    Kevin Wei      N/A            A0.02   修正，Fun:doWork。確保在執行BackgroundPLCWork時不會因為搜尋
//                                                     Dictionary時因找不到Key值，而跳出Exception。
//**********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.ohxc.wpf.Schedule
{
    /// <summary>
    /// Class BackgroundWorkDriver.
    /// </summary>
    public class BackgroundWorkDriver
    {
        /// <summary>
        /// Key: Work Key
        /// Value: Work Item List
        /// </summary>
        private Dictionary<string, Queue<BackgroundWorkItem>> workDic =
            new Dictionary<string, Queue<BackgroundWorkItem>>();
        /// <summary>
        /// The work key FLG dic
        /// </summary>
        private Dictionary<string, WorkKeyFlag> workKeyFlgDic = new Dictionary<string, WorkKeyFlag>();
        /// <summary>
        /// The _lock
        /// </summary>
        private Object _lock = new Object();
        /// <summary>
        /// The work
        /// </summary>
        private IBackgroundWork work;
        /// <summary>
        /// The maximum background queue count
        /// </summary>
        private long MaxBackgroundQueueCount = 0;
        /// <summary>
        /// The logger
        /// </summary>
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Gets the name of the driver.
        /// </summary>
        /// <value>The name of the driver.</value>
        public string DriverName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkDriver"/> class.
        /// </summary>
        /// <param name="work">The work.</param>
        public BackgroundWorkDriver(IBackgroundWork work)
        {
            this.work = work;
            this.MaxBackgroundQueueCount = work.getMaxBackgroundQueueCount();
            this.DriverName = work.getDriverName();
        }

        /// <summary>
        /// Triggers the background work.
        /// </summary>
        /// <param name="workKey">The work key.</param>
        /// <param name="workItem">The work item.</param>
        public void triggerBackgroundWork(string workKey, BackgroundWorkItem workItem)
        {
            string wKey = workKey.Trim();
            WorkKeyFlag flag = null;
            lock (_lock)
            {
                Queue<BackgroundWorkItem> queue = null;
                if (!workDic.ContainsKey(wKey))
                {
                    queue = new Queue<BackgroundWorkItem>();
                    workDic.Add(wKey, queue);
                }
                else
                {
                    queue = workDic[wKey];
                }
                if (!workKeyFlgDic.ContainsKey(wKey))
                {
                    flag = new WorkKeyFlag();
                    workKeyFlgDic.Add(wKey, flag);
                }
                else
                {
                    flag = workKeyFlgDic[wKey];
                }
                if (MaxBackgroundQueueCount > 0 && queue.Count >= MaxBackgroundQueueCount)
                {
                    queue.Dequeue();
                    logger.Debug("Over Max Background Queue Count[DriverName:{0}][Count:{1}]", DriverName, 
                        MaxBackgroundQueueCount);
                }
                queue.Enqueue(workItem);

                //workDic[wKey].Add(workItem);
            }
            if (flag.Compare(WorkKeyFlag.UNSET))
            {
                ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(doWork), wKey);
                //doWork(wKey);
            }
        }

        /// <summary>
        /// Does the work.
        /// </summary>
        /// <param name="wKey">The w key.</param>
        public void doWork(Object wKey)
        {
            string workKey = wKey as string;
            WorkKeyFlag flag = null;
            lock (_lock)
            {
                //A0.01 flag = workKeyFlgDic[workKey];
                //Start A0.01
                if (!workKeyFlgDic.ContainsKey(workKey))
                {
                    logger.Warn(string.Format("workKeyFlgDic ,key:{0} not exist."
                                              , workKey));
                    return;
                }
                else
                {
                    flag = workKeyFlgDic[workKey];
                }
                //End A0.01

            }
            if (flag.CompareAndSet(WorkKeyFlag.UNSET, WorkKeyFlag.SET))
            {
                while (true)
                {
                    try
                    {
                        BackgroundWorkItem workItem = null;
                        lock (_lock)
                        {
                            if (!workDic.ContainsKey(workKey) || workDic[workKey].Count == 0)
                            {
                                if (workDic.ContainsKey(workKey))
                                {
                                    workDic.Remove(workKey);
                                }
                                if (workKeyFlgDic.ContainsKey(workKey))
                                {
                                    workKeyFlgDic.Remove(workKey);
                                }
                                break;
                            }
                            workItem = workDic[workKey].Dequeue();
                        }
                        work.doWork(workKey, workItem);
                    }
                    catch (Exception ex) 
                    {
                        logger.WarnException("Do Work Exception !", ex);
                    }
                }

                flag.CompareAndSet(WorkKeyFlag.SET, WorkKeyFlag.UNSET);
            }
        }
    }

    /// <summary>
    /// 背景執行器實際執行事項之介面類別。
    /// 所有使用背景執行器者，必須實作此介面，實現執行事項。
    /// </summary>
    public interface IBackgroundWork
    {
        /// <summary>
        /// Does the work.
        /// </summary>
        /// <param name="workKey">The work key.</param>
        /// <param name="item">The item.</param>
        void doWork(string workKey, BackgroundWorkItem item);
        /// <summary>
        /// Gets the maximum background queue count.
        /// </summary>
        /// <returns>System.Int64.</returns>
        long getMaxBackgroundQueueCount();
        /// <summary>
        /// Gets the name of the driver.
        /// </summary>
        /// <returns>System.String.</returns>
        string getDriverName();
    }

    /// <summary>
    /// Class WorkKeyFlag.
    /// </summary>
    public class WorkKeyFlag
    {
        /// <summary>
        /// The set
        /// </summary>
        public static readonly int SET = 1;
        /// <summary>
        /// The unset
        /// </summary>
        public static readonly int UNSET = 0;

        //public int EventPoint { get; set; }
        /// <summary>
        /// The event point
        /// </summary>
        private int eventPoint;
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkKeyFlag"/> class.
        /// </summary>
        public WorkKeyFlag()
        {
            eventPoint = UNSET;
        }

        /// <summary>
        /// Exchanges the specified update.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>System.Int32.</returns>
        public int Exchange(int update)
        {
            return Interlocked.Exchange(ref eventPoint, update);
        }

        /// <summary>
        /// Compares the specified expect.
        /// </summary>
        /// <param name="expect">The expect.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Compare(int expect)
        {
            return Interlocked.CompareExchange(ref eventPoint, eventPoint, expect) == expect;
        }

        /// <summary>
        /// Compares the and set.
        /// </summary>
        /// <param name="expect">The expect.</param>
        /// <param name="update">The update.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CompareAndSet(int expect, int update)
        {
            return Interlocked.CompareExchange(ref eventPoint, update, expect) == expect;
        }
    }
}
