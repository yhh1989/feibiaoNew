using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using HealthExaminationSystem.ThirdParty.DataTransmission.Properties;


namespace HealthExaminationSystem.ThirdParty.DataTransmission
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //MessageBox.Show("1");
            // 设置全局异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
          //  MessageBox.Show("2");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // MessageBox.Show("3");
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");         
            try
            {
                // login.Authenticate(loginModel);
                //MessageBox.Show("5");
                Application.Run(new FrmNewPic());
                // Application.Run(new Startup());
               // MessageBox.Show("6");
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message );
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                var str = GetExceptionMsg(e.Exception, e.ToString());
                //File.AppendAllText(Path.Combine(Variables.LogDirectory, $"LastExceptionInfo-{DateTime.Now:yyyyMMdd}.txt"), str);
            }
            catch
            {
                // ignored
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
               // File.AppendAllText(Path.Combine(Variables.LogDirectory, $"LastExceptionInfo-{DateTime.Now:yyyyMMdd}.txt"), str);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>  
        /// 生成自定义异常消息  
        /// </summary>  
        /// <param name="ex">异常对象</param>  
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>  
        /// <returns>异常字符串文本</returns>  
        private static string GetExceptionMsg(Exception ex, string backStr)
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
