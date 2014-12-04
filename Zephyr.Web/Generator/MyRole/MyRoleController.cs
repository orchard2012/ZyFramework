
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
    public class MyRoleController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Sys/MyRole",
                    remove = "/api/Sys/MyRole/",
                    billno = "/api/Sys/MyRole/getnewbillno",
                    audit = "/api/Sys/MyRole/audit/",
                    edit = "/Sys/MyRole/edit/"
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
                    RoleName = "" ,
                    RoleCode = "" ,
                    Description = "" 
                },
                idField="RoleCode"
            };

            return View(model);
        }
    }

    public class MyRoleApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new sys_roleService.GetNewKey("RoleCode", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='RoleCode'>
    <select>*</select>
    <from>sys_role</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='RoleName'		cp='equal'></field>   
        <field name='RoleCode'		cp='equal'></field>   
        <field name='Description'		cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_roleService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
