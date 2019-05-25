using System.Net;
using System;
using System.Management;
using System.Runtime.InteropServices;
namespace QTranser.QTranseLib
{
    public class GetMac
    {
        /// <summary>  
        /// 获取网卡硬件地址  
        /// </summary>  
        public string GetMacAddress()
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    mac = mo["MacAddress"].ToString();
                    break;
                }
            }
            return mac;
        }
    }

}
