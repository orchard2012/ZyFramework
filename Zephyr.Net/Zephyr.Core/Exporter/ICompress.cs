﻿/*************************************************************************
 * 文件名称 ：ICompress.cs                          
 * 描述说明 ：文件压缩接口
 * 
 * 创建信息 : create by (person) on (date)
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 (corp)
**************************************************************************/

using System.IO;

namespace Zephyr.Core
{
    public interface ICompress
    {
        string Suffix(string orgSuffix);
        Stream Compress(Stream fileStream,string fullName);
    }
}
