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
    if (PageName === "checkbroker") {
        CheckBroker.init();
    }
    else if (PageName == "workeruc") {
        UcPage.init();
        addTabBar();
    }
};

var UcPage = {
    init: function () {
        var photoUrl = $get('photourl');
        if (photoUrl != null && photoUrl != "") {
            $("img").attr("src", photoUrl);
        }        
        $("a.weui-cell_access:last").on("click", function () {
            $ClearSet();
            weui.toast("成功退出", function () {
                $go("../broker/login.aspx");
            });
        });
        UcPage.LoadWorker();
    },
    LoadWorker: function () {
        $ajax({ fn: 203, openid: $get("openid"), workno: $get("account") }, UcPage.LoadWorker_cb, true);
    },
    LoadWorker_cb: function (o) {
        if (o.Return == 0) {
            $("img").attr("src", o.data.PhotoUrl);            
            $("h5").text(o.data.Mobile +" "+o.data.RoleName);
            if (o.data.Enable) {
                $("h4").text(o.data.Name + " " + o.data.WorkNo);
            }
            else {                
                $("h4").text(o.data.Name + " 已禁用");
            }

        }
    }
};

var CheckBroker = {
    init: function () {
        CheckBroker.loadInfo();
        $(".page__bd").hide();
        $("#btnOk").on("click", function () { CheckBroker.docheckBroker(1); });
        $("#btnCancel").on("click", function () { CheckBroker.docheckBroker(2); });
    },
    loadInfo: function () {
        $ajax({ fn: 201, bno: request("bno"), wno: request("wno"), state: request("state") }, CheckBroker.loadInfo_cb, true);
    },
    loadInfo_cb: function (o) {
        if (o.Return == 0) {
            $(".page__bd").show();
            $("#txtName").text("姓名：" + o.data.Name);
            $("#txtMobile").text("姓名：" + o.data.Mobile);
            $("#txtWorkNo").text("工号：" + o.data.WorkNo);
        }
        else {
            weui.dialog({
                title: "提示", content: o.Msg, buttons: [{
                    label: "确定", type: "default", onClick: function () {
                        $go("brokerlist.aspx");
                    }
                }]
            });            
        }
    },
    docheckBroker: function (n) {
        $ajax({ fn: 202, id: request("state"),info:$("#txtMemo").val(), bno: request("bno"), wno: request("wno"), state: n }, CheckBroker.docheckBroker_cb, true);
    },
    docheckBroker_cb: function (o) {
        if (o.Return == 0) {
            weui.toast("审核完成", function () { $go("brokerlist.aspx");})
        }
        else {
            weui.dialog({ title: '提示', content: o.Msg });
        }
    }
};


$(document).ready(function () {
    SSJ.init();
});

function addTabBar() {
    var html = '<div class="weui-tabbar">';
    var RoldId = $get("RoleId");
    if (RoldId == 100) {
        html += '<a href="/app/worker/brokerList.aspx" class="weui-tabbar__item"><i class="iconfont icon-jingjiren1 weui-tabbar__icon"></i><p class="weui-tabbar__label">经纪人</p></a >';
        html += '<a href="/app/report/datacenter.aspx" class="weui-tabbar__item"><i class="iconfont icon-wxbbaobiao weui-tabbar__icon"></i><p class="weui-tabbar__label">数据中心</p></a>';
        
    } else if (RoleId == 101) {
        html += '<a href="/app/worker/CheckInBuilder.aspx" class="weui-tabbar__item"><i class="iconfont icon-erweima weui-tabbar__icon"></i><p class="weui-tabbar__label">到场验证</p></a>';
        html += '<a href="/app/worker/CheckBuilderHistory.aspx" class="weui-tabbar__item"><i class="iconfont icon-shijian weui-tabbar__icon"></i><p class="weui-tabbar__label">验证历史</p></a>';
    } else if (RoleId == 102) {
        html += '<a href="/app/worker/NeedPayFee.aspx" class="weui-tabbar__item"><i class="iconfont icon-wodehongbao weui-tabbar__icon"></i><p class="weui-tabbar__label">待返佣</p></a>';
        html += '<a href="/app/worker/PayFeeHistory.aspx" class="weui-tabbar__item"><i class="iconfont icon-shijian weui-tabbar__icon"></i><p class="weui-tabbar__label">返佣历史</p></a>';
    } else if (RoleId == 103) {
        html += '<a href="/app/builder/list.aspx" class="weui-tabbar__item"><i class="iconfont icon-jiudian weui-tabbar__icon"></i><p class="weui-tabbar__label">楼盘</p></a >';
    } else if (RoleId == 104) {
        html += '<a href="/app/worker/brokerList.aspx" class="weui-tabbar__item"><i class="iconfont icon-jingjiren1 weui-tabbar__icon"></i><p class="weui-tabbar__label">经纪人</p></a>';
    }   
    html += '<a href="/app/worker/workeruc.aspx" class="weui-tabbar__item"><i class="iconfont icon-account weui-tabbar__icon"></i><p class="weui-tabbar__label">我的</p></a></div>';
    $(".weui-tab:first").append(html);
    if (PageName == "brokerlist" || PageName == "checkinbuilder" || PageName == "needpayfee" || PageName=="list") { $(".weui-tabbar__item:first").addClass("weui-bar__item_on"); }
    if (PageName == "datacenter" || PageName == "checkbuilderhistory" || PageName == "payfeehistory") { $(".weui-tabbar__item:eq(1)").addClass("weui-bar__item_on"); }
    if (PageName == "workeruc") { $(".weui-tabbar__item:last").addClass("weui-bar__item_on"); }
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

function fmtPrice_2(price) {
    if (price.toString().indexOf('.') > -1) {
        return price.toFixed(2).toString();
    }
    else {
        return price.toString();
    }
}

function fmtPrice(price) {
    var ma = 0;
    var sub = 0;
    if (price.toString().indexOf('.') > -1) {
        ma = price / 1;
        sub = (price % 1.0).toFixed(2);
    }
    else {
        ma = price;
    }
    if (ma > 1000) {
        var num = (ma || 0).toString(), result = '';
        while (num.length > 3) {
            result = ',' + num.slice(-3) + result;
            num = num.slice(0, num.length - 3);
        }
        if (num) { result = num + result; }
        if (sub > 0) { result = result + sub.split('.')[0] }
        return result;
    }
    else {
        ma += sub;
        return ma;
    }
}

function fmtBankCard(cardno) {
    var result = '';
    while (cardno.length > 4) {
        result = result + cardno.substr(0, 4) + ' ';
        cardno = cardno.slice(4, cardno.length);
    }
    if (cardno) { result = result + " " + cardno; }
    return result;
}

function ConvertShowState(n) {
    var result = "";
    switch (n) {
        case 0:
            result = "报备失败";
            break;
        case 1:
            result = "已报备";
            break;
        case 2:
            result = "已过报备期";
            break;
        case 3:
            result = "已到案场";
            break;
        case 4:
            result = "已预订";
            break;
        case 5:
            result = "已到场，继续跟进";
            break;
        case 6:
            result = "已过案场保护期";
            break;
        case 7:
            result = "已签约";
            break;
        case 8:
            result = "已签约，等待返佣";
            break;
        case 9:
            result = "已返佣";
            break;
        case 10:
            result = "用户退房";
            break;
        case 11:
            result = "佣金返还至平台";
            break;
        case 12:
            result = "佣金退还至开发商";
            break;
        default:
            result = "处理中";
            break;
    }
    return result;
}

