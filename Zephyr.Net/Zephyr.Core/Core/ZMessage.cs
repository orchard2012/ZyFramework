﻿/*************************************************************************
 * 文件名称 ：ZMessage.cs                          
 * 描述说明 ：框架消息定义
 * 
 * 创建信息 : create by (person) on (date)
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 (Corp)
 **************************************************************************/

using System.Runtime.Serialization;

namespace Zephyr.Core
{
    [DataContract]
    public class AjaxMessge
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string text { get; set; }

        public AjaxMessge()
        {
            Set(MsgType.Success, string.Empty);
        }

        public AjaxMessge Set(MsgType msgtype, string message)
        {
            code = msgtype.ToString().ToLower();
            text = message;
            return this;
        }

        public AjaxMessge Set(string txtcode, string message)
        {
            code = txtcode;
            text = message;
            return this;
        }
    }
}
