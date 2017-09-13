<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankEdit.aspx.cs" Inherits="app_Broker_BankEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>我的银行卡</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="frmEdit" runat="server">
    <div class="container">
        <div class="page register-page js_show">
            <div class="page__bd" style="padding-top:.5rem">
                <div class="weui-cells weui-cells_form"> 
                     <div class="weui-cell">                        
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" required="required" id="txtBankInfo" placeholder="填写开户行信息"/>
                        </div>
                    </div>
                     <div class="weui-cell">                        
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" required="required" id="txtAccount" placeholder="填写户名"/>
                        </div>
                    </div>
                    <div class="weui-cell">                       
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="number" id="txtCardNo" required="required" pattern="[0-9]{16,}" placeholder="请输入银行卡号"/>
                        </div>
                    </div>   
                </div>
                <div class="btn-row">                    
                    <a href="javascript:;" id="btnOk" class="weui-btn weui-btn_plain-primary">确定</a>
                    <a href="javascript:;" id="btnCancel" class="weui-btn weui-btn_default">取消</a>
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