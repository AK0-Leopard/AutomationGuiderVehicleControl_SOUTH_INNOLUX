//*********************************************************************************
//      CsvUtility.cs
//*********************************************************************************
// File Name: CsvUtility.cs
// Description: CSV File Import/Export Function
//
//(c) Copyright 2015, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
//**********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GenericParsing;
using NLog;
using System.IO;
using ViewerObject;

namespace com.mirle.ibg3k0.sc.Common
{
    public class CsvUtility
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static public DataTable loadCSVToDataTable(string tableName, string path)
        {
            DataTable dt = new System.Data.DataTable(tableName);

            using (GenericParser parser = new GenericParser())
            {
                try
                {
                    parser.SetDataSource(path);
                    parser.ColumnDelimiter = ',';
                    parser.FirstRowHasHeader = true;
                    parser.MaxBufferSize = 1024;

                    bool isfirst = true;
                    int count = 0;
                    while (parser.Read())
                    {
                        count++;
                        int cs = parser.ColumnCount;
                        if (isfirst)
                        {
                            for (int i = 0; i < cs; i++)
                            {
                                dt.Columns.Add(parser.GetColumnName(i), typeof(string));
                            }
                            isfirst = false;
                        }

                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < cs; i++)
                        {
                            string val = parser[i];
                            dr[i] = val;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception:");
                }
            }

            return dt;
        }

        private static object CMDData_lock = new object();
        static public bool exportLotDataToCSV(List<VTRANSFER> lts)
        {
            lock (CMDData_lock)
            {
                bool rtnFlg = false;

                string dirPath = Environment.CurrentDirectory + "\\" + "ExportData";
                //string dirPath = SCApplication.getMessageString("ReportDataDir");
                string fileName = "Command_Record_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                    if (!dirInfo.Exists)
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string path = dirPath + "//" + fileName;
                    FileStream fileStream = new FileStream(path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.GetEncoding(-0));
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("No,");
                    sb.Append("MCS Command ID,");
                    sb.Append("Status,");
                    sb.Append("Carrier ID,");
                    sb.Append("Load Port,");
                    sb.Append("Unload Port,");
                    sb.Append("Priority Sum,");
                    sb.Append("MCS Priority,");
                    sb.Append("Port Priority,");
                    sb.Append("Time Priority,");
                    sb.Append("Create Time,");
                    sb.Append("Receive CMD. Time,");
                    sb.Append("Assign CMD. Time,");
                    sb.Append("Start Time,");
                    sb.Append("Complete Time,");
                    sb.Append("Vehicle Command ID,");
                    sb.Append("Vehicle ID");

                    sw.WriteLine(sb);

                    for (int i = 0; i < lts.Count; i++)
                    {
                        sb.Clear();
                        //string columnValue = "";
                        //sb.Append((i + 1));
                        //sb.Append(",");

                        sb.Append(lts[i].CMD_ID);
                        sb.Append(",");
                        sb.Append(lts[i].TRANSFER_STATUS);
                        sb.Append(",");

                        sb.Append(lts[i].CARRIER_ID);
                        sb.Append(",");

                        sb.Append(lts[i].HOSTSOURCE);
                        sb.Append(",");

                        sb.Append(lts[i].HOSTDESTINATION);
                        sb.Append(",");

                        sb.Append(lts[i].PRIORITY_SUM);
                        sb.Append(",");

                        sb.Append(lts[i].PRIORITY);
                        sb.Append(",");

                        sb.Append(lts[i].PORT_PRIORITY);
                        sb.Append(",");

                        sb.Append(lts[i].TIME_PRIORITY);
                        sb.Append(",");

                        sb.Append(lts[i].STR_INSERT_TIME);
                        sb.Append(",");

                        sb.Append(lts[i].STR_ASSIGN_TIME);
                        sb.Append(",");

                        sb.Append(lts[i].STR_FINISH_TIME);
                        sb.Append(",");


                        sb.Append(lts[i].CMD_ID);
                        sb.Append(",");

                        sb.Append(lts[i].VH_ID);


                        sw.WriteLine(sb);
                    }
                    sw.Close();
                    fileStream.Close();
                    rtnFlg = true;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception:");
                    return false;
                }

                return rtnFlg;
            }
        }

    }
}
