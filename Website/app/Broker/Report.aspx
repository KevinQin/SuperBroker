<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="app_Broker_Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>客户报备</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="frmReport" runat="server">
     <div class="container">
        <div class="page register-page js_show">
            <div class="page__bd" >
                <div class="weui-cells_form">
                <div class="weui-cells__title">客户信息</div>
                <div class="weui-cells">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input class="weui-input" id="txtName" type="text" placeholder="请填写客户姓名" required="required" maxlength="6"/>
                        </div>                        
                    </div>
                    <div class="weui-cell weui-cell_select">
                        <div class="weui-cell__bd">
                            <select class="weui-select" id="selGender" name="selGender">
                                <option selected="selected" value="1">先生</option>
                                <option value="2">女士</option>
                            </select>
                        </div>
                    </div>
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input class="weui-input" id="txtMobile" type="number" placeholder="请填写客户电话号码" required="required" pattern="[0-9]{11}"/>
                        </div>
                    </div>
                    
                </div>
                <div class="weui-cells__title">报备信息</div>
                <div class="weui-cells">
                    <div class="weui-cell desp-inp">
                        <div class="weui-cell__hd">楼盘</div>
                        <div class="weui-cell__bd" id="txtBuilder">-</div>
                    </div>
                    <div class="weui-cell desp-inp">
                        <div class="weui-cell__hd">经纪人</div>
                        <div class="weui-cell__bd" id="txtBroker">-</div>
                    </div>
                    <div class="weui-cell desp-inp">
                        <div class="weui-cell__hd">备案日期</div>
                        <div class="weui-cell__bd" id="txtBegin">-</div>
                    </div>
                    <div class="weui-cell desp-inp">
                        <div class="weui-cell__hd">保护至</div>
                        <div class="weui-cell__bd" id="txtEnd">-</div>
                    </div>
                </div>                
                <label for="weuiAgree" class="weui-agree">
                    <input id="chkAgree" name="chkAgree" type="checkbox" value="1" required="required" pattern="{1,}" tips="必须同意条款才能继续备案" class="weui-agree__checkbox"/>
                    <span class="weui-agree__text">
                        阅读并同意<a id="lnkShowMemo" href="javascript:void(0);">《备案条款》</a>
                    </span>
                </label>
                    </div>
                <div class="weui-btn-area">
                    <a class="weui-btn weui-btn_primary" id="btnReport" href="javascript:void(0);">确定</a>
                    <a class="weui-btn weui-btn_default" id="btnCancel" href="../builder/list.aspx">返回</a>
                </div>
            </div>           
        </div>
     </div>
     </form>
</body>
<script src="//r.edmp.cc/jquery/jquery-2.2.3.min.js"></script>
<script src="//r.edmp.cc/jquerycookie/jquery.cookie.js"></script>
<script src="//res.wx.qq.com/open/js/jweixin-1.2.0.js"></script>
<script src="//r.edmp.cc/weui/weui.min.js"></script>
<script src="//r.edmp.cc/seascape/dateclass.js"></script>
<script src="//r.edmp.cc/seascape/common.js"></script>
<script src="../script/broker.js"></script>
<script src="../script/WeChat.js"></script>
</html>
