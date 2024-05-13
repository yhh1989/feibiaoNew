using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HealthExaminationSystem.ThirdParty.DataTransmission.Comm
{
   public class PrintService
    {
        public PrintService()
        {

        }

        #region Members //成员
        public String printName = String.Empty;
        public Font prtTextFont = new Font("Verdana", 16);
        public Font prtTitleFont = new Font("宋体", 16);
        private String[] titles = new String[0];
        public String[] Titles
        {
            set
            {
                titles = value as String[];
                if (null == titles)
                {
                    titles = new String[0];
                }
            }
            get
            {
                return titles;
            }
        }
        private Int32 left = 20;
        private Int32 top = 20;
        public Int32 Top { set { top = value; } get { return top; } }
        public Int32 Left { set { left = value; } get { return left; } }
        private DataTable printedTable;
        private Int32 pheight;
        private Int32 pWidth;
        private Int32 pindex;
        private Int32 curdgi;
        private Int32[] width;
        private Int32 rowOfDownDistance = 3;
        private Int32 rowOfUpDistance = 2;

        Int32 startColumnControls = 0;
        Int32 endColumnControls = 0;

        #endregion

        #region Method for PrintDataTable //打印数据集
        /// <summary>
        /// 打印数据集
        /// </summary>
        /// <param name="table">数据集</param>
        /// <returns></returns>
        public Boolean PrintDataTable(DataTable table)
        {
            PrintDocument prtDocument = new PrintDocument();
            try
            {
                if (printName != String.Empty)
                {
                    prtDocument.PrinterSettings.PrinterName = printName;
                }
                //prtDocument.DefaultPageSettings.Landscape = true;//设置打印方向为横向
                prtDocument.OriginAtMargins = false;
                PrintDialog prtDialog = new PrintDialog();
                prtDialog.AllowSomePages = true;
                prtDialog.ShowHelp = false;
                prtDialog.Document = prtDocument;
                if (DialogResult.OK != prtDialog.ShowDialog())
                {
                    return false;
                }
                printedTable = table;
                pindex = 0;
                curdgi = 0;
                width = new Int32[printedTable.Columns.Count];
                pheight = prtDocument.PrinterSettings.DefaultPageSettings.Bounds.Bottom + 400;
                //pheight = prtDocument.PrinterSettings.DefaultPageSettings.Bounds.Bottom;
                pWidth = prtDocument.PrinterSettings.DefaultPageSettings.Bounds.Right;
                prtDocument.PrintPage += new PrintPageEventHandler(docToPrint_PrintPage);
                prtDocument.Print();
            }
            catch (InvalidPrinterException ex)
            {
                MessageBox.Show("没有安装打印机");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            prtDocument.Dispose();
            return true;
        }
        #endregion

        #region Handler for docToPrint_PrintPage //设置打印机开始打印的事件处理函数
        /// <summary>
        /// 设置打印机开始打印的事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void docToPrint_PrintPage(object sender,
         System.Drawing.Printing.PrintPageEventArgs ev)//设置打印机开始打印的事件处理函数
        {
            Int32 x = left;
            Int32 y = top;
            Int32 rowOfTop = top;
            Int32 i;
            Pen pen = new Pen(Brushes.Black, 1);
            if (0 == pindex)
            {
                for (i = 0; i < titles.Length; i++)
                {
                    ev.Graphics.DrawString(titles[i], prtTitleFont, Brushes.Black, left, y + rowOfUpDistance);
                    y += prtTextFont.Height + rowOfDownDistance;
                }
                rowOfTop = y;
                foreach (System.Data.DataRow dr in printedTable.Rows)
                {
                    for (i = 0; i < printedTable.Columns.Count; i++)
                    {
                        Int32 colwidth = Convert.ToInt16(ev.Graphics.MeasureString(dr[i].ToString().Trim(), prtTextFont).Width);
                        if (colwidth > width[i])
                        {
                            width[i] = colwidth;
                        }
                    }
                }
            }
            for (i = endColumnControls; i < printedTable.Columns.Count; i++)
            {
                String headtext = printedTable.Columns[i].ColumnName.Trim();
                if (pindex == 0)
                {
                    int colwidth = Convert.ToInt16(ev.Graphics.MeasureString(headtext, prtTextFont).Width);
                    if (colwidth > width[i])
                    {
                        width[i] = colwidth;
                    }
                }
                //判断宽是否越界
                if (x + width[i] > pheight)
                {
                    break;
                }
                ev.Graphics.DrawString(headtext, prtTextFont, Brushes.Black, x, y + rowOfUpDistance);
                x += width[i];
            }
            startColumnControls = endColumnControls;
            if (i < printedTable.Columns.Count)
            {
                endColumnControls = i;
                ev.HasMorePages = true;
            }
            else
            {
                endColumnControls = printedTable.Columns.Count;
            }
            ev.Graphics.DrawLine(pen, left, rowOfTop, x, rowOfTop);
            y += rowOfUpDistance + prtTextFont.Height + rowOfDownDistance;
            ev.Graphics.DrawLine(pen, left, y, x, y);
            //打印数据
            for (i = curdgi; i < printedTable.Rows.Count; i++)
            {
                x = left;
                for (Int32 j = startColumnControls; j < endColumnControls; j++)
                {
                    ev.Graphics.DrawString(printedTable.Rows[i][j].ToString().Trim(), prtTextFont, Brushes.Black, x, y + rowOfUpDistance);
                    x += width[j];
                }
                y += rowOfUpDistance + prtTextFont.Height + rowOfDownDistance;
                ev.Graphics.DrawLine(pen, left, y, x, y);
                //判断高是否越界
                if (y > pWidth - prtTextFont.Height - 400) //if (y > pWidth - prtTextFont.Height - 30)
                {
                    break;
                }
            }
            ev.Graphics.DrawLine(pen, left, rowOfTop, left, y);
            x = left;
            for (Int32 k = startColumnControls; k < endColumnControls; k++)
            {
                x += width[k];
                ev.Graphics.DrawLine(pen, x, rowOfTop, x, y);
            }
            //判断高是否越界
            if (y > pWidth - prtTextFont.Height - 400) //if (y > pWidth - prtTextFont.Height - 30) 
            {
                pindex++; if (0 == startColumnControls)
                {
                    curdgi = i + 1;
                }
                if (!ev.HasMorePages)
                {
                    endColumnControls = 0;
                }
                ev.HasMorePages = true;
            }
        }
        #endregion
    }
}
