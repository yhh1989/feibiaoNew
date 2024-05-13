using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Text;


namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    public static class SQLHelper
    {

        public static string SelectSQl(string strsql, string strcon)
        {
            string strres = "";
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(strsql, con);
                con.Open();
                try
                {
                    strres = cmd.ExecuteScalar().ToString();
                }
                catch (Exception)
                {


                }

                WriteTxt(strsql);
                con.Dispose();
                con.Close();
            }
            return strres;

        }
        public static DataTable SelectData(string strsql, string strcon)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(strsql, con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                con.Dispose();
                con.Close();
                //WriteTxt(strsql);
            }
            return dt;

        }
        public static int UpdateSQl(string strsql, string strcon)
        {
            int ires = 0;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(strsql, con);
                con.Open();
                ires = cmd.ExecuteNonQuery();
                WriteTxt(strsql);
                con.Dispose();
                con.Close();
            }
            return ires;

        }
      
       
        public static bool WriteTxt(string str)
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\log";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs = new FileStream(System.Environment.CurrentDirectory + "\\log\\Log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.WriteLine(str);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool AddSystemLog(string EmployeeId, string Object, string LogText, string strjcon)
        {
            LogText = LogText.Replace("'", "$");
            string strsql = "insert into systemlog(LogTime,EmployeeId,IP" +
                    ",Object,LogText) values(getdate(),'" + EmployeeId + "','192.168.0.1','" + Object + "','" + LogText + "')";
            UpdateSQl(strsql, strjcon);

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtitem">需要转换的数据</param>
        /// <param name="DataTableName">转入的表名</param>
        /// <param name="strcolumnName">暂时没有</param>
        /// <param name="progressBarControl1">进度条</param>
        public static void InsertCusData(DataTable dtitem, string DataTableName, string strCusName, string strcon)
        {
            //SQLHelpe.v9_ExcuteNonQuery(dtitem, DataTableName);
            //return;
            StringBuilder strinsertsql = new StringBuilder();
            strinsertsql.Append(" insert into QZJ." + DataTableName + " (");
            for (int d = 0; d < dtitem.Columns.Count; d++)
            {
                if (dtitem.Columns[d].ToString() != "UPLOADDATETIME")
                {
                    strinsertsql.Append(dtitem.Columns[d].ToString() + ",");
                }

            }
            int inum = 1;
            string strsql = "";
            StringBuilder strsuminsertsql = new StringBuilder();
            int isumnum = 1;

            foreach (DataRow item in dtitem.Rows)
            {
                try
                {
                    StringBuilder strb2 = new StringBuilder();
                    strb2.Append(strinsertsql.ToString().TrimEnd(',') + ") VALUES (");
                    for (int d = 0; d < dtitem.Columns.Count; d++)
                    {
                        if (dtitem.Columns[d].ToString() == "UPLOADDATETIME")
                        {
                            continue;
                        }
                        if (dtitem.Columns[d].ToString() == "BATCHID")
                        {
                            strb2.Append("'" + strCusName + "',");
                        }
                        else
                        {
                            strb2.Append("'" + item[dtitem.Columns[d].ToString()].ToString() + "',");
                        }

                    }
                    strsql = strb2.ToString().TrimEnd(',') + ")";
                    strsuminsertsql.Append(strsql + " ");
                    if (isumnum == 1000 || inum == dtitem.Rows.Count)
                    {
                        isumnum = 1;
                        //System.Threading.Thread.Sleep(10);
                       
                        strsuminsertsql.Clear();
                        //WriteTxt("转换:" + DataTableName
                        //    + "表\t;" + item[strcolumnName].ToString() +
                        //    "\t\t数据成功\t;总计:" + dtitem.Rows.Count + "条;当前记录:" + inum + "条; \n\r", mtxtiteminfo);
                    }


                    inum++;
                    isumnum++;
                }
                catch (Exception ex)
                {
                    WriteTxt("转换失败:" + DataTableName + "\n\r 错误原因:" + ex.Message + "\n\r SQL:" + strsuminsertsql.ToString());
                    //SQLHelpe.WriteData("转换" + DataTableName + "表数据时出现错误：" + ex.Message + "; SQL:" + strsql.ToString() + "\n\r");
                }
            }
        }
    }
}