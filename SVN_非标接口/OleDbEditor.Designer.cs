namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    partial class OleDbEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.simpleButtonTest = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonOk = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.textEditDbPwd = new DevExpress.XtraEditors.TextEdit();
            this.textEditPwd = new DevExpress.XtraEditors.TextEdit();
            this.textEditUser = new DevExpress.XtraEditors.TextEdit();
            this.buttonEditDbFile = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dxErrorProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDbPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditDbFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.simpleButtonTest);
            this.layoutControl1.Controls.Add(this.simpleButtonOk);
            this.layoutControl1.Controls.Add(this.simpleButtonCancel);
            this.layoutControl1.Controls.Add(this.textEditDbPwd);
            this.layoutControl1.Controls.Add(this.textEditPwd);
            this.layoutControl1.Controls.Add(this.textEditUser);
            this.layoutControl1.Controls.Add(this.buttonEditDbFile);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(679, 106, 450, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(325, 202);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // simpleButtonTest
            // 
            this.simpleButtonTest.AutoWidthInLayoutControl = true;
            this.simpleButtonTest.Location = new System.Drawing.Point(16, 159);
            this.simpleButtonTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButtonTest.MinimumSize = new System.Drawing.Size(75, 0);
            this.simpleButtonTest.Name = "simpleButtonTest";
            this.simpleButtonTest.Size = new System.Drawing.Size(75, 27);
            this.simpleButtonTest.StyleController = this.layoutControl1;
            this.simpleButtonTest.TabIndex = 10;
            this.simpleButtonTest.Text = "测试(&T)";
            this.simpleButtonTest.Click += new System.EventHandler(this.simpleButtonTest_Click);
            // 
            // simpleButtonOk
            // 
            this.simpleButtonOk.AutoWidthInLayoutControl = true;
            this.simpleButtonOk.Location = new System.Drawing.Point(153, 159);
            this.simpleButtonOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButtonOk.MinimumSize = new System.Drawing.Size(75, 0);
            this.simpleButtonOk.Name = "simpleButtonOk";
            this.simpleButtonOk.Size = new System.Drawing.Size(75, 27);
            this.simpleButtonOk.StyleController = this.layoutControl1;
            this.simpleButtonOk.TabIndex = 9;
            this.simpleButtonOk.Text = "确定(&S)";
            this.simpleButtonOk.Click += new System.EventHandler(this.simpleButtonOk_Click);
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.AutoWidthInLayoutControl = true;
            this.simpleButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButtonCancel.Location = new System.Drawing.Point(234, 159);
            this.simpleButtonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButtonCancel.MinimumSize = new System.Drawing.Size(75, 0);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(75, 27);
            this.simpleButtonCancel.StyleController = this.layoutControl1;
            this.simpleButtonCancel.TabIndex = 8;
            this.simpleButtonCancel.Text = "取消(&C)";
            // 
            // textEditDbPwd
            // 
            this.textEditDbPwd.Location = new System.Drawing.Point(109, 106);
            this.textEditDbPwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEditDbPwd.Name = "textEditDbPwd";
            this.textEditDbPwd.Size = new System.Drawing.Size(200, 24);
            this.textEditDbPwd.StyleController = this.layoutControl1;
            this.textEditDbPwd.TabIndex = 7;
            // 
            // textEditPwd
            // 
            this.textEditPwd.Location = new System.Drawing.Point(109, 76);
            this.textEditPwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEditPwd.Name = "textEditPwd";
            this.textEditPwd.Size = new System.Drawing.Size(200, 24);
            this.textEditPwd.StyleController = this.layoutControl1;
            this.textEditPwd.TabIndex = 6;
            // 
            // textEditUser
            // 
            this.textEditUser.Location = new System.Drawing.Point(109, 46);
            this.textEditUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEditUser.Name = "textEditUser";
            this.textEditUser.Size = new System.Drawing.Size(200, 24);
            this.textEditUser.StyleController = this.layoutControl1;
            this.textEditUser.TabIndex = 5;
            // 
            // buttonEditDbFile
            // 
            this.dxErrorProvider.SetIconAlignment(this.buttonEditDbFile, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.buttonEditDbFile.Location = new System.Drawing.Point(109, 16);
            this.buttonEditDbFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEditDbFile.Name = "buttonEditDbFile";
            this.buttonEditDbFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditDbFile.Properties.ReadOnly = true;
            this.buttonEditDbFile.Size = new System.Drawing.Size(200, 24);
            this.buttonEditDbFile.StyleController = this.layoutControl1;
            this.buttonEditDbFile.TabIndex = 4;
            this.buttonEditDbFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditDbFile_ButtonClick);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlGroup1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.emptySpaceItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(325, 202);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.buttonEditDbFile;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(299, 30);
            this.layoutControlItem1.Text = "数据库文件：";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 18);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 120);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(299, 23);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.textEditUser;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(299, 30);
            this.layoutControlItem2.Text = "用户名：";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(90, 18);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.textEditPwd;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(299, 30);
            this.layoutControlItem3.Text = "密码：";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(90, 18);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.textEditDbPwd;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(299, 30);
            this.layoutControlItem4.Text = "数据库密码：";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(90, 18);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.simpleButtonCancel;
            this.layoutControlItem5.Location = new System.Drawing.Point(218, 143);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(81, 33);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.simpleButtonOk;
            this.layoutControlItem6.Location = new System.Drawing.Point(137, 143);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(81, 33);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.simpleButtonTest;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 143);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(81, 33);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(81, 143);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(56, 33);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Access 文件|*.mdb";
            // 
            // dxErrorProvider
            // 
            this.dxErrorProvider.ContainerControl = this;
            // 
            // OleDbEditor
            // 
            this.AcceptButton = this.simpleButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.simpleButtonCancel;
            this.ClientSize = new System.Drawing.Size(325, 202);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "OleDbEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Access 数据库连接编辑器";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditDbPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditDbFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonTest;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOk;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.TextEdit textEditDbPwd;
        private DevExpress.XtraEditors.TextEdit textEditPwd;
        private DevExpress.XtraEditors.TextEdit textEditUser;
        private DevExpress.XtraEditors.ButtonEdit buttonEditDbFile;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider;
    }
}