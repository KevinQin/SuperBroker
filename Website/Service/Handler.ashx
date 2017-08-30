<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using LitJson;
using System.Collections.Generic;
using com.seascape.tools;
using System.Text;
using com.superbroker.data;
using com.superbroker.model;

public class Handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    const string DefaultWrokNo = "100";
    static Response response;
    const String SYSTEM_TAG = "SYSTEM";

    public void ProcessRequest(HttpContext c) {
        response = new Response();
        int F = string.IsNullOrEmpty(c.Request["fn"]) ? 0 : Convert.ToInt16(c.Request["fn"]);
        c.Response.ContentType = "text/plain";
        c.Response.Write(GetResult(F, c));
    }


    /// <summary>
    /// 功能导航
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public string GetResult(int f, HttpContext c)
    {
        string Result = null;
        int serverCode = getServerCode();
        Log.D("请求[" + serverCode + ":FN-" + f + "]", c, SYSTEM_TAG);
        try
        {
            switch (f)
            {
                case 1:
                    Result = DoLogin(c);//登录                     
                    break;
                case 2:
                    Result = GetLog(c);//日志                     
                    break;
                case 3:
                    Result = ChangePwd(c);//修改密码                     
                    break;
                default:
                    Result = response.Fail("Exception Method:" + f);
                    break;
            }
        }
        catch (Exception e)
        {
            Result = response.Fail(e.Message.ToString(), 9);
        }
        Log.D("响应[" + serverCode + ":FN-" + f + "]" + Result, c, SYSTEM_TAG);
        return Result;
    }

    private int getServerCode() {
        Random r = new Random(DateTime.Now.Millisecond);
        return r.Next(100000000,999999999);
    }

    private string GetLog(HttpContext c) {
        return response.Success(new DSysLog().GetLog());
    }

    private string DoLogin(HttpContext c) {
        string account=c.Request["account"].ToString();
        string password = c.Request["password"].ToString();
        string vcode = c.Request["vcode"].ToString();
        if (!vcode.Equals(c.Session["vcode"])) {
            return response.Fail("vcode Error", 2);
        }

        Admin admin = new DAdmin().Login(account, password);
        if (admin == null)
        {
            return response.Fail("Password Error", 1);
        }
        else {
            return response.Success(admin,new { DepartmentName="",RoleName="系统管理员"});
        }
    }

    private string ChangePwd(HttpContext c)
    {
        string oldpwd = c.Request["oldpwd"].ToString();
        string password = c.Request["newpwd"].ToString();
        string account = c.Request["mobile"].ToString();
        Admin admin = new DAdmin().Login(account, oldpwd);
        if (admin != null)
        {
            bool isChange = new DAdmin().ChangePwd(password, admin.Id);
            if (isChange)
            {
                return response.Success(isChange, new { Msg = "Change Success" });
            }
            else
            {
                return response.Success(isChange, new { Msg = "Server Error" });
            }
        }
        else
        {
            return response.Fail("Password Error", 1);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}
