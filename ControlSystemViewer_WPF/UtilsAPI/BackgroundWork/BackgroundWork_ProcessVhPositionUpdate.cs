//*********************************************************************************
//      BackgroundPLCWorkProcessData.cs
//*********************************************************************************
// File Name: BackgroundPLCWorkProcessData.cs
// Description: 背景執行上報Process Data至MES的實際作業
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
//
//**********************************************************************************
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Schedule;
using System;

namespace com.mirle.ibg3k0.ohxc.wpf.BackgroundWork
{
    public class BackgroundWork_ProcessVhPositionUpdate : IBackgroundWork
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public long getMaxBackgroundQueueCount()
        {
            return 1000;
        }

        public string getDriverName()
        {
            return this.GetType().Name;
        }

        public void doWork(string workKey, BackgroundWorkItem item)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
