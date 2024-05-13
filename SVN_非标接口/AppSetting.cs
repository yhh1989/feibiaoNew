using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using HealthExamination.HardwareDrivers.Commons;
using HealthExaminationSystem.ThirdParty.DataTransmission.Properties;

namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    public partial class AppSetting : XtraForm
    {
        public AppSetting()
        {
            InitializeComponent();
        }

        private void buttonEditMonitoringFolder_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                    buttonEditMonitoringFolder.Text = folderBrowserDialog.SelectedPath;
            }
            else if (e.Button.Kind == ButtonPredefines.Delete)
            {
                buttonEditMonitoringFolder.Text = string.Empty;
            }
        }

        private void AppSetting_Load(object sender, EventArgs e)
        {
            buttonEditMonitoringFolder.Text = Settings.Default.MonitoringFolder;
            buttonEditInstrumentId.Text = Settings.Default.InstrumentId;
            if (string.IsNullOrWhiteSpace(Settings.Default.InstrumentId))
            {
                buttonEditInstrumentId.PerformClick(buttonEditInstrumentId.Properties.Buttons[0]);
            }
            buttonEditMonitoringFolderOfData.Text = Settings.Default.MonitoringFolderOfData;
            buttonEditSqlConnectionString.Text = Settings.Default.SqlConnectionString;
            radioGroupDbType.EditValue = Settings.Default.DbType;
            txturl.EditValue = Settings.Default.url;
            txtmethodname.EditValue = Settings.Default.methodname;
            txtclassname.EditValue = Settings.Default.classname;
        }

        private void ButtonEditMonitoringFolder_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.MonitoringFolder = buttonEditMonitoringFolder.Text;
            Settings.Default.Save();
        }

        private void ButtonEditInstrumentId_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.OK)
                buttonEditInstrumentId.Text = Guid.NewGuid().ToString("D").ToUpper();
        }

        private void buttonEditInstrumentId_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.InstrumentId = buttonEditInstrumentId.Text;
            Settings.Default.Save();
        }

        private void buttonEditMonitoringFolderOfData_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.MonitoringFolderOfData = buttonEditMonitoringFolderOfData.Text;
            Settings.Default.Save();
        }

        private void buttonEditMonitoringFolderOfData_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                    buttonEditMonitoringFolderOfData.Text = folderBrowserDialog.SelectedPath;
            }
            else if (e.Button.Kind == ButtonPredefines.Delete)
            {
                buttonEditMonitoringFolderOfData.Text = string.Empty;
            }
        }

        private void buttonEditSqlConnectionString_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (radioGroupDbType.EditValue.Equals(1))
            {
                using (var frm = new DbEditor())
                {
                    frm.SqlConnectionStringOk += sql => { buttonEditSqlConnectionString.Text = sql; };
                    frm.ShowDialog(this);
                }
            }
            else if (radioGroupDbType.EditValue.Equals(2))
            {
                using (var frm = new OleDbEditor())
                {
                    frm.SqlConnectionStringOk += sql => { buttonEditSqlConnectionString.Text = sql; };
                    frm.ShowDialog(this);
                }
            }
        }

        private void buttonEditSqlConnectionString_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.SqlConnectionString = buttonEditSqlConnectionString.Text;
            Settings.Default.Save();
        }

        private void radioGroupDbType_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Default.DbType = (int)radioGroupDbType.EditValue;
            Settings.Default.Save();
        }

        private void txtmethodname_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Default.methodname = (string)txtmethodname.EditValue;
            Settings.Default.Save();
        }

        private void txturl_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Default.url = (string)txturl.EditValue;
            Settings.Default.Save();
        }

        private void txtclassname_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Default.classname = (string)txtclassname.EditValue;
            Settings.Default.Save();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strpath = System.Environment.CurrentDirectory + "log.txt";
            if (!File.Exists(strpath))
            {
                File.Create(strpath);
            }
            try
            {
                object[] objs = new object[1];
                object _req_data = "201904250004";
                objs[0] = _req_data;
                object ab = WebServiceHelper.InvokeWebService(txturl.Text, txtclassname.Text, txtmethodname.Text, objs);
                string str = ab.ToString();
                MessageBox.Show("获取结果1" + txturl.Text + txtclassname.Text + txtmethodname.Text);
                MessageBox.Show("获取结果"+str);
                DirFileHelper.WriteText(strpath, str, Encoding.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取结果2" + ex.Message+"\\\\"+ex.Source);
               

                DirFileHelper.WriteText(strpath, GetExceptionMsg(ex,"测试"), Encoding.Default);
               
            }
        }
        private  string GetExceptionMsg(Exception ex, string backStr)
        {
            var sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now);
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    var result = GetExceptionMsg(ex.InnerException, backStr);
                    sb.AppendLine(result);
                }
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}