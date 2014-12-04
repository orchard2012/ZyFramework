/*************************************************************************
 * 文件名称 ：InsertEventArgs.cs                          
 * 描述说明 ：插入事件参数
 * 
 * 创建信息 : create by (person) on (date)
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 (Corp)
 **************************************************************************/

using Zephyr.Data;

namespace Zephyr.Core
{
    public class InsertEventArgs
    {
        public IDbContext db { get; set; }
        public ParamInsertData data { get; set; }
        public int executeValue { get; set; }
    }
}
