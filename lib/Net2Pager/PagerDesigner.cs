// ***********************************************************************
// Assembly         : Net2Pager
// Author           : PeterLiu
// Created          : 07-19-2014
//
// Last Modified By : PeterLiu
// Last Modified On : 01-04-2007
// ***********************************************************************
// <copyright file="PagerDesigner.cs" company="Megadotnet">
//     Copyright (c) Megadotnet. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.IO;

namespace DiyControl.Pager
{
    #region Net2Pager Control Designer
    /// <summary>
    /// <see cref="Net2Pager" /> �������ؼ��������
    /// </summary>
    /// <remarks>http://wintersun.cnblogs.com/</remarks>
    public class PagerDesigner : ControlDesigner
    {
        /// <summary>
        /// ��ʼ�� PagerDesigner ����ʵ����
        /// </summary>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public PagerDesigner()
        {
            //       this.ReadOnly = true;
        }
        /// <summary>
        /// The wb
        /// </summary>
        private Net2Pager wb;

        /// <summary>
        /// ��ȡ���������ʱ��ʾ�����ؼ��� HTML��
        /// </summary>
        /// <returns>���������ʱ��ʾ�ؼ��� HTML��</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public override string GetDesignTimeHtml()
        {

            wb = (Net2Pager)Component;
            wb.RecordCount = 225;
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            wb.RenderControl(writer);
            return sw.ToString();
        }

        /// <summary>
        /// ��ȡ�ڳ��ֿؼ�ʱ��������������ʱΪָ�����쳣��ʾ�� HTML��
        /// </summary>
        /// <param name="e">ҪΪ����ʾ������Ϣ���쳣��</param>
        /// <returns>���ʱΪָ�����쳣��ʾ�� HTML��</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string errorstr = "�����ؼ�ʱ����" + e.Message;
            return CreatePlaceHolderDesignTimeHtml(errorstr);
        }
    }
    #endregion
}
