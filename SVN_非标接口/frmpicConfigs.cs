using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;


namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    public partial class frmpicConfigs : XtraForm
    {
        public frmpicConfigs()
        {
            InitializeComponent();
        }

        private void frmpicConfigs_Load(object sender, EventArgs e)
        {
            LoadData();
           
        }
        void LoadData()
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string configname = config.AppSettings.Settings["SqlDBConn"].Value;

            string[] configSplit = configname.Split(';');
            var TheFirst = configSplit[0].Split('=');
            textServerName.Text = TheFirst[1].ToString();
            var TheSecond = configSplit[1].Split('=');
            textDbName.Text = TheSecond[1].ToString();
            var TheThird = configSplit[2].Split('=');
            textUser.Text = TheThird[1].ToString();
            var TheFourth = configSplit[3].Split('=');
            textPwd.Text = TheFourth[1].ToString();

            Configuration checkDoctorconfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string checkDoctorconfigname = config.AppSettings.Settings["JCdoc"].Value;
            textCheckdoctor.Text = checkDoctorconfigname;


            Configuration AudioDoctorconfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string AudioDoctorconfigname = config.AppSettings.Settings["SHdoc"].Value;
            textAudiodoctor.Text = AudioDoctorconfigname;


            Configuration Itemconfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string Itemconfigname = config.AppSettings.Settings["JCItemName"].Value;
            txtzh.Text = Itemconfigname;

            Configuration filepath = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string filepaths = config.AppSettings.Settings["DownImagePath"].Value;
            filepat.Text = filepaths;

            Configuration server = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string serever = config.AppSettings.Settings["url"].Value;
            serverURL.Text = serever;

            Configuration deleted = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string isdeleted = config.AppSettings.Settings["ISDelete"].Value;
            isdelete.Text = isdeleted;
        }
        void config()
        {
            //获取Configuration对象
            string isdeleted = isdelete.Text;
            string serverurl = serverURL.Text;
            string apiurl = serverurl + "/api/services/app/";
            string loginurl = serverurl + "/api/";
            string path = filepat.Text;
            SetValue("DownImagePath", path);
            SetValue("ISDelete", isdeleted);
            SetValue("url", serverurl);
            SetValue("loginurl", loginurl);
            SetValue("apiurl", apiurl);

        }
        private bool CheckError()
        {
           
            var tf = true;
            var serviceName = textServerName.Text.Trim();
            var dbname = textDbName.Text.Trim();
            var sqlName = textUser.Text.Trim();
            var sqlpPwd = textPwd.Text.Trim();
            var checkDoctor = textCheckdoctor.Text.Trim();
            var AudioDoctor = textAudiodoctor.Text.Trim();
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                XtraMessageBox.Show(this, "服务器名称为必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tf = false;
            }

            if (string.IsNullOrWhiteSpace(dbname))
            {
                XtraMessageBox.Show(this, "数据库名称为必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tf = false;
            }

            if (!checkEditWindows.Checked && string.IsNullOrWhiteSpace(sqlName))
            {
                XtraMessageBox.Show(this, "用户名为必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tf = false;
            }

            if (!checkEditWindows.Checked && string.IsNullOrWhiteSpace(sqlpPwd))
            {
                XtraMessageBox.Show(this, "密码为必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tf = false;
            }
            if (string.IsNullOrWhiteSpace(checkDoctor))
            {
                XtraMessageBox.Show(this, "检查医生为必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tf = false;
            }
            if (string.IsNullOrWhiteSpace(AudioDoctor))
            {
                XtraMessageBox.Show(this, "审核医生为必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tf = false;
            }

            return tf;
        }

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            //数据库连接对象
            var serviceName = textServerName.Text.Trim();
            var dbname = textDbName.Text.Trim();
            var sqlName = textUser.Text.Trim();
            var sqlpPwd = textPwd.Text.Trim();
            //检查医生
            var checkDoctor = textCheckdoctor.Text.Trim();
            var AudioDoctor = textAudiodoctor.Text.Trim();//审核医生
            var txtzhs = txtzh.Text.Trim();//检查项目
            string isdeleted = isdelete.Text;
            string serverurl = serverURL.Text;
            string apiurl = serverurl + "/api/services/app/";
            string loginurl = serverurl + "/api/";
            string path = filepat.Text;
            if (CheckError())
            {
                string values = "Server="+ serviceName + ";database="+ dbname + ";User ID="+ sqlName + ";Password="+ sqlpPwd+";" ;
                SetValue("SqlDBConn", values);
                SetValue("JCdoc", checkDoctor);
                SetValue("SHdoc", AudioDoctor);
                SetValue("JCItemName", txtzhs);
                SetValue("DownImagePath", path);
                SetValue("ISDelete", isdeleted);
                SetValue("url", serverurl);
                SetValue("loginurl", loginurl);
                SetValue("apiurl", apiurl);
                Application.Restart();

            }

        }

        /// <summary>
        /// 根据Key 修改value
        /// </summary>
        /// <param name="key">要修改的的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key,string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appconfig");
        }
    

        public event Action<string> SqlConnectionStringOk;

        private void simpleButtonTest_Click(object sender, EventArgs e)
        {
            var serviceName = textServerName.Text.Trim();
            var dbname = textDbName.Text.Trim();
            var sqlName = textUser.Text.Trim();
            var sqlpPwd = textPwd.Text.Trim();
            var checkDoctor = textCheckdoctor.Text.Trim();
            var AudioDoctor = textAudiodoctor.Text.Trim();
            if (CheckError())
            {
                if (checkEditWindows.Checked && DbHelper.TestConnectionString(serviceName, dbname))
                {
                    XtraMessageBox.Show(this, "测试成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (!checkEditWindows.Checked && DbHelper.TestConnectionString(serviceName, dbname, sqlName, sqlpPwd))
                {
                    XtraMessageBox.Show(this, "测试成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show(this, "连接失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                //文件夹路径
                filepat.Text = folder.SelectedPath;
            }
        }
    }
}
