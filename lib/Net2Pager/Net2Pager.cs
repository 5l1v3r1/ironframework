// ***********************************************************************
// Assembly         : Net2Pager
// Author           : PeterLiu
// Created          : 07-19-2014
//
// Last Modified By : PeterLiu
// Last Modified On : 01-19-2007
// ***********************************************************************
// <copyright file="Net2Pager.cs" company="Megadotnet">
//     Copyright (c) Megadotnet. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region ����
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
#endregion
namespace DiyControl.Pager
{
    #region Net2Pager Server Control

    #region �ؼ�˵����ʾ��
    /// <summary>
    /// ����ASP.NET WebӦ�ó����ж����ݽ��з�ҳ�ĵķ������ؼ���
    /// </summary>
    /// <example>����ʾ��˵�������Net2Pager��GridView���з�ҳ��
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
    /// pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
    /// pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
    /// pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
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
    ///   <asp:GridView id="dataGrid1" runat="server" />
    ///   <Net2Pager:Net2Pager id="pager"
    /// runat="server"
    /// PageSize="8"
    /// NumericButtonCount="8"
    /// ShowCustomInfoSection="left"
    /// PagingButtonSpacing="0"
    /// ShowInputBox="always"
    /// CssClass="mypager"
    /// HorizontalAlign="right"
    /// OnPageChanged="ChangePage"
    /// SubmitButtonText="ת��"
    /// NumericButtonTextFormatString="[{0}]"/>
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
    ///   </code></example>
    /// <remarks>��ͬ��GridGrid�ؼ���Net2Pager��ҳ�ؼ���������ʾ�κ����ݣ���ֻ��ʾҳ����Ԫ�أ�������ҳ���ϵ���ʾ��ʽ��ÿؼ��޹ء��ÿؼ�����ΪGridView��DataList��Repeater�Լ��Զ���ؼ����з�ҳ�����Sql�洢���̣���ҳ���ܽ�ʹ��GridView��ҳ�����������������ǵ���������ʱ���ܿ�����������
    /// <p>Ҫʹ�� Net2Pager ��ҳ�ؼ�����������ָ������ <see cref="RecordCount" /> ���ԣ�ָ������д <see cref="PageChanged" /> �¼��Ĵ������
    /// <see cref="RecordCount" /> ����ָ��Ҫ��ҳ���������ݵ�����������δָ����ֵ���ֵС�ڵ��� <see cref="PageSize" /> ����Net2Pager�ؼ�������ʾ�κ����ݡ�
    /// ��δָ������д <see cref="PageChanged" /> �¼�����������û����ҳ����Ԫ�ػ���ҳ�����ı�������ʽ����ҳ�������ύʱNet2Pager������ת��ָ����ҳ��
    /// Net2Pager�ؼ��ķ�ҳ������GridView������ͬ���������� <see cref="PageChanged" /> �¼���������н������¼����ݵ� <see cref="PageChangedEventArgs" /> �� <see cref="PageChangedEventArgs.NewPageIndex" />ֵ���� Net2Pager�� <see cref="CurrentPageIndex" />���ԣ�Ȼ�����½��µ�������������ʾ�ؼ��󶨡� </p></remarks>
    [DefaultProperty("PageSize")]
    [DefaultEvent("PageChanged")]
    [ParseChildren(false)]
    [PersistChildren(false)]
    [Description("ר����ASP.Net2.0WebӦ�ó���ķ�ҳ�ؼ�")]
    [Designer(typeof(PagerDesigner))]
    [ToolboxData("<{0}:Net2Pager runat=server></{0}:Net2Pager>")]
    public class Net2Pager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        /// <summary>
        /// The CSS class name
        /// </summary>
        private string cssClassName;
        /// <summary>
        /// The input page index
        /// </summary>
        private string inputPageIndex;

        #region Properties

        #region Navigation Buttons

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵ��ʾ�����ָ����ͣ�ڵ�����ť��ʱ�Ƿ���ʾ������ʾ��
        /// </summary>
        /// <value><c>true</c> if [show navigation tool tip]; otherwise, <c>false</c>.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("������ť"),
            DefaultValue(true),
            Description("ָ�������ͣ���ڵ�����ť��ʱ���Ƿ���ʾ������ʾ")]
        public bool ShowNavigationToolTip
        {
            get
            {
                object obj = ViewState["ShowNavigationToolTip"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["ShowNavigationToolTip"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����õ�����ť������ʾ�ı��ĸ�ʽ��
        /// </summary>
        /// <value>The navigation tool tip text format string.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("������ť"),
            DefaultValue("ת����{0}ҳ"),
            Description("ҳ������ť������ʾ�ı��ĸ�ʽ")]
        public string NavigationToolTipTextFormatString
        {
            get
            {
                object obj = ViewState["NavigationToolTipTextFormatString"];
                return (obj == null) ? "ת����{0}ҳ" : (string)obj;
            }
            set
            {
                string tip = value;
                if (tip.Trim().Length < 1 && tip.IndexOf("{0}") < 0)
                    tip = "{0}";
                ViewState["NavigationToolTipTextFormatString"] = tip;
            }
        }

        #region ��ȡ������һ��ֵ����ֵָʾ�Ƿ�ҳ������ť���������ִ���

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ�ҳ������ť���������ִ��档
        /// </summary>
        /// <value><c>true</c> if [chinese page index]; otherwise, <c>false</c>.</value>
        /// <remarks>����ֵ��Ϊtrue����δʹ��ͼƬ��ťʱ��ҳ������ť�е���ֵ1��2��3�Ƚ��ᱻ�����ַ�һ���������ȴ��档</remarks>
        [Browsable(true),
            Category("������ť"),
            DefaultValue(false),
            Description("�Ƿ�ҳ������ֵ��ť����������һ���������ȴ���")]
        public bool ChinesePageIndex
        {
            get
            {
                object obj = ViewState["ChinesePageIndex"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["ChinesePageIndex"] = value;
            }
        }

        #endregion


        /// <summary>
        /// ��ȡ������ҳ������ֵ������ť�����ֵ���ʾ��ʽ��
        /// </summary>
        /// <value>�ַ�����ָ��ҳ������ֵ��ť�����ֵ���ʾ��ʽ��Ĭ��ֵΪ<see cref="String.Empty" />����δ���ø����ԡ�</value>
        /// <remarks>ʹ��NumericButtonTextFormatString����ָ��ҳ������ֵ��ť����ʾ��ʽ����δ���ø�ֵʱ������ť�ı������ǣ�1 2 3 ...�����ø�ֵ���ı�������ť�ı�����ʾ��ʽ��
        /// �罫��ֵ��Ϊ��[{0}]���������ı�����ʾΪ��[1] [2] [3] ...������ֵ��Ϊ��-{0}-�����ʹ�����ı���Ϊ��-1- -2- -3- ...��</remarks>
        [Browsable(true),
            DefaultValue(""),
            Category("������ť"),
            Description("ҳ������ֵ��ť�����ֵ���ʾ��ʽ")]
        public string NumericButtonTextFormatString
        {
            get
            {
                object obj = ViewState["NumericButtonTextFormatString"];
                return (obj == null) ? String.Empty : (string)obj;
            }
            set
            {
                ViewState["NumericButtonTextFormatString"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����÷�ҳ������ť�����ͣ���ʹ�����ֻ���ͼƬ��
        /// </summary>
        /// <value>The type of the paging button.</value>
        /// <example>
        /// ���´���Ƭ��ʾ�����ʹ��ͼƬ��ť��
        ///   <p>
        ///   <code><![CDATA[
        ///   <Net2Pager:Net2Pager runat="server"
        /// id="pager1"
        /// OnPageChanged="ChangePage"
        /// PagingButtonType="image"
        /// ImagePath="images"
        /// ButtonImageNameExtension="n"
        /// DisabledButtonImageNameExtension="g"
        /// ButtonImageExtension="gif"
        /// CpiButtonImageNameExtension="r"
        /// PagingButtonSpacing=5/>
        /// ]]>
        ///   </code>
        ///   </p>
        ///   </example>
        /// <remarks>Ҫʹ��ͼƬ��ť������Ҫ׼������ͼƬ����0��9��ʮ����ֵͼƬ����ShowPageIndex��Ϊtrueʱ������һҳ����һҳ����һҳ�����һҳ������ҳ��...�������ťͼƬ����ShowFirstLast��ShowPrevNext����Ϊtrueʱ����
        /// ����Ҫʹ��ǰҳ��������ֵ��ť��ͬ�ڱ��ҳ������ֵ��ť������׼����ǰҳ�����İ�ťͼƬ��
        /// ����Ҫʹ�ѽ��õĵ�һҳ����һҳ����һҳ�����һҳ��ťͼƬ��ͬ�������İ�ťͼƬ������׼�����ĸ���ť�ڽ���״̬�µ�ͼƬ��
        /// <p><b>ͼƬ�ļ��������������£�</b></p>
        /// <p>��0��9ʮ����ֵ��ťͼƬ��������Ϊ����ֵ+ButtonImageNameExtension+ButtonImageExtension�������е�ButtonImageNameExtension���Բ������ã�
        /// ButtonImageExtension��ͼƬ�ļ��ĺ�׺������ .gif�� .jpg�ȿ��������������ʾ���κ�ͼƬ�ļ����͡���ҳ������1����ͼƬ�ļ�������Ϊ��1.gif����1.jpg����
        /// ���������׻������ͼƬ�ļ�ʱ������ͨ��ָ��ButtonImageNameExtension����ֵ�����ֲ�ͬ�׵�ͼƬ�����һ��ͼƬ���Բ�����ButtonImageNameExtension����ͼƬ�ļ��������ڡ�1.gif������2.gif���ȵȣ����ڶ���ͼƬ������ButtonImageNameExtensionΪ��f����ͼƬ�ļ��������ڡ�1f.gif������2f.gif���ȵȡ�</p>
        /// <p>��һҳ��ť��ͼƬ�ļ����ԡ�first����ͷ����һҳ��ťͼƬ���ԡ�prev����ͷ����һҳ��ťͼƬ���ԡ�next����ͷ�����һҳ��ťͼƬ���ԡ�last����ͷ������ҳ��ťͼƬ���ԡ�more����ͷ���Ƿ�ʹ��ButtonImageNameExtensionȡ������ֵ��ť�����ü��Ƿ��и�����ͼƬ��</p></remarks>
        [Browsable(true),
            DefaultValue(PagingButtonType.Text),
            Category("������ť"),
            Description("��ҳ������ť�����ͣ���ʹ�����ֻ���ͼƬ")]
        public PagingButtonType PagingButtonType
        {
            get
            {
                object obj = ViewState["PagingButtonType"];
                return (obj == null) ? PagingButtonType.Text : (PagingButtonType)obj;
            }
            set
            {
                ViewState["PagingButtonType"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������ҳ������ֵ��ť�����ͣ���ֵ����PagingButtonType��ΪImageʱ����Ч��
        /// </summary>
        /// <value>The type of the numeric button.</value>
        /// <remarks>������PagingButtonType��ΪImage���ֲ�����ҳ������ֵ��ťʹ��ͼƬʱ�����Խ���ֵ��ΪText�����ʹҳ�������ݰ�ťʹ���ı�������ͼƬ��ť��</remarks>
        [Browsable(true),
            DefaultValue(PagingButtonType.Text),
            Category("������ť"),
            Description("ҳ������ֵ��ť������")]
        public PagingButtonType NumericButtonType
        {
            get
            {
                object obj = ViewState["NumericButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["NumericButtonType"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����õ�һҳ����һҳ����һҳ�����һҳ��ť�����ͣ���ֵ����PagingButtonType��ΪImageʱ����Ч��
        /// </summary>
        /// <value>The type of the navigation button.</value>
        /// <remarks>������PagingButtonType��ΪImage���ֲ����õ�һҳ����һҳ����һҳ�����һҳ��ťʹ��ͼƬ������Խ���ֵ��ΪText�����ʹǰ����ĸ���ťʹ���ı�������ͼƬ��ť��</remarks>
        [Browsable(true),
            Category("������ť"),
            DefaultValue(PagingButtonType.Text),
            Description("��һҳ����һҳ����һҳ�����һҳ��ť������")]
        public PagingButtonType NavigationButtonType
        {
            get
            {
                object obj = ViewState["NavigationButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["NavigationButtonType"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����á�����ҳ����...����ť�����ͣ���ֵ����PagingButtonType��ΪImageʱ����Ч��
        /// </summary>
        /// <value>The type of the more button.</value>
        /// <remarks>������PagingButtonType��ΪImage���ֲ����ø���ҳ��...����ťʹ��ͼƬʱ�����Խ���ֵ��ΪText�����ʹ����ҳ��ťʹ���ı�������ͼƬ��ť��</remarks>
        [Browsable(true),
            Category("������ť"),
            DefaultValue(PagingButtonType.Text),
            Description("������ҳ����...����ť������")]
        public PagingButtonType MoreButtonType
        {
            get
            {
                object obj = ViewState["MoreButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["MoreButtonType"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����÷�ҳ������ť֮��ļ�ࡣ
        /// </summary>
        /// <value>The paging button spacing.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("������ť"),
            DefaultValue(typeof(Unit), "5px"),
            Description("��ҳ������ť֮��ļ��")]
        public Unit PagingButtonSpacing
        {
            get
            {
                object obj = ViewState["PagingButtonSpacing"];
                return (obj == null) ? Unit.Pixel(5) : (Unit.Parse(obj.ToString()));
            }
            set
            {
                ViewState["PagingButtonSpacing"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ���ҳ����Ԫ������ʾ��һҳ�����һҳ��ť��
        /// </summary>
        /// <value><c>true</c> if [show first last]; otherwise, <c>false</c>.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("�Ƿ���ҳ����Ԫ������ʾ��һҳ�����һҳ��ť"),
            Category("������ť"),
            DefaultValue(true)]
        public bool ShowFirstLast
        {
            get
            {
                object obj = ViewState["ShowFirstLast"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowFirstLast"] = value; }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ���ҳ����Ԫ������ʾ��һҳ����һҳ��ť��
        /// </summary>
        /// <value><c>true</c> if [show prev next]; otherwise, <c>false</c>.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("�Ƿ���ҳ����Ԫ������ʾ��һҳ����һҳ��ť"),
            Category("������ť"),
            DefaultValue(true)]
        public bool ShowPrevNext
        {
            get
            {
                object obj = ViewState["ShowPrevNext"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowPrevNext"] = value; }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ���ҳ����Ԫ������ʾҳ������ֵ��ť��
        /// </summary>
        /// <value><c>true</c> if [show page index]; otherwise, <c>false</c>.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("�Ƿ���ҳ����Ԫ������ʾ��ֵ��ť"),
            Category("������ť"),
            DefaultValue(false)]
        public bool ShowPageIndex
        {
            get
            {
                object obj = ViewState["ShowPageIndex"];
                return (obj == null) ? false : (bool)obj;
            }
            set { ViewState["ShowPageIndex"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ϊ��һҳ��ť��ʾ���ı���
        /// </summary>
        /// <value>The first page text.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("��һҳ��ť����ʾ���ı�"),
            Category("������ť"),
            DefaultValue("��ҳ")]
        public string FirstPageText
        {
            get
            {
                object obj = ViewState["FirstPageText"];
                return (obj == null) ? "��ҳ" : (string)obj;
            }
            set { ViewState["FirstPageText"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ϊ��һҳ��ť��ʾ���ı���
        /// </summary>
        /// <value>The prev page text.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("��һҳ��ť����ʾ���ı�"),
            Category("������ť"),
            DefaultValue("��һҳ")]
        public string PrevPageText
        {
            get
            {
                object obj = ViewState["PrevPageText"];
                return (obj == null) ? "��һҳ" : (string)obj;
            }
            set { ViewState["PrevPageText"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ϊ��һҳ��ť��ʾ���ı���
        /// </summary>
        /// <value>The next page text.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("��һҳ��ť����ʾ���ı�"),
            Category("������ť"),
            DefaultValue("��һҳ")]
        public string NextPageText
        {
            get
            {
                object obj = ViewState["NextPageText"];
                return (obj == null) ? "��һҳ" : (string)obj;
            }
            set { ViewState["NextPageText"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ϊ���һҳ��ť��ʾ���ı���
        /// </summary>
        /// <value>The last page text.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("���һҳ��ť����ʾ���ı�"),
            Category("������ť"),
            DefaultValue("ĩҳ")]
        public string LastPageText
        {
            get
            {
                object obj = ViewState["LastPageText"];
                return (obj == null) ? "ĩҳ" : (string)obj;
            }
            set { ViewState["LastPageText"] = value; }
        }

        /// <summary>
        /// ��ȡ�������� <see cref="Net2Pager" /> �ؼ���ҳ����Ԫ����ͬʱ��ʾ����ֵ��ť����Ŀ��
        /// </summary>
        /// <value>The numeric button count.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("Ҫ��ʾ��ҳ������ֵ��ť����Ŀ"),
            Category("������ť"),
            DefaultValue(10)]
        public int NumericButtonCount
        {
            get
            {
                object obj = ViewState["NumericButtonCount"];
                return (obj == null) ? 10 : (int)obj;
            }
            set { ViewState["NumericButtonCount"] = value; }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָ���Ƿ���ʾ�ѽ��õİ�ť��
        /// </summary>
        /// <value><c>true</c> if [show disabled buttons]; otherwise, <c>false</c>.</value>
        /// <remarks>��ֵ����ָ���Ƿ���ʾ�ѽ��õķ�ҳ������ť������ǰҳΪ��һҳʱ����һҳ����һҳ��ť�������ã�����ǰҳΪ���һҳʱ����һҳ�����һҳ��ť�������ã������õİ�ťû�����ӣ��ڰ�ť�ϵ��Ҳ�������κ����á�</remarks>
        [Browsable(true),
            Category("������ť"),
            Description("�Ƿ���ʾ�ѽ��õİ�ť"),
            DefaultValue(true)]
        public bool ShowDisabledButtons
        {
            get
            {
                object obj = ViewState["ShowDisabledButtons"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["ShowDisabledButtons"] = value;
            }
        }

        #endregion

        #region Image Buttons

        /// <summary>
        /// ��ȡ�����õ�ʹ��ͼƬ��ťʱ��ͼƬ�ļ���·����
        /// </summary>
        /// <value>The image path.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("ͼƬ��ť"),
            Description("��ʹ��ͼƬ��ťʱ��ָ��ͼƬ�ļ���·��"),
            DefaultValue(null)]
        public string ImagePath
        {
            get
            {
                string imgPath = (string)ViewState["ImagePath"];
                if (imgPath != null)
                    imgPath = this.ResolveUrl(imgPath);
                return imgPath;
            }
            set
            {
                string imgPath = value.Trim().Replace("\\", "/");
                ViewState["ImagePath"] = (imgPath.EndsWith("/")) ? imgPath : imgPath + "/";
            }
        }

        /// <summary>
        /// ��ȡ�����õ�ʹ��ͼƬ��ťʱ��ͼƬ�����ͣ���gif��jpg����ֵ��ͼƬ�ļ��ĺ�׺����
        /// </summary>
        /// <value>The button image extension.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("ͼƬ��ť"),
            DefaultValue(".gif"),
            Description("��ʹ��ͼƬ��ťʱ��ͼƬ�����ͣ���gif��jpg����ֵ��ͼƬ�ļ��ĺ�׺��")]
        public string ButtonImageExtension
        {
            get
            {
                object obj = ViewState["ButtonImageExtension"];
                return (obj == null) ? ".gif" : (string)obj;
            }
            set
            {
                string ext = value.Trim();
                ViewState["ButtonImageExtension"] = (ext.StartsWith(".")) ? ext : ("." + ext);
            }
        }

        /// <summary>
        /// ��ȡ�������Զ���ͼƬ�ļ����ĺ�׺�ַ����������ֲ�ͬ���͵İ�ťͼƬ��
        /// </summary>
        /// <value>The button image name extension.</value>
        /// <remarks><note>ע�⣺</note>��ֵ�����ļ���׺��������Ϊ���ֲ�ͬ��ͼƬ�ļ�����ͼƬ���м�����ַ������磺
        /// ��ǰ�����װ�ťͼƬ������һ���еġ�1����ͼƬ����Ϊ��1f.gif������һ���еġ�1����ͼƬ������Ϊ��1n.gif�������е�f��n��ΪButtonImageNameExtension��</remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("ͼƬ��ť"),
            Description("�Զ���ͼƬ�ļ����ĺ�׺�ַ��������ļ���׺��������ͼƬ��1f.gif����ButtonImageNameExtension��Ϊ��f��")]
        public string ButtonImageNameExtension
        {
            get
            {
                return (string)ViewState["ButtonImageNameExtension"];
            }
            set
            {
                ViewState["ButtonImageNameExtension"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����õ�ǰҳ������ť��ͼƬ����׺��
        /// </summary>
        /// <value>The cpi button image name extension.</value>
        /// <remarks>�� <see cref="PagingButtonType" /> ��Ϊ Image ʱ�����������������õ�ǰҳ������ֵ��ťʹ�õ�ͼƬ����׺�ַ�����˿���ʹ��ǰҳ������ť������ҳ������ťʹ�ò�ͬ��ͼƬ����δ���ø�ֵ����Ĭ��ֵΪ<see cref="ButtonImageNameExtension" />������ǰҳ������ť������ҳ������ťʹ����ͬ��ͼƬ��</remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("ͼƬ��ť"),
            Description("��ǰҳ������ť��ͼƬ����׺�ַ���")]
        public string CpiButtonImageNameExtension
        {
            get
            {
                object obj = ViewState["CpiButtonImageNameExtension"];
                return (obj == null) ? ButtonImageNameExtension : (string)obj;
            }
            set
            {
                ViewState["CpiButtonImageNameExtension"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�������ѽ��õ�ҳ������ťͼƬ����׺�ַ�����
        /// </summary>
        /// <value>The disabled button image name extension.</value>
        /// <remarks>�� <see cref="PagingButtonType" /> ��Ϊ Image ʱ�� ��ֵ�����������ѽ��ã���û�����ӣ����������޷�Ӧ����ҳ������ť��������һҳ����һҳ����һҳ�����һҳ�ĸ���ť����ͼƬ�ļ�����׺�ַ�������˿���ʹ�ѽ��õ�ҳ������ť��ͬ��������ҳ������ť����δ���ø�ֵ����Ĭ��ֵΪ<see cref="ButtonImageNameExtension" />�����ѽ��õ�ҳ������ť��������ҳ������ťʹ����ͬ��ͼƬ��</remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("ͼƬ��ť"),
            Description("�ѽ��õ�ҳ������ť��ͼƬ����׺�ַ���")]
        public string DisabledButtonImageNameExtension
        {
            get
            {
                object obj = ViewState["DisabledButtonImageNameExtension"];
                return (obj == null) ? ButtonImageNameExtension : (string)obj;
            }
            set
            {
                ViewState["DisabledButtonImageNameExtension"] = value;
            }
        }
        /// <summary>
        /// ָ����ʹ��ͼƬ��ťʱ��ͼƬ�Ķ��뷽ʽ��
        /// </summary>
        /// <value>The button image align.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("ָ����ʹ��ͼƬ��ťʱ��ͼƬ�Ķ��뷽ʽ"),
            DefaultValue(ImageAlign.Baseline),
            Category("ͼƬ��ť")]
        public ImageAlign ButtonImageAlign
        {
            get
            {
                object obj = ViewState["ButtonImageAlign"];
                return (obj == null) ? ImageAlign.Baseline : (ImageAlign)obj;
            }
            set { ViewState["ButtonImageAlign"] = value; }
        }


        #endregion

        #region Paging

        /// <summary>
        /// ��ȡ�����õ�ǰ��ʾҳ��������
        /// </summary>
        /// <value>The index of the current page.</value>
        /// <remarks>ʹ�ô�������ȷ���� Net2Pager �ؼ��е�ǰ��ʾ��ҳ����ǰ��ʾ��ҳ�������������Ժ�ɫ����Ӵ���ʾ�������Ի������Ա�̵ķ�ʽ��������ʾ��ҳ��
        /// <p>��<b>ע�⣺</b>��ͬ��GridView�ؼ���CurrentPageIndex��Net2Pager��CurrentPageIndex�����Ǵ�1��ʼ�ġ�</p></remarks>
        [ReadOnly(true),
            Browsable(false),
            Description("��ǰ��ʾҳ������"),
            Category("��ҳ"),
            DefaultValue(1),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPageIndex
        {
            get
            {
                object cpage = ViewState["CurrentPageIndex"];
                int pindex = (cpage == null) ? 1 : (int)cpage;
                if (pindex > PageCount && PageCount > 0)
                    return PageCount;
                else if (pindex < 1)
                    return 1;
                return pindex;
            }
            set
            {
                int cpage = value;
                if (cpage < 1)
                    cpage = 1;
                else if (cpage > this.PageCount)
                    cpage = this.PageCount;
                ViewState["CurrentPageIndex"] = cpage;
            }
        }

        /// <summary>
        /// ��ȡ��������Ҫ��ҳ�����м�¼��������
        /// </summary>
        /// <value>The record count.</value>
        /// <example>
        /// �����ʾ����ʾ����Ա�̷�ʽ����Sql��䷵�صļ�¼�������������ԣ�
        ///   <p>
        ///   <code><![CDATA[
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
        /// cmd=new SqlCommand("select count(id) from news",conn);
        /// conn.Open();
        /// pager.RecordCount=(int)cmd.ExecuteScalar();
        /// conn.Close();
        /// BindData();
        /// }
        /// }
        /// void BindData()
        /// {
        /// cmd=new SqlCommand("GetPagedNews",conn);
        /// cmd.CommandType=CommandType.StoredProcedure;
        /// cmd.Parameters.Add("@pageindex",pager.CurrentPageIndex);
        /// cmd.Parameters.Add("@pagesize",pager.PageSize);
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
        ///   <asp:GridView id="dataGrid1" runat="server" />
        ///   <Net2Pager:Net2Pager id="pager" runat="server"
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
        ///   </code></p>
        ///   <p>��ʾ��ʹ�õĴ洢���̴������£�</p>
        ///   <code><![CDATA[
        /// CREATE procedure GetPagedNews
        /// (@pagesize int,
        /// @pageindex int)
        /// as
        /// set nocount on
        /// declare @indextable table(id int identity(1,1),nid int)
        /// declare @PageLowerBound int
        /// declare @PageUpperBound int
        /// set @PageLowerBound=(@pageindex-1)*@pagesize
        /// set @PageUpperBound=@PageLowerBound+@pagesize
        /// set rowcount @PageUpperBound
        /// insert into @indextable(nid) select id from news order by addtime desc
        /// select O.id,O.title,O.source,O.addtime from news O,@indextable t where O.id=t.nid
        /// and t.id>@PageLowerBound and t.id<=@PageUpperBound order by t.id
        /// set nocount off
        /// GO
        /// ]]>
        ///   </code>
        ///   </example>
        /// <remarks>��ҳ���һ�μ���ʱ��Ӧ�Ա�̷�ʽ���Ӵ洢���̻�Sql����з��ص����ݱ�������Ҫ��ҳ�ļ�¼��������������ԣ�Net2Pager�Ὣ�䱣���ViewState�в���ҳ��ط�ʱ��ViewState�л�ȡ��ֵ����˱�����ÿ�η�ҳ��Ҫ�������ݿ��Ӱ���ҳ���ܡ�Net2Pager����Ҫ��ҳ���������ݵ��������� <see cref="PageSize" /> ������������ʾ����������Ҫ����ҳ������ <see cref="PageCount" />��ֵ��</remarks>
        [Browsable(false),
            Description("Ҫ��ҳ�����м�¼����������ֵ���ڳ�������ʱ���ã�Ĭ��ֵΪ225��Ϊ���ʱ֧�ֶ����õĲ���ֵ��"),
            Category("Data"),
            DefaultValue(225)]
        public int RecordCount
        {
            get
            {
                object obj = ViewState["Recordcount"];
                return (obj == null) ? 0 : (int)obj;
            }
            set { ViewState["Recordcount"] = value; }
        }

        /// <summary>
        /// ��ȡ��ǰҳ֮��δ��ʾ��ҳ��������
        /// </summary>
        /// <value>The pages remain.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PagesRemain
        {
            get
            {
                return PageCount - CurrentPageIndex;
            }
        }

        /// <summary>
        /// ��ȡ������ÿҳ��ʾ��������
        /// </summary>
        /// <value>The size of the page.</value>
        /// <example>����ʾ���� <see cref="Net2Pager" /> ����Ϊ����ÿҳ��ʾ8�����ݣ�
        ///   <code>
        ///   <![CDATA[
        /// ...
        ///   <Net2Pager:Net2Pager id="pager" runat="server" PageSize=8 OnPageChanged="ChangePage"/>
        /// ...
        /// ]]></code></example>
        /// <remarks>��ֵ��ȡ���������ݳ��ֿؼ�ÿ��Ҫ��ʾ���ݱ��еĵ����ݵ�������Net2Pager���ݸ�ֵ�� <see cref="RecordCount" /> ��������ʾ����������Ҫ����ҳ������ <see cref="PageCount" />��ֵ��</remarks>
        [Browsable(true),
            Description("ÿҳ��ʾ�ļ�¼��"),
            Category("��ҳ"),
            DefaultValue(10)]
        public int PageSize
        {
            get
            {
                object obj = ViewState["PageSize"];
                return (obj == null) ? 10 : (int)obj;
            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�ڵ�ǰҳ֮��δ��ʾ��ʣ���¼��������
        /// </summary>
        /// <value>The records remain.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecordsRemain
        {
            get
            {
                if (CurrentPageIndex < PageCount)
                    return RecordCount - (CurrentPageIndex * PageSize);
                return 0;
            }
        }


        /// <summary>
        /// ��ȡ����Ҫ��ҳ�ļ�¼��Ҫ����ҳ����
        /// </summary>
        /// <value>The page count.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageCount
        {
            get { return (int)Math.Ceiling((double)RecordCount / (double)PageSize); }
        }


        #endregion

        #region TextBox and Submit Button

        /// <summary>
        /// ��ȡ������ҳ�����ı������ʾ��ʽ��
        /// </summary>
        /// <value>The show input box.</value>
        /// <remarks>ҳ�����ļ��������û���ʽ����Ҫ���ʵ�ҳ����������ҳ���ǳ���ʱ����ʾҳ�����ı���ǳ������û���ת��ָ����ҳ��Ĭ������£����ı���ֻ������ҳ�����ڻ���� <see cref="ShowBoxThreshold" /> ��ֵʱ����ʾ��������ʾ��Ҫ����ı����κ�ʱ����ʾ���뽫��ֵ��ΪAlways����ϣ���κ�ʱ�򶼲���ʾ����Ӧ��ΪNever��</remarks>
        [Browsable(true),
            Description("ָ��ҳ�����ı������ʾ��ʽ"),
            Category("�ı����ύ��ť"),
            DefaultValue(ShowInputBox.Auto)]
        public ShowInputBox ShowInputBox
        {
            get
            {
                object obj = ViewState["ShowInputBox"];
                return (obj == null) ? ShowInputBox.Auto : (ShowInputBox)obj;
            }
            set { ViewState["ShowInputBox"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ӧ����ҳ���������ı����CSS������
        /// </summary>
        /// <value>The input box class.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�ı����ύ��ť"),
            DefaultValue(null),
            Description("Ӧ����ҳ���������ı����CSS����")]
        public string InputBoxClass
        {
            get
            {
                return (string)ViewState["InputBoxClass"];
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["InputBoxClass"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������ҳ���������ı����CSS��ʽ�ı���
        /// </summary>
        /// <value>The input box style.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�ı����ύ��ť"),
            DefaultValue(null),
            Description("Ӧ����ҳ���������ı����CSS��ʽ�ı�")]
        public string InputBoxStyle
        {
            get
            {
                return (string)ViewState["InputBoxStyle"];
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["InputBoxStyle"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������ҳ����ҳ���������ı���ǰ���ı��ַ���ֵ��
        /// </summary>
        /// <value>The text before input box.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�ı����ύ��ť"),
            DefaultValue(null),
            Description("ҳ���������ı���ǰ���ı������ַ���")]
        public string TextBeforeInputBox
        {
            get
            {
                return (string)ViewState["TextBeforeInputBox"];
            }
            set
            {
                ViewState["TextBeforeInputBox"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������ҳ�����ı���������ı������ַ���ֵ��
        /// </summary>
        /// <value>The text after input box.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("�ı����ύ��ť"),
            Description("ҳ���������ı������ı������ַ���")]
        public string TextAfterInputBox
        {
            get
            {
                return (string)ViewState["TextAfterInputBox"];
            }
            set
            {
                ViewState["TextAfterInputBox"] = value;
            }
        }


        /// <summary>
        /// ��ȡ�������ύ��ť�ϵ��ı���
        /// </summary>
        /// <value>The submit button text.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�ı����ύ��ť"),
            DefaultValue("ת��"),
            Description("�ύ��ť�ϵ��ı�")]
        public string SubmitButtonText
        {
            get
            {
                object obj = ViewState["SubmitButtonText"];
                return (obj == null) ? "ת��" : (string)obj;
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["SubmitButtonText"] = value;
            }
        }
        /// <summary>
        /// ��ȡ������Ӧ�����ύ��ť��CSS������
        /// </summary>
        /// <value>The submit button class.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�ı����ύ��ť"),
            DefaultValue(null),
            Description("Ӧ�����ύ��ť��CSS����")]
        public string SubmitButtonClass
        {
            get
            {
                return (string)ViewState["SubmitButtonClass"];
            }
            set
            {
                ViewState["SubmitButtonClass"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������Ӧ�����ύ��ť��CSS��ʽ��
        /// </summary>
        /// <value>The submit button style.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�ı����ύ��ť"),
            DefaultValue(null),
            Description("Ӧ�����ύ��ť��CSS��ʽ")]
        public string SubmitButtonStyle
        {
            get
            {
                return (string)ViewState["SubmitButtonStyle"];
            }
            set
            {
                ViewState["SubmitButtonStyle"] = value;
            }
        }
        /// <summary>
        /// ��ȡ�������Զ���ʾҳ���������ı���������ʼҳ����
        /// </summary>
        /// <value>The show box threshold.</value>
        /// <remarks>�� <see cref="ShowInputBox" /> ��ΪAuto��Ĭ�ϣ�����Ҫ��ҳ�����ݵ���ҳ���ﵽ��ֵʱ���Զ���ʾҳ���������ı���Ĭ��ֵΪ30����ѡ� <see cref="ShowInputBox" /> ��ΪNever��Alwaysʱû���κ����á�</remarks>
        [Browsable(true),
            Description("ָ����ShowInputBox��ΪShowInputBox.Autoʱ������ҳ���ﵽ����ʱ����ʾҳ���������ı���"),
            Category("�ı����ύ��ť"),
            DefaultValue(30)]
        public int ShowBoxThreshold
        {
            get
            {
                object obj = ViewState["ShowBoxThreshold"];
                return (obj == null) ? 30 : (int)obj;
            }
            set { ViewState["ShowBoxThreshold"] = value; }
        }


        #endregion

        #region CustomInfoSection

        /// <summary>
        /// ��ȡ��������ʾ�û��Զ�����Ϣ���ķ�ʽ��
        /// </summary>
        /// <value>The show custom info section.</value>
        /// <remarks>������ֵ��ΪLeft��Rightʱ���ڷ�ҳ����Ԫ����߻��ұ߻���һ��ר�ŵ���������ʾ�й��û��Զ�����Ϣ����ΪNeverʱ����ʾ��</remarks>
        [Browsable(true),
            Description("��ʾ��ǰҳ����ҳ����Ϣ��Ĭ��ֵΪ����ʾ��ֵΪShowCustomInfoSection.Leftʱ����ʾ��ҳ����ǰ��ΪShowCustomInfoSection.Rightʱ����ʾ��ҳ������"),
            DefaultValue(ShowCustomInfoSection.Never),
            Category("�Զ�����Ϣ��")]
        public ShowCustomInfoSection ShowCustomInfoSection
        {
            get
            {
                object obj = ViewState["ShowCustomInfoSection"];
                return (obj == null) ? ShowCustomInfoSection.Never : (ShowCustomInfoSection)obj;
            }
            set { ViewState["ShowCustomInfoSection"] = value; }
        }

        /// <summary>
        /// ��ȡ�������û��Զ�����Ϣ���ı��Ķ��뷽ʽ��
        /// </summary>
        /// <value>The custom info text align.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�Զ�����Ϣ��"),
            DefaultValue(HorizontalAlign.Left),
            Description("�û��Զ�����Ϣ���ı��Ķ��뷽ʽ")]
        public HorizontalAlign CustomInfoTextAlign
        {
            get
            {
                object obj = ViewState["CustomInfoTextAlign"];
                return (obj == null) ? HorizontalAlign.Left : (HorizontalAlign)obj;
            }
            set
            {
                ViewState["CustomInfoTextAlign"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�������û��Զ�����Ϣ���Ŀ�ȡ�
        /// </summary>
        /// <value>The width of the custom info section.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�Զ�����Ϣ��"),
            DefaultValue(typeof(Unit), "40%"),
            Description("�û��Զ�����Ϣ���Ŀ��")]
        public Unit CustomInfoSectionWidth
        {
            get
            {
                object obj = ViewState["CustomInfoSectionWidth"];
                return (obj == null) ? Unit.Percentage(40) : (Unit)obj;
            }
            set
            {
                ViewState["CustomInfoSectionWidth"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������Ӧ�����û��Զ�����Ϣ���ļ�����ʽ��������
        /// </summary>
        /// <value>The custom info class.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�Զ�����Ϣ��"),
            DefaultValue(null),
            Description("Ӧ�����û��Զ�����Ϣ���ļ�����ʽ������")]
        public string CustomInfoClass
        {
            get
            {
                object obj = ViewState["CustomInfoClass"];
                return (obj == null) ? CssClass : (string)obj;
            }
            set
            {
                ViewState["CustomInfoClass"] = value;
            }
        }

        /// <summary>
        /// ��ȡ������Ӧ�����û��Զ�����Ϣ����CSS��ʽ�ı���
        /// </summary>
        /// <value>�ַ���ֵ��ҪӦ�����û��Զ�����Ϣ����CSS��ʽ�ı���</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�Զ�����Ϣ��"),
            DefaultValue(null),
            Description("Ӧ�����û��Զ�����Ϣ����CSS��ʽ�ı�")]
        public string CustomInfoStyle
        {
            get
            {
                object obj = ViewState["CustomInfoStyle"];
                return (obj == null) ? GetStyleString() : (string)obj;
            }
            set
            {
                ViewState["CustomInfoStyle"] = value;
            }
        }

        /// <summary>
        /// ��ȡ����������ʾ���û��Զ�����Ϣ�����û��Զ����ı���
        /// </summary>
        /// <value>The custom info text.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Category("�Զ�����Ϣ��"),
            DefaultValue(null),
            Description("Ҫ��ʾ���û��Զ�����Ϣ�����û��Զ�����Ϣ�ı�")]
        public string CustomInfoText
        {
            get
            {
                return (string)ViewState["CustomInfoText"];
            }
            set
            {
                ViewState["CustomInfoText"] = value;
            }
        }

        #endregion

        #region Others

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָ���Ƿ�������ʾNet2Pager��ҳ��������ʹҪ��ҳ������ֻ��һҳ��
        /// </summary>
        /// <value><c>true</c> if [always show]; otherwise, <c>false</c>.</value>
        /// <remarks>Ĭ������£���Ҫ��ҳ������С����ҳʱ��Net2Pager������ҳ������ʾ�κ����ݣ���������ֵ��Ϊtrueʱ����ʹ��ҳ��ֻ��һҳ��Net2PagerҲ����ʾ��ҳ����Ԫ�ء�</remarks>
        [Browsable(true),
            Category("Behavior"),
            DefaultValue(false),
            Description("������ʾ��ҳ�ؼ�����ʹҪ��ҳ������ֻҪһҳ")]
        public bool AlwaysShow
        {
            get
            {
                object obj = ViewState["AlwaysShow"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["AlwaysShow"] = value;
            }
        }


        /// <summary>
        /// ��ȡ�������� Net2Pager �������ؼ��ڿͻ��˳��ֵļ�����ʽ�� (CSS) �ࡣ
        /// </summary>
        /// <value>The CSS class.</value>
        /// <returns>The CSS class rendered by the Web server control on the client. The default is <see cref="F:System.String.Empty" />.</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("Ӧ���ڿؼ���CSS����"),
            Category("Appearance"),
            DefaultValue(null)]
        public override string CssClass
        {
            get { return base.CssClass; }
            set
            {
                base.CssClass = value;
                cssClassName = value;
            }
        }


        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ Net2Pager �������ؼ��Ƿ��򷢳�����Ŀͻ��˱����Լ�����ͼ״̬�������Ծ���д��������Ϊfalse��
        /// </summary>
        /// <value><c>true</c> if [enable view state]; otherwise, <c>false</c>.</value>
        /// <returns>true if the server control maintains its view state; otherwise false. The default is true.</returns>
        /// <remarks><see cref="Net2Pager" /> �������ؼ���һЩ��Ҫ�ķ�ҳ��Ϣ������ViewState�У���ʹ��Url��ҳ��ʽʱ����Ȼ��ͼ״̬�ڷ�ҳ������û���κ����ã�������ǰҳ��Ҫ�ط��������������ͼ״̬�Ա��ҳ�ؼ�����ҳ��ط����ȡ�ط�ǰ�ķ�ҳ״̬����ͨ��ҳ��ط���PostBack���ķ�ʽ����ҳʱ��ҪʹNet2Pager��������������������ͼ״̬��
        ///   <p><note>�����Բ����ܽ�ֹ�û���<![CDATA[<%@Page EnableViewState=false%> ]]>ҳָ������������ҳ�����ͼ״̬����ʹ�ô�ָ�������Net2Pagerͨ��ҳ��ط�����ҳʱ��Net2Pager��Ϊ�޷���ȡ�������Ϣ����������������</note></p></remarks>
        [Browsable(false),
            Description("�Ƿ����ÿؼ�����ͼ״̬�������Ե�ֵ����Ϊtrue���������û����á�"),
            DefaultValue(true),
            Category("Behavior")]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }
            set
            {
                base.EnableViewState = true;
            }
        }

        /// <summary>
        /// ��ȡ�����õ��û������ҳ����������Χ���������ҳ������С����Сҳ������ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��
        /// </summary>
        /// <value>The page index out of range error string.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("���û������ҳ����������Χ���������ҳ������С����Сҳ������ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��"),
            DefaultValue("ҳ��������Χ��"),
            Category("Data")]
        public string PageIndexOutOfRangeErrorString
        {
            get
            {
                object obj = ViewState["PageIndexOutOfRangeErrorString"];
                return (obj == null) ? "ҳ��������Χ��" : (string)obj;
            }
            set
            {
                ViewState["PageIndexOutOfRangeErrorString"] = value;
            }
        }

        /// <summary>
        /// ��ȡ�����õ��û�������Ч��ҳ��������ֵ������֣�ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��
        /// </summary>
        /// <value>The invalid page index error string.</value>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        [Browsable(true),
            Description("���û�������Ч��ҳ��������ֵ������֣�ʱ�ڿͻ�����ʾ�Ĵ�����Ϣ��"),
            DefaultValue("ҳ������Ч��"),
            Category("Data")]
        public string InvalidPageIndexErrorString
        {
            get
            {
                object obj = ViewState["InvalidPageIndexErrorString"];
                return (obj == null) ? "ҳ������Ч��" : (string)obj;
            }
            set
            {
                ViewState["InvalidPageIndexErrorString"] = value;
            }
        }




        #endregion

        #endregion

        #region Control Rendering Logic

        /// <summary>
        /// ��д <see cref="System.Web.UI.Control.OnLoad" /> ������
        /// </summary>
        /// <param name="e">�����¼����ݵ� <see cref="EventArgs" /> ����</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        protected override void OnLoad(EventArgs e)
        {
            inputPageIndex = Page.Request.Form[this.UniqueID + "_input"];
            base.OnLoad(e);
        }
        /// <summary>
        /// ��д<see cref="System.Web.UI.Control.OnPreRender" />������
        /// </summary>
        /// <param name="e">�����¼����ݵ� <see cref="EventArgs" /> ����</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        protected override void OnPreRender(EventArgs e)
        {
            if (PageCount > 1)
            {
                string checkscript = "<script language=\"Javascript\">function doCheck(el){var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(r.test(el.value)){if(RegExp.$1<1||RegExp.$1>" + PageCount.ToString() + "){alert(\"" + PageIndexOutOfRangeErrorString + "\");document.all[\'" + this.UniqueID + "_input\'].select();return false;}return true;}alert(\"" + InvalidPageIndexErrorString + "\");document.all[\'" + this.UniqueID + "_input\'].select();return false;}</script>";

                ClientScriptManager manager1 = Page.ClientScript;
                Type type1 = base.GetType();

                if ((ShowInputBox == ShowInputBox.Always) || (ShowInputBox == ShowInputBox.Auto && PageCount >= ShowBoxThreshold))
                {
                    if (!manager1.IsClientScriptBlockRegistered("checkinput"))
                        manager1.RegisterClientScriptBlock(type1,"checkinput", checkscript);
  
                }
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// ��д<see cref="System.Web.UI.WebControls.WebControl.AddAttributesToRender" /> ����������Ҫ���ֵ� HTML ���Ժ���ʽ��ӵ�ָ���� <see cref="System.Web.UI.HtmlTextWriter" /> ��
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);
            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// ��д <see cref="System.Web.UI.WebControls.WebControl.RenderBeginTag" /> �������� <see cref="Net2Pager" /> �ؼ��� HTML ��ʼ��������ָ���� <see cref="System.Web.UI.HtmlTextWriter" /> ��д���С�
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //��дRenderBeginTag ����ʾ�Զ�����Ϣʱ�Ĺ���һ�����ͷ
            bool showPager = (PageCount > 1 || (PageCount <= 1 && AlwaysShow));

            base.RenderBeginTag(writer);
            //			if(!showPager)
            //			{
            //				writer.Write("<!-----��Ϊ��ҳ��ֻ��һҳ������AlwaysShow������Ϊfalse��Net2Pager����ʾ�κ����ݣ���Ҫ����ҳ��ֻ��һҳ���������ʾNet2Pager���뽫AlwaysShow������Ϊtrue��");
            //				writer.Write("----->");
            //			}
            if ((ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right) && showPager)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, GetStyleString());
                if (Height != Unit.Empty)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                WriteCellAttributes(writer, true);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }
        }

        /// <summary>
        /// ��д <see cref="System.Web.UI.WebControls.WebControl.RenderEndTag" /> �������� <see cref="Net2Pager" /> �ؼ��� HTML ������������ָ���� <see cref="System.Web.UI.HtmlTextWriter" /> ��д���С�
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            ////��дRenderEndTag ����ʾ�Զ�����Ϣʱ�Ĺ���һ������β
            if ((ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right) && (PageCount > 1 || (PageCount <= 1 && AlwaysShow)))
            {
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            base.RenderEndTag(writer);

        }


        /// <summary>
        /// ��д <see cref="System.Web.UI.WebControls.WebControl.RenderContents" /> ���������ؼ������ݳ��ֵ�ָ�� <see cref="System.Web.UI.HtmlTextWriter" /> �ı�д���С�
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (PageCount <= 1 && !AlwaysShow)
                return;

            if (ShowCustomInfoSection == ShowCustomInfoSection.Left)
            {
                writer.Write(CustomInfoText);
                writer.RenderEndTag();
                WriteCellAttributes(writer, false);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }

            int midpage = ((CurrentPageIndex - 1) / NumericButtonCount);
            int pageoffset = midpage * NumericButtonCount;
            int endpage = ((pageoffset + NumericButtonCount) > PageCount) ? PageCount : (pageoffset + NumericButtonCount);
            this.CreateNavigationButton(writer, "first");
            this.CreateNavigationButton(writer, "prev");
            if (ShowPageIndex)
            {
                if (CurrentPageIndex > NumericButtonCount)
                    CreateMoreButton(writer, pageoffset);
                for (int i = pageoffset + 1; i <= endpage; i++)
                {
                    CreateNumericButton(writer, i);
                }
                if (PageCount > NumericButtonCount && endpage < PageCount)
                    CreateMoreButton(writer, endpage + 1);
            }
            this.CreateNavigationButton(writer, "next");
            this.CreateNavigationButton(writer, "last");
            if ((ShowInputBox == ShowInputBox.Always) || (ShowInputBox == ShowInputBox.Auto && PageCount >= ShowBoxThreshold))
            {
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                if (TextBeforeInputBox != null)
                    writer.Write(TextBeforeInputBox);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentPageIndex.ToString());
                if (InputBoxStyle != null && InputBoxStyle.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, InputBoxStyle);
                if (InputBoxClass != null && InputBoxClass.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, InputBoxClass);
                if (PageCount <= 1 && AlwaysShow)
                    writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "_input");
                string scriptRef = "doCheck(document.all[\'" + this.UniqueID + "_input\'])";
                string postRef = "if(event.keyCode==13){if(" + scriptRef + ")__doPostBack(\'" + this.UniqueID + "\',document.all[\'" + this.UniqueID + "_input\'].value);else{event.returnValue=false;}}";
                string keydownScript = "if(event.keyCode==13){if(" + scriptRef + "){event.returnValue=false;document.all[\'" + this.UniqueID + "\'][1].click();}else{event.returnValue=false;}}";
                writer.AddAttribute("onkeydown", postRef);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                if (TextAfterInputBox != null)
                    writer.Write(TextAfterInputBox);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "Submit");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
                writer.AddAttribute(HtmlTextWriterAttribute.Value, SubmitButtonText);
                if (SubmitButtonClass != null && SubmitButtonClass.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, SubmitButtonClass);
                if (SubmitButtonStyle != null && SubmitButtonStyle.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, SubmitButtonStyle);
                if (PageCount <= 1 && AlwaysShow)
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return " + scriptRef);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }

            if (ShowCustomInfoSection == ShowCustomInfoSection.Right)
            {
                writer.RenderEndTag();
                WriteCellAttributes(writer, false);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(CustomInfoText);
            }
        }


        #endregion

        #region Private Helper Functions

        /// <summary>
        /// �����ؼ���Styleת��ΪCSS�ַ�����
        /// </summary>
        /// <returns>System.String.</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private string GetStyleString()
        {
            if (Style.Count > 0)
            {
                string stl = null;
                string[] skeys = new string[Style.Count];
                Style.Keys.CopyTo(skeys, 0);
                for (int i = 0; i < skeys.Length; i++)
                {
                    stl += String.Concat(skeys[i], ":", Style[skeys[i]], ";");
                }
                return stl;
            }
            return null;
        }

        /// <summary>
        /// Ϊ�û��Զ�����Ϣ����ҳ������ť����td������ԡ�
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="leftCell">�Ƿ�Ϊ��һ��td</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void WriteCellAttributes(HtmlTextWriter writer, bool leftCell)
        {
            string customUnit = CustomInfoSectionWidth.ToString();
            if (ShowCustomInfoSection == ShowCustomInfoSection.Left && leftCell || ShowCustomInfoSection == ShowCustomInfoSection.Right && !leftCell)
            {
                if (CustomInfoClass != null && CustomInfoClass.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CustomInfoClass);
                if (CustomInfoStyle != null && CustomInfoStyle.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, CustomInfoStyle);
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
                writer.AddAttribute(HtmlTextWriterAttribute.Align, CustomInfoTextAlign.ToString().ToLower());
            }
            else
            {
                if (CustomInfoSectionWidth.Type == UnitType.Percentage)
                {
                    customUnit = (Unit.Percentage(100 - CustomInfoSectionWidth.Value)).ToString();
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, HorizontalAlign.ToString().ToLower());
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
        }

        /// <summary>
        /// ��ȡ��ҳ������ť�ĳ������ַ�����
        /// </summary>
        /// <param name="pageIndex">�÷�ҳ��ť���Ӧ��ҳ������</param>
        /// <returns>��ҳ������ť�ĳ������ַ�����</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private string GetHrefString(int pageIndex)
        {
          // .net 1.1�÷�
          //  return Page.GetPostBackClientHyperlink(this, pageIndex.ToString());
            return Page.ClientScript.GetPostBackClientHyperlink(this, pageIndex.ToString());

        }

        /// <summary>
        /// ������һҳ����һҳ����һҳ�����һҳ��ҳ��ť��
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <param name="btnname">��ҳ��ť����</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void CreateNavigationButton(HtmlTextWriter writer, string btnname)
        {
            if (!ShowFirstLast && (btnname == "first" || btnname == "last"))
                return;
            if (!ShowPrevNext && (btnname == "prev" || btnname == "next"))
                return;
            string linktext = "";
            bool disabled;
            int pageIndex;
            bool imgButton = (PagingButtonType == PagingButtonType.Image && NavigationButtonType == PagingButtonType.Image);
            if (btnname == "prev" || btnname == "first")
            {
                disabled = (CurrentPageIndex <= 1);
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btnname == "first") ? 1 : (CurrentPageIndex - 1);
                if (imgButton)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                        AddToolTip(writer, pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btnname == "prev") ? PrevPageText : FirstPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    else
                    {
                        WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            else
            {
                disabled = (CurrentPageIndex >= PageCount);
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btnname == "last") ? PageCount : (CurrentPageIndex + 1);
                if (imgButton)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                        AddToolTip(writer, pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btnname == "next") ? NextPageText : LastPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    else
                    {
                        WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            WriteButtonSpace(writer);
        }

        /// <summary>
        /// д��CSS������
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void WriteCssClass(HtmlTextWriter writer)
        {
            if (cssClassName != null && cssClassName.Trim().Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassName);
        }

        /// <summary>
        /// ���뵼����ť��ʾ�ı���
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <param name="pageIndex">������ť��Ӧ��ҳ������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void AddToolTip(HtmlTextWriter writer, int pageIndex)
        {
            if (ShowNavigationToolTip)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format(NavigationToolTipTextFormatString, pageIndex));
            }
        }

        /// <summary>
        /// ������ҳ��ֵ������ť��
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <param name="index">Ҫ������ť��ҳ������ֵ��</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void CreateNumericButton(HtmlTextWriter writer, int index)
        {
            bool isCurrent = (index == CurrentPageIndex);
            if (PagingButtonType == PagingButtonType.Image && NumericButtonType == PagingButtonType.Image)
            {
                if (!isCurrent)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
                    AddToolTip(writer, index);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    CreateNumericImages(writer, index, isCurrent);
                    writer.RenderEndTag();
                }
                else
                    CreateNumericImages(writer, index, isCurrent);
            }
            else
            {
                if (isCurrent)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    writer.RenderBeginTag(HtmlTextWriterTag.Font);
                    if (NumericButtonTextFormatString.Length > 0)
                        writer.Write(String.Format(NumericButtonTextFormatString, index.ToString()));
                  //    writer.Write(String.Format(NumericButtonTextFormatString, (ChinesePageIndex == true) ? GetChinesePageIndex(index) : (index.ToString())));
                    else
                        writer.Write(index.ToString());
                 //     writer.Write((ChinesePageIndex == true) ? GetChinesePageIndex(index) : index.ToString());
                    writer.RenderEndTag();
                }
                else
                {
                    WriteCssClass(writer);
                    AddToolTip(writer, index);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    if (NumericButtonTextFormatString.Length > 0)
                        writer.Write(String.Format(NumericButtonTextFormatString, index.ToString()));
                      //  writer.Write(String.Format(NumericButtonTextFormatString, (ChinesePageIndex == true) ? GetChinesePageIndex(index) : (index.ToString())));
                    else
                        writer.Write(index.ToString());
                     //   writer.Write((ChinesePageIndex == true) ? GetChinesePageIndex(index) : index.ToString());
                    writer.RenderEndTag();
                }
            }
            WriteButtonSpace(writer);
        }

        /// <summary>
        /// �ڷ�ҳ����Ԫ�ؼ����ո�
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void WriteButtonSpace(HtmlTextWriter writer)
        {
            if (PagingButtonSpacing.Value > 0)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, PagingButtonSpacing.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.RenderEndTag();
            }
        }

        #region ��ȡ����ҳ�����ַ���
        /// <summary>
        /// ��ȡ����ҳ�����ַ���
        /// </summary>
        /// <param name="index">�����ַ���Ӧ��ҳ������ֵ</param>
        /// <returns>��Ӧ��ҳ������ֵ�������ַ�</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private string GetChinesePageIndex(int index)
        {
            Hashtable cnChars = new Hashtable();
            cnChars.Add("0", "��");
            cnChars.Add("1", "һ");
            cnChars.Add("2", "��");
            cnChars.Add("3", "��");
            cnChars.Add("4", "��");
            cnChars.Add("5", "��");
            cnChars.Add("6", "��");
            cnChars.Add("7", "��");
            cnChars.Add("8", "��");
            cnChars.Add("9", "��");
            string indexStr = index.ToString();
            string retStr = "";
            for (int i = 0; i < indexStr.Length; i++)
            {
                retStr = String.Concat(retStr, cnChars[indexStr[i].ToString()]);
            }
            return retStr;
        }
        #endregion

        /// <summary>
        /// ����ҳ����ͼƬ��ť��
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <param name="index">ҳ������ֵ��</param>
        /// <param name="isCurrent">�Ƿ��ǵ�ǰҳ������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void CreateNumericImages(HtmlTextWriter writer, int index, bool isCurrent)
        {
            string indexStr = index.ToString();
            for (int i = 0; i < indexStr.Length; i++)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, indexStr[i], (isCurrent == true) ? CpiButtonImageNameExtension : ButtonImageNameExtension, ButtonImageExtension));
                writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// ����������ҳ����...����ť��
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter" />����ʾҪ�ڿͻ��˳��� HTML ���ݵ��������</param>
        /// <param name="pageIndex">���ӵ���ť��ҳ��������</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        private void CreateMoreButton(HtmlTextWriter writer, int pageIndex)
        {
            WriteCssClass(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
            AddToolTip(writer, pageIndex);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            if (PagingButtonType == PagingButtonType.Image && MoreButtonType == PagingButtonType.Image)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, "more", ButtonImageNameExtension, ButtonImageExtension));
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            else
                writer.Write("...");
            writer.RenderEndTag();
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, PagingButtonSpacing.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.RenderEndTag();
        }

        #endregion

        #region IPostBackEventHandler Implementation

        /// <summary>
        /// ʵ��<see cref="IPostBackEventHandler" /> �ӿڣ�ʹ <see cref="Net2Pager" /> �ؼ��ܹ��������巢�͵�������ʱ�������¼���
        /// </summary>
        /// <param name="args">The args.</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public void RaisePostBackEvent(string args)
        {
            int pageIndex = CurrentPageIndex;
            try
            {
                if (args == null || args == "")
                    args = inputPageIndex;
                pageIndex = int.Parse(args);
            }
            catch { }
            OnPageChanged(new PageChangedEventArgs(pageIndex));

        }


        #endregion

        #region IPostBackDataHandler Implementation

        /// <summary>
        /// ʵ�� <see cref="IPostBackDataHandler" /> �ӿڣ�Ϊ <see cref="Net2Pager" /> �������ؼ�����ط����ݡ�
        /// </summary>
        /// <param name="pkey">�ؼ�����Ҫ��ʶ����</param>
        /// <param name="pcol">���д�������ֵ�ļ��ϡ�</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
        {
            string str = pcol[this.UniqueID + "_input"];
            if (str != null && str.Trim().Length > 0)
            {
                try
                {
                    int pindex = int.Parse(str);
                    if (pindex > 0 && pindex <= PageCount)
                    {
                        inputPageIndex = str;
                        Page.RegisterRequiresRaiseEvent(this);
                    }
                }
                catch
                { }
            }
            return false;
        }

        /// <summary>
        /// ʵ�� <see cref="IPostBackDataHandler" /> �ӿڣ����ź�Ҫ��������ؼ�����֪ͨ ASP.NET Ӧ�ó���ÿؼ���״̬�Ѹ��ġ�
        /// </summary>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public virtual void RaisePostDataChangedEvent() { }

        #endregion

        #region PageChanged Event
        /// <summary>
        /// ��ҳ����Ԫ��֮һ���������û��ֹ�����ҳ�����ύʱ������
        /// </summary>
        /// <example>�����ʾ����ʾ���ΪPageChanged�¼�ָ���ͱ�д�¼���������ڸ��¼�������������°�GridView����ʾ�����ݣ�
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
        ///   <asp:GridView id="dataGrid1" runat="server" />
        ///   <Net2Pager:Net2Pager id="pager" runat="server" PageSize="8" NumericButtonCount="8" ShowCustomInfoSection="before" ShowInputBox="always" CssClass="mypager" HorizontalAlign="center" OnPageChanged="ChangePage" />
        ///   </form>
        ///   </body>
        ///   </HTML>
        /// ]]>
        ///   </code>
        ///   <p>��ʾ�����õ�Sql Server�洢���̴������£�</p>
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
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        public event PageChangedEventHandler PageChanged;

        #endregion

        #region OnPageChanged Method

        /// <summary>
        /// ���� <see cref="PageChanged" /> �¼�����ʹ������Ϊ�¼��ṩ�Զ��崦�����
        /// </summary>
        /// <param name="e">һ�� <see cref="PageChangedEventArgs" />���������¼����ݡ�</param>
        /// <remarks>http://wintersun.cnblogs.com/</remarks>
        protected virtual void OnPageChanged(PageChangedEventArgs e)
        {
            if (this.PageChanged != null)
                PageChanged(this, e);
        }

        #endregion
    }


    #endregion

    #region PageChangedEventHandler Delegate
    /// <summary>
    /// ��ʾ���� <see cref="Net2Pager.PageChanged" /> �¼��ķ�����
    /// </summary>
    /// <param name="src">The SRC.</param>
    /// <param name="e">The <see cref="PageChangedEventArgs"/> instance containing the event data.</param>
    /// <remarks>http://wintersun.cnblogs.com/</remarks>
    public delegate void PageChangedEventHandler(object src, PageChangedEventArgs e);

    #endregion

    

}