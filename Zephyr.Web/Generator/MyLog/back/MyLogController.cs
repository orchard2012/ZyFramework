
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class MyLogController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Sys/MyLog",
                    remove = "/api/Sys/MyLog/",
                    billno = "/api/Sys/MyLog/getnewbillno",
                    audit = "/api/Sys/MyLog/audit/",
                    edit = "/Sys/MyLog/edit/"
                },
                resx = new {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                    Date = "" 
                },
                idField="ID"
            };

            return View(model);
        }
    }

    public class MyLogApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new sys_logService.GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>sys_log</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Date'		cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_logService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
