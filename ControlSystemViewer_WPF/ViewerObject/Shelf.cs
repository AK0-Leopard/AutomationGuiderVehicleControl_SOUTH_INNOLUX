using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewerObject
{
    public class Shelf : ViewerObjectBase
    {
        public string SHELF_ID { get; private set; } = "";
        /// <summary>
        /// DIR 用來表示這shelf在這條軌道順行方向的哪邊
        /// 舊版本，使用Address來產生shelf物件的版本，是以shelf ID第一個字，以1左2又表示
        /// 新版本，使用絕對座標 POS_X跟POS_Y來產生物件，是以shelf的第三個字表示方向，而仍維1左2右
        /// </summary>
        public int DIR => (POS_X==0 && POS_Y == 0) ? Convert.ToInt32(SHELF_ID?.Substring(0, 1) ?? "") : Convert.ToInt32(SHELF_ID?.Substring(2, 1) ?? "");
        public int SNO => Convert.ToInt32(SHELF_ID?.Substring(1, 3) ?? "");
        public int ZONE_SNO => Convert.ToInt32(SHELF_ID?.Substring(4, 2) ?? "");
        public string ZONE_ID { get; private set; } = "";

        private Point zone_dir_vec = new Point(0, 0);
        public Point ZONE_DIR_VEC
        {
            get { return zone_dir_vec; }
            set
            {
                Point valueUnit = getUnitVector(value);
                if (zone_dir_vec.X != valueUnit.X ||
                    zone_dir_vec.Y != valueUnit.Y)
                {
                    zone_dir_vec = valueUnit;
                    setOffsetVector();
                }
            }
        }
        public Point OFFSET_VEC { get; private set; } // 單位向量
        public double OFFSET_LENGTH { get; set; } = 0; // 單位向量 X 長度 = 偏移量
        public Address ADDRESS { get; private set; } = null;

        public int POS_X = 0;
        public int POS_Y = 0;

        public double X => ADDRESS == null ? POS_X + (OFFSET_VEC.X * OFFSET_LENGTH) : ADDRESS.X + (OFFSET_VEC.X * OFFSET_LENGTH);
        public double Y => ADDRESS == null ? POS_Y + (OFFSET_VEC.Y * OFFSET_LENGTH) : ADDRESS.Y + (OFFSET_VEC.Y * OFFSET_LENGTH);

        private bool enable = false;
        public bool ENABLE
        {
            get { return enable; }
            set
            {
                if (enable != value)
                {
                    enable = value;
                    OnPropertyChanged();
                    onShelfStatusChanged();
                }
            }
        }

        private Definition.ShelfStatus shelf_status = Definition.ShelfStatus.Default;
        public Definition.ShelfStatus SHELF_STATUS
        {
            get { return shelf_status; }
            set
            {
                if (shelf_status != value)
                {
                    shelf_status = value;
                    OnPropertyChanged();
                    onShelfStatusChanged();
                }
            }
        }
        public EventHandler ShelfStatusChanged;
        private void onShelfStatusChanged()
        {
            ShelfStatusChanged?.Invoke(this, null);
        }

        private string box_id = "";
        public string BOX_ID
        {
            get 
            {
                return (SHELF_STATUS == Definition.ShelfStatus.Stored ||
                        SHELF_STATUS == Definition.ShelfStatus.Alternate) ?
                        box_id : "";
            }
            set
            {
                if (box_id != value)
                {
                    box_id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string cst_id = "";
        public string CST_ID
        {
            get
            {
                return (SHELF_STATUS == Definition.ShelfStatus.Stored ||
                        SHELF_STATUS == Definition.ShelfStatus.Alternate) ?
                        cst_id : "";
            }
            set
            {
                if (cst_id != value)
                {
                    cst_id = value;
                    OnPropertyChanged();
                }
            }
        }

        public Shelf(string shelf_id, string zone_id, Address address)
        {
            SHELF_ID = shelf_id?.Trim() ?? "";

            ZONE_ID = zone_id?.Trim() ?? "";

            ADDRESS = address;
        }

        public Shelf(string shelf_id, string x, string y)
        {
            SHELF_ID = shelf_id?.Trim() ?? "";
            POS_X = Convert.ToInt32(x);
            POS_Y = Convert.ToInt32(y);
        }

        private Point getUnitVector(Point vec)
        {
            double vec_length = BasicFunction.MathFormula.Distance(vec);
            return new Point(vec.X / vec_length, vec.Y / vec_length);
        }

        private void setOffsetVector()
        {
            try
            {
                double offset_vec_x = ZONE_DIR_VEC.Y;
                double offset_vec_y = -ZONE_DIR_VEC.X;
                if (DIR == 2)
                {
                    offset_vec_x = -offset_vec_x;
                    offset_vec_y = -offset_vec_y;
                }
                OFFSET_VEC = new Point(offset_vec_x, offset_vec_y);
            }
            catch
            {
                OFFSET_VEC = new Point(0, 0);
            }
        }
    }
}
