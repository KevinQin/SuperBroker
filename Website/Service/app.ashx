﻿<%@ WebHandler Language="C#" Class="AppHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;
using com.seascape.tools;
using com.superbroker.data;
using com.superbroker.model;
using com.seascape.wechat;


public class AppHandler:IHttpHandler,IRequiresSessionState {
    //微信公众号相关
    static string APPID = com.seascape.tools.BasicTool.GetConfigPara("appid");
    static string SECRET = com.seascape.tools.BasicTool.GetConfigPara("secret");
    //微信支付商户
    static string mch_id = com.seascape.tools.BasicTool.GetConfigPara("mch_id");
    //微信支付接收返回地址
    static string notify_url = com.seascape.tools.BasicTool.GetConfigPara("notify_url");
    //微信支付提交服务器IP
    static string client_ip = com.seascape.tools.BasicTool.GetConfigPara("client_ip");
    //微信支付密钥
    static string keyValue = com.seascape.tools.BasicTool.GetConfigPara("keyValue");
    static string HjKeyValue = "Seascape.Fast.Fix";
    //默认代金券信息
    static int voucherFee = 10;
    static string voucherS = "J";
    //微信卡券配置信息
    static string wxcardConfig = "pR7BJwMJKGp4Aun2O7OaMOb9nBTM@10";
    //系统相关
    static int perPage = 15;
    static string DefaultWrokNo = "100";
    static string _BASE_URL = com.seascape.tools.BasicTool.GetConfigPara("baseurl");
    static string MsgUrl = "http://"+_BASE_URL+"/service/app.ashx";
    static string BaseUrl = "http://"+_BASE_URL+"/";
    static Response response;
    HttpContext _c;
    static Admin admin = null;
    private DateTime DEF_DATE = new DateTime(2000, 1, 1);

    public void ProcessRequest(HttpContext c) {
        response = new Response();
        _c = c;
        int F = c.xRequest("fn").ToInt16();
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
        string Result = response.Fail("System Exception[" + f + "]", 1);
        Log.D("[A]Request-[FN:" + f + "]", c);
        try
        {
            switch (f)
            {
                case 1:
                    Result = GetOpenId(c);//获取OPENID
                    break;
                case 2:
                    Result = GetMember(c);//获取会员信息
                    break;
                case 3:
                    Result = CreateCode(c);//用户二维码生成
                    break;
                case 4:
                    Result = InitMenu(c);//初始化菜单
                    break;
                case 5:
                    Result = GetPayInfo_(c);//获取支付相关信息
                    break;
                case 6:
                    Result = WxConfig(c);//微信Config
                    break;
                case 7:
                    Result = GetWxCardInfo(c);//获取微信卡包接口
                    break;
                case 8:
                    Result = DestoryWxCard(c);//核销卡券
                    break;
                case 9:
                    Result = GetLocation(c);//获取地理位置
                    break;
                case 101:
                    Result = RegisterBroker(c);//注册经纪人
                    break;
                case 102:
                    Result = BrokerLogin(c);//经纪人登录
                    break;
                case 103:
                    Result = GetBuilderList(c);//得到楼盘列表
                    break;
                case 104:
                    Result = GetBuilder(c);//载入楼盘详细信息
                    break;
                case 105:
                    Result = GetRoomList(c);//得到户型列表
                    break;
                case 106:
                    Result = DoReport(c);//备案
                    break;
                case 107:
                    Result = GetPreReportInfo(c);//获取信息
                    break;
                case 108:
                    Result = GetReportList(c);//获取报备列表
                    break;
                case 109:
                    Result = GetReportDetail(c);//报备详情
                    break;
                case 110:
                    Result = GetReportLog(c);//获取报备日志
                    break;
                case 111:
                    Result = GetFeeList(c);//获取返佣日志
                    break;
                case 112:
                    Result = GetBrokerInfo(c);//
                    break;
                case 113:
                    Result = EditBrokerInfo(c);
                    break;
                case 114:
                    Result = EditBankInfo(c);
                    break;
                case 115:
                    Result = CheckPwd(c);//
                    break;
                case 116:
                    Result = ChangePwd(c);//
                    break;
                case 117:
                    Result = GetNotifyList(c);//
                    break;
                case 118:
                    Result = GetHelpList(c);//
                    break;
                case 201:
                    Result = GetBrokerForCheck(c);//
                    break;
                case 202:
                    Result = UpdateBrokerState(c);
                    break;
                case 203:
                    Result = GetAdminInfo(c);
                    break;
                case 204:
                    Result = GetBrokerList(c);//
                    break;
                case 999:
                    Result = Test(c);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Result = e.Message.ToString();
        }
        Log.D("[A]Result-" + f + ":" + Result.ToString(), c);
        return ReplaceTableName(Result);
    }

    private string GetBrokerList(HttpContext c) {
        string openid = c.Request["openid"];
        string workno = c.Request["workno"];
        //业务、管理
        return "";
    }

    private string GetAdminInfo(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        string workno = c.Request["workno"].ToString();
        Admin admin = new DAdmin().Get(openid, workno);
        Member member = new DMember().Get(openid);
        if (admin != null)
        {
            return response.Success(new {
                Name =admin.Name,
                WorkNo =admin.WorkNo,
                RoldId =admin.RoleId,
                RoleName = GetRoleName((Role)Enum.Parse(typeof(Role),admin.RoleId.ToString())),
                Enable=admin.Enable,
                PhotoUrl=member.PhotoUrl,
                Mobile= member.Mobile
            });
        }
        else {
            return response.Fail("Error");
        }
    }

    private string GetRoleName(Role role) {
        string result = "";
        switch (role)
        {
            case Role.Admin:
                result = "管理员";
                break;
            case Role.Builder:
                result = "案场员工";
                break;
            case Role.Fund:
                result = "财务员工";
                break;
            case Role.Editer:
                result = "内容编辑";
                break;
            case Role.Bussiness:
                result = "业务经理";
                break;
            case Role.Broker:
                result = "经纪人";
                break;
            default:
                result = "未知";
                break;
        }
        return result;
    }

    private string UpdateBrokerState(HttpContext c) {
        int Id = c.xRequest("id").ToInt();
        string brokerNo = c.xRequest("bno");
        string workNo = c.xRequest("wno");
        int _state = c.xRequest("state").ToInt16();
        string info = c.xRequest("info");
        BrokerState state = (BrokerState)Enum.Parse(typeof(BrokerState), _state.ToString());

        Broker broker = new DBroker().Get(Id,"",brokerNo);
        if (broker == null)
        {
            return response.Fail("没有相关的经纪人[-2]");
        }
        else if (broker.CheckWorkNo != workNo)
        {
            return response.Fail("没有审批权限[-3]");
        }
        else {
            if (new DBroker().ChangeState(brokerNo, Id, workNo, info, state))
            {
                return response.Success("审批完成");
            }
            else {
                return response.Fail("审批失败，稍候再试[-4]");
            }

        }
    }

    private string GetHelpList(HttpContext c) {
        List<Help> list = new DHelp().Get();
        if (list != null)
        {
            return response.Success(list);
        }
        else {
            return response.Fail("Error");
        }
    }

    private string GetNotifyList(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        int pagenum = c.Request["pageno"].ToInt16();
        if (broker == null) {
            return response.Fail("没有权限[-2]");
        }
        else {
            int PageCount = 0;
            List<Notify> list = new DNotify().Get(out PageCount, broker.WorkNo, pagenum);
            if (list != null)
            {
                return response.Success(list, new { PageCount = PageCount, PageNo = pagenum });
            }
            else {
                return response.Fail("Error");
            }
        }
    }

    private string ChangePwd(HttpContext c) {
        string oldpwd = c.Request["oldpwd"].ToString();
        string openid = c.Request["openid"].ToString();
        string pwd = c.Request["pwd"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        int id = 0;
        if (broker != null && new DBroker().Login(openid, broker.Mobile,oldpwd,out id)!=null)
        {
            if (id == 1) {

                broker.Password = pwd;
                if (new DBroker().Update(broker))
                {
                    Log.D("经纪人修改登录密码为【"+ pwd +"】", c);
                    return response.Success("Success");
                }
                else {
                    Log.D("经纪人修改登录密码失败", c);
                    return response.Fail("ChangeError");
                }
            }
            else
            {
                Log.D("经纪人修改登录密码失败", c);
                return response.Fail("Error["+ id +"]");
            }

        }
        else {
            Log.D("经纪人修改登录密码失败", c);
            return response.Fail("Error["+ id +"]");
        }
    }

    private string CheckPwd(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        string oldpwd = c.Request["oldpwd"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        int id = 0;
        if (broker != null && new DBroker().Login(openid, broker.Mobile,oldpwd,out id)!=null)
        {
            if (id == 1) {
                return response.Success("Success");
            }
            else
            {
                return response.Fail("Error["+ id +"]");
            }

        }
        else {
            return response.Fail("Error["+ id +"]");
        }
    }

    private string EditBankInfo(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null ) {
            return response.Fail("没有权限[-2]");
        }
        string bankinfo = c.xRequest("bankinfo");
        string account = c.xRequest("account");
        string cardno = c.xRequest("cardno");
        broker.BankInfo = bankinfo;
        broker.BankCardNo = cardno;
        broker.AccountName = account;
        if (new DBroker().Update(broker))
        {
            Log.D("经纪人修改银行卡信息为【"+ bankinfo +"】【"+ account +"】【"+ cardno +"】", c);
            return response.Success("Success");
        }
        else {
            Log.D("经纪人修改银行卡信息失败", c);
            return response.Fail("err");
        }
    }

    private string EditBrokerInfo(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        int id = c.xRequest("id").ToInt();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null || broker.Id!=id) {
            return response.Fail("没有权限[-2]");
        }
        string mobile = c.xRequest("mobile");
        int gender = c.xRequest("gender").ToInt16();
        string tel = c.xRequest("tel");
        string city = c.xRequest("city");
        string address = c.xRequest("address");
        string trade = c.xRequest("trade");
        string company = c.xRequest("company");
        broker.Mobile = mobile;
        broker.Gender = gender;
        broker.Tel = tel;
        broker.Area = city;
        broker.Address = address;
        broker.Trade = trade;
        broker.Company = company;
        if (new DBroker().Update(broker))
        {
            Log.D("经纪人修改个人信息完成", c);
            return response.Success("Success");
        }
        else {
            Log.D("经纪人修改个人信息失败", c);
            return response.Fail("err");
        }
    }

    private string GetFeeList(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("没有权限[-2]");
        }
        string BrokerNo = broker.WorkNo;
        List<ReportingSimpleView> list = new DReporting().GetFeeList(BrokerNo);
        if (list == null)
        {
            return response.Fail("Load Log Error");
        }
        else
        {
            return response.Success(list);
        }
    }

    private string GetReportLog(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("没有权限[-2]");
        }
        string BrokerNo = broker.WorkNo;
        string rno = c.xRequest("rno");
        List<ReportLog> list = new DReportLog().Get(rno);
        if (list == null)
        {
            return response.Fail("Load Log Error");
        }
        else
        {
            return response.Success(list);
        }
    }

    private string GetReportDetail(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("没有权限[-2]");
        }
        string BrokerNo = broker.WorkNo;
        string rno = c.xRequest("rno");
        ReportingSimpleView view = new DReporting().Get(BrokerNo, rno);
        if (view == null)
        {
            return response.Fail("Load Detail Error");
        }
        else
        {
            return response.Success(view, new { BankInfo = broker.BankInfo, BankCard = broker.BankCardNo, BankAccount = broker.AccountName });
        }
    }

    private string GetReportList(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("没有权限[-2]");
        }
        string BrokerNo = broker.WorkNo;
        int state = c.xRequest("state").ToInt16();
        int pageno = c.xRequest("pageno").ToInt16();
        int pagecount = 0;
        if (pageno < 1) { pageno = 1; }
        List<ReportingSimpleView> list = new DReporting().Get(BrokerNo, (ReportState)Enum.Parse(typeof(ReportState), state.ToString()), pageno, out pagecount);
        return response.Success(list, new { PageCount = pagecount, PageNo = pageno });
    }

    private string GetPreReportInfo(HttpContext c) {
        string builderNo = c.xRequest("bno");
        string openid = c.xRequest("openid");
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("search broker Error", -3);
        }


        Builder builder = new DBuilder().Get(0, builderNo);
        if (builder == null) {
            return response.Fail("search builder Error", -4);
        }

        return response.Success(new {BuilderName=builder.Name, BrokerName= broker.Name, BrokerWorkNo=broker.WorkNo, Begin=DateTime.Now.ToString("yyyy年M月d日 HH点"),End=DateTime.Now.AddDays(7.0).ToString("yyyy年M月d日 HH点")});
    }

    private string DoReport(HttpContext c) {
        string name = c.xRequest("name");
        string mobile = c.xRequest("mobile");
        string builderNo = c.xRequest("bno");
        string openid = c.xRequest("openid");
        string memo = c.xRequest("memo");
        int gender = c.xRequest("gender").ToInt16();
        int pageOut = 0;
        List<Custom> list = new DCustom().Get("", name, mobile, 1, DEF_DATE, DEF_DATE, out pageOut);
        string customNo = "";
        if (list!=null && list.Count > 0)
        {
            customNo = list[0].CustomNo;
            list.Clear();
            list = null;
        }
        else {
            Custom custom = new Custom()
            {
                AddOn = DateTime.Now,
                CustomNo = GetOrderNo(),
                Gender = gender,
                Memo = "",
                Mobile = mobile,
                Name = name,
                OpenId = "",
                Tel = "",
                UnionId = ""
            };
            int CustomId = 0;
            bool addCustom= new DCustom().Add(out CustomId, custom);
            addCustom = CustomId > 1;
            if (addCustom) {
                customNo = custom.CustomNo;
            }
        }
        if (string.IsNullOrEmpty(customNo)) { return response.Fail("备案失败，[-2]",-2); }

        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("备案失败，无效的经纪人", -3);
        }
        string BrokerNo = broker.WorkNo;

        Builder builder = new DBuilder().Get(0, builderNo);
        if (builder == null) {
            return response.Fail("备案失败，无效的楼盘", -4);
        }
        int fee = builder.FeePeer;
        int feeType = builder.FeeType;
        //是否已备案查询
        List<Reporting> _list = new DReporting().Get(1, out pageOut,name, mobile, builder.Name, "", "", DEF_DATE, DEF_DATE, ReportState.OutFail);
        ReportState state = ReportState.ReportSuccess;
        if (_list!=null && _list.Count > 0)
        {
            state = ReportState.ReportFail;
            _list.Clear();
            _list = null;
        }

        Reporting report = new Reporting() {
            AddOn=DateTime.Now, ArriveOn=DEF_DATE, BackFeeOn=DEF_DATE, BrokerNo=BrokerNo, BuilderNo=builderNo, CustomNo=customNo, DebitedOn=DEF_DATE,
            DisableOn=DEF_DATE, DownPaymentOn=DEF_DATE, FailOn=DEF_DATE, Fee=fee, FeeType=feeType, HouseNo="", PayFeeOn=DEF_DATE, ProtectedOn=DateTime.Now.AddDays(7),
            ReportNo=GetOrderNo(), ReportOn=DateTime.Now, ReturnFeeOn=DEF_DATE, RoomNo="", SignedOn=DEF_DATE, State=state, TotalPrice=0
        };
        int id = 0;
        if ( new DReporting().Add(out id, report))
        {
            if (state ==  ReportState.ReportSuccess)
            {
                if (id > 0)
                {
                    //增加一条日志
                    ReportLog log = new ReportLog() {
                        AddOn=DateTime.Now, Memo="成功报备", ReportNo=report.ReportNo, State=ReportState.ReportSuccess, WorkNo= BrokerNo
                    };
                    new DReportLog().Add(out id, log);
                    return response.Success(report.ReportNo);
                }
                else {
                    return response.Fail("备案失败，请重试[-6]",-6);
                }
            }
            else {
                ReportLog log = new ReportLog() {
                    AddOn=DateTime.Now, Memo="报备失败", ReportNo=report.ReportNo, State=ReportState.ReportSuccess, WorkNo= BrokerNo
                };
                return response.Fail("备案失败，该客户已报备",-7);
            }

        }
        else {
            return response.Fail("备案失败，请重试[-5]",-5);
        }
    }

    private string GetRoomList(HttpContext c) {
        string bno = c.xRequest("bno");
        int PageCount = 0;
        List<Room> list = new DRoom().Get(out PageCount, 1, bno, "", DEF_DATE, DEF_DATE);
        if (list == null)
        {
            return response.Fail("load room error");
        }
        else {
            return response.Success(list);
        }
    }

    private string GetBuilder(HttpContext c)
    {
        string bno = c.xRequest("bno");
        Builder builder = new DBuilder().Get(0, bno);
        if (builder != null)
        {
            return response.Success(builder);
        }
        else {
            return response.Fail("Load Detail Info Error");
        }
    }

    private string GetBuilderList(HttpContext c) {
        int PageNo = c.xRequest("page").ToInt16();
        string key = c.xRequest("key");
        string location = c.xRequest("location");
        int pageOut = 0;
        List<SimpleBuilderView> list = new DBuilder().Get(out pageOut, key, key, location, PageNo);
        if (list != null)
        {
            return response.Success(list, new { PageCount = pageOut,PageNo=PageNo });
        }
        else {
            return response.Fail("Get Info Error");
        }
    }

    private string GetBrokerForCheck(HttpContext c) {
        int workerno = c.xRequest("wno").ToInt();
        int brokerno = c.xRequest("bno").ToInt();
        int id = c.xRequest("state").ToInt();
        Broker broker = new DBroker().Get(id,"",brokerno.ToString());
        if (broker == null)
        {
            return response.Fail("没有相关的经纪人[-2]");
        }
        else if (Convert.ToInt32(broker.CheckWorkNo) != workerno)
        {
            return response.Fail("没有审批权限[-3]");
        }
        else if (broker.State!= BrokerState.New)
        {
            return response.Fail("该经纪人已审批[-4]");
        }
        else {
            return response.Success(broker);
        }
    }

    private string GetBrokerInfo(HttpContext c) {
        string openid = c.Request["openid"].ToString();
        Broker broker = new DBroker().Get(0, openid);
        if (broker == null) {
            return response.Fail("无权获取当前备案信息[-2]");
        }
        return response.Success(broker);
    }

    private string BrokerLogin(HttpContext c) {
        string account = c.xRequest("account").ToString();
        string openid = c.Request["openid"].ToString();
        string pwd = c.Request["pwd"].ToString();
        int ErrCode = 0;
        Role RoleId =  Role.Broker;
        if (account.Length == 3 ) {
            int.TryParse(account, out ErrCode);
            if (ErrCode > 100) { RoleId =  Role.Admin; }
            ErrCode = 0;
        }
        object obj = null;
        if (RoleId== Role.Admin)
        {
            Admin admin = new DAdmin().Login(account, pwd);

            obj = admin;
            if (obj == null)
            {
                ErrCode = 0;
            }
            else {
                ErrCode = 1;
                RoleId = admin.RoleId;
            }
        }
        else {
            obj= new DBroker().Login(openid, account, pwd, out ErrCode);
        }
        if (ErrCode == 1)
        {
            return response.Success(obj,new { RoleId = RoleId});
        }
        else {
            return response.Fail("login fail", ErrCode*-1);
        }
    }

    private string RegisterBroker(HttpContext c) {
        string name = c.xRequest("name").ToString();
        string mobile = c.xRequest("mobile").ToString();
        string pwd = c.Request["pwd"].ToString();
        string openid = c.Request["openid"].ToString();
        int code = c.Request["code"].ToInt();
        if (!new DAdmin().GetAdminByWorkNo(code.ToString())) {
            return response.Fail("邀请码无效", -4);
        }

        Member member = new DMember().Get(openid);
        Broker b= new DBroker().Get(0, openid);
        if (b == null)
        {
            int WorkNo = new DBroker().GetWorkNo();
            Broker broker = new Broker()
            {
                AccountName = "",
                AddOn = DateTime.Now,
                Address = "",
                Area = member.Country + "," + member.Province + "," + member.City,
                AvatarMediaId = member.PhotoUrl,
                BankCardNo = "",
                BankInfo = "",
                CheckInfo = "等待审核",
                CheckOn = DEF_DATE,
                CheckWorkNo = code.ToString(),
                Company = "",
                FeePeer = 0,
                Gender = member.Gender,
                Memo = "",
                Mobile = mobile,
                Name = name,
                OpenId = openid,
                Password = pwd,
                State =  BrokerState.New,
                Tel = "",
                Trade = "",
                UnionId = "",
                UptimeOn = DEF_DATE,
                WorkNo = WorkNo.ToString()
            };
            int Id = 0;
            if (new DBroker().Add(out Id, broker))
            {
                if (Id > 0)
                {
                    Log.D("注册经纪人成功【" + broker.Name + "," + broker.Mobile + "】，邀请码【" + code + "】", c);
                    return response.Success("注册成功");
                }
                else
                {
                    Log.D("注册经纪人失败【" + broker.Name + "," + broker.Mobile + "】，邀请码【" + code + "】", c);
                    return response.Fail("注册失败", -1);
                }
            }
            else
            {
                return response.Fail("注册失败", -2);
            }
        }
        else {
            return response.Fail("已注册，不能重复注册", -3);
        }
    }

    public string Test(HttpContext c)
    {
        /*string xml = "<xml><ToUserName><![CDATA[gh_1ead2193ad3d]]></ToUserName><FromUserName><![CDATA[o3MRawEmK5OK-ringNnTyiNPA6uM]]></FromUserName><CreateTime>1504167115</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[unsubscribe]]></Event><EventKey><![CDATA[]]></EventKey></xml>";
        BaseMessage msg= Common.ConvertObj<EventMessage>(xml);
        Log.F(msg.FromUserName,c);
        msg.ResText("猜猜我是谁？");*/
        return response.Success("");
    }

    public string GetOrderNo()
    {
        return string.Format("{0}{1:00}", DateTime.Now.ToString("yyMMddHHmmss"), new Random().Next(99));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private string ReplaceTableName(string str)
    {
        Regex reg = new Regex(",\\\"TABLENAME\\\":\\\"\\w*\\\"");
        str = reg.Replace(str, "");
        return str;
    }

    #region About Wechat
    public string GetLocation(HttpContext c) {
        double latitude = Convert.ToDouble(c.Request["latitude"]);
        double longitude = Convert.ToDouble(c.Request["longitude"]);
        return Util.GetFormattedAddress(latitude, longitude);
    }

    /// <summary>
    /// 通过Code获取OpenID
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetOpenId(HttpContext c)
    {
        string code = string.IsNullOrEmpty(c.Request["code"]) ? "" : c.Request["code"].ToString();
        string source = string.IsNullOrEmpty(c.Request["source"]) ? "" : c.Request["source"].ToString();
        Log.D("code:"+code,_c);
        Common.appid = APPID;
        Common.secret = SECRET;
        access_token_UserInfo at = Common.Get_access_token_UserInfo(code);
        if (!string.IsNullOrEmpty(at.openid))
        {
            Log.D("openid:" + at.openid, _c);
            //------------------写用户------------------
            Member u = new DMember().Get(at.openid);
            if (u == null)
            {
                u = new Member();
                string r = "";
                UserInfo ui =UserInfo.getUserInfoByCode(at.access_token, at.openid, out r);
                Log.D("r:" + r, _c);
                if (ui != null && !string.IsNullOrEmpty(ui.nickname))
                {
                    source = source.Replace("i", "1");
                    u.NickName = ui.nickname.Replace("'", "").Replace(@"""", "");
                    u.PhotoUrl = ui.headimgurl;
                    u.Country = ui.country;
                    u.Province = ui.province;
                    u.Source = source;
                    u.IsFllow = false;
                    u.QrCode = 0;
                    u.City= ui.city;
                    u.Gender = ui.sex;
                    u.OpenId = ui.openid;
                    u.Mobile = "";
                    u.Name = "";
                    u.AddOn = DateTime.Now;
                    u.Memo = "";
                    int id = 0;
                    new DMember().Add(out id, u);
                    u.Id = id;
                }
            }
            return response.Success(at.openid);
        }
        else
        {
            Log.D("openid:" + at.errcode.ToString(), _c);
            return response.Fail(at.errcode.ToString(),1);
        }
    }


    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetMember(HttpContext c)
    {
        string openId = string.IsNullOrEmpty(c.Request["openId"]) ? "" : c.Request["openId"].ToString();
        string source = string.IsNullOrEmpty(c.Request["source"]) ? "" : c.Request["source"].ToString();
        if (openId.Length > 0)
        {
            Member u = new DMember().Get(openId);
            if (u != null)
            {
                return response.Success(u);
            }
            else
            {
                u = new Member();
                string r = "";
                UserInfo ui = UserInfo.getUserInfoByGlobal(WeChat.GetAccessToken(c), openId, out r);
                Log.D("r:"+ r,_c);
                if (ui != null && !string.IsNullOrEmpty(ui.nickname))
                {
                    u.NickName = ui.nickname.Replace("'", "").Replace(@"""", "");
                    u.PhotoUrl = ui.headimgurl;
                    u.Country = ui.country;
                    u.Province = ui.province;
                    u.Source = source;
                    u.IsFllow = false;
                    u.QrCode = 0;
                    u.City= ui.city;
                    u.Gender = ui.sex;
                    u.OpenId = ui.openid;
                    u.Mobile = "";
                    u.Name = "";
                    u.AddOn = DateTime.Now;
                    u.Memo = "";
                    int Id = 0;
                    new DMember().Add(out Id,u);
                    u.Id = Id;
                    return response.Success(u);
                }
                else
                {
                    return response.Fail("get member info error", 1);
                }
            }
        }
        return response.Fail("open id length is too short", 2);
    }



    /// <summary>
    /// 微信认证
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string WxConfig(HttpContext c)
    {
        Common.WxConfig w = InitJsApi(c);
        if (w != null)
        {
            return w.signature + "|" + w.timestamp + "|" + w.nonce + "|" + w.appid;
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 用户二维码生成
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string CreateCode(HttpContext c)
    {
        int uid = string.IsNullOrEmpty(c.Request["uid"]) ? 0 : Convert.ToInt16(c.Request["uid"]);
        if (uid > 0)
        {
            string FilePath = c.Server.MapPath("/QrCode/user/");
            BasicTool.CheckFolder(FilePath,true);
            FilePath+= uid + ".jpg";
            int SceneID = uid;
            string Result = new Common(APPID, SECRET).GetQR_Code(FilePath, SceneID);
            Log.D("生成二维码成功["+ uid +"]", _c);
            return response.Success("success");
        }
        return response.Fail("error", 1);
    }


    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="o"></param>
    /// <param name="key"></param>
    /// <param name="openid"></param>
    /// <param name="url"></param>
    public void SendTemplateMessage(HttpContext c, object o, string key, string openid, string url, string MsgContent,string orderno="")
    {
        TMessage t = new TMessage
        {
            touser = openid,
            data = o,
            template_id = key,
            url = url,
            topcolor = ""
        };
        try
        {
            TemplateMsg tmp = new TemplateMsg
            {
                MsgId = key,
                MsgBody = JsonMapper.ToJson(o),
                MsgUrl = url,
                OpenId = openid,
                AddOn = DateTime.Now,
                MsgContent = MsgContent,
                OrderNo = orderno
            };
            int Id = 0;
            new DTemplateMsg().Add(out Id, tmp);
        }
        catch { }
        new TMessage().Send_TemplateMsg(t, WeChat.GetAccessToken(c));
    }


    /// <summary>
    /// 获取支付相关信息--字符串
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetPayInfo_(HttpContext c)
    {
        return "";
    }

    /// <summary>
    /// 处理支付
    /// </summary>
    /// <param name="OrderNo"></param>
    /// <param name="prepayID"></param>
    /// <returns></returns>
    public WxPay.WxPayConfig OperPay(string OrderNo, string prepayID)
    {
        WxPay.WxPayConfig wp = null;
        return wp;
    }


    /// <summary>
    /// 发送菜单
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string InitMenu(HttpContext c)
    {
        string errmsg = "ERR";
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("    \"button\": [");
            sb.Append("        {");
            sb.Append("           \"name\": \"经纪人\", ");
            sb.Append("            \"sub_button\": [");
            sb.Append("                {");
            sb.Append("                    \"type\": \"view\", ");
            sb.Append("                    \"name\": \"我要加入\", ");
            sb.Append("                    \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fBroker%2fRegister.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("                }, ");
            sb.Append("                {");
            sb.Append("                    \"type\": \"view\", ");
            sb.Append("                    \"name\": \"进入平台\", ");
            sb.Append("                    \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fBroker%2fLogin.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("                } ");
            sb.Append("            ]");
            sb.Append("        },");
            sb.Append("        {");
            sb.Append("            \"type\": \"view\", ");
            sb.Append("            \"name\": \"楼盘介绍\", ");
            sb.Append("            \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fBuilder%2fList.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("        }, ");
            sb.Append("        {");
            sb.Append("            \"type\": \"view\", ");
            sb.Append("            \"name\": \"关于我们\", ");
            sb.Append("            \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fAbout.html&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("        }, ");
            sb.Append("    ]");
            sb.Append("}");
            string ACCESS_TOKEN = WeChat.GetAccessToken(c);
            string GetUrl = " https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + ACCESS_TOKEN;
            string JsonStr = new Common(APPID,SECRET).webRequest(GetUrl, sb.ToString());
            JsonData jd = JsonMapper.ToObject(JsonStr);
            if (jd["errcode"].ToString() == "0")
            {
                return "{Return:0,Msg:'OK'}";
            }
            else
            {
                try
                {
                    errmsg = "[" + jd["errcode"].ToString() + "]" + jd["errmsg"].ToString();
                }
                catch { }
            }
        }
        return "{Return:1,Msg:'" + errmsg + "'}";
    }


    /// <summary>
    /// 初始化微信JS
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public Common.WxConfig InitJsApi(HttpContext c)
    {
        try
        {
            string Urls = c.Request["Url"] == null ? "" : c.Request["Url"].ToString();
            bool isFail = string.IsNullOrEmpty(c.Request["isFail"]) ? false : true;
            if (Urls.IndexOf("#") != -1)
            {
                Urls = Urls.Split('#')[0].ToString();
            }
            Urls = Urls.Replace("%26", "&");
            Urls = Urls.Replace("|", "%7C");
            string jsApi_ticket = "";
            string Access_Token = "";
            if (isFail || c.Cache["Para_JsApiTicket"] == null || c.Cache["Para_JsApiTicket"].ToString().Length == 0)
            {
                Access_Token = WeChat.GetAccessToken(c);
                //获取网页调用临时票据
                string r = "";
                jsApi_ticket = new Common(APPID, SECRET).Get_jsapi_ticket(Access_Token, out r);
                Log.D("r:" + r, c);
                Log.D("jsApi_ticket:" + jsApi_ticket, c);
                c.Cache.Add("Para_JsApiTicket", jsApi_ticket, null, System.DateTime.UtcNow.AddMinutes(100), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                jsApi_ticket = c.Cache["Para_JsApiTicket"].ToString();
            }


            string TempS = "";
            Log.D("isFail-Urls:" + isFail.ToString() + Urls, c);
            Common.WxConfig w = Common.Get_Config_(Urls, jsApi_ticket, out TempS);
            Log.D("TempS:" + TempS, c);
            return w;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 调用微信卡包接口
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetWxCardInfo(HttpContext c)
    {
        string nonce_str = new Random().Next(100000, 999999).ToString();
        string card_id = "";// "pR7BJwD6sYq-5TAhAMQd2U4Ko-9g";
        string r = "";
        string api_ticket = new WxCard().Get_api_ticket(WeChat.GetAccessToken(c), out r);
        if (api_ticket.Length > 0)
        {
            long timestamp = 0;
            string sign = new WxCard().Get_Signature(APPID, nonce_str, card_id, api_ticket, out timestamp);
            if (sign.Length > 0)
            {
                return card_id + "|" + timestamp + "|" + nonce_str + "|" + sign + "|" + wxcardConfig;
            }
        }
        return "";
    }

    /// <summary>
    /// 核销卡券
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string DestoryWxCard(HttpContext c)
    {
        string card_id = string.IsNullOrEmpty(c.Request["cardid"]) ? "" : c.Request["cardid"].ToString();
        string code = string.IsNullOrEmpty(c.Request["cardno"]) ? "" : c.Request["cardno"].ToString();
        Log.D("DestoryWxCard-card_id:" + card_id + ",code:" + code,c);
        return response.Success(new WxCard().DestroyCode(WeChat.GetAccessToken(c), card_id, code));
    }

    /// <summary>
    /// 获取微信卡券金额
    /// </summary>
    /// <param name="Card_ID"></param>
    /// <returns></returns>
    public int GetWxCardFee(string Card_ID)
    {
        string[] w = (wxcardConfig + ",").Split(',');
        foreach (string ws in w)
        {
            if (ws.Length > 0)
            {
                if (ws.Split('@')[0] == Card_ID)
                {
                    return Convert.ToInt16(ws.Split('@')[1]);
                }
            }
        }
        return 0;
    }
    #endregion
}