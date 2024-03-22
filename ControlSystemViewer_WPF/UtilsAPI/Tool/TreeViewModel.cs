using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ViewerObject;

namespace UtilsAPI.Tool
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        //private bool _isVisible;
        //public bool IsVisible
        //{
        //    get { return _isVisible; }
        //    set { _isVisible = value; NotifyPropertyChanged("IsVisible"); }
        //}

        public TreeViewModel(string name)
        {
            Name = name;
            Children = new List<TreeViewModel>();
        }

        public TreeViewModel(string name, string text, Brush nameBrush = null, Brush textBrush = null, List<TreeViewModel> children = null, object objectIcon = null)
        {
            Name = name ?? "";
            Text = text ?? "";
            if (nameBrush != null) NameBrush = nameBrush;
            if (textBrush != null) TextBrush = textBrush;
            Children = children ?? new List<TreeViewModel>();
            ObjectIcon = objectIcon;
            VisibilityIcon = ObjectIcon == null ? Visibility.Collapsed : Visibility.Visible;
        }

        #region Properties

        public string Name { get; private set; }
        public Brush NameBrush { get; private set; } = Brushes.White;
        public string Text { get; private set; }
        public Brush TextBrush { get; private set; } = Brushes.White;
        public List<TreeViewModel> Children { get; private set; }
        public object ObjectIcon { get; private set; }
        public Visibility VisibilityIcon { get; private set; }

        bool isInitiallySelected = false;
        public bool IsInitiallySelected 
        {
            get { return isInitiallySelected; }
            set
            {
                if (isInitiallySelected != value)
                {
                    isInitiallySelected = value;
                    NotifyPropertyChanged("IsInitiallySelected");
                }
            } 
        }

        bool? _isChecked = false;
        TreeViewModel _parent;

        #region IsChecked

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        public void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked) return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null) _parent.VerifyCheckedState();

            NotifyPropertyChanged("IsChecked");
        }

        void VerifyCheckedState()
        {
            bool? state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(state, false, true);
        }

        #endregion

        #endregion

        void Initialize()
        {
            foreach (TreeViewModel child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        public static List<TreeViewModel> SetTree(string topLevelName, List<VUASFNC> funtionCodeList)
        {
            List<TreeViewModel> treeView = new List<TreeViewModel>();

            TreeViewModel selectAll = new TreeViewModel(topLevelName);
            selectAll.Text = topLevelName;
            treeView.Add(selectAll);

            TreeViewModel tvm_system = new TreeViewModel("System_Function");
            TreeViewModel tvm_operation = new TreeViewModel("Operation_Function");
            TreeViewModel tvm_maintenance = new TreeViewModel("Maintenance_Function");
            //TreeViewModel tvm_debug = new TreeViewModel("Debug_Function");

            tvm_system.Text = "System";
            tvm_operation.Text = "Operation";
            tvm_maintenance.Text = "Maintenance";
            //tvm_debug.Text = "Debug";

            selectAll.Children.Add(tvm_system);
            selectAll.Children.Add(tvm_operation);
            selectAll.Children.Add(tvm_maintenance);
            //selectAll.Children.Add(tvm_debug);

            var System_type = typeof(UAS_Def.System_Function).GetFields();
            var Operation_type = typeof(UAS_Def.Operation_Function).GetFields();
            var Maintenance_type = typeof(UAS_Def.Maintenance_Function).GetFields();
            //var Debug_type = typeof(UAS_Def.Debug_Function).GetFields();

            //Perform recursive method to build treeview 

            foreach (var functionCode in funtionCodeList)
            {
                TreeViewModel a;

                string System_value = (string)System_type.Where(x => x.Name == functionCode.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;
                string Operation_value = (string)Operation_type.Where(x => x.Name == functionCode.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;
                string Maintenance_value = (string)Maintenance_type.Where(x => x.Name == functionCode.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;
                //string Debug_value = (string)Debug_type.Where(x => x.Name == functionCode.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;

                if (!string.IsNullOrEmpty(System_value))
                {
                    a = new TreeViewModel(functionCode.FUNC_CODE.Trim());
                    tvm_system.Children.Add(a);
                    a.Text = functionCode.FUNC_NAME?.Trim();
                }
                else if (!string.IsNullOrEmpty(Operation_value))
                {
                    a = new TreeViewModel(functionCode.FUNC_CODE.Trim());
                    tvm_operation.Children.Add(a);
                    a.Text = functionCode.FUNC_NAME?.Trim();
                }
                else if (!string.IsNullOrEmpty(Maintenance_value))
                {
                    a = new TreeViewModel(functionCode.FUNC_CODE.Trim());
                    tvm_maintenance.Children.Add(a);
                    a.Text = functionCode.FUNC_NAME?.Trim();
                }
                //else if (!string.IsNullOrEmpty(Debug_value))
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.Trim());
                //    tvm_debug.Children.Add(a);
                //    a.Text = functionCode.FUNC_NAME?.Trim();
                //}

                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.System_Function.FUNC_LOGIN)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_system.Children.Add(a);
                //    //a.Name = BCAppConstants.System_Function.FUNC_LOGIN;
                //    a.Name = "Login System";
                //}
                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.System_Function.FUNC_USER_MANAGEMENT)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_system.Children.Add(a);
                //    //a.Name = BCAppConstants.System_Function.FUNC_USER_MANAGEMENT;
                //    a.Name = "Account Management";
                //}
                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.System_Function.FUNC_CLOSE_SYSTEM)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_system.Children.Add(a);
                //    //a.Name = BCAppConstants.System_Function.FUNC_CLOSE_SYSTEM;
                //    a.Name = "Exit System";
                //}

                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Operation_Function.FUNC_SYSTEM_CONCROL_MODE)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_operation.Children.Add(a);
                //    //a.Name = BCAppConstants.Operation_Function.FUNC_SYSTEM_CONCROL_MODE;
                //    a.Name = "System Mode Control";
                //}
                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Operation_Function.FUNC_TRANSFER_MANAGEMENT)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_operation.Children.Add(a);
                //    //a.Name = BCAppConstants.Operation_Function.FUNC_TRANSFER_MANAGEMENT;
                //    a.Name = "Transfer Management";
                //}

                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Maintenance_Function.FUNC_ADVANCED_SETTINGS)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_maintenance.Children.Add(a);
                //    //a.Name = BCAppConstants.Maintenance_Function.FUNC_MTS_MTL_MAINTENANCE;
                //    a.Name = "Advanced Settings";
                //}
                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Maintenance_Function.FUNC_MTS_MTL_MAINTENANCE)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_maintenance.Children.Add(a);
                //    //a.Name = BCAppConstants.Maintenance_Function.FUNC_MTS_MTL_MAINTENANCE;
                //    a.Name = "MTL/MTS Maintenance";
                //}
                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Maintenance_Function.FUNC_PORT_MAINTENANCE)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_maintenance.Children.Add(a);
                //    //a.Name = BCAppConstants.Maintenance_Function.FUNC_PORT_MAINTENANCE;
                //    a.Name = "Port Maintenance";
                //}
                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Maintenance_Function.FUNC_VEHICLE_MANAGEMENT)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_maintenance.Children.Add(a);
                //    //a.Name = BCAppConstants.Operation_Function.FUNC_VEHICLE_MANAGEMENT;
                //    a.Name = "Vehicle Management";
                //}

                //if (functionCode.FUNC_CODE.Trim() == BCAppConstants.Debug_Function.FUNC_DEBUG)
                //{
                //    a = new TreeViewModel(functionCode.FUNC_CODE.ToString());
                //    tvm_debug.Children.Add(a);
                //    //a.Name = BCAppConstants.Debug_Function.FUNC_DEBUG;
                //    a.Name = "Debug(Used during testing)";
                //}
            }

            //#region Test Data
            ////Doing this below for this example, you should do it dynamically 
            ////***************************************************           
            //TreeViewModel t1 = new TreeViewModel("System");
            //tv.Children.Add(t1);
            //t1.Children.Add(new TreeViewModel("Account Management"));
            //t1.Children.Add(new TreeViewModel("Exit System"));

            //TreeViewModel t2 = new TreeViewModel("Operation");
            //tv.Children.Add(t2);
            //t2.Children.Add(new TreeViewModel("System Mode Control"));
            //t2.Children.Add(new TreeViewModel("Transfer Management"));

            //TreeViewModel t3 = new TreeViewModel("Maintenance");
            //tv.Children.Add(t3);
            //t3.Children.Add(new TreeViewModel("Port Maintenance"));
            //t3.Children.Add(new TreeViewModel("Vehicle Management"));
            ////***************************************************
            //#endregion

            selectAll.Initialize();

            return treeView;
        }

        public static List<string> GetTree()
        {
            List<string> selected = new List<string>();

            //select = recursive method to check each tree view item for selection (if required)

            return selected;

            //***********************************************************
            //From your window capture selected your treeview control like:   TreeViewModel root = (TreeViewModel)TreeViewControl.Items[0];
            //                                                                List<string> selected = new List<string>(TreeViewModel.GetTree());
            //***********************************************************
        }

        public void SetTreeViewModel(TreeViewModel target)
        {
            if (this.Name != target.Name)
            {
                this.Name = target.Name;
                NotifyPropertyChanged("Name");
            }
            if (this.NameBrush != target.NameBrush)
            {
                this.NameBrush = target.NameBrush;
                NotifyPropertyChanged("NameBrush");
            }
            if (this.Text != target.Text)
            {
                this.Text = target.Text;
                NotifyPropertyChanged("Text");
            }
            if (this.TextBrush != target.TextBrush)
            {
                this.TextBrush = target.TextBrush;
                NotifyPropertyChanged("TextBrush");
            }
            if (this.Children != target.Children)
            {
                this.Children = target.Children;
                NotifyPropertyChanged("Children");
            }
        }

        #region INotifyPropertyChanged Members

        void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
