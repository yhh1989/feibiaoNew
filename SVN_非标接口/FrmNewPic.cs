using HealthExamination.HardwareDrivers;
using HealthExamination.HardwareDrivers.Models.LisInterface;
using HealthExaminationSystem.ThirdParty.DataTransmission.Comm;
using HealthExaminationSystem.ThirdParty.DataTransmission.Models;
using HealthExaminationSystem.ThirdParty.DataTransmission.Models.LisInterfaceDataSetTableAdapters;
using HealthExaminationSystem.ThirdParty.DataTransmission.Properties;

using Microsoft.Win32;
using Newtonsoft.Json;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    public partial class FrmNewPic : Form
    {
        public FrmNewPic()
        {
            InitializeComponent();
            _lisInterfaceDataSet = new LisInterfaceDataSet();
            _interfaceResultTableAdapter = new TInterfaceresultTableAdapter();
            _lisInterfaceHardwareDriver = DriverFactory.GetDriver<ILisInterfaceHardwareDriver>();
        }
        public bool IsRunning { get; set; }
        string DownImagePath = "";
        string UPImagePath = "";
        string SqlDBConn = "";
        string JCdoc = "";
        string SHdoc = "";
        string JCItemName = "";
        string jczd = "";
        string ISDelete = "";
        string Revolve = "";
        private string token;
        string TJURl = "";
        string DepartmentID = "";
        string YQID = "";
        Size size  = new Size();
        List<string> vs = new List<string>();
        private readonly TInterfaceresultTableAdapter _interfaceResultTableAdapter;

        private readonly LisInterfaceDataSet _lisInterfaceDataSet;

        private readonly ILisInterfaceHardwareDriver _lisInterfaceHardwareDriver;
        private void FrmNewPic_Load(object sender, EventArgs e)
        {
            #region 初始化字段
            DownImagePath = ConfigurationManager.AppSettings["DownImagePath"];
            UPImagePath = ConfigurationManager.AppSettings["UPImagePath"];
            SqlDBConn = ConfigurationManager.AppSettings["SqlDBConn"];
            JCdoc = ConfigurationManager.AppSettings["JCdoc"];
            SHdoc = ConfigurationManager.AppSettings["SHdoc"];
            cmbzh.Text = ConfigurationManager.AppSettings["JCItemName"];
            ISDelete = ConfigurationManager.AppSettings["ISDelete"];
            var PICFormat = ConfigurationManager.AppSettings["PICFormat"];
            Revolve = ConfigurationManager.AppSettings["Revolve"];
            YQID = ConfigurationManager.AppSettings["YQID"];
            txtzd.Text = ConfigurationManager.AppSettings["DefSetting"];
            #endregion

            try
            {
                #region 初始化医生
                if (JCdoc != "")
                {
                    var namelist = JCdoc.Split('|');
                    if (namelist.Length > 1)
                    {
                        foreach (var conname in namelist)
                        {
                            if (!string.IsNullOrEmpty(conname))
                            {
                                tb_jyys.Items.Add(conname);
                            }
                        }
                        tb_jyys.Text = namelist[0];
                    }
                    else
                    {
                        tb_jyys.Text = JCdoc;
                    }
                }
                if (SHdoc != "")
                {

                    var namelist = SHdoc.Split('|');
                    if (namelist.Length > 1)
                    {
                        foreach (var conname in namelist)
                        {
                            if (!string.IsNullOrEmpty(conname))
                            {
                                tb_shys.Items.Add(conname);
                            }
                        }
                        tb_shys.Text = namelist[0];
                    }
                    else
                    {
                        tb_shys.Text = SHdoc;

                    }
                }
                #endregion
                #region 初始化科室和字典 
                showItemInfo(cmbzh.Text);
                loaddep();
                
                #endregion

                #region 初始化列表
                showCus();
                #endregion

                

                
            }
            catch
            { }

            #region 建立图片监控
            if (PICFormat == "PDF")
            {
                FileSystemWatcher file = new FileSystemWatcher(DownImagePath, "*.PDF");
                file.NotifyFilter = NotifyFilters.Size;
                file.Created += new FileSystemEventHandler(PDF_Monitor);
                file.Changed += new FileSystemEventHandler(PDF_Monitor);
                file.IncludeSubdirectories = true;
                file.EnableRaisingEvents = true;
                //MessageBox.Show("PDF文件监控建立成功！");
            }
            else if (PICFormat == "JPG")
            {
                MyFileSystemWatcher fsw = new MyFileSystemWatcher(DownImagePath, "*.JPG");
                fsw.Created += new System.IO.FileSystemEventHandler(fsw_Created);
                fsw.Changed += new System.IO.FileSystemEventHandler(fsw_Changed);
                fsw.EnableRaisingEvents = true;
                fsw.IncludeSubdirectories = true;
            }
            else if (PICFormat == "PNG")
            {
                MyFileSystemWatcher fsw = new MyFileSystemWatcher(DownImagePath, "*.PNG");
                fsw.Created += new System.IO.FileSystemEventHandler(fsw_Created);
                fsw.Changed += new System.IO.FileSystemEventHandler(fsw_Changed);
                fsw.EnableRaisingEvents = true;
                fsw.IncludeSubdirectories = true;
            }
            else if (PICFormat == "JPEG")
            {
                //MessageBox.Show("确定建立了JPEG文件监控");
                MyFileSystemWatcher fsw = new MyFileSystemWatcher(DownImagePath, "*.jpeg");
                fsw.Created += new System.IO.FileSystemEventHandler(fsw_Created);
                fsw.Changed += new System.IO.FileSystemEventHandler(fsw_Changed);
                fsw.EnableRaisingEvents = true;
                fsw.IncludeSubdirectories = true;
            }
            else
            { MessageBox.Show("请填写正确的图片格式！{PICFormat}"); }
            #endregion

            #region 双缓冲解决频闪
            Type type = dataGridView1.GetType();
            PropertyInfo pi = type.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView1, true, null);
            #endregion
        }
        #region 控件方法
        /// <summary>
        /// 查询人员列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Showcus_Click(object sender, EventArgs e)
        {
            showCus();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Convert_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                if (MessageBox.Show("没有图片是否要继续保存？", "提示",MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            pictureBox1.Image = null;
            try
            { 
                string strjcsj = Convert.ToDateTime(dateTimePicker2.Value).ToString("yyyy-MM-dd") + DateTime.Now.ToString(" HH:mm:ss");
                string strsqlw = "select COUNT(1) FROM TInterfaceresult where initemid='" + cmbzh.Text + "' and inactivenum='" + tb_id.Text + "'";
                string isnum = SQLHelper.SelectSQl(strsqlw, SqlDBConn);
                //MessageBox.Show(isnum);
                string strdate = "select GETDATE()";
                System.Data.DataTable tb_date = SQLHelper.SelectData(strdate, SqlDBConn);
                LisInterfaceResult lisInterfaceResult = new LisInterfaceResult();
                lisInterfaceResult.ItemId = cmbzh.Text;
                lisInterfaceResult.InstrumentId = YQID;
                lisInterfaceResult.DoctorId = tb_jyys.Text;
                lisInterfaceResult.AuditDoctorId = tb_shys.Text;
                lisInterfaceResult.ResultState = 0;
                //MessageBox.Show("3");
                if (com_Mark.Text == "")
                { MessageBox.Show("项目标识为必填项！");
                    return;
                }
                if (com_Mark.Text == "阳性")
                {
                    lisInterfaceResult.Mark = "P";
                }
                else if (com_Mark.Text == "阴性")
                {
                    lisInterfaceResult.Mark = "M";
                }
                else
                { lisInterfaceResult.Mark = "P"; }
                lisInterfaceResult.PhysicalExamNo = tb_id.Text;
                lisInterfaceResult.Name = txtname.Text;
                lisInterfaceResult.Age = txtage.Text;
                lisInterfaceResult.Sex = txtsex.Text;
                lisInterfaceResult.Value = txtzd.Text;
                //lisInterfaceResult.
                //MessageBox.Show("4");
                if (checkBox1.Checked == true)
                {
                    lisInterfaceResult.CheckDate = Convert.ToDateTime(tb_date.Rows[0]["column1"].ToString());
                    lisInterfaceResult.ResultDate = Convert.ToDateTime(tb_date.Rows[0]["column1"].ToString());
                }
                else
                {
                    lisInterfaceResult.CheckDate = Convert.ToDateTime(strjcsj);
                    lisInterfaceResult.ResultDate = Convert.ToDateTime(strjcsj);
                }
                lisInterfaceResult.PictureDirectory = txtpic.Text;
                //MessageBox.Show("5");
                lisInterfaceResult.IdNum = 1;
                Add(lisInterfaceResult);
                if (Com_Wjz.Text == "A类")
                {
                    string uptwjz = "update TInterfaceresult set wjz = '1' where initemid = '" + cmbzh.Text + "' and inactivenum ='" + tb_id.Text + "' ";
                    int upt =  SQLHelper.UpdateSQl(uptwjz,SqlDBConn);
                    
                }
                if (Com_Wjz.Text == "B类")
                {
                    string uptwjz = "update TInterfaceresult set wjz = '2' where initemid = '" + cmbzh.Text + "' and inactivenum ='" + tb_id.Text + "' ";
                    int upt = SQLHelper.UpdateSQl(uptwjz, SqlDBConn);

                }
                string uptchar = "update TInterfaceresult set   initemchar = '" + txtzd.Text + "' where inactivenum ='" + tb_id.Text + "' and initemid = '" + cmbzh.Text + "' ";
                int charid = SQLHelper.UpdateSQl(uptchar, SqlDBConn);

                if (ISDelete == "Y")
                {
                    try
                    {
                        File.Delete(txtpic.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                        throw;
                    }

                }
                txtzd.Text = jczd;
                tb_id.Text = "";
                txtpic.Text = "";
                txtname.Text = "";
                txtage.Text = "";
                txtsex.Text = "";
                txtclient.Text = "";
                tb_id.Tag = "";
                tb_id.Focus();
                txtzd.Text = jczd;
                this.WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            txtzd.Text = "";
        }
        /// <summary>
        /// 体检号回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                string strsql = "SELECT ArchivesNum,Name,Customer.Sex,Customer.Age,ClientName,CustomerRegID FROM CustomerReg  " +
                 "INNER JOIN Customer on Customer.CustomerID=CustomerReg.CustomerID  " +
                 "LEFT JOIN ClientReg on ClientReg.ClientRegID=CustomerReg.ClientRegID " +
                 "LEFT JOIN ClientInfo on ClientInfo.ClientID=ClientReg.ClientID " +
                 "WHERE ArchivesNum='" + tb_id.Text + "'";
                //金东方项目使用视图为 V_clientreg 维护时注意
                System.Data.DataTable dt = SQLHelper.SelectData(strsql, SqlDBConn);

                //记录今日体检号
                string strpath = System.Environment.CurrentDirectory+"\\"+"log";
                string fileName = DateTime.Now.ToString("yyyyMMdd")+".log";
                TextWriter writer;
                if (File.Exists(strpath + "//" + fileName))
                { writer = File.AppendText(strpath + "//" + fileName); }
                else
                {
                    FileInfo fileInfo = new FileInfo(strpath + "//" + fileName);
                    writer= fileInfo.CreateText();
                }
                writer.WriteLine(tb_id.Text);
                writer.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    txtname.Text = dt.Rows[0]["Name"].ToString();
                    txtsex.Text = dt.Rows[0]["Sex"].ToString();
                    txtage.Text = dt.Rows[0]["Age"].ToString();
                    txtclient.Text = dt.Rows[0]["ClientName"].ToString();
                    tb_id.Tag = dt.Rows[0]["CustomerRegID"].ToString();
                }
                else
                {
                    MessageBox.Show("当前无此检查人");
                }
                // bt_Convert_Click(sender, e);
                showCus();
            }
        }
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstzd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstzd.SelectedItems == null || lstzd.SelectedItems.Count <= 0)
            {
                return;
            }
            ListViewItem lvi = lstzd.SelectedItems[0];
            txtzd.Text += lvi.Text;
        }
        private void cmbzh_SelectedIndexChanged(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["JCItemName"].Value = cmbzh.Text;
            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            showItemInfo(cmbzh.Text);
        }
        /// <summary>
        /// grid绘制触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try 
            {
                
                for (int i = 0; i <= dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["检查状态"].Value.ToString() == "已检")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        
                    }
                    else
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                       
                    }
                }
                foreach (var item in vs)
                {
                    string Gitem = item;

                    if (item == null)
                    { Gitem = ""; }
                    for (int i = 0; i <= dataGridView1.Rows.Count; i++)
                    {

                        if (dataGridView1.Rows[i].Cells["体检号"].Value.ToString() == Gitem)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Blue;
                            
                        }
                        
                        
                    }
                    
                }
            }
            catch { }
            
        }
        /// <summary>
        /// 审核医生初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_shys_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings["SHdoc"].Value = tb_shys.Text;
            //config.AppSettings.SectionInformation.ForceSave = true;
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// 检查医生初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_jyys_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings["JCdoc"].Value = tb_jyys.Text;
            //config.AppSettings.SectionInformation.ForceSave = true;
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "新的Excel.xls";
            saveFileDialog1.Filter = "Excel文件(*.xls) | *.xls";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.Data.DataTable dt = PrintCus();
            //MessageBox.Show("已检查过DT了，不是语句的问题");
           // SaveDStoExcel(saveFileDialog1.FileName, dt);
            DataTabletoExcel(dt,saveFileDialog1.FileName);

        }
        /// <summary>
        /// 配置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            frmNewConfigs frmNew = new frmNewConfigs();
            frmNew.Show();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 查询人员列表
        /// </summary>
        private void showCus()
        {
            int yijian = 0;
            int weijian = 0;
            int yidengji = 0;
            try
            {

                string distinctsql = "select distinct ArchivesNum from v_cus_checkState where 1=1";
                
                string strcmd = "select  ArchivesNum as 体检号,Name as 姓名,ClientName as 单位,case sex when '1' then '男' when '2' then '女' else '不详' END as 性别 ,age as 年龄 ,checkdate as 检查时间,checkstate as 检查状态 from v_cus_checkState where 1=1 ";
                if (dateTimePicker1.Text != "")
                {
                    strcmd += "and CheckDate >='" + dateTimePicker1.Text + "'";
                    distinctsql += "and CheckDate >='" + dateTimePicker1.Text + "'";
                }
                if (CusDepShow.Text != "")
                {
                    string str_dep_cmd = "select top 1 DepartmentID from Department where DepartmentName = '" + CusDepShow.Text + "'";
                    System.Data.DataTable dataTable = SQLHelper.SelectData(str_dep_cmd, SqlDBConn);
                    DepartmentID = dataTable.Rows[0][0].ToString();
                    strcmd += "and DepartmentID ='" + DepartmentID + "'";
                    distinctsql += "and DepartmentID ='" + DepartmentID + "'";
                }
                 strcmd += "order by ArchivesNum";
                System.Data.DataTable dt1 = SQLHelper.SelectData(distinctsql, SqlDBConn);
                System.Data.DataTable dt = SQLHelper.SelectData(strcmd, SqlDBConn);

                dataGridView1.DataSource = dt;
                
                foreach (DataRow item in dt1.Rows)
                {
                    string statenum = "select top 1 checkstate from v_cus_checkState where ArchivesNum = '"+ item["ArchivesNum"].ToString() + "'";
                    System.Data.DataTable dt2 = SQLHelper.SelectData(statenum, SqlDBConn);
                    //MessageBox.Show(""+statenum+"");
                    foreach (DataRow item1 in dt2.Rows)
                    {
                        if (item1["checkstate"].ToString() == "已检")
                        {
                            yijian++;
                        }
                        else
                        {
                            weijian++;
                        }
                    }
                    
                }
                label3.Text ="已预约"+ weijian.ToString() + "人";
                label2.Text = "体检已上传"+yijian.ToString() + "人";
                foreach (var item in vs)
                {
                    yidengji++;
                }
                label4.Text = "已登记"+yidengji.ToString() + "人";
                string vs_temp = "";
                string strpath = System.Environment.CurrentDirectory + "\\" + "log";
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                FileStream fs = new FileStream(strpath + "//" + fileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while ((vs_temp = sr.ReadLine()) != null)
                {
                    vs.Add(vs_temp);
                    vs_temp = sr.ReadLine();
                }
                
                sr.Close();
                fs.Close();
            }
            catch(Exception ex)
            {
                
            }
            
            
        }

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void loaddep()
        {
            string strsql = "SELECT DepartmentName FROM Department";
            System.Data.DataTable dt = SQLHelper.SelectData(strsql, SqlDBConn);
            foreach (DataRow item in dt.Rows)
            {
                cmbzh.Items.Add(item[0].ToString());
                CusDepShow.Items.Add(item[0].ToString());
            }
        }
        /// <summary>
        /// 初始化字典
        /// </summary>
        /// <param name="cmbzh"></param>
        private void showItemInfo(string cmbzh)
        {
            string itemzd = "SELECT Word FROM InterfaceItemComparison " +
            "INNER JOIN ItemDictionary on InterfaceItemComparison.ItemId =ItemDictionary.ItemId " +
            "WHERE ObverseItemID='" + cmbzh + "' " +
            "ORDER BY OrderNum";
            System.Data.DataTable dtzd = SQLHelper.SelectData(itemzd, SqlDBConn);
            lstzd.Items.Clear();
            foreach (DataRow item in dtzd.Rows)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = item;
                lvi.Text = item[0].ToString();
                lstzd.Items.Add(lvi);
            }
            lstzd.Refresh();
            string itemstand = "SELECT Symbol FROM InterfaceItemComparison " +
"INNER JOIN ItemStandard on InterfaceItemComparison.ItemId =ItemStandard.ItemId " +
"WHERE ObverseItemID='" + cmbzh + "' AND IsNormal=0 ";
            jczd = SQLHelper.SelectSQl(itemstand, SqlDBConn);
        }
        /// <summary>
        /// 获得Token
        /// </summary>
        /// <returns></returns>
        private bool getToken()
        {


            var Tenant = Settings.Default.Tenant;
            var User = Settings.Default.User;
            var tjPwd = Settings.Default.Pwd;
            string url = TJURl + "api/Account";
            string json = "{TenancyName:'" + Tenant + "',UsernameOrEmailAddress:'" + User + "',Password:'" + tjPwd + "'}";
            var str = HttpApi(url, json, "POST");
            if (str == "failure")
            {
                MessageBox.Show("获取token失败");
                return false;
            }
            var oJson = JsonConvert.DeserializeObject<dynamic>(str);
            if (Convert.ToBoolean(oJson.success))
            {
                token = oJson.result;
                return true;
            }
            else
            {
                return false;
            }
        }
        private string HttpApi(string url, string jsonstr, string type, bool needToken = false)
        {
            try
            {
                System.Windows.Forms.Application.DoEvents();

                if (needToken)
                {
                    getToken();
                }

                Thread.Sleep(1000);

                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//webrequest请求api地址  
                request.Accept = "text/html,application/xhtml+xml,*/*";
                request.ContentType = "application/json";
                request.Method = type.ToUpper().ToString();//get或者post  
                if (needToken)
                {
                    request.Headers["Authorization"] = "Bearer " + token; //传递进去认证Token
                }
                byte[] buffer = encoding.GetBytes(jsonstr);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {

                return "failure";
            }

        }
        /// <summary>
        /// 插入数据库
        /// </summary>
        /// <param name="value"></param>
        public void Add(LisInterfaceResult value)
        {
            try
            {
                  // MessageBox.Show("开始插入数据");
                var data = _interfaceResultTableAdapter.GetDataByItemIdAndPhysicalExamNoAndInstrumentId(value.ItemId,
                    value.PhysicalExamNo, value.InstrumentId);
                //MessageBox.Show("1111");
                if (data.Rows.Count == 0)
                {
                    // MessageBox.Show("无数据");
                    if (!string.IsNullOrWhiteSpace(value.PictureDirectory))
                    {
                        var pictrue = value.PictureDirectory.Split('|');
                        var ids = new List<string>();
                        foreach (var s in pictrue)
                        {

                            try
                            {
                                // MessageBox.Show("开始处理照片");
                                //var pic = _pictureController.Uploading(s, "PACS");
                                //ids.Add(pic.Id.ToString("D"));
                                TJURl = ConfigurationManager.AppSettings["url"];
                                var url = TJURl + "Picture/Uploading";
                                WebClient webClient = new WebClient();
                                webClient.QueryString.Set("belong", "QMWS");
                                getToken();//获取      
                                webClient.Headers.Set("Authorization", "Bearer " + token);
                                var outData = webClient.UploadFile(url, s);
                                //上传体检
                                var str = webClient.Encoding.GetString(outData);

                                var oJson = JsonConvert.DeserializeObject<dynamic>(str);

                                var ss = oJson.result;
                                var id = ss.id;
                                
                                ids.Add(id.ToString("D"));
                              // MessageBox.Show("照片上传完毕");
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.ToString());
                            }

                        }

                        value.PictureDirectory = string.Join("|", ids.ToArray());
                    }

                    _interfaceResultTableAdapter.Insert(
                        value.ItemId,
                        value.ItemName,
                        value.Name,
                        value.Age,
                        value.Sex,
                        value.PhysicalExamNo,
                        value.Value,
                        value.Diagnose,
                        value.DoctorId,
                        value.DoctorName,
                        value.AuditDoctorId,
                        value.AuditDoctorName,
                        value.CheckDate,
                        value.InstrumentId,
                        value.PictureDirectory,
                        value.ReferenceValue,
                        value.Mark,
                        value.Unit,
                        value.BarCode,
                        value.CustomerRegId,
                        value.ResultState,
                        value.ResultDate
                        );
                }
                else
                {
                    // MessageBox.Show("有数据");

                    var row = data[0];
                    try
                    {

                        if (!string.IsNullOrWhiteSpace(value.PictureDirectory))
                        {
                            if (row.inPicDirs == null || row.inPicDirs == "")
                            {
                                var pictrue = value.PictureDirectory.Split('|');
                                var ids = new List<string>();
                                foreach (var s in pictrue)
                                {
                                    // MessageBox.Show("开始图片1");
                                    //var pic = _pictureController.Uploading(s, "PACS");
                                    //ids.Add(pic.Id.ToString("D"));
                                    TJURl = ConfigurationManager.AppSettings["url"];
                                    var url = TJURl + "Picture/Uploading";
                                    WebClient webClient = new WebClient();
                                    webClient.QueryString.Set("belong", "QMWS");
                                    getToken();//获取      
                                    webClient.Headers.Set("Authorization", "Bearer " + token);
                                    var outData = webClient.UploadFile(url, s);
                                    //上传体检
                                    var str = webClient.Encoding.GetString(outData);

                                    var oJson = JsonConvert.DeserializeObject<dynamic>(str);

                                    var ss = oJson.result;
                                    var id = ss.id;

                                    ids.Add(id.ToString("D"));
                                    //  MessageBox.Show("开始图片2");
                                }

                                value.PictureDirectory = string.Join("|", ids.ToArray());
                            }
                            else
                            {
                                var pictrue = value.PictureDirectory.Split('|');
                                var ids = row.inPicDirs.Split('|');
                                var newIds = ids.ToList();
                                //if (ids.Length >= pictrue.Length)
                                //{
                                for (var i = 0; i < pictrue.Length; i++)
                                {
                                    //var pic = _pictureController.Update(pictrue[i], new Guid(ids[i]));
                                    //if (new Guid(newIds[i]) != pic.Id)
                                    //{
                                    //    newIds[i] = pic.Id.ToString("D");
                                    //}

                                    TJURl = ConfigurationManager.AppSettings["url"];
                                    var url = TJURl + "Picture/Uploading";
                                    WebClient webClient = new WebClient();
                                    webClient.QueryString.Set("belong", "QMWS");
                                    getToken();//获取      
                                    webClient.Headers.Set("Authorization", "Bearer " + token);
                                    var outData = webClient.UploadFile(url, pictrue[i]);
                                    //上传体检
                                    var str = webClient.Encoding.GetString(outData);

                                    var oJson = JsonConvert.DeserializeObject<dynamic>(str);

                                    var ss = oJson.result;
                                    var id = ss.id;
                                    newIds[i] = id.ToString("D");

                                }
                                //}
                                //else
                                //{
                                //    for (var i = 0; i < ids.Length; i++)
                                //        _pictureController.Update(pictrue[i], new Guid(ids[i]));
                                //    for (var i = ids.Length; i < pictrue.Length; i++)
                                //    {
                                //        var pic = _pictureController.Uploading(pictrue[i], "PACS");
                                //        newIds.Add(pic.Id.ToString("D"));
                                //    }
                                //}

                                value.PictureDirectory = string.Join("|", newIds.ToArray());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("错误了:" + ex.Message + ex.Source);
                        throw ex;

                    }


                    _interfaceResultTableAdapter.UpdateQuery(
                        value.ItemId,
                        value.ItemName,
                        value.Name,
                        value.Age,
                        value.Sex,
                        value.PhysicalExamNo,
                        value.Value,
                        value.Diagnose,
                        value.DoctorId,
                        value.DoctorName,
                        value.AuditDoctorId,
                        value.AuditDoctorName,
                        value.CheckDate ?? DateTime.Now,
                        value.InstrumentId,
                        value.PictureDirectory,
                        value.ReferenceValue,
                        value.Mark,
                        value.Unit,
                        value.BarCode,
                        value.CustomerRegId,
                        value.ResultState ?? 1,
                        value.ResultDate ?? DateTime.Now,
                        row.idnum);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }
        /// <summary>
        /// 查询需要打印的表
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable PrintCus()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                string print_ctr = " select distinct reg.LoginDate as 检查时间, reg.CustomerBM as 体检号,cus.Name as 姓名,cus.Age as 年龄," +
  "case cus.Sex when'1' then '男' when '2' then '女' end as 性别, g.ItemGroupName 检查组合" +
  " from TjlCustomerItemGroups g" +
" inner join TjlCustomerRegs reg" +
" on reg.id = g.CustomerRegBMId " +
" inner join tjlcustomers cus on cus.id= reg.CustomerId" +
  "  where 1 = 1 ";
                if (CusDepShow.Text != "")
                {
                    string str_dep_cmd = "select top 1 DepartmentID from Department where DepartmentName = '" + CusDepShow.Text + "'";
                    System.Data.DataTable dataTable = SQLHelper.SelectData(str_dep_cmd, SqlDBConn);
                    DepartmentID = dataTable.Rows[0][0].ToString();
                    print_ctr += "and g.DepartmentId = '" + DepartmentID + "'";
                }
                //循环文本文件
                string strpath = System.Environment.CurrentDirectory + "\\" + "log";
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                FileStream fs = new FileStream(strpath + "//" + fileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string vs_temp = "";
                System.Data.DataTable lsdt;
                dt = SQLHelper.SelectData("" + print_ctr + " and reg.CustomerBM ='1'", SqlDBConn);
                while ((vs_temp = sr.ReadLine()) != null)
                {
                    string numstar = @"and reg.CustomerBM =";
                    numstar += "'" + vs_temp + "'";
                    string ls = "";
                    ls = print_ctr + numstar;
                    lsdt = SQLHelper.SelectData(ls, SqlDBConn);
                   // MessageBox.Show("循环文本了！");
                    foreach (DataRow item in lsdt.Rows)
                    {
                        //MessageBox.Show("" + print_ctr + "");
                       // MessageBox.Show("AD数据了"+item[0].ToString()+"");
                        dt.ImportRow(item);
                       
                    }
                }
                DataColumn column = new DataColumn("id",Type.GetType("System.Int32"));
                dt.Columns.Add(column);
                dt.Columns["id"].SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][0] = i + 1;
                }
                //MessageBox.Show("要返回了");
                
            }
            catch (Exception ex)
                { MessageBox.Show(ex.Message); }

            return dt;

        }
        /// <summary>
        /// datatableTOExcel
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="dtExcel"></param>
        private void SaveDStoExcel(string Path, System.Data.DataTable dtExcel)
        {
            //try
            //{
            //    //MessageBox.Show("方法进来了");
            //    //创建Excel应用程序
            //    Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            //    if (xApp == null)
            //    {
            //        MessageBox.Show("错误：Excel不能打开！");
            //        return;
            //    }
            //    object objOpt = System.Reflection.Missing.Value;
            //    Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks.Add(true);//添加新工作簿
            //    Microsoft.Office.Interop.Excel.Sheets xSheets = xBook.Sheets;
            //    Microsoft.Office.Interop.Excel._Worksheet xSheet = null;
            //    //
            //    //创建空的sheet
            //    //
            //    xSheet = (Microsoft.Office.Interop.Excel._Worksheet)(xBook.Sheets.Add(objOpt, objOpt, objOpt, objOpt));

            //    if (xSheet == null)
            //    {
            //        MessageBox.Show("错误：工作表为空！");
            //        return;
            //    }
            //    //写数据集表头
            //    for (int k = 0; k < dtExcel.Columns.Count; k++)
            //        xSheet.Cells[1, k + 1] = dtExcel.Columns[k].ColumnName.ToString().Trim();
            //    //写数据集数据
            //    for (int i = 0; i < dtExcel.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < dtExcel.Columns.Count; j++)
            //            xSheet.Cells[i + 2, j + 1] = dtExcel.Rows[i][j];
            //    }
            //    //保存文件
            //    xBook.Saved = true;
            //    xBook.SaveCopyAs(Path);

            //    //显示文件
            //    xApp.Visible = true;
            //    //
            //    //释放资源
            //    //
            //    Marshal.ReleaseComObject(xSheet);
            //    xSheet = null;
            //    Marshal.ReleaseComObject(xSheets);
            //    xSheets = null;

            //    Marshal.ReleaseComObject(xBook);
            //    xBook = null;
            //    xApp.Quit();
            //    Marshal.ReleaseComObject(xApp);
            //    xApp = null;
            //    GC.Collect();//强行销毁
            //    //textBox1.Text = "成功写入";

            //}
            //catch (Exception ex)
            //{
            //    //textBox1.Text = "写入Excel发生错误：" + ex.Message;
            //}
        }
        /// <summary>
        /// datatableTOExcel
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="dtExcel"></param>
        public static void DataTabletoExcel(System.Data.DataTable tmpDataTable, string strFileName)
        {

            //if (tmpDataTable == null)

            //    return;

            //int rowNum = tmpDataTable.Rows.Count;

            //int columnNum = tmpDataTable.Columns.Count;

            //int rowIndex = 1;

            //int columnIndex = 0;



            //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            //xlApp.DefaultFilePath = "";

            //xlApp.DisplayAlerts = true;

            //xlApp.SheetsInNewWorkbook = 1;

            //Workbook xlBook = xlApp.Workbooks.Add(true);
            ////将DataTable的列名导入Excel表第一行

            //foreach (DataColumn dc in tmpDataTable.Columns)

            //{

            //    columnIndex++;

            //    xlApp.Cells[rowIndex, columnIndex] = dc.ColumnName;

            //}



            ////将DataTable中的数据导入Excel中

            //for (int i = 0; i < rowNum; i++)

            //{

            //    rowIndex++;

            //    columnIndex = 0;

            //    for (int j = 0; j < columnNum; j++)

            //    {

            //        columnIndex++;

            //        xlApp.Cells[rowIndex, columnIndex] = tmpDataTable.Rows[i][j].ToString();

            //    }

            //}

            ////xlBook.SaveCopyAs(HttpUtility.UrlDecode(strFileName, System.Text.Encoding.UTF8));

            //xlBook.SaveCopyAs(strFileName);

        }
        #endregion

        #region PDF  
        void PDF_Monitor(object sender, System.IO.FileSystemEventArgs e)
        {
             
            Thread.Sleep(1000);
            if (System.IO.Path.GetExtension(e.FullPath).ToLower() == ".pdf")
            {
                MessageBox.Show("开始");
                #region PDFtoJPG
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    try
                    {
                        var watcher = sender as FileSystemWatcher;
                        watcher.EnableRaisingEvents = false;

                        //PDFtoJPG(e.FullPath, e.Name);
                        PDFConvertToJPG(e.FullPath, 0);
                        watcher.EnableRaisingEvents = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                });
                #endregion

            }
        }
        /// <summary>
        /// PDF转JPG
        /// </summary>
        /// <param name="StrImgPath"></param>
        /// <param name="StrimgName"></param>
        //private void PDFtoJPG(string StrImgPath, string StrimgName)
        //{
        //    BeginInvoke((System.Action)(() =>
        //    {

        //        if (System.IO.Path.GetExtension(StrimgName) == ".pdf")
        //        {
        //            PDFFile pdfFile = PDFFile.Open(StrImgPath);
        //            int qxd = 100;
        //            //if (com_qxd.Text == "超清")
        //            //{ qxd = 500; }
        //            //else if (com_qxd.Text == "高清")
        //            //{ qxd = 250; }

        //            Bitmap pageImage = pdfFile.GetPageImage(0, qxd * 1);
        //            var path = Path.GetDirectoryName(StrImgPath);//目录信息
        //            var filename = Path.GetFileNameWithoutExtension(StrimgName);//文件名
        //                                                                        //对图像处理
        //            int canKao = pageImage.Width > pageImage.Height ? pageImage.Height : pageImage.Width;
        //            int newHeight = canKao > 1080 ? pageImage.Height / 2 : pageImage.Height;
        //            int newWidth = canKao > 1080 ? pageImage.Width / 2 : pageImage.Width;
        //            Graphics g = Graphics.FromImage(pageImage);
        //            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //            g.DrawImage(pageImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight),
        //            new System.Drawing.Rectangle(0, 35, pageImage.Width, pageImage.Height - 35), GraphicsUnit.Pixel);
        //            if (Revolve == "Y")
        //            {
        //                pageImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //            }

        //            g.Dispose();
        //            pdfFile.Dispose();
        //            pageImage.Save(path + @"\" + filename + "." + "jpg", ImageFormat.Jpeg);
        //            pageImage.Clone();
        //            pageImage.Dispose();
        //            List<string> images = new List<string>();
        //            images.Add(path + @"\" + filename + "." + "jpg");
        //            foreach (var a in images)
        //            {
        //                txtpic.Text = a;
        //            }
        //            pictureBox1.Image = Image.FromFile(txtpic.Text);
        //            if (pictureBox1.Image != null)
        //            { label12.Text = "JPG格式转换完成！"; }
        //            tb_id.Focus();
        //            this.WindowState = FormWindowState.Normal;
        //        }
        //    }));

        //}


        /// <summary>
        /// PDF转JPG(不丢失文字)
        /// </summary>
        /// <param name="inFilePath">输入物理路径（E:\\pdf\\test.pdf）</param>
        /// <param name="pagenum">打印页数（E:\\img\\test.jpg）</param>
        public void PDFConvertToJPG(string inFilePath, int pagenum, bool all = false, string NewPath = "")
        {
            BeginInvoke((System.Action)(() =>
            {
                List<string> images = new List<string>();
            var path = Path.GetDirectoryName(inFilePath);//目录信息
            if (NewPath != "")
            {
                path = NewPath;
            }
            var filename = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(inFilePath));//文件名
            using (var document = PdfDocument.Load(inFilePath))
            {
                var pageCount = document.PageCount;
                var dpi = 50;
                if (all == true)
                {
                    for (int i = 0; i < document.PageCount; i++)
                    {
                        using (var image = document.Render(i, dpi, dpi, PdfRenderFlags.CorrectFromDpi))
                        {
                            var encoder = ImageCodecInfo.GetImageEncoders()
                                .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                            var encParams = new EncoderParameters(1);
                            encParams.Param[0] = new EncoderParameter(
                                System.Drawing.Imaging.Encoder.Quality, 10L);
                            var imageName = path + @"\" + filename + "_" + i.ToString() + "." + "jpg";
                            image.Save(imageName, encoder, encParams);
                            images.Add( imageName);
                        }
                    }
                }
                else
                {


                    using (var image = document.Render(pagenum, dpi, dpi, PdfRenderFlags.CorrectFromDpi))
                    {
                        var encoder = ImageCodecInfo.GetImageEncoders()
                            .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        var encParams = new EncoderParameters(1);
                        encParams.Param[0] = new EncoderParameter(
                            System.Drawing.Imaging.Encoder.Quality, 10L);
                        images .Add(path + @"\" + filename + "." + "jpg");
                        image.Save(path + @"\" + filename + "." + "jpg", encoder, encParams);
                    }
                }

                foreach (var a in images)
                {
                    txtpic.Text = a;
                }
                pictureBox1.Image = Image.FromFile(txtpic.Text);
                if (pictureBox1.Image != null)
                { label12.Text = "JPG格式转换完成！"; }
                tb_id.Focus();
                this.WindowState = FormWindowState.Normal;
            }
            }));
        }
        #endregion

        #region JPG
        void fsw_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            AddInterfaceData(e.FullPath, e.Name);
        }
        void fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            AddInterfaceData(e.FullPath, e.Name);
        }
        public void AddInterfaceData(string StrImgPath, string StrimgName)
        {
            Thread.Sleep(1000);
            if (System.IO.Path.GetExtension(StrimgName) == ".JPEG"||System.IO.Path.GetExtension(StrimgName) == ".JPG" || System.IO.Path.GetExtension(StrimgName) == ".Jpg" || System.IO.Path.GetExtension(StrimgName) == ".jpg" || System.IO.Path.GetExtension(StrimgName) == ".jpeg" || System.IO.Path.GetExtension(StrimgName) == ".png" || System.IO.Path.GetExtension(StrimgName) == ".jpeg")
            {

                //MessageBox.Show("监控到了！");
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    try
                    {
                        shouinf(StrImgPath);
                    }
                    catch (Exception)
                    {


                    }

                });
            }

        }
        private void shouinf(string StrImgPath)
        {
            BeginInvoke((System.Action)(() =>
            {
                Bitmap map = new Bitmap(StrImgPath);
                if (Revolve == "Y")
                {
                    //if (MessageBox.Show("是否旋转？","提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                        map.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        File.Delete(StrImgPath);
                        map.Save(StrImgPath);
                    //}
                    
                }
                map.Dispose();

                txtpic.Text = StrImgPath;
                if (!string.IsNullOrEmpty(txtpic.Text))
                {
                    FileStream fileStream = new FileStream(txtpic.Text, FileMode.Open, FileAccess.Read);
                    pictureBox1.Image = Image.FromStream(fileStream);
                    fileStream.Close();
                    fileStream.Dispose();
                }
                this.WindowState = FormWindowState.Normal;
                tb_id.Focus();
            }));
        }
        #endregion

        private void check_wjz_CheckedChanged(object sender, EventArgs e)
        {
            //if (check_wjz.Checked == true)
            //{ com_Mark.Text = "阳性"; }
            //else if ( check_wjz.Checked == false)
            //        { com_Mark.Text = "阴性"; }
        }
    }
}
