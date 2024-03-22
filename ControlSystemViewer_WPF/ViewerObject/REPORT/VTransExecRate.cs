using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
    public class VTransExecRate
    {
        public VTransExecRate(string _HostSource,string _HostDestination,
                               int _MCSTotalCount, int _EntryCount, int _CompletedCount, int _EmptyRetrievalErrorCount, int _DoubleStorageErrorCount, int _InterLockCount, int _EQPTAbortErrorCount,
                               int _BarcodeReadFailCount, int _ScanCount, int _LoadingInterLock, int _UnLoadingInterLock,
                              double _AvgLeadTime, //int _LeadTime, int _MaxLeadTime, int _MinLeadTime,
                              int _CycleTime, int _MaxCycleTime, int? _MinCycleTime,double _AvgCycleTime,
                              DateTime StartTime,DateTime EndTime,
                               int _TransTime128, int _TransExecTime128,
                              double _TotalTransExecRate =0.0)//僅計算Total資料時填參數，因Total計算較特別
        {
            hostsource = _HostSource;
            hostdestination = _HostDestination;
            
            //Count
            mcstotalcount = _MCSTotalCount;
            entrycount = _EntryCount;
            completedcount = _CompletedCount;
            emptyretrievalerrorcount = _EmptyRetrievalErrorCount;
            doublestorageerrorcount = _DoubleStorageErrorCount;
            interlockcount = _InterLockCount;
            vehicleabort = _EQPTAbortErrorCount;
            barcodereadfailcount = _BarcodeReadFailCount;
            scancount = _ScanCount;
            loadinginterlockcount = _LoadingInterLock;
            unloadinginterlock = _UnLoadingInterLock;
            //Time

            avgleadtime = _AvgLeadTime;

            cycletime = _CycleTime;
            maxcycletime = _MaxCycleTime;
            mincycletime = _MinCycleTime;
            avgcycletime = _AvgCycleTime;

            TransTime128 = _TransTime128;
            TransExecTime128 = _TransExecTime128;

            if (_TotalTransExecRate ==0.0)
            {
                utilizationrate = Math.Round(_CycleTime / (EndTime.Subtract(StartTime).TotalMinutes) * 100, 2);
            }
            else
            {
                utilizationrate = _TotalTransExecRate;
            }
           
        }

        private string hostsource{get;set;}
        public string HostSource
        {
            get { return hostsource; }
        }

        private string hostdestination { get; set; }
        public string HostDestination
        {
            get { return hostdestination; }
        }

        #region Count
        private int mcstotalcount { get; set; }
        public int MCSTotalCount
        {
            get { return mcstotalcount; }
        }

        private int entrycount { get; set; }
        public int EntryCount
        {
            get { return entrycount; }
        }

        private int completedcount { get; set; }
        public int Completed
        {
            get { return completedcount; }
        }

        public double CMDSuccessRate
        {
            get { return Math.Round((double)completedcount / entrycount * 100, 2); ; }
        }

        private int vehicleabort { get; set; }
        public int VehicleAbort
        {
            get { return vehicleabort; }
        }

        private int interlockcount { get; set; }
        public int InterLock
        {
            get { return interlockcount; }
        }
        private int loadinginterlockcount { get; set; }
        public int LoadingInterLock
        {
            get { return loadinginterlockcount; }
        }

        private int unloadinginterlock { get; set; }
        public int UnLoadingInterLock
        {
            get { return unloadinginterlock; }
        }

        private int emptyretrievalerrorcount { get; set; }
        public int EmptyRetrieval
        {
            get { return emptyretrievalerrorcount; }
        }

        private int doublestorageerrorcount { get; set; }
        public int DoubleStorage
        {
            get { return doublestorageerrorcount; }
        }

       

        private int barcodereadfailcount { get; set; }
        public int BarcodeRead
        {
            get { return barcodereadfailcount; }
        }

        private int scancount { get; set; }
        public int Scan
        {
            get { return scancount; }
        }

        
        #endregion

        private double utilizationrate { get; set; }
        public double UtilizationRate
        {
            get { return utilizationrate; }
        }


       

        #region Time
        //CycleTime
        private int cycletime { get; set; }
        public int CycleTime//Minute
        {
            get { return cycletime; }
        }

        private double avgcycletime { get; set; }
        public double AvgCycleTime//Second
        {
            get { return avgcycletime; }
        }

        private int maxcycletime { get; set; }
        public int MaxCycleTime//Second
        {
            get { return maxcycletime; }
        }

        private int? mincycletime { get; set; }
        public int? MinCycleTime//Second
        {
            get { return mincycletime; }
        }


        //LeadTime
        //private int leadtime { get; set; }
        //public int LeadTime//Minute
        //{
        //    get { return leadtime; }
        //}

        private double avgleadtime { get; set; }
        public double AvgLeadTime//Second
        {
            get { return avgleadtime; }
        }

        //private int maxleadtime { get; set; }
        //public int MaxLeadTime//Second
        //{
        //    get { return maxleadtime; }
        //}

        //private int minleadtime { get; set; }
        //public int MinLeadTime//Second
        //{
        //    get { return minleadtime; }
        //}

        public int TransTime128 { get; set; }
        public int TransExecTime128 { get; set; }
        #endregion


        #region M4 Customer
        public int LoadPIOError { get; set; } = 0;
        public int UnLoadPIOError { get; set; } = 0;
        #endregion
    }

    public class VTransExecRateByVHID
    {
        public VTransExecRateByVHID(string _VHID,
                              int _MCSTotalCount,int _EntryCount, int _CompletedCount, int _EmptyRetrievalErrorCount, int _DoubleStorageErrorCount, int _InterLockCount,int _EQPTAbortErrorCount,
                              int _BarcodeReadFailCount, int _ScanCount, int _LoadingInterLock, int _UnLoadingInterLock,
                              double _AvgLeadTime, //int _LeadTime, int _MaxLeadTime, int _MinLeadTime, 
                              int _CycleTime, int _MaxCycleTime, int? _MinCycleTime, double _AvgCycleTime,
                              DateTime StartTime, DateTime EndTime,
                              int _TransTime128, int _TransExecTime128,
                               double _TotalTransExecRate = 0.0)//僅計算Total資料時填參數，因Total計算較特別
        {
            vhid = _VHID;

            //Count
            mcstotalcount = _MCSTotalCount;
            entrycount = _EntryCount;
            completedcount = _CompletedCount;
            emptyretrievalerrorcount = _EmptyRetrievalErrorCount;
            doublestorageerrorcount = _DoubleStorageErrorCount;
            interlockcount = _InterLockCount;
            vehicleabort = _EQPTAbortErrorCount;
            barcodereadfailcount = _BarcodeReadFailCount;
            scancount = _ScanCount;
            loadinginterlockcount = _LoadingInterLock;
            unloadinginterlock = _UnLoadingInterLock;

            //Time
            avgleadtime = _AvgLeadTime;

            cycletime = _CycleTime;
            maxcycletime = _MaxCycleTime;
            mincycletime = _MinCycleTime;
            avgCycletime = _AvgCycleTime;

            TransTime128 = _TransTime128;
            TransExecTime128 = _TransExecTime128;

            if (_TotalTransExecRate == 0.0)
            {
                utilizationrate = Math.Round(_CycleTime / (EndTime.Subtract(StartTime).TotalMinutes) * 100, 2);
            }
            else
            {
                utilizationrate = _TotalTransExecRate;
            }
        }

        private string vhid { get; set; }
        public string VHID
        {
            get { return vhid; }
        }


        #region Count
        private int mcstotalcount { get; set; }
        public int MCSTotalCount
        {
            get { return mcstotalcount; }
        }

        private int entrycount { get; set; }
        public int EntryCount
        {
            get { return entrycount; }
        }

        private int completedcount { get; set; }
        public int Completed
        {
            get { return completedcount; }
        }

        public double CMDSuccessRate
        {
            get { return Math.Round((double)completedcount / entrycount * 100, 2); ; }
        }

        private int vehicleabort { get; set; }
        public int VehicleAbort
        {
            get { return vehicleabort; }
        }

        private int interlockcount { get; set; }
        public int InterLock
        {
            get { return interlockcount; }
        }

        private int loadinginterlockcount { get; set; }
        public int LoadingInterLock
        {
            get { return loadinginterlockcount; }
        }

        private int unloadinginterlock { get; set; }
        public int UnLoadingInterLock
        {
            get { return unloadinginterlock; }
        }

        private int emptyretrievalerrorcount { get; set; }
        public int EmptyRetrieval
        {
            get { return emptyretrievalerrorcount; }
        }

        private int doublestorageerrorcount { get; set; }
        public int DoubleStorage
        {
            get { return doublestorageerrorcount; }
        }

        private int barcodereadfailcount { get; set; }
        public int BarcodeRead
        {
            get { return barcodereadfailcount; }
        }

        private int scancount { get; set; }
        public int Scan
        {
            get { return scancount; }
        }
       
        #endregion

        private double utilizationrate { get; set; }
        public double UtilizationRate
        {
            get { return utilizationrate; }
        }


       

        #region Time
        //CycleTime
        private int cycletime { get; set; }
        public int CycleTime//Minute
        {
            get { return cycletime; }
        }

        private double avgCycletime { get; set; }
        public double AvgCycleTime//Second
        {
            get { return avgCycletime; }
        }

        private int maxcycletime { get; set; }
        public int MaxCycleTime//Second
        {
            get { return maxcycletime; }
        }

        private int? mincycletime { get; set; }
        public int? MinCycleTime//Second
        {
            get { return mincycletime; }
        }


        //LeadTime
        //private int leadtime { get; set; }
        //public int LeadTime//Minute
        //{
        //    get { return leadtime; }
        //}

        private double avgleadtime { get; set; }
        public double AvgLeadTime//Second
        {
            get { return avgleadtime; }
        }

        //private int maxleadtime { get; set; }
        //public int MaxLeadTime//Second
        //{
        //    get { return maxleadtime; }
        //}

        //private int minleadtime { get; set; }
        //public int MinLeadTime//Second
        //{
        //    get { return minleadtime; }
        //}

        public int TransTime128 { get; set; }
        public int TransExecTime128 { get; set; }
        #endregion

        #region M4 Customer
        public int LoadPIOError { get; set; } = 0;
        public int UnLoadPIOError { get; set; } = 0;
        #endregion
    }
}
