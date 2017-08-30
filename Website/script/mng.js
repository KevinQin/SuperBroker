var Mng = {};
var ajaxServerUrl = "/service/Handler.ashx";
var PageName = "";
var BASE_URL = "http://b.4009990351.com/";

Mng.init = function () {
    var pageUrl = window.location.href;
    if (pageUrl.indexOf("?") > -1) {
        pageUrl = pageUrl.split("?")[0];
    }
    var pageItems = pageUrl.split("/");
    PageName = pageItems[pageItems.length - 1].split('.')[0].toLowerCase();

    if (PageName == "member") {
        MemberPage.init();
    } else if (PageName == "enterprise") {
        EnterprisePage.init();
    } else if (PageName == "policy") {
        PolicyPage.init();
    } else if (PageName == "professor") {
        ProfessorPage.init();
    } else if (PageName == "investor") {
        InvestorPage.init();
    } else if(PageName=="demand"){
        DemandPage.init();
    }
};

var DemandPage = {
    UserData: null,
    CurrIndex: 0,
    CurrShowId: 0,
    init: function () {
        $("#btnSearch").on("click", DemandPage.loadList).trigger("click");
        $("#btnReset").on("click", DemandPage.SearchBarReset);
        $("#btnSave").on("click", DemandPage.SaveDemand);
        $("#btnBackList").on("click", function () {
            $("#list").show();
            $("#inList").hide("fast");
        });
    },    
    SearchBarReset: function () {
        $("#txtName").val("");
        $("#txtContact").val("");
        $("#txtMobile").val("");
    },
    SaveDemand: function () {
        var subject = $('#sel_Subject').val();
        var project = $('#txt_Project').val();
        var content = $('#txt_Content').val();
        var price = $("#txt_Price").val();
        var finishOn = $('#txt_FinishOn').val();
        var finishType = $('#txt_FinishType').val();     
        var state = $('#sel_State').val();
        var memo = $('#txt_Memo').val();
        if (finishOn == "") { finishOn = "2000-1-1";}
        $ajax({
            fn: 120, id: DemandPage.CurrShowId, subject: subject, project: project, content: content, price: price, finishOn: finishOn, finishType: finishType, memo: memo, state: state
        }, DemandPage.SaveCallback, true);
    },
    SaveCallback: function (o) {
        if (o.Return == 0) {
            showToast("维护需求信息成功");
            DemandPage.ResetForm();
            DemandPage.loadList();
        }
    },
    loadList: function () {
        var name = $("#txtName").val();
        var category = $("#selCategory").val();
        var subject = $("#selSubject").val();
        var state = $("#selState").val();
        var pageNum = 1;
        if ($(this) && !isNaN(parseInt($(this).text()))) {
            pageNum = parseInt($(this).text());
        }
        $("#MsgList").empty();
        $ajax({ fn: 119, project: name, category: category, subject: subject, state: state,pagenum:pageNum }, DemandPage.loadListCallbak, true);
    },
    loadListCallbak: function (o) {
        if (o.Return != 0) { return; }
        $("#list").show();
        $("#inList").hide("fast");
        var template = '<tr data-index="{1}" data-id="{0}"><td data-lnk="">{2}</td><td data-lnk="">{3}</td><td data-lnk="">{4}</td><td data-lnk="">{5}</td><td data-lnk="">{6}</td><td data-lnk="">{7}</td><td><a href="#" data-act="del" data-id="{-1}">删除</a></td></tr>';
        DemandPage.UserData = o.data;
        for (var i = 0; i < o.data.length; i++) {
            var obj = o.data[i];
            var catename=GetMemberType(obj.PostCategory);
            if(obj.PostCategory<4){catename+="["+ obj.Subject +"]";}
            var html = template.replace("{0}", obj.Id).replace("{1}", i).replace("{-1}", obj.Id).replace("{2}", catename).replace("{3}", obj.Project).replace("{4}", obj.Contact).replace("{5}", obj.Mobile).replace("{6}", getFullDateTime(obj.AddOn)).replace("{7}", GetState(obj.State));
            $("#MsgList").append(html);
        }
        $("td[data-lnk]").on("click", DemandPage.showDetail);
        $("a[data-act]").on("click", DemandPage.ConformDelete);
        DemandPage.CurrShowId = 0;
        if (o.data.length <= 0) {
            $("#MsgList").parents("table").hide();
        }
        else {
            $("#MsgList").parents("table").show();
        }
        //分页
        $(".pagination").empty();
        if (o.Ext.PageCount > 1) {
            for (var i = 1; i <= o.Ext.PageCount; i++) {
                if (i == o.Ext.CurrPage) {
                    $(".pagination").append('<li class="active"><a href="#">' + i + ' <span class="sr-only">(current)</span></a></li>')
                } else {
                    $(".pagination").append('<li><a href="#">' + i + '</a></li>');
                }
            }
            $(".pagination").show();
            $(".pagination li a").on("click", DemandPage.loadList);
        }
        else {
            $(".pagination").hide();
        }
    },
    showDetail: function () {
        var uid = $(this).parents("tr").attr("data-id");
        var index = $(this).parents("tr").attr("data-index");
        DemandPage.CurrIndex = index;
        DemandPage.CurrShowId = uid;
        var cdata = DemandPage.UserData[index];        
        $('#sel_Subject').val(cdata.Subject);
        $('#txt_Project').val(cdata.Project);
        $('#txt_Content').val(cdata.Content);
        $("#txt_Price").val(cdata.Price);
        var d=new Date(cdata.FinishOn);
        if(d.getYear()>=2017){
            $('#txt_FinishOn').val(formatDate(cdata.FinishOn));
        }        
        $('#sel_State').val(cdata.State);
        $('#txt_FinishType').val(cdata.FinishType);
        $('#txt_Memo').val(cdata.Memo);


        $("#list").hide("fast");       
        $("#inList").show();
    },
    ResetForm: function () {
        $('#sel_Subject').val("");
        $('#txt_Project').val("");
        $('#txt_Content').val("");
        $("#txt_Price").val("");
        $('#txt_FinishOn').val("");
        $('#sel_State').val(0);
        $('#txt_FinishType').val("");       
        $('#txt_Memo').val("");
    },
    ConformDelete: function () {
        var id = $(this).attr("data-id");
        if (confirm("确认删除吗？") == true) {
            DemandPage.DoDelete(id);
        }
    },
    DoDelete: function (id) {
        $ajax({ fn: 125, id: id, cid:4 }, function (o) {
            if (o.Return == 0) {
                DemandPage.loadList();
            }
        }, true);
    }
};


var InvestorPage = {
    UserData: null,
    CurrIndex: 0,
    CurrShowId: 0,
    init: function () {
        $("#btnSearch").on("click", InvestorPage.loadInvestor).trigger("click");
        $("#btnReset").on("click", InvestorPage.SearchBarReset);
        $("#btnSave").on("click", InvestorPage.SaveInvestor);
        $("#btnBackList").on("click", function () {
            $("#list").show();
            $("#inList").hide("fast");
        });
        $("#lnkEditAttach").on("click", InvestorPage.showAttachDlg);
    },
    showAttachDlg: function () {
        AttachPage.show(InvestorPage.EditAttachCallBack, InvestorPage.CurrShowId, 4);
    },
    loadAttach: function () {
        $ajax({ fn: 111, cid: 4, id: InvestorPage.CurrShowId }, function (o) {
            if (o.Return == 0) { InvestorPage.EditAttachCallBack(o.data); }
        }, false);
    },
    EditAttachCallBack: function (data) {
        if (data.length > 0) {
            $("#lnkEditAttach").text("共" + data.length + "个附件，点击编辑");
        }
    },
    SearchBarReset: function () {
        $("#txtName").val("");
        $("#txtContact").val("");
        $("#txtMobile").val("");
    },
    SaveInvestor: function () {
        var name = $('#txt_Name').val();
        var company = $('#txt_Company').val();
        var mobile = $('#txt_Mobile').val();
        var tel = $("#txt_Tel").val();
        var address = $('#txt_Address').val();
        var amount = $('#txt_Amount').val();
        var scope = $('#txt_Scope').val();
        var hascast = $('#txt_HasCast').val();
        var ininvest = $('#txt_InInvest').val();
        var state = $('#sel_State').val();
        var memo = $('#txt_Memo').val();
        $ajax({
            fn: 118, id: InvestorPage.CurrShowId, scope:scope, name: name, company: company, mobile: mobile, tel: tel, address: address,amount: amount, hascast: hascast, ininvest: ininvest, memo: memo, state: state
        }, InvestorPage.SaveInvestorCallback, true);
    },
    SaveInvestorCallback: function (o) {
        if (o.Return == 0) {
            showToast("保存投资人信息成功");
            InvestorPage.ResetForm();
            InvestorPage.loadInvestor();
        }
    },
    loadInvestor: function () {
        var name = $("#txtName").val();
        var contact = $("#txtContact").val();
        var mobile = $("#txtMobile").val();
        var state = $("#selState").val();
        var pageNum = 1;
        if ($(this) && !isNaN(parseInt($(this).text()))) {
            pageNum = parseInt($(this).text());
        }
        $("#MsgList").empty();
        $ajax({ fn: 117, name: name, company: contact,state:state, mobile: mobile, pageNum: pageNum }, InvestorPage.loadInvestorCallbak, true);
    },
    loadInvestorCallbak: function (o) {
        if (o.Return != 0) { return; }
        $("#list").show();
        $("#inList").hide("fast");
        var template = '<tr data-index="{1}" data-id="{0}"><td data-lnk="">{2}</td><td data-lnk="">{3}</td><td data-lnk="">{4}</td><td data-lnk="">{5}</td><td data-lnk="">{6}</td><td><a href="#" data-act="del" data-id="{-1}">删除</a></td></tr>';
        InvestorPage.UserData = o.data;
        for (var i = 0; i < o.data.length; i++) {
            var obj = o.data[i];
            var html = template.replace("{0}", obj.Id).replace("{1}", i).replace("{-1}", obj.Id).replace("{2}", obj.Name).replace("{3}", obj.Company).replace("{4}", obj.Mobile).replace("{5}", obj.Scope).replace("{6}", GetState(obj.State));
            $("#MsgList").append(html);
        }
        $("td[data-lnk]").on("click", InvestorPage.showInvestorDetail);
        $("a[data-act]").on("click", InvestorPage.ConformDelete);
        InvestorPage.CurrShowId = 0;
        if (o.data.length <= 0) {
            $("#MsgList").parents("table").hide();
        }
        else {
            $("#MsgList").parents("table").show();
        }
        //分页
        $(".pagination").empty();
        if (o.Ext.PageCount > 1) {
            for (var i = 1; i <= o.Ext.PageCount; i++) {
                if (i == o.Ext.CurrPage) {
                    $(".pagination").append('<li class="active"><a href="#">' + i + ' <span class="sr-only">(current)</span></a></li>')
                } else {
                    $(".pagination").append('<li><a href="#">' + i + '</a></li>');
                }
            }
            $(".pagination").show();
            $(".pagination li a").on("click", InvestorPage.loadInvestor);
        }
        else {
            $(".pagination").hide();
        }
    },
    showInvestorDetail: function () {
        var uid = $(this).parents("tr").attr("data-id");
        var index = $(this).parents("tr").attr("data-index");
        InvestorPage.CurrIndex = index;
        InvestorPage.CurrShowId = uid;
        var cdata = InvestorPage.UserData[index];
        $('#txt_Name').val(cdata.Name);
        $('#txt_Company').val(cdata.Company);
        $('#txt_Mobile').val(cdata.Mobile);
        $("#txt_Tel").val(cdata.Tel);
        $('#txt_Address').val(cdata.Address);
        $('#txt_Scope').val(cdata.Scope);
        $('#sel_State').val(cdata.State);
        $('#txt_Memo').val(cdata.Memo);
        $('#txt_Amount').val(cdata.Amount);
        $('#txt_HasCast').val(cdata.HasCast);
        $('#txt_InInvest').val(cdata.InInvest);
        $("#list").hide("fast");
        $("#inList").show();
        InvestorPage.loadAttach();
    },
    ResetForm: function () {
        $('#txt_Name').val("");
        $('#txt_Company').val("");
        $('#txt_Mobile').val("");
        $("#txt_Tel").val("");
        $('#txt_Address').val("");
        $('#sel_State').val(0);
        $('#txt_Memo').val("");
        $('#txt_Amount').val("");
        $('#txt_HasCast').val("");
        $('#txt_InInvest').val("");
        $('#txt_Scope').val("");
    },
    ConformDelete: function () {
        var id = $(this).attr("data-id");
        if (confirm("确认删除吗？") == true) {
            InvestorPage.DoDelete(id);
        }
    },
    DoDelete: function (id) {
        $ajax({ fn: 125, id: id, cid: 3 }, function (o) {
            if (o.Return == 0) {
                InvestorPage.loadInvestor();
            }
        }, true);
    }
};

var ProfessorPage = {
    UserData: null,
    CurrIndex: 0,
    CurrShowId: 0,
    init: function () {
        $("#btnSearch").on("click", ProfessorPage.loadProfessor).trigger("click");
        $("#btnReset").on("click", ProfessorPage.SearchBarReset);
        $("#btnSave").on("click", ProfessorPage.SaveProfessor);
        $("#btnBackList").on("click", function () {
            $("#list").show();
            $("#inList").hide("fast");
        });
        $("#lnkEditAttach").on("click", ProfessorPage.showAttachDlg);
    },
    showAttachDlg: function () {
        AttachPage.show(ProfessorPage.EditAttachCallBack, ProfessorPage.CurrShowId, 4);
    },
    loadAttach: function () {
        $ajax({ fn: 111, cid: 4, id: ProfessorPage.CurrShowId }, function (o) {
            if (o.Return == 0) { ProfessorPage.EditAttachCallBack(o.data); }
        }, false);
    },
    EditAttachCallBack: function (data) {
        if (data.length > 0) {
            $("#lnkEditAttach").text("共" + data.length + "个附件，点击编辑");
        }        
    },
    SearchBarReset: function () {
        $("#txtName").val("");
        $("#txtContact").val("");
        $("#txtMobile").val("");
    },
    SaveProfessor: function () {
        var name = $('#txt_Name').val();
        var company = $('#txt_Company').val();
        var mobile = $('#txt_Mobile').val();
        var tel = $("#txt_Tel").val();
        var address = $('#txt_Address').val();
        var core = $('#txt_Core').val();
        var paper = $('#txt_Paper').val();
        var property = $('#txt_Property').val();
        var method = $('#sel_Method').val();
        var state = $('#sel_State').val();
        var memo = $('#txt_Memo').val();
        $ajax({
            fn: 116, id: ProfessorPage.CurrShowId, name: name, company: company, mobile: mobile, tel: tel, address: address, core: core, paper: paper, property: property,method:method,memo:memo,state:state
        }, ProfessorPage.SaveProfessorCallback, true);
    },
    SaveProfessorCallback: function (o) {
        if (o.Return == 0) {
            showToast("保存专家信息成功");
            ProfessorPage.ResetForm();
            ProfessorPage.loadProfessor();
        }
    },
    loadProfessor: function () {
        var name = $("#txtName").val();
        var contact = $("#txtContact").val();
        var mobile = $("#txtMobile").val();
        var state = $("#selState").val();
        var pageNum = 1;
        if ($(this) && !isNaN(parseInt($(this).text()))) {
            pageNum = parseInt($(this).text());
        }
        $("#MsgList").empty();
        $ajax({ fn: 115, name: name, company: contact, state:state, mobile: mobile, pageNum: pageNum }, ProfessorPage.loadProfessorCallbak, true);
    },
    loadProfessorCallbak: function (o) {
        if (o.Return != 0) { return; }
        $("#list").show();
        $("#inList").hide("fast");
        var template = '<tr data-index="{1}" data-id="{0}"><td data-lnk="">{2}</td><td  data-lnk="">{3}</td><td  data-lnk="">{4}</td><td  data-lnk="">{5}</td><td  data-lnk="">{6}</td><td><a href="#" data-act="del" data-id="{-1}">删除</a></td></tr>';
        ProfessorPage.UserData = o.data;
        for (var i = 0; i < o.data.length; i++) {
            var obj = o.data[i];
            var html = template.replace("{0}", obj.Id).replace("{1}", i).replace("{-1}", obj.Id).replace("{2}", obj.Name).replace("{3}", obj.Company).replace("{4}", obj.Mobile).replace("{5}", obj.Core).replace("{6}",GetState(obj.State));
            $("#MsgList").append(html);
        }
        $("td[data-lnk]").on("click", ProfessorPage.showProfessorDetail);
        $("a[data-act]").on("click", ProfessorPage.ConformDelete);
        ProfessorPage.CurrShowId = 0;
        if (o.data.length <= 0) {
            $("#MsgList").parents("table").hide();
        }
        else {
            $("#MsgList").parents("table").show();
        }
        //分页
        $(".pagination").empty();
        if (o.Ext.PageCount > 1) {
            for (var i = 1; i <= o.Ext.PageCount; i++) {
                if (i == o.Ext.CurrPage) {
                    $(".pagination").append('<li class="active"><a href="#">' + i + ' <span class="sr-only">(current)</span></a></li>')
                } else {
                    $(".pagination").append('<li><a href="#">' + i + '</a></li>');
                }
            }
            $(".pagination").show();
            $(".pagination li a").on("click", ProfessorPage.loadProfessor);
        }
        else {
            $(".pagination").hide();
        }
    },
    showProfessorDetail: function () {
        var uid = $(this).parents("tr").attr("data-id");
        var index = $(this).parents("tr").attr("data-index");
        ProfessorPage.CurrIndex = index;
        ProfessorPage.CurrShowId = uid;
        var cdata = ProfessorPage.UserData[index];
        $('#txt_Name').val(cdata.Name);
        $('#txt_Company').val(cdata.Company);
        $('#txt_Mobile').val(cdata.Mobile);
        $("#txt_Tel").val(cdata.Tel);
        $('#txt_Address').val(cdata.Address);
        $('#txt_Core').val(cdata.Core);
        $('#txt_Paper').val(cdata.Paper);
        $('#txt_Property').val(cdata.Property);
        $('#sel_Method').val(cdata.Method);
        $('#sel_State').val(cdata.State);
        $('#txt_Memo').val(cdata.Memo);
        $("#list").hide("fast");
        $("#inList").show();
        ProfessorPage.loadAttach();
    },
    ResetForm: function () {
        $('#txt_Name').val("");
        $('#txt_Company').val("");
        $('#txt_Mobile').val("");
        $("#txt_Tel").val("");
        $('#txt_Address').val("");
        $('#txt_Core').val("");
        $('#txt_Paper').val("");
        $('#txt_Property').val("");
        $('#sel_Method').val(0);
        $('#sel_State').val(0);
        $('#txt_Memo').val("");
    },
    ConformDelete: function () {
        var id = $(this).attr("data-id");
        if (confirm("确认删除吗？") == true) {
            ProfessorPage.DoDelete(id);
        }
    },
    DoDelete: function (id) {
        $ajax({ fn: 125, id: id, cid: 2 }, function (o) {
            if (o.Return == 0) {
                ProfessorPage.loadProfessor();
            }
        }, true);
    }
};

var PolicyPage = {
    UserData: null,
    CurrIndex: 0,
    CurrShowId: 0,
    init: function () {
        initDatePicker();
        $("#btnNew").on("click", PolicyPage.AddNewPolicy);
        $("#btnSearch").on("click", PolicyPage.loadPolicy).trigger("click");
        $("#btnReset").on("click", PolicyPage.SearchBarReset);
        $("#btnSave").on("click", PolicyPage.SavePolicy);
        $("#btnBackList").on("click", function () {
            $("#list").show();
            $("#inList").hide("fast");
        });
        $("#lnkEditAttach").on("click", PolicyPage.showAttachDlg);
    },
    showAttachDlg: function () {
        AttachPage.show(PolicyPage.EditAttachCallBack,PolicyPage.CurrShowId,4);
    },
    loadAttach: function () {
        $ajax({ fn: 111, cid: 4, id: PolicyPage.CurrShowId }, function (o) {
            if (o.Return == 0) { PolicyPage.EditAttachCallBack(o.data);}
        }, false);
    },
    EditAttachCallBack: function (data) {
        $("#lnkEditAttach").text("共" + data.length + "个附件，点击编辑");
    },
    SearchBarReset: function () {
        $("#txtName").val("");
        $("#txtContact").val("");
    },
    AddNewPolicy: function () {
        PolicyPage.CurrIndex = -1;
        PolicyPage.CurrShowId = 0;
        $("#list").hide("fast");
        $("#inList").show();
    },
    SavePolicy: function () {
        var title = $('#txt_Title').val();
        var desp = $('#txt_Desp').val();
        var keyword = $('#txt_KeyWord').val();
        var from = $("#txt_From").val();
        var postOn = $('#txt_PostOn').val();
        var disableOn = $('#txt_DisableOn').val();
        var target = $('#txt_Target').val();
        var content = $('#txt_Content').val();       
        $ajax({
            fn: 109, id: PolicyPage.CurrShowId, title:title,desp:desp,keyword:keyword,from:from,postOn:postOn,disableOn:disableOn,target:target,content:content
        }, PolicyPage.SavePolicyCallback, true);
    },
    SavePolicyCallback: function (o) {       
        if (o.Return == 0) {
            showToast("保存政策成功");
            PolicyPage.ResetForm();
            PolicyPage.loadPolicy();
        }        
    },
    loadPolicy: function () {
        var name = $("#txtName").val();
        var contact = $("#txtContact").val();
        var pageNum = 1;
        if ($(this) && !isNaN(parseInt($(this).text()))) {
            pageNum = parseInt($(this).text());
        }
        $("#MsgList").empty();
        $ajax({ fn: 108, title: name, keyword: contact, pageNum: pageNum }, PolicyPage.loadPolicyCallbak, true);
    },
    loadPolicyCallbak: function (o) {
        if (o.Return != 0) { return; }
        $("#list").show();
        $("#inList").hide("fast");
        var template = '<tr data-index="{1}" data-id="{0}"><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>';
        PolicyPage.UserData = o.data;
        for (var i = 0; i < o.data.length; i++) {
            var obj = o.data[i];
            var html = template.replace("{0}", obj.Id).replace("{1}", i).replace("{2}", obj.Title).replace("{3}", obj.Label).replace("{4}", formatDate(obj.PostOn)).replace("{5}", formatDate(obj.DisableOn));
            $("#MsgList").append(html);
        }
        $("#MsgList tr").on("click", PolicyPage.showPolicyDetail);
        PolicyPage.CurrShowId = 0;
        if (o.data.length <= 0) {
            $("#MsgList").parents("table").hide();
        }
        else {
            $("#MsgList").parents("table").show();
        }
        //分页
        $(".pagination").empty();
        if (o.Ext.PageCount > 1) {
            for (var i = 1; i <= o.Ext.PageCount; i++) {
                if (i == o.Ext.CurrPage) {
                    $(".pagination").append('<li class="active"><a href="#">' + i + ' <span class="sr-only">(current)</span></a></li>')
                } else {
                    $(".pagination").append('<li><a href="#">' + i + '</a></li>');
                }
            }
            $(".pagination").show();
            $(".pagination li a").on("click", PolicyPage.loadPolicy);
        }
        else {
            $(".pagination").hide();
        }
    },
    showPolicyDetail: function () {
        var uid = $(this).attr("data-id");
        var index = $(this).attr("data-index");
        PolicyPage.CurrIndex = index;     
        PolicyPage.CurrShowId = uid;
        var cdata = PolicyPage.UserData[index];
        $("#txt_Title").val(cdata.Title);
        $("#txt_Desp").val(cdata.Desp);
        $("#txt_KeyWord").val(cdata.Label);
        $("#txt_From").val(cdata.From);
        $("#txt_PostOn").val(formatDate(cdata.PostOn));
        $("#txt_DisableOn").val(formatDate(cdata.DisableOn));
        $("#txt_Target").val(cdata.Target);
        if (cdata.Content == "") {
            PolicyPage.GetContent();
        }
        else {
            $("#txt_Content").val(cdata.Content);           
        }
        $("#list").hide("fast");
        $("#inList").show();
        PolicyPage.loadAttach();
    },
    ResetForm: function () {
        $("#txt_Title").val("");
        $("#txt_Desp").val("");
        $("#txt_KeyWord").val("");
        $("#txt_From").val("");
        $("#txt_PostOn").val("");
        $("#txt_DisableOn").val("");
        $("#txt_Target").val("");
        $("#txt_Content").val("");       
    },
    GetContent: function () {
        $ajax({ fn: 114, Id: PolicyPage.CurrShowId }, function (o) {
            $("#txt_Content").val(o.data);
            PolicyPage.UserData[PolicyPage.CurrIndex].Content = o.data;
        }, true);
    }
};

var EnterprisePage = {
    UserData: null,   
    CurrIndex: 0,
    CurrShowId: 0, 
    init: function () {
        initCheckRadio();
        $("#btnSearch").on("click", EnterprisePage.LoadEnterpriseList).trigger("click");
        $("#btnReset").on("click", EnterprisePage.SearchBarReset);        
    },
    showAttachDlg: function () {
        AttachPage.show(EnterprisePage.EditAttachCallBack, EnterprisePage.CurrShowId, 1);
    },
    loadAttach: function () {
        $ajax({ fn: 111, cid: 1, id: EnterprisePage.CurrShowId }, function (o) {
            if (o.Return == 0) { EnterprisePage.EditAttachCallBack(o.data); }
        }, false);
    },
    EditAttachCallBack: function (data) {
        $("#lnkEditAttach").text("共" + data.length + "个附件，点击编辑");
    },
    SearchBarReset: function () {
        $("#txtName").val("");
        $("#txtContact").val("");        
        $("#txtMobile").val("");
    },
    LoadEnterpriseList: function () {
        var name = $("#txtName").val();
        var mobile = $("#txtMobile").val();
        var contact = $("#txtContact").val();
        var state = $("#selState").val();
        var pageNum = 1;
        if ($(this) && !isNaN(parseInt($(this).text()))) {
            pageNum = parseInt($(this).text());
        }
        $("#MsgList").empty();
        $ajax({ fn: 106, name: name, mobile: mobile, state:state, contact:contact, pageNum: pageNum }, EnterprisePage.LoadEnterpriseListCallBack, true);
    },
    LoadEnterpriseListCallBack: function (o) {
        if (o.Return != 0) { return; }
        $("#list").show();
        $("#inList").hide("fast");
        var template = '<tr data-index="{1}" data-id="{0}"><td  data-lnk="">{2}</td><td  data-lnk="">{3}</td><td  data-lnk="">{4}</td><td  data-lnk="">{5}</td><td  data-lnk="">{6}</td><td  data-lnk="">{7}</td><td><a href="#" data-act="del" data-id="{-1}">删除</a></td></tr>';
        EnterprisePage.UserData = o.data;
        for (var i = 0; i < o.data.length; i++) {
            var obj = o.data[i];
            var html = template.replace("{0}", obj.Id).replace("{-1}", obj.Id).replace("{1}", i).replace("{2}", obj.Name).replace("{3}", obj.Contact).replace("{4}", obj.Mobile).replace("{5}", obj.Address).replace("{6}", obj.Project).replace("{7}", GetState(obj.State));
            $("#MsgList").append(html);
        }
        $("td[data-lnk]").on("click", EnterprisePage.showEnterpriseDetail);
        $("a[data-act]").on("click", EnterprisePage.ConformDelete);
       
        EnterprisePage.CurrShowId = 0;
        if (o.data.length <= 0) {
            $("#MsgList").parents("table").hide();
        }
        else {
            $("#MsgList").parents("table").show();
        }
        //分页
        $(".pagination").empty();
        if (o.Ext.PageCount > 1) {
            for (var i = 1; i <= o.Ext.PageCount; i++) {
                if (i == o.Ext.CurrPage) {
                    $(".pagination").append('<li class="active"><a href="#">' + i + ' <span class="sr-only">(current)</span></a></li>')
                } else {
                    $(".pagination").append('<li><a href="#">' + i + '</a></li>');
                }
            }
            $(".pagination").show();
            $(".pagination li a").on("click", EnterprisePage.LoadEnterpriseList);
        }
        else {
            $(".pagination").hide();
        }
    },
    showEnterpriseDetail: function () {
        var uid = $(this).parents("tr").attr("data-id");
        var index = $(this).parents("tr").attr("data-index");
        CurrIndex = index;
        //载入数据       
        EnterprisePage.CurrShowId = uid;
        var cdata = EnterprisePage.UserData[index];
        var html = '<div class="row" style="margin:0 0 2rem 1rem;font-weight:bold;font-size:2.4rem;">' + cdata.Name + '</div>'
        html += '<form class="form-horizontal" style="margin-bottom:4rem;">'
        html += '<div class="form-group"><label class="col-sm-2 control-label"><em style="color:red;font-style:normal;">*</em> 企业名称</label><div class="col-sm-8"><input type="text" class="form-control" value="' + cdata.Name + '" id="txt_Name" placeHolder="填写企业名称" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label"><em style="color:red;font-style:normal;">*</em> 负责人</label><div class="col-sm-5"><input type="text" class="form-control" value="' + cdata.Contact + '" id="txt_Contact" placeHolder="负责人姓名" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label"><em style="color:red;font-style:normal;">*</em> 电话</label><div class="col-sm-5"><input type="text" class="form-control" value="' + cdata.Mobile + '" id="txt_Mobile" placeHolder="联系电话" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">备用电话</label><div class="col-sm-5"><input type="text" class="form-control" value="' + cdata.Tel + '" id="txt_Tel" placeHolder="备用电话" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">经营地址</label><div class="col-sm-8"><input type="text" class="form-control" value="' + cdata.Address + '" id="txt_Address" placeHolder="企业经营地址" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">核心业务</label><div class="col-sm-8"><input type="text" class="form-control" value="' + cdata.Core + '" id="txt_Core" placeHolder="核心业务" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">业务范围</label><div class="col-sm-8"><input type="text" class="form-control" value="' + cdata.Scope + '" id="txt_Scope" placeHolder="业务范围" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">上年度业务收入</label><div class="col-sm-5"><input type="number" class="form-control" value="' + cdata.Income + '" id="txt_Income" placeHolder="单位：万元" /></div><div class="col-sm-1">万元</div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">近期的研发项目</label><div class="col-sm-8"><input type="text" class="form-control" value="' + cdata.Project + '" id="txt_Project" placeHolder="近期的研发项目" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label"><em style="color:red;font-style:normal;">*</em> 最关心的服务领域</label><div class="col-sm-8"><input type="text" class="form-control" value="' + cdata.Range + '" id="txt_Range" placeHolder="最关心的服务领域" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">企业人数</label><div class="col-sm-8"><input type="number" class="form-control" value="' + cdata.Num + '" id="txt_Num" placeHolder="企业人数" /></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">状态</label><div class="col-sm-8"><select class="form-control" id="sel_State">'
        html += '<option value="0" '+ (cdata.State == 0 ? "selected" : "") +'>未审核</option>'
        html += '<option value="1"' + (cdata.State == 1 ? "selected" : "") + '>已审</option>'
        html += '<option value="2"' + (cdata.State == 2 ? "selected" : "") + '>拒绝</option>'
        html += '<option value="9"' + (cdata.State == 9 ? "selected" : "") + '>删除</option>'
        html += '</select></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">附件</label><div class="col-sm-10"><span id="lnkEditAttach" style="color:#5cb85c;font-weight:600;">点击编辑附件</span></div></div>'
        html += '<div class="form-group"><label class="col-sm-2 control-label">备注</label><div class="col-sm-8"><textarea id="txt_Memo" class="form-control" placeHolder="其它备注信息">' + (cdata.Memo != null ? cdata.Memo : "") + '</textarea></div></div>'
        html += '<div class="col-xs-2"></div><div style="padding-top:1rem;" class="col-xs-10"><button id="btnSave" type="button" class="btn btn-dropbox">保  存</button>&nbsp;<button id="btnBackList"  type="button" class="btn btn-google">返  回</button></div>'
        html +='</form>'       
        $("#inList .content").empty().append(html);       
        $("#MsgList2").empty();
        $("#list").hide("fast");
        $("#inList").show();
        $("#btnBackList").on("click", function () {
            $("#list").show();
            $("#inList").hide("fast");
        });
        EnterprisePage.loadAttach();
        $("#btnSave").unbind().on("click", EnterprisePage.SaveEdit);
        $("#lnkEditAttach").unbind().on("click", EnterprisePage.showAttachDlg);
    },
    SaveEdit: function () {
        var Name = $("#txt_Name").val();
        var Contact = $("#txt_Contact").val();
        var Mobile = $("#txt_Mobile").val();
        var Range = $("#txt_Range").val();
        if (!val.Required(Name)) {
            showToast("企业名称不可为空");
            return;
        }
        if (!val.Required(Contact)) {
            showToast("联系人不可为空");
            return;
        }
        if (!val.CheckMobile(Mobile)) {
            showToast("电话格式不正确");
            return;
        }
        if (!val.Required(Range)) {
            showToast("最关心的服务领域不可为空");
            return;
        }
        var Tel = $("#txt_Tel").val();
        var Address = $("#txt_Address").val();
        var Core = $("#txt_Core").val();
        var Scope = $("#txt_Scope").val();
        var Income = $("#txt_Income").val();
        var Project = $("#txt_Project").val();
        var Num = $("#txt_Num").val();
        var State = $("#sel_State").val();
        var Memo = $("#txt_Memo").val();
        var Id = EnterprisePage.CurrShowId;
        $ajax({ fn: 107, Id: Id, Name: Name, Contact: Contact, Mobile: Mobile, Tel: Tel, Address: Address, Core: Core, Scope: Scope, Income: Income, Project: Project, Range: Range, Num: Num, State: State, Memo: Memo }, function (o) {
            if (o.Return == 0) {
                EnterprisePage.ResetForm();
                EnterprisePage.LoadEnterpriseList();
            } else {
                showToast("保存企业信息出错,请稍候再试")
            }
        }, true);
    }, ResetForm: function () {
        $("#txt_Name").val("");
        $("#txt_Contact").val("");
        $("#txt_Mobile").val("");
        $("#txt_Range").val("");
        $("#txt_Tel").val("");
        $("#txt_Address").val("");
        $("#txt_Core").val("");
        $("#txt_Scope").val("");
        $("#txt_Income").val("");
        $("#txt_Project").val("");
        $("#txt_Num").val("");
        $("#sel_State").val("0");
        $("#txt_Memo").val("");
    },
    ConformDelete: function () {
        var id = $(this).attr("data-id");
        if (confirm("确认删除吗？") == true) {
            EnterprisePage.DoDelete(id);
        }
    },
    DoDelete: function (id) {
        $ajax({ fn: 125, id: id,cid:1 }, function (o) {
            if (o.Return == 0) {
                EnterprisePage.LoadEnterpriseList();
            }
        }, true);
    }
};

var MemberPage = {
    UserData: null,
    InsuredData:null,
    CurrIndex: 0,
    CurrShowId: 0,
    CurrInId:0,
    init: function () {
        initCheckRadio();
        $("#btnSearch").on("click", MemberPage.LoadMemberList).trigger("click");
        $("#btnReset").on("click", MemberPage.SearchBarReset);
    },
    SearchBarReset: function () {
        $("#txtName").val("");
        $("#txtMobile").val("");
    },
    LoadMemberList: function () {
        var name = $("#txtName").val();
        var mobile = $("#txtMobile").val();

        var pageNum = 1;
        if ($(this) && !isNaN(parseInt($(this).text()))) {
            pageNum = parseInt($(this).text());
        }
        $("#MsgList").empty();
        $ajax({ fn: 101, name: name, mobile: mobile, pageNum: pageNum }, MemberPage.loadMemberListCallBack, true);
    },
    loadMemberListCallBack: function (o) {
        if (o.Return != 0) { return; }
        var template = '<tr data-index="{6}" data-id="{0}"><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{7}</td></tr>';
        MemberPage.UserData = o.data;
        for (var i = 0; i < o.data.length; i++) {
            var obj = o.data[i];
            var html = template.replace("{0}", obj.Id).replace("{1}", "<img src='" + obj.PhotoUrl + "' style='width:2.5rem;' class='img-circle'/>").replace("{2}", obj.NickName).replace("{3}", obj.Name).replace("{4}", obj.Mobile).replace("{5}", obj.Area).replace("{6}", i).replace("{7}", GetMemberType(obj.Category));
            $("#MsgList").append(html);
        }
        $("#MsgList tr").on("click", MemberPage.showMemberDetail);
        MemberPage.CurrShowId = 0;
        if (o.data.length <= 0) {
            $("#MsgList").parents("table").hide();
        }
        else {
            $("#MsgList").parents("table").show();
        }
        //分页
        $(".pagination").empty();
        if (o.Ext.PageCount > 1) {
            for (var i = 1; i <= o.Ext.PageCount; i++) {
                if (i == o.Ext.CurrPage) {
                    $(".pagination").append('<li class="active"><a href="#">' + i + ' <span class="sr-only">(current)</span></a></li>')
                } else {
                    $(".pagination").append('<li><a href="#">' + i + '</a></li>');
                }
            }
            $(".pagination").show();
            $(".pagination li a").on("click", MemberPage.LoadMemberList);
        }
        else {
            $(".pagination").hide();
        }
    },
    showMemberDetail: function () {
        var uid = $(this).attr("data-id");
        var index = $(this).attr("data-index");
        MemberPage.CurrIndex = index;
        //载入数据       
        MemberPage.CurrShowId = uid;
        var cdata = MemberPage.UserData[index];
        var html = '<div class="row"><div class="col-xs-1" style="text-align:center;"><img src="' + cdata.PhotoUrl + '" style="width:3rem;" class="img-circle" /></div><div class="col-xs-3"><span style="margin:0;font-weight:bold;font-size:2.4rem;">' + cdata.Name + '</span>&nbsp;'
        if (cdata.Gender == 1) {
            html += '<i class="fa fa-user" style="color:#033898;font-size:1.8rem;"></i>'
        } else {
            html += '<i class="fa fa-user" style="color:#f70767;font-size:1.8rem;"></i>'
        }
        html += "</div><div class='col-xs-4'><button type='button' id='btnBackList' class='btn btn-google'>返回列表</button>&nbsp;</div></div>";
        html += '<div class="row" style="padding-top:2rem;padding-left:2rem;line-height:2rem;">'
        html += '<div class="col-xs-12">类别：' + GetMemberType(cdata.Category) + '</div>'
        html += '<div class="col-xs-12">姓名：' + cdata.Name + '</div>'
        html += '<div class="col-xs-12">昵称：' + cdata.NickName + '</div>'
        html += '<div class="col-xs-12">手机：' + cdata.Mobile + '</div>'
        html += '<div class="col-xs-12">区域：' + cdata.Area + '</div>'
        html += '<div class="col-xs-12">备注：' + (cdata.Memo != null ? cdata.Memo : "--") + '</div>'
        //会员信息
        $("#inList .content-header .row").empty().append(html);        
        $("#list").hide("fast");
        $("#inList").show();
        $("#btnBackList").on("click", function () {
            $("#list").show();
            $("#inList").hide("fast");
        });       
    }
};

function initCheckRadio() {
    $('input[type="radio"],input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
}

function setDialog(msg, okHandler) {
    $("#warningDlg .modal-dialog .modal-content .modal-body p").text(msg);
    $("#btnWarningdlgOk").unbind().on("click", function () { okHandler(), $("#warningDlg").modal("hide"); });
    $("#warningDlg").modal("show");
}

function initDatePicker() {
    $('.input-group.date').datepicker({
        format: 'yyyy-mm-dd',
        startDate: '2000-01-01',
        autoclose: true,
        endDate: '2049-12-31',
        language: 'zh-CN'
    });
}

function GetMemberType(n)
{
    if (n == 1) { return "企业"; }
    if (n == 2) { return "专家"; }
    if (n == 3) { return "投资人"; }
    if (n == 4) { return "创新创业"; }
    return "未知";
}

function GetState(n) {
    if (n == 0) { return "未审核"; }
    if (n == 1) { return "正常"; }
    if (n == 2) { return "拒绝"; }
    if (n == 9) { return "删除"; }
}

function GetFileType(file) {
    var extName = "";
    var f = file.split('.');
    if (f.length > 1) {
        extName = f[f.length - 1].toLowerCase();
    }
    if (extName == "jpg" || extName == "jpeg" || extName == "png" || extName == "gif" || extName == "bmp" || extName == "psd" || extName == "ai") {
        return 1;
    }
    if (extName == "mp3" || extName == "wav" || extName == "rm" || extName == "amr") {
        return 2;
    }
    if (extName == "mp4" || extName == "wmv" || extName == "rmvb" || extName == "flv" || extName == "swf" || extName == "kvw") {
        return 3;
    }
    if (extName == "doc" || extName == "docx") {
        return 4;
    }
    if (extName == "ppt" || extName == "pptx") {
        return 5;
    }
    if (extName == "xls" || extName == "xlsx") {
        return 6;
    }
    if (extName == "pdf") {
        return 7;
    }
    return 0;
}

function GetIcon(typeCode) {   
    if (typeCode==1) {
        return "jpg.jpg";
    }
    if (typeCode == 2) {
        return "mp3.jpg";
    }
    if (typeCode == 3) {
        return "mp4.jpg";
    }
    if (typeCode == 4) {
        return "doc.jpg";
    }
    if (typeCode == 5) {
        return "ppt.jpg";
    }
    if (typeCode == 6) {
        return "xls.jpg";
    }
    if (typeCode == 7) {
        return "pdf.jpg";
    }
    return "def.jpg";
}

$(document).ready(function () {
    Mng.init();
});

var FileInput = function () {
    var oFile = new Object();
    //初始化fileinput控件（第一次初始化）
    oFile.Init = function (ctrlName, uploadUrl) {
        var control = $('#' + ctrlName);
        //初始化上传控件的样式
        control.fileinput({
            language: 'zh', //设置语言
            uploadUrl: uploadUrl, //上传的地址
            allowedFileExtensions: ['jpg','jpeg', 'gif', 'png', 'pdf','bmp','xls','xlsx','doc','docx','ppt','pptx','mp3','mp4','wmv','wav','avi','rm','rmvb','flv','fla','swf','txt'],//接收的文件后缀
            showUpload: true, //是否显示上传按钮
            showCaption: true,//是否显示标题
            browseClass: "btn btn-primary", //按钮样式 
            dropZoneEnabled: false,//是否显示拖拽区域
            //minImageWidth: 50, //图片的最小宽度
            //minImageHeight: 50,//图片的最小高度
            //maxImageWidth: 1000,//图片的最大宽度
            //maxImageHeight: 1000,//图片的最大高度
            //maxFileSize: 0,//单位为kb，如果为0表示不限制文件大小
            //minFileCount: 0,
            maxFileCount: 10, //表示允许同时上传的最大文件个数
            enctype: 'multipart/form-data',
            //validateInitialCount:true,
            previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
            msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！",
        }).on("fileuploaded", function (event, data, previewId, index) {
            $("#" + previewId).attr("data-url", data.response.url);
            $("#" + previewId + " .file-thumbnail-footer .file-footer-caption").text(data.files[index].name);
            $(".file-caption-name").text(data.files[index].name + " 上传完毕");
            isUpload = false;
        }).on("fileclear", function () {
            $("#picUrl").val("");
        }).on("filereset", function () {
            $("#picUrl").val("");
        }).on("filedeleted", function (event, key) {
            console.log('Key = ' + key);
        }).on('filesuccessremove', function (event, id) {
            console.log('Uploaded thumbnail successfully removed(' + id + ')');
        });
    }
    return oFile;
};