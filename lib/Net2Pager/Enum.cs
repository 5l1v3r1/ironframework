// ***********************************************************************
// Assembly         : Net2Pager
// Author           : PeterLiu
// Created          : 07-19-2014
//
// Last Modified By : PeterLiu
// Last Modified On : 01-04-2007
// ***********************************************************************
// <copyright file="Enum.cs" company="Megadotnet">
//     Copyright (c) Megadotnet. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace DiyControl.Pager
{
    #region ShowInputBox,ShowCustomInfoSection and PagingButtonType Enumerations
    /// <summary>
    /// ָ��ҳ���������ı������ʾ��ʽ���Ա��û������ֹ�����ҳ������
    /// </summary>
    /// <remarks>http://wintersun.cnblogs.com/</remarks>
    public enum ShowInputBox : byte
    {
        /// <summary>
        /// �Ӳ���ʾҳ���������ı���
        /// </summary>
        Never,
        /// <summary>
        /// �Զ���ѡ����������� <see cref="Net2Pager.ShowBoxThreshold" /> �ɿ��Ƶ���ҳ���ﵽ����ʱ�Զ���ʾҳ���������ı���
        /// </summary>
        Auto,
        /// <summary>
        /// ������ʾҳ���������ı���
        /// </summary>
        Always
    }


    /// <summary>
    /// ָ����ǰҳ��������ҳ����Ϣ����ʾ��ʽ��
    /// </summary>
    /// <remarks>http://wintersun.cnblogs.com/</remarks>
    public enum ShowCustomInfoSection : byte
    {
        /// <summary>
        /// ����ʾ��
        /// </summary>
        Never,
        /// <summary>
        /// ��ʾ��ҳ����Ԫ��֮ǰ��
        /// </summary>
        Left,
        /// <summary>
        /// ��ʾ��ҳ����Ԫ��֮��
        /// </summary>
        Right
    }

    /// <summary>
    /// ָ��ҳ������ť�����͡�
    /// </summary>
    /// <remarks>http://wintersun.cnblogs.com/</remarks>
    public enum PagingButtonType : byte
    {
        /// <summary>
        /// ʹ�����ְ�ť��
        /// </summary>
        Text,
        /// <summary>
        /// ʹ��ͼƬ��ť��
        /// </summary>
        Image
    }


    #endregion
}
