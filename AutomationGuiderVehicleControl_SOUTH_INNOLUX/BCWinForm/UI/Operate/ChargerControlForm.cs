using com.mirle.ibg3k0.bc.winform.App;
using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.bcf.App;
using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Data;
using com.mirle.ibg3k0.sc.Data.SECS.AGVC;
using com.mirle.ibg3k0.sc.Data.ValueDefMapAction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static com.mirle.ibg3k0.sc.App.SCAppConstants;

namespace com.mirle.ibg3k0.bc.winform.UI
{
    public partial class ChargerControlForm : Form
    {

        BCMainForm mainForm;
        BCApplication bcApp;
#pragma warning disable CS0414 // 已指派欄位 'DebugForm.blocked_queues'，但從未使用過其值。
        List<BLOCKZONEQUEUE> blocked_queues = null;
#pragma warning restore CS0414 // 已指派欄位 'DebugForm.blocked_queues'，但從未使用過其值。
        AEQPT mCharger = null;
        public ChargerControlForm(BCMainForm _mainForm)
        {
            InitializeComponent();
            mainForm = _mainForm;
            bcApp = mainForm.BCApp;

            cb_passCouplerHPSafetySingnal.Checked = DebugParameter.isPassCouplerHPSafetySignal;

            num_BatteryLowBoundaryValue.Value = AVEHICLE.BATTERYLEVELVALUE_LOW;
            //num_BatteryMiddleBoundaryValue.Value = AVEHICLE.BATTERYLEVELVALUE_MIDDLE;
            num_BatteryHighBoundaryValue.Value = AVEHICLE.BATTERYLEVELVALUE_HIGH;

            mCharger = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("MCharger");
            if (mCharger != null)
            {
                foreach (AUNIT unit in mCharger.UnitList)
                {
                    cb_Charger.Items.Add(unit.UNIT_ID);
                    cb_ChargerID.Items.Add(unit.UNIT_ID);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                cb_PIOCoupler.Items.Add(i + 1);
            }
            initChargerValue();
            registerEvent();
        }

        //private void initHandlerEvent()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "Exception");
        //    }
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void agvcAliveChange(AEQPT eqpt)
        {


            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                tb_agvcAlive.Text = eqpt.AGVCAliveIndex.ToString();
            }), null);

        }

        private void MChargerAbnormalIndexChange(AEQPT eqpt)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                tb_ErrorReportIndex.Text = eqpt.AbnormalReportIndex.ToString();
                tb_ErrorReportCode1.Text = eqpt.abnormalReportCode01.ToString();
                tb_ErrorReportCode2.Text = eqpt.abnormalReportCode02.ToString();
                tb_ErrorReportCode3.Text = eqpt.abnormalReportCode03.ToString();
                tb_ErrorReportCode4.Text = eqpt.abnormalReportCode04.ToString();
                tb_ErrorReportCode5.Text = eqpt.abnormalReportCode05.ToString();
            }), null);
        }

        private void ChargerAliveChange(AUNIT unit)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                if (unit.UNIT_ID != cb_ChargerID.Text) return;
                tb_ChargerAlive.Text = unit.ChargerAlive.ToString();
            }), null);
        }


        private void CurrentSupplyStatusBlockChange(AUNIT unit)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                if (unit.UNIT_ID != cb_ChargerID.Text) return;
                tb_inputVoltage.Text = unit.inputVoltage.ToString();
                tb_chargeVoltage.Text = unit.chargeVoltage.ToString();
                textBox_chargeCurrent.Text = unit.chargeCurrent.ToString();
                textBox_chargePower.Text = unit.chargePower.ToString();
                tb_couplerChargeVoltage.Text = unit.couplerChargeVoltage.ToString();
                tb_couplerChargeCurrent.Text = unit.couplerChargeCurrent.ToString();
                textBox_couplerID.Text = unit.couplerID.ToString();
            }), null);
        }

        private void ChargerStatusIndexChange(AUNIT unit)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                if (unit.UNIT_ID != cb_ChargerID.Text) return;
                tb_ChargerStatusReportIndex.Text = unit.ChargerStatusIndex.ToString();
                tb_Reserve.Text = unit.chargerReserve.ToString();
                tb_ConstantVoOutput.Text = unit.chargerConstantVoltageOutput.ToString();
                tb_ContantCurrentOutput.Text = unit.chargerConstantCurrentOutput.ToString();
                tb_HighInputVoProtect.Text = unit.chargerHighInputVoltageProtection.ToString();
                tb_LowInputVoProtect.Text = unit.chargerLowInputVoltageProtection.ToString();
                tb_HighOutputVoProtect.Text = unit.chargerHighOutputVoltageProtection.ToString();
                tb_HighOutputCurrentProtect.Text = unit.chargerHighOutputCurrentProtection.ToString();
                tb_OverheatProtect.Text = unit.chargerOverheatProtection.ToString();
                tb_RS485Status.Text = unit.chargerRS485Status.ToString();
                //tb_CouplerStatus1.Text = unit.coupler1Status.ToString();
                //tb_CouplerStatus2.Text = unit.coupler2Status.ToString();
                //tb_CouplerStatus3.Text = unit.coupler3Status.ToString();
                setCouplerStatus(unit);
                tb_Coupler1HPSafety.Text = unit.coupler1HPSafety.ToString();
                tb_Coupler2HPSafety.Text = unit.coupler2HPSafety.ToString();
                tb_Coupler3HPSafety.Text = unit.coupler3HPSafety.ToString();

            }), null);
        }

        private void ChargerCurrentParameterindexChange(AUNIT unit)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                if (unit.UNIT_ID != cb_ChargerID.Text) return;
                tb_ChargerCurrentParameterSettingIndex.Text = unit.ChargerCurrentParameterIndex.ToString();
                tb_OutputVo.Text = unit.chargerOutputVoltage.ToString();
                tb_OutputCurrent.Text = unit.chargerOutputCurrent.ToString();
                tb_OverloadVo.Text = unit.chargerOverVoltage.ToString();
                tb_OverloadCurrent.Text = unit.chargerOverCurrent.ToString();
            }), null);
        }


        private void CouplerChargeInfoIndexChange(AUNIT unit)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                if (unit.UNIT_ID != cb_ChargerID.Text) return;
                tb_CouplerChargeInfoReportIndex.Text = unit.CouplerChargeInfoIndex.ToString();
                tb_CouplerID.Text = unit.chargerCouplerID.ToString();
                tb_ChargeStartTime.Text = unit.chargerChargingStartTime.ToString("yyyy/MM/dd hh:mm:ss");
                tb_ChargeEndTime.Text = unit.chargerChargingEndTime.ToString("yyyy/MM/dd hh:mm:ss");
                tb_InputAh.Text = unit.chargerInputAH.ToString();
                tb_ChargeResult.Text = unit.chargerChargingResult.ToString();
            }), null);
        }

        private void PIOIndexChange(AUNIT unit)
        {
            Adapter.Invoke(new SendOrPostCallback((o1) =>
            {
                tb_PIOIndex.Text = unit.PIOIndex.ToString();
                int x;
                bool result = int.TryParse(cb_PIOCoupler.Text, out x);
                if (result)
                {
                    if (unit.PIOInfos.Count >= x)
                    {
                        tb_PIOCouplerID.Text = unit.PIOInfos[x - 1].CouplerID.ToString();
                        tb_PIOHandshakeTime.Text = unit.PIOInfos[x - 1].Timestamp.ToString("yyyy/MM/dd hh:mm:ss");
                        tb_PIOSignal1.Text = unit.PIOInfos[x - 1].signal1.Replace(",", "").Replace("True", "1").Replace("False", "0");
                        tb_PIOSignal2.Text = unit.PIOInfos[x - 1].signal2.Replace(",", "").Replace("True", "1").Replace("False", "0");
                    }
                }
            }), null);
        }

        private void cb_ChargerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            AUNIT unit = bcApp.SCApplication.getEQObjCacheManager().getUnitByUnitID(cb_ChargerID.Text);
            if (unit != null)
            {
                tb_ChargerAlive.Text = unit.ChargerAlive.ToString();
                tb_inputVoltage.Text = unit.inputVoltage.ToString();
                tb_chargeVoltage.Text = unit.chargeVoltage.ToString();
                textBox_chargeCurrent.Text = unit.chargeCurrent.ToString();
                textBox_chargePower.Text = unit.chargePower.ToString();
                tb_couplerChargeVoltage.Text = unit.couplerChargeVoltage.ToString();
                tb_couplerChargeCurrent.Text = unit.couplerChargeCurrent.ToString();
                textBox_couplerID.Text = unit.couplerID.ToString();

                tb_ChargerStatusReportIndex.Text = unit.ChargerStatusIndex.ToString();
                tb_Reserve.Text = unit.chargerReserve.ToString();
                tb_ConstantVoOutput.Text = unit.chargerConstantVoltageOutput.ToString();
                tb_ContantCurrentOutput.Text = unit.chargerConstantCurrentOutput.ToString();
                tb_HighInputVoProtect.Text = unit.chargerHighInputVoltageProtection.ToString();
                tb_LowInputVoProtect.Text = unit.chargerLowInputVoltageProtection.ToString();
                tb_HighOutputVoProtect.Text = unit.chargerHighOutputVoltageProtection.ToString();
                tb_HighOutputCurrentProtect.Text = unit.chargerHighOutputCurrentProtection.ToString();
                tb_OverheatProtect.Text = unit.chargerOverheatProtection.ToString();
                tb_RS485Status.Text = unit.chargerRS485Status.ToString();

                setCouplerStatus(unit);

                tb_Coupler1HPSafety.Text = unit.coupler1HPSafety.ToString();
                tb_Coupler2HPSafety.Text = unit.coupler2HPSafety.ToString();
                tb_Coupler3HPSafety.Text = unit.coupler3HPSafety.ToString();

                tb_ChargerCurrentParameterSettingIndex.Text = unit.ChargerCurrentParameterIndex.ToString();
                tb_OutputVo.Text = unit.chargerOutputVoltage.ToString();
                tb_OutputCurrent.Text = unit.chargerOutputCurrent.ToString();
                tb_OverloadVo.Text = unit.chargerOverVoltage.ToString();
                tb_OverloadCurrent.Text = unit.chargerOverCurrent.ToString();

                tb_CouplerChargeInfoReportIndex.Text = unit.CouplerChargeInfoIndex.ToString();
                tb_CouplerID.Text = unit.chargerCouplerID.ToString();
                tb_ChargeStartTime.Text = unit.chargerChargingStartTime.ToString("yyyy/MM/dd hh:mm:ss");
                tb_ChargeEndTime.Text = unit.chargerChargingEndTime.ToString("yyyy/MM/dd hh:mm:ss");
                tb_InputAh.Text = unit.chargerInputAH.ToString();
                tb_ChargeResult.Text = unit.chargerChargingResult;

                tb_PIOIndex.Text = unit.PIOIndex.ToString();
                int x;
                bool result = int.TryParse(cb_PIOCoupler.Text, out x);
                if (result)
                {
                    if (unit.PIOInfos.Count >= x)
                    {
                        tb_PIOCouplerID.Text = unit.PIOInfos[x - 1].CouplerID.ToString();
                        tb_PIOHandshakeTime.Text = unit.PIOInfos[x - 1].Timestamp.ToString("yyyy/MM/dd hh:mm:ss");
                        tb_PIOSignal1.Text = unit.PIOInfos[x - 1].signal1.Replace(",", "").Replace("True", "1").Replace("False", "0");
                        tb_PIOSignal2.Text = unit.PIOInfos[x - 1].signal2.Replace(",", "").Replace("True", "1").Replace("False", "0");
                    }
                }
            }

        }

        private void setCouplerStatus(AUNIT unit)
        {
            switch (bcApp.SCApplication.BC_ID)
            {
                case "SOUTH_INNOLUX":
                case "NORTH_INNOLUX":
                case "SOUTH_INXF4":
                    //tb_CouplerStatus1.Text = unit.coupler1Status_NORTH_INNOLUX.ToString();
                    //tb_CouplerStatus2.Text = unit.coupler2Status_NORTH_INNOLUX.ToString();
                    //tb_CouplerStatus3.Text = unit.coupler3Status_NORTH_INNOLUX.ToString();
                    tb_CouplerStatus1.Text = CouplerStatusConvert(unit.coupler1Status_NORTH_INNOLUX);
                    tb_CouplerStatus2.Text = CouplerStatusConvert(unit.coupler2Status_NORTH_INNOLUX);
                    tb_CouplerStatus3.Text = CouplerStatusConvert(unit.coupler3Status_NORTH_INNOLUX);
                    break;
                default:
                    tb_CouplerStatus1.Text = unit.coupler1Status.ToString();
                    tb_CouplerStatus2.Text = unit.coupler2Status.ToString();
                    tb_CouplerStatus3.Text = unit.coupler3Status.ToString();
                    break;
            }
        }

        private string CouplerStatusConvert(SCAppConstants.CouplerStatus_NORTH_INNOLUX status)
        {
            if (Enum.IsDefined(typeof(SCAppConstants.CouplerStatus_NORTH_INNOLUX), status))
            {
                switch (status)
                {
                    case com.mirle.ibg3k0.sc.App.SCAppConstants.CouplerStatus_NORTH_INNOLUX.None:
                        return "Disable";
                    case com.mirle.ibg3k0.sc.App.SCAppConstants.CouplerStatus_NORTH_INNOLUX.Auto:
                        return "Enable";
                    case com.mirle.ibg3k0.sc.App.SCAppConstants.CouplerStatus_NORTH_INNOLUX.Error:
                        return "Alarm";
                    default:
                        return status.ToString();
                }
            }
            else
            {
                switch ((int)status)
                {
                    case 5: return "Close";
                    case 6: return "Maintain";
                    case 7: return "Pause";
                    case 8: return "Wait";
                    case 9: return "Stop";
                    case 10: return "Interlock";
                    default: return status.ToString();
                }

            }
        }

        private void cb_PIOCoupler_SelectedIndexChanged(object sender, EventArgs e)
        {
            AUNIT unit = bcApp.SCApplication.getEQObjCacheManager().getUnitByUnitID(cb_ChargerID.Text);
            if (unit != null)
            {
                tb_PIOIndex.Text = unit.PIOIndex.ToString();
                int x;
                bool result = int.TryParse(cb_PIOCoupler.Text, out x);
                if (result)
                {
                    if (unit.PIOInfos.Count >= x)
                    {
                        tb_PIOCouplerID.Text = unit.PIOInfos[x - 1].CouplerID.ToString();
                        tb_PIOHandshakeTime.Text = unit.PIOInfos[x - 1].Timestamp.ToString("yyyy/MM/dd hh:mm:ss");
                        tb_PIOSignal1.Text = unit.PIOInfos[x - 1].signal1.Replace(",", "").Replace("True", "1").Replace("False", "0");
                        tb_PIOSignal2.Text = unit.PIOInfos[x - 1].signal2.Replace(",", "").Replace("True", "1").Replace("False", "0");
                    }
                }
            }
        }


        private void initChargerValue()
        {
            if (mCharger != null)
            {
                agvcAliveChange(mCharger);
                MChargerAbnormalIndexChange(mCharger);
                foreach (AUNIT unit in mCharger.UnitList)
                {
                    ChargerAliveChange(unit);
                    CurrentSupplyStatusBlockChange(unit);
                    ChargerStatusIndexChange(unit);
                    ChargerCurrentParameterindexChange(unit);
                    CouplerChargeInfoIndexChange(unit);
                    PIOIndexChange(unit);
                }
            }
        }
        private void registerEvent()
        {
            string Handler = this.Name;
            if (mCharger != null)
            {
                mCharger.addEventHandler(Handler, BCFUtility.getPropertyName(() => mCharger.AGVCAliveIndex),
                    (s1, e1) => agvcAliveChange(mCharger));

                mCharger.addEventHandler(Handler, BCFUtility.getPropertyName(() => mCharger.AbnormalReportIndex),
                    (s1, e1) => MChargerAbnormalIndexChange(mCharger));

                foreach (AUNIT unit in mCharger.UnitList)
                {
                    unit.addEventHandler(Handler, BCFUtility.getPropertyName(() => unit.ChargerAlive),
                        (s1, e1) => ChargerAliveChange(unit));
                    unit.addEventHandler(Handler, BCFUtility.getPropertyName(() => unit.CurrentSupplyStatusBlock),
                        (s1, e1) => CurrentSupplyStatusBlockChange(unit));
                    unit.addEventHandler(Handler, BCFUtility.getPropertyName(() => unit.ChargerStatusIndex),
                        (s1, e1) => ChargerStatusIndexChange(unit));
                    unit.addEventHandler(Handler, BCFUtility.getPropertyName(() => unit.ChargerCurrentParameterIndex),
                        (s1, e1) => ChargerCurrentParameterindexChange(unit));
                    unit.addEventHandler(Handler, BCFUtility.getPropertyName(() => unit.CouplerChargeInfoIndex),
                        (s1, e1) => CouplerChargeInfoIndexChange(unit));
                    unit.addEventHandler(Handler, BCFUtility.getPropertyName(() => unit.PIOIndex),
                        (s1, e1) => PIOIndexChange(unit));

                }
            }
        }
        private void unregisterEvent()
        {
            if (mCharger != null)
            {
                mCharger.RemoveAllEvents();

                foreach (AUNIT unit in mCharger.UnitList)
                {
                    unit.RemoveAllEvents();
                }
            }
        }

        private void ChargerControlForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void ChargerControlForm_Closed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            unregisterEvent();
            mainForm.removeForm(typeof(ChargerControlForm).Name);
        }

        #region MTL Test


        private void btn_mtl_dateTimeSync_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var mtl_mapaction = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("MCharger").
                    getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction)) as
                    com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction;
                DateTime dateTime = DateTime.Now;
                //DateTime dateTime = DateTime.Now.AddMilliseconds(int.Parse(numericUpDownForDateTimeAddMilliseconds.Value.ToString()));
                mtl_mapaction.DateTimeSyncCommand(dateTime);
            }
            );
        }
        #endregion MTL Test

        private void btn_mtl_message_download_Click(object sender, EventArgs e)
        {
            bool is_enable = cb_all_coupler_enable.Checked;
            Task.Run(() =>
            {
                var mtl_mapaction = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("MCharger").
                    getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction)) as
                    com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction;
                mtl_mapaction.AGVCToChargerAllCouplerEnable(is_enable);
            }
            );
        }

        private void btn_mtl_vh_realtime_info_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var mtl_mapaction = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("MCharger").
                    getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction)) as
                    com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction;
                mtl_mapaction.AGVCToMChargerAllChargerChargingFinish();
            }
            );
        }

        private void btn_mtl_car_out_notify_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var mtl_mapaction = bcApp.SCApplication.getEQObjCacheManager().getEquipmentByEQPTID("MCharger").
                    getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction)) as
                    com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction;
                mtl_mapaction.AGVCToMChargerAlarmReset();
            }
            );
        }

        private void btn_coulper_enable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cb_Charger.Text))
            {
                MessageBox.Show("Please select charger.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_charger_coupler_id.Text))
            {
                MessageBox.Show("Please enter coupler ID.");
                return;
            }

            string charger_id = cb_Charger.Text.Trim();
            string coupler_id = txt_charger_coupler_id.Text.Trim();
            if (!uint.TryParse(coupler_id, out uint i_coupler_id))
            {
                MessageBox.Show("Coupler ID could only be numbers.");
                return;
            }
            //uint.TryParse(coupler_id, out uint i_coupler_id);
            bool is_enable = cb_coupler_enable.Checked;
            Task.Run(() =>
            {
                var mtl_mapaction = bcApp.SCApplication.getEQObjCacheManager().getUnit("MCharger", charger_id).
                getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.NorthInnolux.SubChargerValueDefMapAction)) as
                com.mirle.ibg3k0.sc.Data.ValueDefMapAction.NorthInnolux.SubChargerValueDefMapAction;
                mtl_mapaction.AGVCToChargerCouplerEnable(i_coupler_id, is_enable);
            });
        }


        private void btn_Charger1ForceFinish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cb_Charger.Text))
            {
                MessageBox.Show("Please select charger.");
            }
            string charger_id = cb_Charger.Text.Trim();
            Task.Run(() =>
            {
                var mtl_mapaction = bcApp.SCApplication.getEQObjCacheManager().getUnit("MCharger", charger_id)
                .getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.SubChargerValueDefMapAction)) as
                    com.mirle.ibg3k0.sc.Data.ValueDefMapAction.SubChargerValueDefMapAction;
                mtl_mapaction.AGVCToChargerForceStopCharging();
            }
            );
        }




        private void cb_passCouplerHPSafetySingnal_CheckedChanged(object sender, EventArgs e)
        {
            DebugParameter.isPassCouplerHPSafetySignal = cb_passCouplerHPSafetySingnal.Checked;
        }
    }
}
