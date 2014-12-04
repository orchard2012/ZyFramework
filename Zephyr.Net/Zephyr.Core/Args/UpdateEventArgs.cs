/*************************************************************************
 * 文件名称 ：UpdateEventArgs.cs                          
 * 描述说明 ：更新事件参数
 * 
 * 创建信息 : create by (person) on (date)
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 (Corp)
 **************************************************************************/

using Zephyr.Data;

namespace Zephyr.Core
{
    public class UpdateEventArgs
    {
        public IDbContext db { get; set; }
        public ParamUpdateData data { get; set; }
        public int executeValue { get; set; }
    }
}
