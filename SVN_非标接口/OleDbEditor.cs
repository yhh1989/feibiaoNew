using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    public partial class OleDbEditor : DevExpress.XtraEditors.XtraForm
    {
        public OleDbEditor()
        {
            InitializeComponent();
        }

        public bool CheckEditor()
        {
            dxErrorProvider.ClearErrors();
            var tf = true;
            var dbFile = buttonEditDbFile.Text.Trim();
            if (string.IsNullOrWhiteSpace(dbFile))
            {
                dxErrorProvider.SetError(buttonEditDbFile, "数据库名称为必填项！");
                tf = false;
            }

            return tf;
        }

        private void buttonEditDbFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    buttonEditDbFile.Text = openFileDialog.FileName;
                }
            }
        }

        private void simpleButtonTest_Click(object sender, EventArgs e)
        {
            if (CheckEditor())
            {
                var serverName = buttonEditDbFile.Text.Trim();
                var user = textEditUser.Text.Trim();
                var pwd = textEditPwd.Text.Trim();
                var dbPwd = textEditDbPwd.Text.Trim();
                if (DbHelper.TestOleDbConnectionString(serverName, user, pwd, dbPwd))
                {
                    XtraMessageBox.Show(this, "测试成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show(this, "连接失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public event Action<string> SqlConnectionStringOk;

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            if (CheckEditor())
            {
                var serverName = buttonEditDbFile.Text.Trim();
                var user = textEditUser.Text.Trim();
                var pwd = textEditPwd.Text.Trim();
                var dbPwd = textEditDbPwd.Text.Trim();
                var sqlConnectionString = DbHelper.GetOleDbConnectionString(serverName, user, pwd, dbPwd);
                SqlConnectionStringOk?.Invoke(sqlConnectionString);
                DialogResult = DialogResult.OK;
            }
        }
    }
}