using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// NumericUpDown.xaml 的互動邏輯
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        decimal MaxValue = decimal.MaxValue;
        decimal MinValue = decimal.MinValue;
        decimal IncValue = 1;
        decimal _Value = 0;

        public NumericUpDown()
        {
            InitializeComponent();
            TBox_Num.Text = Value.ToString();
        }

        public decimal Value
        {
            get { return _Value; }
            set
            {
                if (value > MaxValue)
                { throw new ArgumentOutOfRangeException("指定的值是大於Maximum屬性值。 "); }
                else if (value < MinValue)
                { throw new ArgumentOutOfRangeException("指定的值是小於Minimum屬性值。"); }
                else
                {
                    _Value = value;
                    TBox_Num.Text = value.ToString();
                }
            }
        }

        public decimal Increment
        {
            get { return IncValue; }
            set
            {
                if (value <= 0)
                { throw new ArgumentOutOfRangeException("指派的值未大於或等於零。"); }
                else
                { IncValue = value; }
            }
        }

        public decimal Maximum
        {
            get { return MaxValue; }
            set { MaxValue = value; }
        }

        public decimal Minimum
        {
            get { return MinValue; }
            set { MinValue = value; }
        }

        private void RptBtm_Up_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((MaxValue - IncValue) < _Value) { return; }
                _Value = Convert.ToDecimal(TBox_Num.Text) + Increment;
                TBox_Num.Text = (Convert.ToDecimal(TBox_Num.Text) + Increment).ToString();
            }
            catch
            {
                _Value = 0;
                TBox_Num.Text = _Value.ToString();
            }
        }
        private void RptBtm_Down_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((MinValue + IncValue) > _Value) { return; }
                _Value = Convert.ToDecimal(TBox_Num.Text) - Increment;
                TBox_Num.Text = (Convert.ToDecimal(TBox_Num.Text) - Increment).ToString();
            }
            catch
            {
                _Value = 0;
                TBox_Num.Text = _Value.ToString();
            }
        }
        private void TBox_Num_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            { e.Handled = false; }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9)) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            { e.Handled = false; }
            else if (e.Key == Key.Subtract & TBox_Num.SelectionStart == 0)
            { e.Handled = false; }
            else
            { e.Handled = true; }
        }
        private void TBox_Num_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValueChanged = true;
            if (!(TBox_Num.Text != "-" ^ TBox_Num.Text != ""))
            {
                try
                {
                    string ValStr = "";
                    for (int i = 0; i < TBox_Num.Text.Length; i++)
                    {
                        if ((i == 0 & TBox_Num.Text.Substring(i, 1) == "-") || Char.IsDigit(TBox_Num.Text, i))
                        { ValStr += TBox_Num.Text.Substring(i, 1); }
                    }
                    if (TBox_Num.Text != Convert.ToDecimal(ValStr).ToString())
                    { IsValueChanged = false; }
                    TBox_Num.Text = Convert.ToDecimal(ValStr).ToString();
                    _Value = Convert.ToDecimal(TBox_Num.Text);
                    if (TBox_Num.Text.Length == 1)
                    { TBox_Num.SelectionStart = 1; }
                }
                catch
                { TBox_Num.Text = _Value.ToString(); }
            }
            else
            { _Value = 0; }

            if (IsValueChanged == true)
            { RaiseEvent(new RoutedEventArgs(NumericUpDown.ValueChangedEvent)); }
        }
        private void TBox_Num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TBox_Num.Text == "")
            {
                _Value = 0;
                TBox_Num.Text = _Value.ToString();
            }
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));


        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
    }
}
