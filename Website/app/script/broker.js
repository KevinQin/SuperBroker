var SSJ = {};
var ajaxServerUrl = "/service/app.ashx";
var PageName = "";
var BASE_URL = "http://b.seascapeapp.cn/";
var debug_openId = "o3MRawEmK5OK-ringNnTyiNPA6uM";
var storage = window.localStorage;
var cookie_before = "SupeBrokerV1_";

SSJ.init = function () {
    var pageUrl = window.location.href;
    var pageItems = pageUrl.split("/");
    PageName = pageItems[pageItems.length - 1].split('.')[0].toLowerCase();
    if (PageName === "login") {
        LoginPage.init();        
    }
    else if (PageName == "register") {
        RegisterPage.init();
    }   
    else if (PageName == "uc") {
        addTabBar();
    }
    else if (PageName == "list") {
       
    }
    else if (PageName == "detail") {
        
    }
    else if (PageName == "fee") {

    }
    else if (PageName == "report") {

    }
    else if (PageName == "history") {

    }
    else if (PageName == "notify") {
 
    }    
}

var LoginPage = {
    init: function () {
        var code = getParam("code");
        var openid = $get('openid');
        
        if (code != "" && (openid == null || openid == "")) {
            getOpenId(code, LoginPage.getOpenId_cb);
        }
        else {
            LoginPage.setAccount();
        }
        $("#btnLogin").on("click", LoginPage.doLogin);
        $("input").on("focus", RegisterPage.inputFocus);
    },
    getOpenId_cb: function () {
        getMemberInfo();
        LoginPage.setAccount();
    },
    setAccount: function () {
        var account = $get('account');
        var mobile = $get('moble');
        if (( account==null || account == "") && mobile != "") {
            account = mobile;
        }
        $("#txtAccount").val(account);
    },
    doLogin: function () {
        weui.form.validate('#frmLogin', function (error) {
            if (!error) {
                var account = $("#txtAccount").val();
                var Password = $("#txtPwd").val();               
                var openid = $get('openid');               
                $ajax({ fn: 102, account: account, pwd: Password, openid: openid }, LoginPage.login_cb, true);
            }
            //当return true时，不会显示错误
        });
    },
    login_cb: function (o) {
        if (o.Return == 0) {
            $set('account', $("#txtAccount").val());
            weui.toast('登录成功', {
                duration: 1500,
                className: 'custom-classname',
                callback: function () { $go("uc.aspx"); }
            });
        }
        else {
            weui.toast("登录失败["+ o.SubCode +"]", 3000);
        }
    }
};

var RegisterPage = {
    init: function () {
        var code = getParam("code");
        var openid = $get('openid');
        if (code != "" && (openid == null || openid == "")) {
            getOpenId(code, RegisterPage.getOpenId_cb);
        }
        $("#btnRegister").on("click", RegisterPage.doRegister);
        $("input").on("focus", RegisterPage.inputFocus);
    },
    getOpenId_cb: function () {
        getMemberInfo();
    },
    inputFocus: function () {
        $(this).parents(".weui-cell_warn").removeClass("weui-cell_warn");
    },
    doRegister: function () {      
        weui.form.validate('#frmReg', function (error) {
            if (!error) {
                var name = $("#txtName").val();
                var mobile = $("#txtMobile").val();
                var Password = $("#txtPwd").val();
                var RePwd = $("#txtRePwd").val();
                var openid = $get('openid');
                if (Password != RePwd) {
                    $("#txtRePwd").val("");
                    $("#txtRePwd").parents(".weui-cell").addClass("weui-cell_warn");
                    weui.topTips('两次密码不一致，请重新填写', 3000);
                    return;
                }
                $ajax({ fn: 101, name: name, mobile: mobile, pwd: Password, openid:openid }, RegisterPage.register_cb, true);
            }            
            //当return true时，不会显示错误
        });
    },
    register_cb: function (o) {
        if (o.Return == 0) {
            $set('mobile', o.data.Mobile);
            weui.toast('注册成功', {
                duration: 3000,
                className: 'custom-classname',
                callback: function () { $go("uc.aspx"); }
            });
        }
        else {
            weui.toast(o.Msg, 3000);
        }
    }
};

$(document).ready(function () {
    SSJ.init();
});

function addTabBar() {
    var html = '<div class="weui-tabbar">';
    html += '<a href="/app/builder/list.aspx" class="weui-tabbar__item"><i class="icon iconfont icon-wxbzhuye weui-tabbar__icon"></i><p class="weui-tabbar__label">楼盘</p></a >';
    html += '<a href="/app/broker/history.aspx" class="weui-tabbar__item"><i class="icon iconfont icon-all weui-tabbar__icon"></i><p class="weui-tabbar__label">历史</p></a>';
    html += '<a href="/app/broker/fee.aspx" class="weui-tabbar__item"><span style="display: inline-block;position: relative;"><i class="icon iconfont icon-cart weui-tabbar__icon"></i><span class="weui-badge" style="display:none; position: absolute;top: -2px;right: -13px;">0</span></span><p class="weui-tabbar__label">佣金</p></a>';
    html += '<a href="/app/broker/uc.aspx" class="weui-tabbar__item"><i class="icon iconfont icon-account weui-tabbar__icon"></i><p class="weui-tabbar__label">我的</p></a></div>';
    $(".weui-tab").append(html);
    if (PageName == "list") { $(".weui-tabbar__item:eq(0)").addClass("weui-bar__item_on"); }
    if (PageName == "history") { $(".weui-tabbar__item:eq(1)").addClass("weui-bar__item_on"); }
    if (PageName == "fee") { $(".weui-tabbar__item:eq(2)").addClass("weui-bar__item_on"); }
    if (PageName == "uc") { $(".weui-tabbar__item:eq(3)").addClass("weui-bar__item_on"); }
}

var controlId = -1;
function controlLayerSwitch() {
    window.onpopstate = function () {
        if (controlId > 1000) {
            if (controlId == 1001) {
                _DRP.hide();                
            } else if (controlId == 1002) {
                CityPiacker.hide();
            } else if (controlId == 1003 || controlId == 1004) {
                $(".hotel-keyword-picker").hide();           
            }
            $(".page").show();
        }
    }
}

function fmtPrice(price) {
    if (price.toString().indexOf('.') > -1) {
        return price.toFixed(2).toString();
    }
    else {
        return price.toString();
    }
}

function ConvertShowState(n) {
    var result = "";
    switch (n) {
        case 0:
            result = "待付款";
            break;
        case 1:
            result = "待发货";
            break;
        case 2:
            result = "待收货";
            break;
        case 3:
            result = "待评价";
            break;
        case 9:
            result = "已退货";
            break;               
        default:
            result = "处理中";
            break;
    }
    return result;
}

function getOpenId(code, callback) {
    if ($isDebug) {
        $set('openid', debug_openId, cookie_obj);
        callback();
    }
    else {
        var data = { fn: 1, code: code, source: request("state") };
        $ajax(data, function (o) {
            if (o.Return == 1) {
                return;
            }
            else {
                $set('openid', o.Msg);
                callback();
            }
        }, false);
    }
}

function getMemberInfo() {
    if ($get('userid') == null) {
        $ajax({ fn: 2, openid: $get('openid'), code: request("code"), source: request("state") }, getMemberInfoCallBack, false);
    }
}

function getMemberInfoCallBack(o) {
    if (o.Return == 0) {
        var user = o.Info;
        var express = o.eInfo;
        if (user != null) {
            $set('userid', user.id);
            $set('photourl', user.photoUrl);
            $set('mobile', user.mobile);
            $set('sex', user.sex);
        }
    }
}

