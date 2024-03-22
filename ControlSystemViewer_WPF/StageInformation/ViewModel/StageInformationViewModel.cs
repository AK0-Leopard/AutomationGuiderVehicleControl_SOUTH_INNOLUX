//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

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

namespace StageInformation.ViewModel
{
	public class StageInformationViewModel : INotifyPropertyChanged
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

		public StageInformationViewModel()
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
			app.ObjCacheManager.PortStationUpdateComplete += ObjCacheManager_PortDefUpdateComplete;
		}

		private void ObjCacheManager_PortDefUpdateComplete(object sender, EventArgs e)
		{
			PortViews = null;
			InitPortDefs();
		}

		private void InitPortDefs()
		{
			var portDefs = app.ObjCacheManager.GetPortStation().Where(p => p.PORT_ID.StartsWith("OHT"));
            //var aa = portDefs.Where(data => data.PLCPortID == "B7_OHBLINE1_T01");

            PortViews = portDefs.Select(t => new PortView(t, app)).ToList();
        }
	}

	public class Stage
	{
		public Stage(bool loadPosition, string loadPositionCST, string CST_ID)
		{
			LoadPosition = loadPosition;
			//LoadPositionBOX = loadPositionBOX;
			//LoadPositionCST = loadPositionCST; 
			LoadPositionCST = loadPositionCST?.Trim();
			if (LoadPosition && string.IsNullOrEmpty(LoadPositionCST))
				LoadPositionCST = CST_ID?.Trim();
		}

		public Stage(bool loadPosition) : this(loadPosition, null, null)
		{
		}

		public bool LoadPosition { get; set; }
		//public string LoadPositionBOX { get; set; }
		public string LoadPositionCST { get; set; }
	}

	public class PortView
	{
		private APORTSTATION _portDef = null;
		private WindownApplication app;

		public PortView(APORTSTATION portDef, WindownApplication app)
		{
			this._portDef = portDef;
			this.app = app;
		}
		public SolidColorBrush BorderColor
		{
			get 
			{ 
				if (_portDef.PORT_SERVICE_STATUS == com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage.PortStationServiceStatus.InService)
					return new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
				else // NoDefinition, OutOfService
					return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
			}
		}

		public string ID => _portDef.PORT_ID;
		//public E_PortType PortType => _portDef.PORT_TYPE;
		//public E_PORT_STATUS? AGVState => _portDef.AGVState;

		public Stage[] Stages => new[]
				{
#if IS_FOR_OHTC_NOT_AGVC
					new Stage(_portDef.IsCSTPresenceLoc1,_portDef.CSTPresenceID1, _portDef.CST_ID),
					new Stage(_portDef.IsCSTPresenceLoc2,_portDef.CSTPresenceID2, _portDef.CST_ID),
					new Stage(_portDef.IsCSTPresenceLoc3,_portDef.CSTPresenceID3, _portDef.CST_ID),
					new Stage(_portDef.IsCSTPresenceLoc4,_portDef.CSTPresenceID4, _portDef.CST_ID),
					new Stage(_portDef.IsCSTPresenceLoc5,_portDef.CSTPresenceID5, _portDef.CST_ID)
#else
					new Stage(_portDef.IsCSTPresence,_portDef.CST_ID, _portDef.CST_ID)
#endif
				};

		//public int StageNum => Convert.ToInt32(_portDef.stageCount);
		public int StageNum => 1;

		public IEnumerable<Stage> ShowStage
		{
			get
			{
				return Stages.Take(StageNum);
			}
		}

		public string StrPortType
		{
			get
			{
				if (_portDef.IsInPutMode)
				{
					return "<------";
				}
				else
				{
					return "------>";
				}
				//return PortType == E_PortType.In ? "<------" : "------>";
			}
		}
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