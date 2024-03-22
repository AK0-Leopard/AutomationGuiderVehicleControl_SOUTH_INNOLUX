using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
    public class VSysExcuteQuality
    {
        public VSysExcuteQuality(string _CMD_ID_MCS,DateTime _CMD_Insert_Time,
                                 string _CMD_Finish_Status,string VH_ID,string _VH_Start_Sec_ID,
                                 string _Source_ADR,string _Sec_CNt_to_Source,string _Sec_Dis_to_Source,
                                 string _Destination_ADR,string _Sec_CNt_to_Destn,string _Sec_Dis_to_Destn,
                                 double _Cmdqueue_time,double _Move_to_source_time,double _Total_Block_Time_to_Source,double _Total_OCS_Time_to_Source,double _Total_OCS_Count_to_Source,
                                 double _Move_to_Destn_Time,double _Total_Block_Time_to_Destn,double _Total_OCS_Time_to_Destn,double _Total_Block_Count_to_Destn,double _Total_OCS_Count_to_Destn,
                                 double _Total_Pause_Time,double _CMD_Total_Excution_Time,double _Total_ACT_VH_Count,double _Paking_VH_Count,double _CycleRun_VH_Count,double _Total_Idle_VH_Count,
                                 DateTime? _CMD_Start_Time=null,DateTime? _CMD_Finish_Time=null)
        {
            cmd_id_mcs = _CMD_ID_MCS;
            cmd_insert_time = _CMD_Insert_Time;
            cmd_finish_status = _CMD_Finish_Status;
            vh_id = VH_ID;
            vh_start_sec_id = _VH_Start_Sec_ID;
            source_adr = _Source_ADR;
            sec_cnt_to_source = _Sec_CNt_to_Source;
            sec_dis_to_source = _Sec_Dis_to_Source;
            destination_adr = _Destination_ADR;
            sec_cnt_to_destn = _Sec_CNt_to_Destn;
            sec_dis_to_destn = _Sec_Dis_to_Destn;
            cmdqueue_time = _Cmdqueue_time;
            move_to_source_time = _Move_to_source_time;
            total_block_time_to_source = _Total_Block_Time_to_Source;
            total_ocs_time_to_source = _Total_OCS_Time_to_Source;
            total_ocs_count_to_source = _Total_OCS_Count_to_Source;
            move_to_destn_time = _Move_to_Destn_Time;
            total_block_time_to_destn = _Total_Block_Time_to_Destn;
            total_ocs_time_to_destn = _Total_OCS_Time_to_Destn;
            total_block_count_to_destn = _Total_Block_Count_to_Destn;
            total_ocs_count_to_destn = _Total_OCS_Count_to_Destn;
            total_pause_time = _Total_Pause_Time;
            cmd_total_execution_time = _CMD_Total_Excution_Time;
            total_act_vh_count = _Total_ACT_VH_Count;
            paking_vh_count = _Paking_VH_Count;
            cyclerun_vh_count = _CycleRun_VH_Count;
            total_idle_vh_count = _Total_Idle_VH_Count;
            cmd_start_time = _CMD_Start_Time;
            cmd_finish_time = _CMD_Finish_Time;
        }
        private string cmd_id_mcs { set; get; }
        public string CMD_ID_MCS
        {
            get { return cmd_id_mcs; }
        }

        private DateTime cmd_insert_time { set; get; }
        public DateTime CMD_Insert_Time
        {
            get { return cmd_insert_time; }
        }

        private DateTime? cmd_start_time { set; get; }
        public DateTime? CMD_Start_Time
        {
            get { return cmd_start_time??null; }
        }

        private DateTime? cmd_finish_time { set; get; }
        public DateTime? CMD_Finish_Time
        {
            get { return cmd_finish_time??null; }
        }

        private string cmd_finish_status { get; set; }
        public string CMD_Finish_Status
        {
            get { return cmd_finish_status??null; }
        }

        private string vh_id { get; set; }
        public string VH_ID
        {
            get { return vh_id??null; }
        }

        private string vh_start_sec_id { get; set; }
        public string VH_Start_Sec_ID
        {
            get { return vh_start_sec_id??null; }
        }

        private string source_adr { get; set; }
        public string Source_ADR
        {
            get { return source_adr??null; }
        }

        private string sec_cnt_to_source { get; set; }
        public string Sec_CNt_to_Source
        {
            get { return sec_cnt_to_source; }
        }

        private string sec_dis_to_source { get; set; }
        public string Sec_Dis_to_Source
        {
            get { return sec_dis_to_source; }
        }

        private string destination_adr { get; set; }
        public string Destination_ADR
        {
            get { return destination_adr??null; }
        }

        private string sec_cnt_to_destn { get; set; }
        public string Sec_CNt_to_Destn
        {
            get { return sec_cnt_to_destn; }
        }

        private string sec_dis_to_destn { get; set; }
        public string Sec_Dis_to_Destn
        {
            get { return sec_dis_to_destn; }
        }

        private double cmdqueue_time { get; set; }
        public double Cmdqueue_time
        {
            get { return cmdqueue_time; }
        }

        private double move_to_source_time{get;set;}
        public double Move_to_source_time
        {
            get { return move_to_source_time; }
        }

        private double total_block_time_to_source { get; set; }
        public double Total_Block_Time_to_Source
        {
            get { return total_block_time_to_source; }
        }

        private double total_ocs_time_to_source { get; set; }
        public double Total_OCS_Time_to_Source
        {
            get { return total_ocs_time_to_source; }
        }

        private double total_block_count_to_source { get; set; }
        public double Total_Block_Count_to_Source
        {
            get { return total_block_count_to_source; }
        }

        private double total_ocs_count_to_source { get; set; }
        public double Total_OCS_Count_to_Source
        {
            get { return total_ocs_count_to_source; }
        }

        private double move_to_destn_time { get; set; }
        public double Move_to_Destn_Time
        {
            get { return move_to_destn_time; }
        }

        private double total_block_time_to_destn { get; set; }
        public double Total_Block_Time_to_Destn
        {
            get { return total_block_time_to_destn; }
        }

        private double total_ocs_time_to_destn { get; set; }
        public double Total_OCS_Time_to_Destn
        {
            get { return total_ocs_time_to_destn; }
        }

        private double total_block_count_to_destn { get; set; }
        public double Total_Block_Count_to_Destn
        {
            get { return total_block_count_to_destn; }
        }

        private double total_ocs_count_to_destn { get; set; }
        public double Total_OCS_Count_to_Destn
        {
            get { return total_ocs_count_to_destn; }
        }

        private double total_pause_time { get; set; }
        public double Total_Pause_Time
        {
            get { return total_pause_time; }
        }

        private double cmd_total_execution_time { get; set; }
        public double CMD_Total_Execution_Time
        {
            get { return cmd_total_execution_time; }
        }

        private double total_act_vh_count { get; set; }
        public double Total_ACT_VH_Count
        {
            get { return total_act_vh_count; }
        }

        private double paking_vh_count { get; set; }
        public double Paking_VH_Count
        {
            get { return paking_vh_count; }
        }

        private double cyclerun_vh_count { get; set; }
        public double CycleRun_VH_Count
        {
            get { return cyclerun_vh_count; }
        }

        private double total_idle_vh_count { get; set; }
        public double Total_Idle_VH_Count
        {
            get { return total_idle_vh_count; }
        }
    }
}
