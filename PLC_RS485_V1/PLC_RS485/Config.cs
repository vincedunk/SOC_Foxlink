using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UseTool;
using Microsoft.Win32;
namespace auto_charge_1
{
    class CONFIG
    {
        #region Charge Setting
        public string scharge_voltage;
        public string scharge_current;
        public string sjudge_time;
        public string sjudge_current;
        #endregion

        #region Threshold Setting
        public string send_voltage;
        public string send_current;
        #endregion
 
        #region Protect Setting
        public string smax_charge_time;
        #endregion

        #region Stall Setting
        public string sstall_time;
        #endregion


        public CONFIG()
        {
            #region Charge Setting
            scharge_voltage = 48.ToString();
            scharge_current = 10.ToString();
            sjudge_time = 10.ToString();
            sjudge_current = 0.2.ToString();
            #endregion

            #region Threshold Setting
            send_voltage = 42.ToString();
            send_current = 5.ToString();
            #endregion

            #region Protect Setting
            smax_charge_time = 240.ToString();
            #endregion

            #region Stall Setting
            sstall_time = 30.ToString();
            #endregion
        }
    public void LoadConfigFile(string sFile)
        {
            string sValue;
            IniFile ini = new IniFile(sFile);

            #region Charge Setting
            sValue = ini.ReadValue("Charge Setting", "charge_voltage");
            if (sValue != "")
                scharge_voltage = sValue;
            sValue = ini.ReadValue("Charge Setting", "charge_current");
            if (sValue != "")
                scharge_current = sValue;
            sValue = ini.ReadValue("Charge Setting", "judge_time");
            if (sValue != "")
                sjudge_time = sValue;
            sValue = ini.ReadValue("Charge Setting", "judge_current");
            if (sValue != "")
                sjudge_current = sValue;
            #endregion

            #region Threshold Setting
            sValue = ini.ReadValue("Threshold Setting", "end_voltage");
            if (sValue != "")
                send_voltage = sValue;
            sValue = ini.ReadValue("Threshold Setting", "end_current");
            if (sValue != "")
                send_current = sValue;
            #endregion

            #region Protect Setting
            sValue = ini.ReadValue("Protect Setting", "max_charge_time");
            if (sValue != "")
                smax_charge_time = sValue;
            #endregion

            #region Stall Setting
            sValue = ini.ReadValue("Stall Setting", "stall_time");
            if (sValue != "")
                sstall_time = sValue;
            #endregion
        }
        public void SaveConfigFile(string sFile)
        {
            IniFile ini = new IniFile(sFile);

            #region Charge Setting
            ini.WriteValue("Charge Setting", "charge_voltage", scharge_voltage);
            ini.WriteValue("Charge Setting", "charge_current", scharge_current);
            ini.WriteValue("Charge Setting", "judge_time", sjudge_time);
            ini.WriteValue("Charge Setting", "judge_current", sjudge_current);

            #endregion

            #region Threshold Setting
            ini.WriteValue("Threshold Setting", "end_voltage", send_voltage);
            ini.WriteValue("Threshold Setting", "end_current", send_current);
            #endregion

            #region Protect Setting
            ini.WriteValue("Protect Setting", "max_charge_time", smax_charge_time);
            #endregion

            #region Stall Setting
            ini.WriteValue("Stall Setting", "stall_time", sstall_time);
            #endregion
        }
        public void ReadRegTestCountValue()
        {
/*
            string TestCountValue = "";

            RegistryKey key = Registry.LocalMachine;
            RegistryKey subKey = key.OpenSubKey("SOFTWARE\\B94");
            if (subKey == null)
            {
                key.CreateSubKey("SOFTWARE\\B94");
                subKey = key.OpenSubKey("SOFTWARE\\B94", true);
                subKey.SetValue("TestCountValue", 0);
            }
            TestCountValue = subKey.GetValue("TestCountValue").ToString();
            iTestCountValue = int.Parse(TestCountValue);
            subKey.Close();
            key.Close();
*/
        }

        public void SetRegTestCountValue(string TestCountValue)
        {
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey subKey = Key.OpenSubKey("SOFTWARE\\B94");
            if (subKey == null)
            {
                Key.CreateSubKey("SOFTWARE\\B94");
            }
            subKey = Key.OpenSubKey("SOFTWARE\\B94", true);
            subKey.SetValue("TestCountValue", TestCountValue);
            subKey.Close();
            Key.Close();
        }
    }

    class ERRORINFO
    {
        List<int> CodeList = new List<int>();
        List<string> InfoList = new List<string>();

        public ERRORINFO()
        {
            #region Test ErrorNumber & ErrorCode
            AddItem(-2, "");
            AddItem(-1, "Error");
            AddItem(0, "PASS");

            AddItem(201, "DUT Fuse Blown");
            AddItem(202, "DUT Connect Fail");
            AddItem(203, "DUT FW Verify Fail");
            AddItem(204, "DUT FW Erase Check Fail");
            AddItem(205, "DUT Blow Fuse Fail");
            AddItem(206, "DUT Read Check Fail");

            AddItem(999, "Total Test Fail!!");
            #endregion
        }

        void AddItem(int nCode, string sInfo)
        {
            CodeList.Add(nCode);
            InfoList.Add(sInfo);
        }

        public string GetInfo(int nCode)
        {
            string sRet = "";
            bool bFound = false;
            int nIdx;

            for (nIdx = 0; nIdx < CodeList.Count; nIdx++)
            {
                if (nCode == CodeList[nIdx])
                {
                    sRet = InfoList[nIdx];
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
                sRet = "Invalid Error";

            return sRet;
        }
    }

    class TESTLOG
    {
        public string PCBALogFileName;
        public string sWorkOrder;
        public string SerialNo;
        public string ShopFloorDir;
        public bool Result;
        public int ErrorCode;
        public DateTime TestDateTime;

        public string sFWFileName;

        #region Test Item Log Data
        // 0: N/A, 1: Pass, 2: Not Connected, 3: Verify Fail, 4: Fuse Blown, 5: Erase Check Fail, 6: Blow Fuse Fail
        public int nProgram;
        public int nVerify;
        public int nBlowFuse;
        public int nFuseCheck;
        #endregion
        /*-----------------------------------------------------*/
        public float TestTime;

        public TESTLOG()
        {
            PCBALogFileName = "";
            ShopFloorDir = "";
            ErrorCode = -2;
        }
        public void InitData()
        {
            TestDateTime = DateTime.Now;

            SerialNo = "";
            sWorkOrder = "";
            Result = false;

            sFWFileName = "";

            #region Test Item Log Data
            nProgram = 0;
            nVerify = 0;
            nBlowFuse = 0;
            nFuseCheck = 0;
            #endregion
        }
        public void CheckHeader()
        {
            string ss = "";
            StreamWriter sw;

            if (File.Exists(PCBALogFileName) == false)
            {
                sw = File.CreateText(PCBALogFileName);
                ss = "DateTime,Work Order,Serial No.,Result,Error Code,FW File,";
                ss += "#1 Program FW,";
                ss += "#2 Verify FW,";
                ss += "#3 Blow Fuse,";
                ss += "#4 Fuse Check,";

                ss += "Test Time (sec)\r\n";
                sw.Write(ss);
                sw.Close();
            }
        }
        string GetResultStr(int nTestResult)
        {
            string sRet = "";

            switch (nTestResult)
            {
                case 0:
                    sRet = ",N/A";
                    break;
                case 1:
                    sRet = ",PASS";
                    break;
                case 2:
                    sRet = ",NOT Connected";
                    break;
                case 3:
                    sRet = ",Verify Fail";
                    break;
                case 4:
                    sRet = ",Fuse Blown";
                    break;
                case 5:
                    sRet = ",Erase Check Fail";
                    break;
                case 6:
                    sRet = ",Blow Fuse Fail";
                    break;
            }
            return sRet;
        }
        public void WriteLog()
        {
            string ss = "";
            StreamWriter sw;

            if (ErrorCode == -1)
                return;

            sw = File.AppendText(PCBALogFileName);

            ss = TestDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            ss += "," + sWorkOrder;
            ss += "," + SerialNo;

            if (Result)
                ss += ",PASS";
            else
                ss += ",FAIL";

            ss += "," + ErrorCode.ToString();

            ss += "," + sFWFileName;
            
            //#1
            ss += GetResultStr(nProgram);

            //#2
            ss += GetResultStr(nVerify);

            //#3
            ss += GetResultStr(nBlowFuse);

            //#4
            ss += GetResultStr(nFuseCheck);

            ss += "," + TestTime.ToString("F3") + "\r\n";

            sw.Write(ss);
            sw.Close();
        }
        public void WriteShopFloor()
        {
            StreamWriter sw;
            string sPath = "";
            string sData = "";
            string sResult = "";
            string sTime = "";

            if (Result)
                sResult = "PASS";
            else
                sResult = "FAIL";
            sTime = DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss");

            sPath = string.Format("{0}\\{1}.txt", ShopFloorDir, SerialNo);
            sData = string.Format("TTime={0};SN={1};Result={2};ErrorCode={3};Work Order={4}",
                sTime, SerialNo, sResult, ErrorCode, sWorkOrder);

            sw = File.CreateText(sPath);
            sw.Write(sData);
            sw.Close();
        }
    }
}
