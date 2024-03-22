using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
	#region MTBF Data Class

	public class VMTBF
	{
		public string EQPT_ID { get; set; }

		public double MTBF
		{
			get
			{
				if (AlarmCount == 0) return double.PositiveInfinity;
				return Math.Round(dActiveTime / (AlarmCount + 1), 2);
			}
		}

		public int AlarmCount { get; set; }
		private double dActiveTime { get; set; }
		public double ActiveTime
		{
			get
			{
				return dActiveTime;
			}
		}
		public string TimeInterval
		{
			get
			{
				return dTimeInterval.ToString("N0");
			}
		}
		private double dTimeInterval { get; set; }

		public VMTBF()
		{
			EQPT_ID = "";
			AlarmCount = 0;
			dActiveTime = 0.0;
		}

		public void AddActiveTime(double value)
		{
			dActiveTime += value;

		}

		public void AddTimeInterval(DateTime Start_Time, DateTime End_Time)
		{
			dTimeInterval = End_Time.Subtract(Start_Time).TotalHours;

		}
	}

	public class FirstReport_MTBF
	{
		public string EQPT_ID { get; set; }
		public DateTime First_RPT_DataTime { get; set; }
		public DateTime Final_Clear_DateTime { get; set; }

	}

	#endregion
}
