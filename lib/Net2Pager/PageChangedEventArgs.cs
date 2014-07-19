// ***********************************************************************
// Assembly         : Net2Pager
// Author           : PeterLiu
// Created          : 07-19-2014
//
// Last Modified By : PeterLiu
// Last Modified On : 01-19-2007
// ***********************************************************************
// <copyright file="PageChangedEventArgs.cs" company="Megadotnet">
//     Copyright (c) Megadotnet. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace DiyControl.Pager
{
    #region PageChangedEventArgs Class
    /// <summary>
    /// Ϊ <see cref="Net2Pager" /> �ؼ��� <see cref="Net2Pager.PageChanged" /> �¼��ṩ���ݡ��޷��̳д��ࡣ
    /// </summary>
    /// <remarks>�� <see cref="Net2Pager" /> �ؼ���ҳ����Ԫ��֮һ���������û�����ҳ�����ύʱ���� <see cref="Net2Pager.PageChanged" /> �¼���
    /// <p>�й� PageChangedEventArgs ʵ���ĳ�ʼ����ֵ�б������ PageChangedEventArgs ���캯����</p></remarks>
    public sealed class PageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The _newpageindex
        /// </summary>
        private readonly int _newpageindex;

        /// <summary>
        /// ʹ����ҳ��������ʼ�� PageChangedEventArgs �����ʵ����
        /// </summary>
        /// <param name="newPageIndex">�û��� <see cref="Net2Pager" /> �ؼ���ҳѡ��Ԫ��ѡ���Ļ���ҳ�����ı������ֹ������ҳ��������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public PageChangedEventArgs(int newPageIndex)
        {
            this._newpageindex = newPageIndex;
        }

        /// <summary>
        /// ��ȡ�û��� <see cref="Net2Pager" /> �ؼ���ҳѡ��Ԫ����ѡ���Ļ���ҳ�����ı������ֹ������ҳ��������
        /// </summary>
        /// <value>�û��� <see cref="Net2Pager" /> �ؼ���ҳѡ��Ԫ����ѡ���Ļ���ҳ�����ı����������ҳ��������</value>
        /// <example>
        /// �����ʾ����ʾ���ʹ�� NewPageIndex ����ȷ���û��� <see cref="Net2Pager" /> �ؼ���ҳѡ��Ԫ����ѡ���Ļ���ҳ�����ı����������ҳ��������
        /// ��ֵȻ����������Ҫ��ʾѡ��ҳ�� Net2Pager �ؼ��� <see cref="Net2Pager.CurrentPageIndex" /> ���ԣ�����������ʾ�ؼ����°����ݡ�
        ///   <code><![CDATA[
        ///   <%@ Page Language="C#"%>
        ///   <%@ Import Namespace="System.Data"%>
        ///   <%@Import Namespace="System.Data.SqlClient"%>
        ///   <%@Import Namespace="System.Configuration"%>
        ///   <%@Register TagPrefix="Net2Pager" Namespace="Wuqi.Net2Pager" Assembly="aspnetpager"%>
        ///   <HTML>
        ///   <HEAD>
        ///   <TITLE>Welcome to Net2Pager.com </TITLE>
        ///   <script runat="server">
        /// SqlConnection conn;
        /// SqlCommand cmd;
        /// void Page_Load(object src,EventArgs e)
        /// {
        /// conn=new SqlConnection(ConfigurationSettings.AppSettings["ConnStr"]);
        /// if(!Page.IsPostBack)
        /// {
        /// cmd=new SqlCommand("GetNews",conn);
        /// cmd.CommandType=CommandType.StoredProcedure;
        /// cmd.Parameters.Add("@pageindex",1);
        /// cmd.Parameters.Add("@pagesize",1);
        /// cmd.Parameters.Add("@docount",true);
        /// conn.Open();
        /// pager.RecordCount=(int)cmd.ExecuteScalar();
        /// conn.Close();
        /// BindData();
        /// }
        /// }
        /// void BindData()
        /// {
        /// cmd=new SqlCommand("GetNews",conn);
        /// cmd.CommandType=CommandType.StoredProcedure;
        /// cmd.Parameters.Add("@pageindex",pager.CurrentPageIndex);
        /// cmd.Parameters.Add("@pagesize",pager.PageSize);
        /// cmd.Parameters.Add("@docount",false);
        /// conn.Open();
        /// dataGrid1.DataSource=cmd.ExecuteReader();
        /// dataGrid1.DataBind();
        /// conn.Close();
        /// }
        /// void ChangePage(object src,PageChangedEventArgs e)
        /// {
        /// pager.CurrentPageIndex=e.NewPageIndex;
        /// BindData();
        /// }
        ///   </script>
        ///   <meta http-equiv="Content-Language" content="zh-cn">
        ///   <meta http-equiv="content-type" content="text/html;charset=gb2312">
        ///   <META NAME="Generator" CONTENT="EditPlus">
        ///   <META NAME="Author" CONTENT="Net2Pager(yhaili@21cn.com)">
        ///   </HEAD>
        ///   <body>
        ///   <form runat="server" ID="Form1">
        ///   <asp:DataGrid id="dataGrid1" runat="server" />
        ///   <Net2Pager:Net2Pager id="pager"
        /// runat="server"
        /// PageSize="8"
        /// NumericButtonCount="8"
        /// ShowCustomInfoSection="before"
        /// ShowInputBox="always"
        /// CssClass="mypager"
        /// HorizontalAlign="center"
        /// OnPageChanged="ChangePage" />
        ///   </form>
        ///   </body>
        ///   </HTML>
        /// ]]>
        ///   </code>
        ///   <p>�����Ǹ�ʾ�����õ�Sql Server�洢���̣�</p>
        ///   <code>
        ///   <![CDATA[
        /// CREATE procedure GetNews
        /// (@pagesize int,
        /// @pageindex int,
        /// @docount bit)
        /// as
        /// set nocount on
        /// if(@docount=1)
        /// select count(id) from news
        /// else
        /// begin
        /// declare @indextable table(id int identity(1,1),nid int)
        /// declare @PageLowerBound int
        /// declare @PageUpperBound int
        /// set @PageLowerBound=(@pageindex-1)*@pagesize
        /// set @PageUpperBound=@PageLowerBound+@pagesize
        /// set rowcount @PageUpperBound
        /// insert into @indextable(nid) select id from news order by addtime desc
        /// select O.id,O.source,O.title,O.addtime from news O,@indextable t where O.id=t.nid
        /// and t.id>@PageLowerBound and t.id<=@PageUpperBound order by t.id
        /// end
        /// set nocount off
        /// GO
        /// ]]>
        ///   </code>
        ///   </example>
        /// <remarks>ʹ�� NewPageIndex ����ȷ���û��� <see cref="Net2Pager" /> �ؼ���ҳѡ��Ԫ����ѡ���Ļ���ҳ�����ı����������ҳ��������
        /// ��ֵ����������Ҫ��ʾѡ��ҳ�� Net2Pager �ؼ��� <see cref="Net2Pager.CurrentPageIndex" /> ���ԡ�</remarks>
        public int NewPageIndex
        {
            get { return _newpageindex; }
        }
    }
    #endregion
}
