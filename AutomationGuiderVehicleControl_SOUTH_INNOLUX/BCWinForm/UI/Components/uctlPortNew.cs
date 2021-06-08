using System.Drawing;
using System.Windows.Forms;


namespace com.mirle.ibg3k0.bc.winform.UI.Components
{
    public partial class uctlPortNew : UserControl
    {
        #region "Internal Variable"

        private string m_sPortName;
        private string m_sAddress;
        private int m_iLocX;
        private int m_iLocY;
        private int m_iSizeW;
        private int m_iSizeH;
        private Color m_clrColor;
        private string m_currentCST;
#pragma warning disable CS0169 // 欄位 'uctlPortNew.m_iStatus' 從未使用過
        private int m_iStatus;
#pragma warning restore CS0169 // 欄位 'uctlPortNew.m_iStatus' 從未使用過

        #endregion	/* Internal Variable */
        #region "Property"

        /// <summary>
        /// Object Name
        /// </summary>
        public string p_PortName
        {
            get { return (m_sPortName); }
            set
            {
                m_sPortName = value;
            }
        }

        public string p_Address
        {
            get { return (m_sAddress); }
            set
            {
                m_sAddress = value;
            }
        }

        public int p_LocX
        {
            get { return (m_iLocX); }
            set
            {
                m_iLocX = value;
                _ChangePortImage();
            }
        }

        public int p_LocY
        {
            get { return (m_iLocY); }
            set
            {
                m_iLocY = value;
                _ChangePortImage();
            }
        }

        public int p_SizeW
        {
            get { return (m_iSizeW); }
            set
            {
                m_iSizeW = value;
                this.Width = value;
                _ChangePortImage();
            }
        }

        public int p_SizeH
        {
            get { return (m_iSizeH); }
            set
            {
                m_iSizeH = value;
                this.Height = value;
                _ChangePortImage();
            }
        }

        public Color p_Color
        {
            get { return (m_clrColor); }
            set
            {
                m_clrColor = value;
                _ChangePortImage();
            }
        }



        #endregion	/* Property */

        public void SetCurrentCSTID(string currentCSTID)
        {
            if (sc.Common.SCUtility.isMatche(currentCSTID, m_currentCST)) return;
            currentCSTID = sc.Common.SCUtility.Trim(currentCSTID, true);
            m_currentCST = currentCSTID;
            if (sc.Common.SCUtility.isEmpty(m_currentCST))
            {
                ChangePortColor(m_clrColor, m_sPortName.Trim());
            }
            else
            {
                ChangePortColor(Color.YellowGreen, m_currentCST);
            }
            _SetRailToolTip(currentCSTID);
        }
        private void ChangePortColor(Color color, string displayName)
        {
            this.Left = this.m_iLocX - (this.Width / 2);
            this.Top = this.m_iLocY - (this.Height / 2);


            this.lblPort.BackColor = color;
            //this.lblPort.Text = m_sPortName.Trim();
            this.lblPort.Text = displayName;
        }

        private void _SetRailToolTip(string cstID)
        {
            this.toolTip1.SetToolTip(this.lblPort,
                        "CST ID : " + cstID);
        }

        public uctlPortNew()
        {
            InitializeComponent();
            _SetInitialVhToolTip();
        }
        private void _SetInitialVhToolTip()
        {
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.ForeColor = Color.Black;
            this.toolTip1.BackColor = Color.White;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;

            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
        }

        private void _ChangePortImage()
        {
            this.Left = this.m_iLocX - (this.Width / 2);
            this.Top = this.m_iLocY - (this.Height / 2);


            this.lblPort.BackColor = m_clrColor;
            this.lblPort.Text = m_sPortName.Trim();
        }

    }
}
