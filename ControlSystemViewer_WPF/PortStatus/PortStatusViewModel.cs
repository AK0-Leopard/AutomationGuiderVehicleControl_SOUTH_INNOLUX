using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.ohxc.wpf.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ViewerObject;

namespace PortStatus
{
	public class PortStatusViewModel : INotifyPropertyChanged
	{
		private WindownApplication app;

		private List<PortView> portViews;

		public event PropertyChangedEventHandler PropertyChanged;

		public List<PortView> PortViews
		{
			get => portViews;
			set
			{
				portViews = value;
				RaisePropertyChanged();
			}
		}

		public PortStatusViewModel()
		{
		}

		internal void RaisePropertyChanged([CallerMemberName] string propertyname = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
		}

		public void Start(WindownApplication _app)
		{
			app = _app;
			InitPortDefs();
			app.ObjCacheManager.PortStationChange += ObjCacheManager_PortStationChange;
		}

		private void ObjCacheManager_PortStationChange(object sender, EventArgs e)
		{
			InitPortDefs();
		}

		private void InitPortDefs()
		{
			var portDefs = app.ObjCacheManager.GetMonitoringPorts();
			PortViews = portDefs.Select(t => new PortView(t, app)).ToList();
		}
	}

	public class Stage
	{
		public Stage(bool loadPosition, string loadPositionCST)
		{
			LoadPosition = loadPosition;
			LoadPositionCST = loadPositionCST;
		}

		public Stage(bool loadPosition) : this(loadPosition, null)
		{
		}

		public bool LoadPosition { get; set; }
		//public string LoadPositionBOX { get; set; }
		public string LoadPositionCST { get; set; }
	}

	public class PortView
	{
		private VPORTSTATION _portDef = null;
		private WindownApplication app;

		public PortView(VPORTSTATION portDef, WindownApplication app)
		{
			this._portDef = portDef;
			this.app = app;
		}
		public SolidColorBrush BorderColor => _portDef.IS_IN_SERVICE ? new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)) :
																	   new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

		public string ID => _portDef.PORT_ID;
		//public E_PortType PortType => _portDef.PORT_TYPE;
		//public E_PORT_STATUS? AGVState => _portDef.AGVState;

		public Stage[] Stages => new[]
				{
					new Stage(_portDef.IS_PRESENCE_LOC_1,_portDef.CST_ID_LOC_1),
					new Stage(_portDef.IS_PRESENCE_LOC_2,_portDef.CST_ID_LOC_2),
					new Stage(_portDef.IS_PRESENCE_LOC_3,_portDef.CST_ID_LOC_3),
					new Stage(_portDef.IS_PRESENCE_LOC_4,_portDef.CST_ID_LOC_4),
					new Stage(_portDef.IS_PRESENCE_LOC_5,_portDef.CST_ID_LOC_5)
				};

		public int StageNum => Convert.ToInt32(_portDef.COUNT_STAGE);

		public IEnumerable<Stage> ShowStage => Stages.Take(StageNum);

		public string StrPortType => _portDef.IS_INPUT_MODE ? "<------" : "------>";
	}

	public class BackConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Stage stage)
			{
				if (stage.LoadPosition)
				{
					return new SolidColorBrush(Colors.LimeGreen);
				}
				else if (!string.IsNullOrWhiteSpace(stage.LoadPositionCST))
				{
					return new SolidColorBrush(Colors.White);
				}
			}
			return new SolidColorBrush(Colors.Gray);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}