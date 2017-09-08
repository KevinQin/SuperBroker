var SSJ = {};
var ajaxServerUrl = "/service/app.ashx";
var PageName = "";
var BASE_URL = "http://b.seascapeapp.cn/";
var debug_openId = "o3MRawEmK5OK-ringNnTyiNPA6uM";
var storage = window.localStorage;
var cookie_before = "SupeBrokerV1_1_";

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
    else if (PageName == "fee") {

    }
    else if (PageName == "report") {
        ReportPage.init();
    }
    else if (PageName == "history") {
        HistoryPage.init();       
    }
    else if (PageName == "reportdetail") {
        RDetailPage.init();
    }
    else if (PageName == "notify") {

    }
    else if (PageName == "list") {
        BuilderList.init();
        addTabBar();
    }
    else if (PageName == "detail") {
        BuilderDetail.init();
        addTabBar();
    }
}

var RDetailPage = {
    Rno: "",
    init: function () {
        RDetailPage.loadDetail();
    },
    loadDetail: function () {

    },
    loadDetail_cb: function (o) {
        if (o.Return == 0) {
            RDetailPage.loadLogs();
        }
    },
    loadLogs: function () {

    },
    loadLogs_cb: function (o) {
        if (o.Return == 0) {

        }
    }
};

var HistoryPage = {
    PageNo: 0,
    TabIndex:0,
    init: function () {
        HistoryPage.initTab();
        HistoryPage.loadHistory();
    },
    initTab: function () {
        weui.tab('#hisTab', {
            defaultIndex: 0,
            onChange: function (index) {
                HistoryPage.TabIndex = index;
                HistoryPage.PageNo = 0;
                $("ul").remove();
                HistoryPage.loadHistory();
            }
        });
    },
    loadHistory: function () {
        var state = 0;
        if (HistoryPage.TabIndex == 0) { state = -1; }
        else if (HistoryPage.TabIndex == 1) { state = 1; }
        else if (HistoryPage.TabIndex == 2) { state = 3; }
        else if (HistoryPage.TabIndex == 3) { state = 9; }
        $ajax({ fn: 108, openid: $get("openid"), state: state }, HistoryPage.loadHistory_cb, true);
    },
    loadHistory_cb: function (o) {
        $(".weui-loadmore").remove();
        if (o.Return == 0) {            
            if (o.data.length > 0) {                
                if ($("ul").length == 0) {
                    $(".weui-tab__panel").append("<ul></ul>");
                }

                $(o.data).each(function (i, _o) {
                    var htm = '<li data-no="' + _o.ReportNo + '"><span>' + _o.ReportNo + '</span>'
                    htm += '<div class="statu-row"><h5>状态：<em>' + ConvertShowState(_o.State) +'</em></h5><h6>' + new Date(_o.ReportOn).fmt("yyyy-MM-dd HH:mm") + '</h6><h5>保护期还剩 <span>' + dateSubFormat(_o.ReportOn, _o.ProtectedOn) + '</span></h5></div>'
                    htm += '<div class="main-card">'
                    htm += '<img src="http://pan.baidu.com/share/qrcode?w=120&h=120&url=http%3a%2f%2fb.seascapeapp.cn%2fapp%2fWorker%2fConform.aspx%3fbno%3d' + _o.ReportNo + '"/>';
                    htm += '<div class="info"><h2>' + _o.CustomName + '</h2><h4><i class="iconfont icon-dianhua"></i>&nbsp;' + _o.CustomMobile + '</h4><h3><i class="iconfont icon-jiudian"></i>&nbsp;' + _o.BuilderName + '</h3></div></div></li>'
                    $("ul").append(htm);
                });

                HistoryPage.PageNo = o.Ext.PageNo;
                if (o.Ext.PageCount > o.Ext.PageNo) {
                    //还有更多
                    var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >加载更多</span></div>';
                    $(".weui-tab__panel").append(moreHtml);
                    $(".weui-loadmore__tips").on("click", HistoryPage.loadHistory);
                }
                else {
                    //没有更多了
                    var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >没有更多了</span></div>';
                    $(".weui-tab__panel").append(moreHtml);
                }

            }
            else {
                var html = '<div style="margin-top:15rem;" class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips">暂无数据，再试一次</span></div>';
                $(".weui-tab__panel").append(html);
                $(".weui-loadmore__tips").on("click", HistoryPage.loadHistory);
            }
        }
        else {
            var html = '<div style="margin-top:15rem;" class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips">服务器打盹了，再试一次</span></div>';
            $(".weui-tab__panel").append(html);
            $(".weui-loadmore__tips").on("click", HistoryPage.loadHistory);
        }
    }
};

var ReportPage = {
    init: function () {
        $("#btnReport").on("click", ReportPage.onReport);
        $("#lnkShowMemo").on("click", ReportPage.showMemo);
        ReportPage.loadInfo();
    },
    loadInfo: function () {
        var bno = request("bno");
        $ajax({ fn: 107, bno: bno, openid: $get("openid") }, ReportPage.loadInfo_cb, true);
    },
    loadInfo_cb: function (o) {
        if (o.Return == 0) {
            $("#txtBuilder").text(o.data.BuilderName);
            $("#txtBroker").text(o.data.BrokerName+" ["+ o.data.BrokerWorkNo +"]");
            $("#txtBegin").text(o.data.Begin);
            $("#txtEnd").text(o.data.End);
        }
    },
    onReport: function () {
        weui.form.validate('#frmReport', function (error) {
            if (!error) {
                var name = $("#txtName").val();
                var mobile = $("#txtMobile").val();
                var openid = $get('openid');
                var gender = $("#selGender").val();
                $ajax({ fn: 106, gender: gender, bno:request("bno"), name: name, mobile: mobile, openid: openid }, ReportPage.report_cb, true);
            }
            //当return true时，不会显示错误
        });
    },
    report_cb: function (o) {
        if (o.Return == 0) {
            var reportno = o.data;
            var html = '<div class="page msg_success js_show"><div class="weui-msg"><div class="weui-msg__icon-area"><i class="weui-icon-success weui-icon_msg"></i></div><div class="weui-msg__text-area"><h2 class="weui-msg__title">备案成功</h2><p class="weui-msg__desc">已备案已经成功，请与七日内带客至案场，超时将视为备案失效<a href="javascript:void(0);">文字链接</a></p></div>'
             html += '<div class="weui-msg__opr-area"><p class="weui-btn-area"><a id="lnkDetail" href="javascript:;" class="weui-btn weui-btn_primary">查看详情</a><a href="javascript:;" id="lnkReport" class="weui-btn weui-btn_default">继续备案</a></p></div>'
             html += '<div class="weui-msg__extra-area"><div class="weui-footer"><p class="weui-footer__links"><a href="../builder/list.aspx" class="weui-footer__link">返回楼盘列表</a></p></div></div></div></div>'
             $(".page").hide();
             $(".container").append(html);
             $("#lnkDetail").on("click", function () { $go("detail.aspx?rno=" + reportno); });
             $("#lnkReport").on("click", function () {
                 $("#txtName").val("");
                 $("#txtMobile").val("");
                 $("#selGender").val("1");
                 $(".msg_success").remove();
                 $(".page").show();
             });            
        }
        else {
            var subCode = o.SubCode;            
            weui.alert(o.Msg);
            $("#txtName").val("");
            $("#txtMobile").val("");
        }
    },
    showMemo: function () { }
};

var BuilderDetail = {
    BNo:"",
    init: function () {
        BuilderDetail.BNo = request("bno");
        BuilderDetail.loadDetail();        
    },
    loadRoom: function () {
        $ajax({ fn: 105, bno: BuilderDetail.BNo }, BuilderDetail.loadRoom_cb, false);
    },
    loadRoom_cb: function (o) {
        var html = "";
        if (o.Return == 0) {
            if (o.data.length > 0) {
                html += '<div class="desp-area"><h4><a id="room" name="room">户型介绍</a></h4><ul>'
                $(o.data).each(function (i, _o) {
                    html += '<li><div class="title-row"><h5>'+ _o.RoomNo +'户型'+ _o.Name +' </h5><h6>建筑面积约：'+ _o.Area +'m<sup>2</sup></h6></div><img src="'+ _o.ImgUrl +'"/><p>' + _o.Description + '</p></li>'
                });                
                html += '</ul></div>'
                $(".detail-info").append(html);
            }
            if (request("act") != null && request("act") == "room") {
                BuilderDetail.goRoom();
            }
        }
    },
    goRoom: function () {
        $(".weui-tab__panel").animate({
            scrollTop: $("#room").offset().top + "px"
        }, 500);
    },
    loadDetail: function () {
        $ajax({ fn: 104, bno: BuilderDetail.BNo }, BuilderDetail.loadDetail_cb, true);
    },
    loadDetail_cb: function (o) {
        if (o.Return == 0) {
            BuilderDetail.initSwiper(o.data.D3Url);
            BuilderDetail.initInfo(o.data);
            BuilderDetail.loadRoom();
            $("a[data-act=room]").on("click", BuilderDetail.goRoom);
            $("a[data-act=report]").on("click", BuilderDetail.goReport);
        }
        else {
            weui.alert("载入楼盘信息失败");
        }
    },
    goReport: function () {
        $go("../broker/report.aspx?bno=" + BuilderDetail.BNo + "&ot=0");
    },
    initInfo: function (_o) {
        var html = '<div class="detail-info">';
        html += '<div class="title-row"><h5>' + _o.Name + '</h5><h6><i class="iconfont icon-mudedi"></i> ' + _o.City + _o.District + '</h6></div>';
        html += '<div class="price-row">均价：<em>' + (_o.Price > 0 ? "￥" + _o.Price : "待定") + '</em></div>';
        html += '<div class="address-row"><i class="iconfont icon-ditu"></i> '+ _o.Address +'</div>'
        if (_o.Label.length > 0) {
            var ls = _o.Label.split(',');
            html += '<div class="label-row">';
            $(ls).each(function (i, __o) {
                if (__o.length > 0) {
                    html += '<label>' + __o + '</label>';
                }
            });
            html += '</div>'
        }
        html += '<div class="btn-row"><a href="javascript:void(0);" data-act="room" class="weui-btn weui-btn_mini weui-btn_default">户型图</a> <a href="javascript:;" data-act="report" class="weui-btn weui-btn_mini weui-btn_primary">报备</a></div>'
        html += '<div class="desp-area"><h4>相关介绍</h4>'
        html += '<p>' + _o.Description +'</p>'
        html += '</div>'
        html += '</div>';
        $(".weui-tab__panel").append(html);
    },
    initSwiper: function (o) {      
        var html = '<div class="swiper-wrapper">';
        var ps = o.split(',');
        $(ps).each(function (i, _o) {
            html += '<div class="swiper-slide"><img src="' + _o + '" /></div>';
        });
        html += '</div><div class="swiper-pagination"></div>';
        $(".swiper-container").append(html);
        var mySwiper = new Swiper('.swiper-container', {
            loop: true,
            autoplay: 5000,
            pagination: '.swiper-pagination'
        })
    }
};

var BuilderList = {
    PageNo:0,
    init: function () {
        BuilderList.getList();
    },
    getList: function () {
        $ajax({ fn: 103, page: BuilderList.PageNo+1, key: "", location: "" }, BuilderList.getList_cb, true);
    },
    getList_cb: function (o) {
        if (o.Return == 0) {
            if ($(".weui-loadmore").length > 0) {
                $(".weui-loadmore").remove();
            }
            if (o.data.length > 0) {
                if ($("ul.builder-list").length == 0) {                   
                    $(".weui-tab__panel").append('<ul class="builder-list"></ul>');
                }
                $(o.data).each(function (i, _o) {
                    var template = '<li data-id="' + _o.BuilderNo + '"><div class="title-row"><h5>' + _o.Name + '</h5><h6><i class="iconfont icon-mudedi"></i> ' + _o.City + _o.District + '</h6></div>';
                    template += '<img src="' + _o.HeadImg + '" /><p>' + _o.Slogen + '</p>';
                    template += '<div class="btn-row"><div class="price-col">均价：<em>' + (_o.Price>0?"￥"+_o.Price:"待定") + '</em></div>'
                    template += '<div class="btn-col"><a href="javascript:;" data-act="room" class="weui-btn weui-btn_mini weui-btn_default">户型图</a> <a href="javascript:;" data-act="report" class="weui-btn weui-btn_mini weui-btn_primary">报备</a></div></div></li>'
                    $("ul").append(template);
                });
                BuilderList.PageNo = o.Ext.PageNo;
                if (o.Ext.PageCount > o.Ext.PageNo) {
                    //还有更多
                    var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >加载更多</span></div>';
                    $(".weui-tab__panel").append(moreHtml);
                    $(".weui-loadmore__tips").on("click", BuilderList.getList);
                }
                else {
                    //没有更多了
                    var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >没有更多了</span></div>';
                    $(".weui-tab__panel").append(moreHtml);
                }
                $(".builder-list li .title-bar,h5,img").off("click").on("click", BuilderList.goDetail);
                $("a[data-act=room]").off("click").on("click", BuilderList.goRoom);
                $("a[data-act=report]").off("click").on("click", BuilderList.goReport);
            }
            else {
                var html = '<div style="margin-top:15rem;" class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >暂无数据，再试一次</span></div>';
                $(".weui-tab__panel").append(html);
                $(".weui-loadmore__tips").on("click", BuilderList.getList);
            }
        }
    },
    goDetail: function () {
        var bno = $(this).parents("li").attr("data-id");
        $go("detail.aspx?bno=" + bno+"&ot=0");
    },
    goRoom: function () {
        var bno = $(this).parents("li").attr("data-id");
        $go("detail.aspx?bno=" + bno+"&act=room&ot=0");
    },
    goReport: function () {
        var bno = $(this).parents("li").attr("data-id");
        $go("../broker/report.aspx?bno=" + bno+"&ot=0");
    }
};

var LoginPage = {
    init: function () {
        var code = getParam("code");
        if (code == "") { return GetWechatLink("broker","login.aspx"); }
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
                callback: function () { $go("../builder/list.aspx"); }
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
        if (code == "") { return GetWechatLink("broker", "register.aspx"); }
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
    html += '<a href="/app/builder/list.aspx" class="weui-tabbar__item"><i class="iconfont icon-jiudian weui-tabbar__icon"></i><p class="weui-tabbar__label">楼盘</p></a >';
    html += '<a href="/app/broker/history.aspx" class="weui-tabbar__item"><i class="iconfont icon-shijian1 weui-tabbar__icon"></i><p class="weui-tabbar__label">历史</p></a>';
    html += '<a href="/app/broker/fee.aspx" class="weui-tabbar__item"><span style="display: inline-block;position: relative;"><i class="iconfont icon-lichengdixian weui-tabbar__icon"></i><span class="weui-badge" style="display:none; position: absolute;top: -2px;right: -13px;">0</span></span><p class="weui-tabbar__label">佣金</p></a>';
    html += '<a href="/app/broker/uc.aspx" class="weui-tabbar__item"><i class="iconfont icon-account weui-tabbar__icon"></i><p class="weui-tabbar__label">我的</p></a></div>';
    $(".weui-tab:first").append(html);
    if (PageName == "list" || PageName == "detail") { $(".weui-tabbar__item:eq(0)").addClass("weui-bar__item_on"); }
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
                $set('openid', o.data);
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
        var user = o.data;
        if (user != null) {
            $set('userid', user.id);
            $set('photourl', user.photoUrl);
            $set('mobile', user.mobile);
            $set('sex', user.sex);
        }
    }
}

function GetWechatLink(folder, page) {
    $go("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9a6c6b62bc80e7d8&redirect_uri=http%3a%2f%2fb.seascapeapp.cn%2fapp%2f" + folder + "%2f" + page + "&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect");
}

