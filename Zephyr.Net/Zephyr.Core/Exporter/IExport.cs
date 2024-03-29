﻿/*************************************************************************
 * 文件名称 ：IExport.cs                          
 * 描述说明 ：文件导出接口
 * 
 * 创建信息 : create by (person) on (date)
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 (corp)
**************************************************************************/

using System.IO;

namespace Zephyr.Core
{
    public interface IExport
    {
        string suffix { get;}
 
        void MergeCell(int x1,int y1,int x2,int y2);
        void FillData(int x, int y,string field, object data);

        void Init(object data);
        Stream SaveAsStream();

        void SetHeadStyle(int x1, int y1, int x2, int y2);
        void SetRowsStyle(int x1, int y1, int x2, int y2);
    }
}
