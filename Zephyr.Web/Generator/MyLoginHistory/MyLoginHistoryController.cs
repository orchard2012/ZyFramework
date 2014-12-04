
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class MyLoginHistoryController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new{
                    dsPricing = code.GetValueTextListByType("Pricing")
                },
                urls = new{
                    query = "/api/Sys/MyLoginHistory",
                    newkey = "/api/Sys/MyLoginHistory/getnewkey",
                    edit = "/api/Sys/MyLoginHistory/edit" 
                },
                resx = new{
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new{
                    UserName = "" ,
                    LoginDate = "" 
                },
                defaultRow = new {
                   
                },
                setting = new{
                    idField = "ID",
                    postListFields = new string[] { "UserCode" ,"UserName" ,"HostName" ,"HostIP" ,"LoginCity" ,"LoginDate" }
                }
            };

            return View(model);
        }
    }

    public class MyLoginHistoryApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>sys_loginHistory</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UserName'		cp='like'></field>   
        <field name='LoginDate'		cp='startwith'></field>   
    </where>
</settings>");
            var service = new sys_loginHistoryService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new sys_loginHistoryService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_loginHistory
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");
            var service = new sys_loginHistoryService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
