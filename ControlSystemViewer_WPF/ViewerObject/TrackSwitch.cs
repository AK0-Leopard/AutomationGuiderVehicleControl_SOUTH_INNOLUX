using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class TrackSwitch
    {
        public string ID { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public Section Track1 { get; private set; }
        public Section Track2 { get; private set; }
        public int AlarmCode { get; set; } = 0; // 0: 正常

        public bool AutoChangeToDefaultDir { get; set; } = false;
        public TrackSwitchDir DefaultDir { get; set; } = TrackSwitchDir.Unknown;

        private bool isAlive = false;
        public bool IsAlive
        {
            get { return isAlive; }
            set
            {
                if (isAlive != value)
                {
                    isAlive = value;
                    onStatusChanged();
                }
            }
        }

        private TrackSwitchStatus status = TrackSwitchStatus.Unknown;
        public TrackSwitchStatus Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    onStatusChanged();
                }
            }
        }
        public EventHandler StatusChanged;
        private void onStatusChanged()
        {
            StatusChanged?.Invoke(this, null);
        }

        private TrackSwitchDir dir = TrackSwitchDir.Unknown;
        public TrackSwitchDir Dir
        {
            get { return dir; }
            set
            {
                if (dir != value)
                {
                    dir = value;
                    onDirChanged();
                }
            }
        }
        public EventHandler DirChanged;
        private void onDirChanged()
        {
            DirChanged?.Invoke(this, null);
        }

        public TrackSwitch(string id, Section track1, Section track2)
        {
            ID = id;
            Track1 = track1;
            Track2 = track2;
            Address address1 = track1.StartAddress;
            Address address2 = track1.EndAddress;
            Address address3 = (track2.StartAdr == track1.StartAdr || track2.StartAdr == track1.EndAdr) ?
                               track2.EndAddress : track2.StartAddress;
            X = (address1.X + address2.X + address3.X) / 3;
            Y = (address1.Y + address2.Y + address3.Y) / 3;
        }
    }

    public enum TrackSwitchStatus
    {
        Unknown = 0,
        Manaul,
        Auto,
        Alarm
    }

    public enum TrackSwitchDir
    {
        Unknown = 0,
        Go_Track1,
        Go_Track2
    }
}
