var SSJ = {};
var ajaxServerUrl = "/service/handler.ashx";
var ajaxBaseUrl = "/service/handler.ashx";
var PageName = "";
var BASE_URL = "http://ns.seascapeapp.cn/";

SSJ.init = function () {
    var pageUrl = window.location.href;
    var pageItems = pageUrl.split("/");
    PageName = pageItems[pageItems.length - 1].split('.')[0].toLowerCase();
    if (PageName === "index" || PageName === "") {
        IndexPage.init();
        addTabBar();
    }
    else if (PageName == "subject") {
        addTabBar();
    }   
    else if (PageName == "uc") {
        addTabBar();
    }
    else if (PageName == "list") {
       
    }
    else if (PageName == "detail") {
        
    }
    else if (PageName == "conform") {

    }
    else if (PageName == "pay") {

    }
    else if (PageName == "success") {
 
    }
    else if (PageName == "orderlist") {
       
    }
    else if (PageName == "orderdetail") {
        
    }
}

var IndexPage = {
    init: function () {
      
    }
};

$(document).ready(function () {
    SSJ.init();
});

function addTabBar() {
    var html = '<div class="weui-tabbar">';
    html += '<a href= "index.aspx" class="weui-tabbar__item"><i class="icon iconfont icon-wxbzhuye weui-tabbar__icon"></i><p class="weui-tabbar__label">备案</p></a >';
    html += '<a href="subject.aspx" class="weui-tabbar__item"><i class="icon iconfont icon-all weui-tabbar__icon"></i><p class="weui-tabbar__label">记录</p></a>';
    html += '<a href="shopcar.aspx" class="weui-tabbar__item"><span style="display: inline-block;position: relative;"><i class="icon iconfont icon-cart weui-tabbar__icon"></i><span class="weui-badge" style="display:none; position: absolute;top: -2px;right: -13px;">0</span></span><p class="weui-tabbar__label">佣金</p></a>';
    html += '<a href="uc.aspx" class="weui-tabbar__item"><i class="icon iconfont icon-account weui-tabbar__icon"></i><p class="weui-tabbar__label">我的</p></a></div>';
    $(".weui-tab").append(html);
    if (PageName == "index") { $(".weui-tabbar__item:eq(0)").addClass("weui-bar__item_on"); }
    if (PageName == "subject") { $(".weui-tabbar__item:eq(1)").addClass("weui-bar__item_on"); }
    if (PageName == "shopcar") { $(".weui-tabbar__item:eq(2)").addClass("weui-bar__item_on"); }
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
