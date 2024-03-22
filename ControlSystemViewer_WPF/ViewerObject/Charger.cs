using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace ViewerObject
{
    //public static class Charger_Def
    //{
    //    public enum ChargerStatus
    //    {
    //        Disable = 0,
    //        Enable,
    //        Auto,
    //        Manual,
    //        Charging,
    //        Error
    //    }
    //}

    public class Charger
    {
        public string ChargerID { get; private set; }
        public string AddressID { get; set; } = "";
        private int iAliveHeartbeat = 0;
        private int iAliveHeartbeat_Rec = 0;
        public int AliveHeartbeat
        {
            get => iAliveHeartbeat;
            set
            {
                if (iAliveHeartbeat != value)
                {
                    iAliveHeartbeat_Rec = iAliveHeartbeat;
                    iAliveHeartbeat = value;
                    IsAlive = true;
                }
            }
        }
        private bool isAlive = false;
        public bool IsAlive
        {
            get => isAlive;
            set
            {
                if (isAlive != value)
                {
                    isAlive = value;

                    if (isAlive)
                    {
                        checkAliveTimer.Stop();
                        checkAliveTimer.Start();
                        stopTimer = false;
                    }
                    else
                    {
                        checkAliveTimer.Stop();
                        stopTimer = true;
                    }
                        
                }
            }
        }
        private DispatcherTimer checkAliveTimer = new DispatcherTimer();
        private bool stopTimer = false;
        //public Charger_Def.ChargerStatus Status { get; set; }
        public Brush StatusBrush
        {
            get
            {
                if (!IsAlive) return Brushes.Gray;
                return Brushes.SkyBlue;            
            }
        }

        public Charger(string charger_id)
        {
            ChargerID = charger_id?.Trim() ?? "";

            checkAliveTimer.Interval = TimeSpan.FromSeconds(60);
            checkAliveTimer.Tick += checkAliveTimer_Tick;
        }

        private void checkAliveTimer_Tick(object sender, EventArgs e)
        {
            if (stopTimer) return;
            IsAlive = AliveHeartbeat != iAliveHeartbeat_Rec;
        }

        public void Update(Charger newCharger)
        {
            if (newCharger == null) return;
            if (newCharger.ChargerID != ChargerID ||
                newCharger.AddressID != AddressID) return;

            AliveHeartbeat = newCharger.AliveHeartbeat;
            //Status = newCharger.Status;
            Couplers = newCharger.Couplers;
        }


        public ConcurrentDictionary<string,Coupler> Couplers =new ConcurrentDictionary<string, Coupler>();

    }
}
