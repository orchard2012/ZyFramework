
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
                    UserName = "" ,
                    Date = "" 
                },
                idField="ID"
            };

            return View(model);
        }
public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new {
                    getdata = "/api/Sys/MyLog/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Sys/MyLog/edit/",                      //数据保存api
                    audit = "/api/Sys/MyLog/audit/",                    //审核api
                    newkey = "/api/Sys/MyLog/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed ="单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new{
                    pageData=new MyLogApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new sys_log().Extend(new {  ID = "",UserCode = "",UserName = "",Target = "",Position = "",Type = "",Message = "",Date = ""}),
                    primaryKeys = new string[]{"ID"}
                },
                tabs = new object[]{
                    new{
                      type = "grid",
                      rowId = "ID",
                      relationId = "ID",
                      postFields = new string[] { "ID","UserCode","UserName","Position","Target","Type","Message","Date"},
                      defaults = new {ID = "",UserCode = "",UserName = "",Position = "",Target = "",Type = "",Message = "",Date = ""}
                    },    
                    new{
                      type = "form",
                      primaryKeys = new string[]{"ID"},
                      defaults = new {ID = "",UserCode = "",UserName = "",Position = "",Target = "",Type = "",Message = "",Date = ""}
                    },    
                    new{
                      type = "form",
                      primaryKeys = new string[]{"UserCode"},
                      defaults = new {UserCode = "",UserName = "",Password = "",RoleName = ""}
                    }    
                }
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
        <field name='UserName'		cp='equal'></field>   
        <field name='Date'		cp='like'></field>   
    </where>
</settings>");
            var service = new sys_logService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
 public dynamic GetPageData(string id)
        {
            var masterService = new sys_logService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new sys_logService().GetDynamicList(pQuery),
                tab1 = new sys_logService().GetModel(pQuery),      
                tab2 = new sys_userService().GetModel(ParamQuery.Instance().AndWhere("UserCode", id))      
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("sys_log")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new sys_logService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }
  
        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type,string key,int qty=1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new sys_logService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }
  
        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        sys_log
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_log</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_log</table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_user</table>
    <where>
        <field name='UserCode' cp='equal'></field>
    </where>
</settings>"));
             
            var service = new sys_logService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
