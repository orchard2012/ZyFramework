
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Sys.Controllers
{
    public class MyUserController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Sys/MyUser",
                    remove = "/api/Sys/MyUser/",
                    billno = "/api/Sys/MyUser/getnewbillno",
                    audit = "/api/Sys/MyUser/audit/",
                    edit = "/Sys/MyUser/edit/"
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
                    RoleName = "" 
                },
                idField="UserCode"
            };

            return View(model);
        }

 public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new {
                    getdata = "/api/Sys/MyUser/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Sys/MyUser/edit/",                      //数据保存api
                    audit = "/api/Sys/MyUser/audit/",                    //审核api
                    newkey = "/api/Sys/MyUser/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed ="单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new{
                    pageData=new MyUserApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new sys_user().Extend(new {  UserCode = "",UserName = "",Description = "",Password = "",RoleName = "",OrganizeName = "",IsEnable = "",ConfigJSON = ""}),
                    primaryKeys = new string[]{"UserCode"}
                },
                tabs = new object[]{
                    new{
                      type = "grid",
                      rowId = "UserCode",
                      relationId = "UserCode",
                      postFields = new string[] { "UserCode","UserSeq","UserName","Description","Password","RoleName","OrganizeName","ConfigJSON","IsEnable","LoginCount","LastLoginDate"},
                      defaults = new {UserCode = "",UserSeq = "",UserName = "",Description = "",Password = "",RoleName = "",OrganizeName = "",ConfigJSON = "",IsEnable = "",LoginCount = "",LastLoginDate = ""}
                    },    
                    new{
                      type = "form",
                      primaryKeys = new string[]{"UserCode"},
                      defaults = new {UserCode = "",UserSeq = "",UserName = "",Description = "",Password = "",RoleName = "",OrganizeName = "",ConfigJSON = "",IsEnable = ""}
                    },    
                    new{
                      type = "grid",
                      rowId = "ID",
                      relationId = "ID",
                      postFields = new string[] { "UserCode","ID","SettingCode","SettingName","SettingValue","Description"},
                      defaults = new {UserCode = "",ID = "",SettingCode = "",SettingName = "",SettingValue = "",Description = ""}
                    }    
                }
            };
            return View(model);
        }
    }

    public class MyUserApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new sys_userService().GetNewKey("UserCode", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='UserCode'>
    <select>*</select>
    <from>sys_user</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UserName'		cp='equal'></field>   
        <field name='RoleName'		cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_userService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

public dynamic GetPageData(string id)
        {
            var masterService = new sys_userService();
            var pQuery = ParamQuery.Instance().AndWhere("UserCode", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("UserCode", id),

                //明细数据
                tab0 = new sys_userService().GetDynamicList(pQuery),
                tab1 = new sys_userService().GetModel(pQuery),      
                tab2 = new sys_userSettingService().GetDynamicList(ParamQuery.Instance().AndWhere("ID", id))
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("sys_user")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("UserCode", id);

            var service = new sys_userService();
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
                    var service0 = new sys_userService();
                    return service0.GetNewKey("UserCode", "maxplus", qty, ParamQuery.Instance().AndWhere("UserCode", key, Cp.Equal));
                case "grid2":
                    var service2 = new sys_userSettingService();
                    return service2.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
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
        sys_user
    </table>
    <where>
        <field name='UserCode' cp='equal'></field>
    </where>
</settings>");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_user</table>
    <where>
        <field name='UserCode' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_user</table>
    <where>
        <field name='UserCode' cp='equal'></field>
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>sys_userSetting</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
             
            var service = new sys_userService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
