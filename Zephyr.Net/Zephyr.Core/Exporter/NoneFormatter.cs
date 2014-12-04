/*************************************************************************
 * 文件名称 ：NoneFormatter.cs                          
 * 描述说明 ：空格式化实现
 * 
 * 创建信息 : create by (person) on (date)
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 (corp)
**************************************************************************/

namespace Zephyr.Core
{
    public class NoneFormatter
    {
        public object Format(object value)
        {
            return value;
        }
    }
}
