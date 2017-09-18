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
        UcPage.init();
        addTabBar();
    }
    else if (PageName == "fee") {
        addTabBar();
        FeePage.init();
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
        NotifyPage.init();
    }
    else if (PageName == "help") {
        HelpPage.init();
    }
    else if (PageName == "list") {
        BuilderList.init();
        addTabBar();
    }
    else if (PageName == "detail") {
        BuilderDetail.init();
        addTabBar();
    }
    else if (PageName == "brokeredit") {
        BEditPage.init();
    }
    else if (PageName == "bankedit") {
        BankEdit.init();
    }
    else if (PageName == "changepwd") {
        ChangePwd.init();
    }
}

var HelpPage = {
    init: function () {
        HelpPage.LoadList();
    },
    LoadList: function () {
        $ajax({ fn: 118 }, HelpPage.LoadList_cb, true);
    },
    LoadList_cb: function (o) {
        if (o.Return == 0) {
            if ($(".weui-loadmore").length > 0) {
                $(".weui-loadmore").remove();
            }
            if (o.data.length > 0) {
                if ($("ul").length == 0) {
                    $(".page__bd").append('<ul></ul>');
                }
                $(o.data).each(function (i, _o) {
                    var template = '<li><h4><em>Q</em> ' + _o.Title + '</h4><p><em>A</em> ' + _o.Content + '</p></li >'
                    $("ul").append(template);
                });               
                //没有更多了
                var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >没有更多了</span></div>';
                $(".page__bd").append(moreHtml);
            }
            else {
                var html = '<div style="margin-top:15rem;" class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >暂无帮助，点击刷新</span></div>';
                $(".page__bd").append(html);
                $(".weui-loadmore__tips").on("click", HelpPage.LoadList);
            }
        }
    }
};

var NotifyPage = {
    PageNum:1,
    init: function () {
        NotifyPage.LoadNotify();
    },
    LoadNotify: function () {
        $ajax({ fn: 117, openid: $get("openid"), pageno: NotifyPage.PageNum }, NotifyPage.LoadNotify_cb, true);
    },
    LoadNotify_cb: function (o) {
        if (o.Return == 0) {
            if ($(".weui-loadmore").length > 0) {
                $(".weui-loadmore").remove();
            }
            if (o.data.length > 0) {
                if ($("ul.notify").length == 0) {
                    $(".page__bd").append('<ul class="notify"></ul>');
                }
                $(o.data).each(function (i, _o) {
                    var template = '<li><h3>' + _o.Title + '</h3><h5>' + new Date(_o.AddOn).fmt("M月d日") +'</h5><p>'+ _o.Content +'</p></li >'
                    $("ul").append(template);
                });
                NotifyPage.PageNo = o.Ext.PageNo;
                if (o.Ext.PageCount > o.Ext.PageNo) {
                    //还有更多
                    var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >加载更多</span></div>';
                    $(".page__bd").append(moreHtml);
                    $(".weui-loadmore__tips").on("click", NotifyPage.LoadNotify);
                }
                else {
                    //没有更多了
                    var moreHtml = '<div class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >没有更多了</span></div>';
                    $(".page__bd").append(moreHtml);
                }          
            }
            else {
                var html = '<div style="margin-top:15rem;" class="weui-loadmore weui-loadmore_line"><span class="weui-loadmore__tips" >暂无消息，点击刷新</span></div>';
                $(".page__bd").append(html);
                $(".weui-loadmore__tips").on("click", NotifyPage.LoadNotify);
            }
        }
    }
};

var ChangePwd = {
    init: function () {
        $("#btnOk").on("click", ChangePwd.dochangePwd);
        $("#btnCancel").on("click", function () { history.back(); });
        $("#txtOldPwd").on("blur", ChangePwd.checkpwd);
        $("input").on("focus", RegisterPage.inputFocus);
    },
    checkpwd: function () {
        if ($(this).val() == "") { return; }
        $ajax({ fn: 115, oldpwd: $(this).val(),openid:$get("openid") }, ChangePwd.checkpwd_cb, false);
    },
    checkpwd_cb: function (o) {
        if (o.Return != 0) {
            $("#txtOldPwd").val("");
            $("#txtOldPwd").parents(".weui-cell").addClass("weui-cell_warn");
            weui.topTips('现有密码验证未通过', 3000);
        }
    },
    dochangePwd: function () {
        weui.form.validate('#frmEdit', function (error) {
            if (!error) {
                var oldpwd = $("#txtOldPwd").val();
                var pwd = $("#txtPwd").val();
                var repwd = $("#txtRePwd").val();
                if (pwd != repwd) {
                    $("#txtRePwd").val("");
                    $("#txtRePwd").parents(".weui-cell").addClass("weui-cell_warn");
                    weui.topTips('两次密码不一致，请重新填写', 3000);
                    return;
                }
                var openid = $get('openid');
                $ajax({ fn: 116, pwd: pwd, oldpwd: oldpwd, openid: openid }, ChangePwd.dochangepwd_cb, true);
            }
            //当return true时，不会显示错误
        });
    },
    dochangepwd_cb: function (o) {
        if (o.Return == 0) {
            weui.toast('修改密码成功', function () { $go("uc.aspx"); });
        }
        else {
            weui.dialog({ title: '提示', content: '修改密码失败，稍侯重试', });
        }
    }
};

var BankEdit = {
    init: function () {
        BankEdit.LoadBroker();
        $("#btnOk").on("click", BankEdit.SaveBank);
        $("#btnCancel").on("click", function () { history.back(); });
        $("input").on("focus", RegisterPage.inputFocus);
    },
    LoadBroker: function () {
        $ajax({ fn: 112, openid: $get("openid") }, BankEdit.LoadBroker_cb, true);
    },
    LoadBroker_cb: function (o) {
        if (o.Return == 0) {
            $("#txtBankInfo").val(o.data.BankInfo);
            $("#txtAccount").val(o.data.AccountName);
            $("#txtCardNo").val(o.data.BankCardNo);
        }
    },
    SaveBank: function () {
        weui.form.validate('#frmEdit', function (error) {
            if (!error) {               
                var bankinfo = $("#txtBankInfo").val();
                var account = $("#txtAccount").val();
                var cardno = $("#txtCardNo").val();               
                var openid = $get('openid');
                $ajax({ fn: 114, bankinfo: bankinfo, account: account, cardno: cardno, openid: openid }, BankEdit.SaveBank_cb, true);
            }
            //当return true时，不会显示错误
        });
    },
    SaveBank_cb: function (o) {
        if (o.Return == 0) {
            weui.toast('修改成功', function () { $go("uc.aspx"); });
        }
        else {
            weui.dialog({ title: '提示', content: '修改银行卡失败，稍侯重试', });
        }
    }
};

var BEditPage = {
    citydata: [],
    tradedata:[],
    init: function () {
        BEditPage.LoadBroker();
        $("#btnOk").on("click", BEditPage.SaveBroker);
        $("#btnCancel").on("click", function () { history.back(); });
        $(_CITYARRAY).each(function (i, o) {
            var pname = o.name;
            if (pname == "其他" || pname=="海外") { return; }
            var cityarr = o.sub;
            var city = [];
            $(cityarr).each(function (j, _o) {
                if (_o.name == "请选择") { return; }
                city.push({label:_o.name,value:j});
            });            
            BEditPage.citydata.push({ label: pname, value: i, children: city });
        });
        BEditPage.tradedata = [
            { label: '房地产/建筑', value: 1 },
            { label: '计算机/互联网/通信/电子', value: 2 },
            { label: '会计/金融/银行/保险', value: 3 },
            { label: '贸易/消费/制造/营运', value: 4 },
            { label: '制药/医疗', value: 5 },
            { label: '广告/媒体', value: 6 },
            { label: '专业服务/教育/培训', value: 7 },
            { label: '服务业', value: 8 },
            { label: '物流/运输', value: 9 },
            { label: '能源/原材料', value: 10 },
            { label: '政府/非赢利机构/其他', value: 11 }
        ];
        $("input").on("focus", RegisterPage.inputFocus);
    },
    LoadBroker: function () {
        $ajax({ fn: 112, openid: $get("openid") }, BEditPage.LoadBroker_cb, true);
    },
    LoadBroker_cb: function (o) {
        if (o.Return == 0) {
            $("#txtBrokerId").val(o.data.Id);
            $("#txtName").text(o.data.Name);
            $("#selGender").val(o.data.Gender);
            $("#txtMobile").val(o.data.Mobile);
            $("#txtTel").val(o.data.Tel);
            $("#txtAddress").val(o.data.Address);
            $("#txtCompany").val(o.data.Company);            
            if (o.data.Trade != "") {
                $("#txtTrade").val(o.data.Trade);
                $("#txtTrade_").text(o.data.Trade);
                $("#txtTrade_").removeClass("gray");
            }
            var reg = /,/g;
            if (o.data.Area != "") {
                $("#txtCity").val(o.data.Area);
                $("#txtCity_").text(o.data.Area.replace(reg, "").replace("中国",""));
                $("#txtCity_").removeClass("gray");
            }

            $("#txtTrade_").parents(".weui-cell").on("click", function () {
                weui.picker(BEditPage.tradedata, {
                    className: 'custom-classname',
                    defaultValue: [1],
                    onChange: function (result) {
                        //console.log(result)
                    },
                    onConfirm: function (result) {
                        var _p = result[0].label;  
                        $("#txtTrade").val(_p);
                        $("#txtTrade_").text(_p);
                        $("#txtTrade_").removeClass("gray");                        
                    },
                    id: 'doubleLinePicker'
                });
            });

            $("#txtCity_").parents(".weui-cell").on("click", function () {
                weui.picker(BEditPage.citydata, {
                    className: 'custom-classname',
                    defaultValue: [11, 1],
                    onChange: function (result) {
                        //console.log(result)
                    },
                    onConfirm: function (result) {
                        var _p = result[0].label;
                        var _c = result[1].label;
                        $("#txtCity").val("中国,"+_p+","+_c);
                        $("#txtCity_").text(_p+_c);
                        $("#txtCity_").removeClass("gray");
                    },
                    id: 'doubleLinePicker'
                });
            });
        }
    },
    SaveBroker: function () {
        weui.form.validate('#frmEdit', function (error) {
            if (!error) {
                var id = $("#txtBrokerId").val();
                var mobile = $("#txtMobile").val();
                var gender = $("#selGender").val();
                var tel = $("#txtTel").val();
                var city = $("#txtCity").val();
                var address = $("#txtAddress").val();
                var trade = $("#txtTrade").val();
                var company = $("#txtCompany").val();
                var openid = $get('openid');
                $ajax({ fn: 113, id: id, mobile: mobile, gender: gender, tel: tel, city: city, address: address, trade: trade, company: company, openid: openid }, BEditPage.SaveBroker_cb, true);
            }
            //当return true时，不会显示错误
        });
    },
    SaveBroker_cb: function (o) {
        if (o.Return == 0) {
            weui.toast('修改成功', function () { $go("uc.aspx"); });
        }
        else {
            weui.dialog({ title: '提示', content: '修改资料失败，稍侯重试', });
        }
    }
};

var UcPage = {
    init: function () {
        var photoUrl = $get('photourl');
        if (photoUrl != null && photoUrl != "") {
            $("img").attr("src", photoUrl);
        }
        var mobile = $get('mobile');
        if (mobile!=null && mobile != "") {
            $("h5").text(mobile);
        }
        $("a.weui-cell_access:last").on("click", function () {
            $ClearSet();
            weui.toast("成功退出", function () {
                $go("login.aspx");
            });            
        });
        UcPage.LoadBroker();
    },
    LoadBroker: function () {
        $ajax({ fn: 112, openid: $get("openid") }, UcPage.LoadBroker_cb, true);
    },
    LoadBroker_cb: function (o) {
        if (o.Return == 0) {
            $("img").attr("src", o.data.AvatarMediaId);            
            $("h5").text(o.data.Mobile);
            if (parseInt( o.data.State) == 1) {
                $("h4").text(o.data.Name + " " + o.data.WorkNo);
            }
            else {
                var state = "审核中";
                if (o.data.State == 0) {
                    state = "审核中";
                }
                else if (o.data.State == 2) {
                    state = "已拒绝";
                }
                else if (o.data.State == 9) {
                    state = "暂时下架";
                }

                $("h4").text(o.data.Name + " " + state);
            }
           
        }
    }
};

var FeePage = {
    init: function () {
        FeePage.loadLogs();
    },
    loadLogs: function () {
        $ajax({ fn: 111, openid: $get("openid") }, FeePage.loadLogs_cb, true);
    },
    loadLogs_cb: function (o) {
        if (o.Return == 0) {
            var countFee = 0;
            var needPay = 0;
            var paied = 0;
            $(".weui-tab__panel").append('<ul class="logs"></ul>');
            $(o.data).each(function (i, _o) {
                var fee = _o.Fee;
                if (fee < 1000) { fee = parseInt(_o.Fee * _o.TotalPrice / 1000); }
                var dateStr = new Date(_o.PayFeeOn).fmt("yyyy-MM-dd");
                if (_o.State == 9) {
                    paied += fee;                    
                } else {
                    needPay += fee;
                    dateStr = "待返";
                }
                countFee += fee;                
                var html = '<li data-rno="' + _o.ReportNo + '"><div class="info-col">' + _o.CustomName + '<br/>' + _o.BuilderName + '</div><div class="price-col">' + dateStr + ' ' + fmtPrice(fee) + ' 元</div></li>';
                $("ul.logs").append(html);
            });
            $(".logs li").on("click", function () { $go("ReportDetail.aspx?rno=" + $(this).attr("data-rno")); });
            var html = '<span>总计(元)</span><h2>' + fmtPrice(countFee) + '</h2><ul><li>已返佣(元)<h5>' + fmtPrice(paied) + '</h5></li><li>待返佣(元)<h5>' + fmtPrice(needPay) + '</h5></li></ul>'
            $('.panel__hd').append(html);
        }
    }
};

var RDetailPage = {
    Rno: "",
    init: function () {
        RDetailPage.Rno = request("rno");
        RDetailPage.loadDetail();
    },
    loadDetail: function () {
        $ajax({ fn: 109, rno: RDetailPage.Rno, openid:$get("openid") }, RDetailPage.loadDetail_cb, true);
    },
    loadDetail_cb: function (o) {
        if (o.Return == 0) {
            RDetailPage.loadLogs();
            var html = '<div class="blank-area"><div class="icon-col"><i class="iconfont icon-shouhuodizhi"></i></div><div class="info-col">';
            html += '<h5>' + o.data.CustomName + '&nbsp;&nbsp;' + o.data.CustomMobile + '</h5><p>' + o.data.City + o.data.District + ' ' + o.data.BuilderName + '</p></div></div>';
            html += '<div class="info-list"><p><em>当前状态：' + ConvertShowState(o.data.State) + '</em></p><p>报备编号：' + o.data.ReportNo + '<br/>报备时间：' + new Date(o.data.ReportOn).fmt("yyyy-MM-dd HH:mm:ss") + '</p>'
            if (o.data.State == 9) {
                var fee = o.data.Fee;
                if (fee < 1000) { fee = parseInt(o.data.Fee * o.data.TotalPrice / 1000); }
                fee = fmtPrice(fee);
                html += '<p>返佣时间：' + new Date(o.data.PayFeeOn).fmt("yyyy-MM-dd HH:mm:ss") + '</p><div class="price-row" ><div class="info-col">'
                html += '银行：' + o.Ext.BankInfo + '<br/>账户：' + o.Ext.BankAccount + '<br/>账号：' + fmtBankCard(o.Ext.BankCard)
                html += '</div><div class="num-col">金额<h3>￥'+ fee +'</h3></div></div>'
            }
            html += '</div >'
            $(".page__bd").append(html);           
        }
    },
    loadLogs: function () {
        $ajax({ fn: 110, rno: RDetailPage.Rno, openid: $get("openid")}, RDetailPage.loadLogs_cb, false);
    },
    loadLogs_cb: function (o) {
        if (o.Return == 0) {
            var html = '<div class="logs-list"><div class="title-row">操作日志</div><ul></ul></div>';
            $(".page__bd").append(html);  
            $(o.data).each(function (i, _o) {
                html = '<li><h5>' + new Date(_o.AddOn).fmt("yyyy-MM-dd HH:mm:ss") + '</h5><p>' + _o.Memo + '</p></li>';
                $("ul").append(html);
            });            
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

                $("li").off("click").on("click", function () {
                    $go("ReportDetail.aspx?rno=" + $(this).attr("data-no"));
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
            $set('RoleId', o.Ext.RoleId);
            weui.toast('登录成功', {
                duration: 1500,
                className: 'custom-classname',
                callback: function () {
                    if (o.Ext.RoleId == 199) {
                        $go("../builder/list.aspx");
                    }
                    else if (o.Ext.RoleId==100) {
                        $go("../report/reportdata.aspx");
                    }
                    else if (o.Ext.RoleId == 101) {
                        $go("../work/checkInBuilder.aspx");
                    }
                    else if (o.Ext.RoleId == 102) {
                        $go("../work/needPayFee.aspx");
                    } 
                    else if (o.Ext.RoleId == 103) {
                        $go("../builder/list.aspx");
                    } 
                    else if (o.Ext.RoleId == 104) {
                        $go("../work/checklist.aspx");
                    } 
                }
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
                var code = $("#txtCode").val();
                if (Password != RePwd) {
                    $("#txtRePwd").val("");
                    $("#txtRePwd").parents(".weui-cell").addClass("weui-cell_warn");
                    weui.topTips('两次密码不一致，请重新填写', 3000);
                    return;
                }
                $ajax({ fn: 101, name: name, code:code, mobile: mobile, pwd: Password, openid:openid }, RegisterPage.register_cb, true);
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
            weui.alert(o.Msg);
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
        ma= price;
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
    if ($isDebug) {
        $set('openid', debug_openId, cookie_obj);
        window.location.href = window.location.href + "?code=123";
    }
    else {
        $go("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9a6c6b62bc80e7d8&redirect_uri=http%3a%2f%2fb.seascapeapp.cn%2fapp%2f" + folder + "%2f" + page + "&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect");
    }
}

