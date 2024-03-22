using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    //Enum: View共用的CommandState，各專案統合版，各專案需要寫轉換Function
    public enum E_MCSCommandState
    {
        //共通Status
        Unknown = -1,//通常看到這個就代表專案沒設定轉換Function

        Idle =0,
        EnRoute =1,
        LoadArrive =2,
        Loading =4,
        LoadComplete =8,
        UnLoadArrive =16,
        UnLoading = 32,
        UnLoadComplete =64,
        CommandFinish =128,

        //OHTC專用Status
        DoubleStorage = 256,
        EmptyRetrieval = 512,
        InterLockError = 1024,
        VehicleAbort = 2048,
        CSTTypeMismatch = 4096,
        InterErrorWhenLoad = 8192,
        InterErrorWhenUnload = 16384
    }

    public static class DB_HCMDConverter
    {
        /// <summary>
        /// 提供預設的DB CommandState轉換成Viewer使用的CommandState，如果數值不同請額外寫一隻在該專案底下
        /// </summary>
        /// <param name="iCommandState">預設的CommandState </param>
        /// <returns></returns>
        public static E_MCSCommandState Default_Int_To_E_MCSCommandState(int iCommandState)
        {
            //這邊不使用Switch，為了增加可讀性改用If，且現今硬體夠用，不會差太多效能
            if (iCommandState == (int)E_MCSCommandState.Idle) { return E_MCSCommandState.Idle; }
            if (iCommandState == (int)E_MCSCommandState.EnRoute) { return E_MCSCommandState.EnRoute; }
            if (iCommandState == (int)E_MCSCommandState.LoadArrive) { return E_MCSCommandState.LoadArrive; }
            if (iCommandState == (int)E_MCSCommandState.Loading) { return E_MCSCommandState.Loading; }
            if (iCommandState == (int)E_MCSCommandState.LoadComplete) { return E_MCSCommandState.LoadComplete; }
            if (iCommandState == (int)E_MCSCommandState.UnLoadArrive) { return E_MCSCommandState.UnLoadArrive; }
            if (iCommandState == (int)E_MCSCommandState.UnLoading) { return E_MCSCommandState.UnLoading; }
            if (iCommandState == (int)E_MCSCommandState.UnLoadComplete) { return E_MCSCommandState.UnLoadComplete; }
            if (iCommandState == (int)E_MCSCommandState.CommandFinish) { return E_MCSCommandState.CommandFinish; }

            if (iCommandState == (int)E_MCSCommandState.DoubleStorage) { return E_MCSCommandState.DoubleStorage; }
            if (iCommandState == (int)E_MCSCommandState.EmptyRetrieval) { return E_MCSCommandState.EmptyRetrieval; }
            if (iCommandState == (int)E_MCSCommandState.InterLockError) { return E_MCSCommandState.InterLockError; }
            if (iCommandState == (int)E_MCSCommandState.VehicleAbort) { return E_MCSCommandState.VehicleAbort; }
            if (iCommandState == (int)E_MCSCommandState.CSTTypeMismatch) { return E_MCSCommandState.CSTTypeMismatch; }
            if (iCommandState == (int)E_MCSCommandState.InterErrorWhenLoad) { return E_MCSCommandState.InterErrorWhenLoad; }
            if (iCommandState == (int)E_MCSCommandState.InterErrorWhenUnload) { return E_MCSCommandState.InterErrorWhenUnload; }

            return E_MCSCommandState.Unknown;//全部都找不到
        }
    }
    public class HCMD_MCS
    {
        
    }
}
