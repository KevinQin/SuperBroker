<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckBroker.aspx.cs" Inherits="app_work_CheckBroker" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>经纪人审核</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server"></form>
    <div class="container">
        <div class="page register-page js_show">
            <div class="page__bd" style="padding-top:.5rem">
                <div class="tip-row">有如下经纪人注册申请，请慎重评估并审核</div>
                 <div class="weui-cells weui-cells_form">                    
                    <div class="weui-cell">                       
                        <div class="weui-cell__bd">                            
                            <span id="txtName">-</span>
                        </div>
                    </div>
                     <div class="weui-cell">                       
                        <div class="weui-cell__bd">
                            <span id="txtMobile">-</span>
                        </div>
                    </div>
                    <div class="weui-cell">                       
                        <div class="weui-cell__bd">
                            <span id="txtWorkNo">-</span>
                        </div>
                    </div>
                     <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <textarea class="weui-textarea" id="txtMemo" maxlength="60" placeholder="请输入您的审核意见" rows="2"></textarea>
                        </div>
                    </div>
                </div>
                <div class="btn-row">                    
                    <a href="javascript:;" id="btnOk" class="weui-btn weui-btn_plain-primary">通过</a>
                    <a href="javascript:;" id="btnCancel" class="weui-btn weui-btn_default">拒绝</a>
                </div>
            </div>
        </div>
    </div>
</body>
<script src="//r.edmp.cc/jquery/jquery-2.2.3.min.js"></script>
<script src="//r.edmp.cc/jquerycookie/jquery.cookie.js"></script>
<script src="//res.wx.qq.com/open/js/jweixin-1.2.0.js"></script>
<script src="//r.edmp.cc/weui/weui.min.js"></script>
<script src="//r.edmp.cc/seascape/dateclass.js"></script>
<script src="//r.edmp.cc/seascape/common.js"></script>
<script src="../script/worker.js"></script>
<script src="../script/WeChat.js"></script>
</html>
