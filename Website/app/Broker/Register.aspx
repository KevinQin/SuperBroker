<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="app_Register" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>加入经纪人团队</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="frmReg" runat="server">  
    <div class="container">
        <div class="page register-page js_show">
            <div class="page__bd">
                <div class="logo-row">
                    <img class="all" src="../images/reg-banner.png" />
                </div>
                <div class="weui-cells weui-cells_form">                    
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-jingjiren"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" required="required" type="text" id="txtName" placeholder="请填写姓名"/>
                        </div>
                    </div>
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-iconfontshouji"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" id="txtMobile" required="required" pattern="[0-9]{11}" placeholder="请填写手机号码"/>
                        </div>
                    </div> 
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-ganxie1"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" id="txtCode" required="required" pattern="[0-9]{3}" placeholder="请填写邀请码"/>
                        </div>
                    </div> 
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-kouling"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="password" required="required" pattern="\w{6,16}" id="txtPwd" placeholder="请填写登录密码,6-16位字符"/>
                        </div>              
                    </div>
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-anquanzhongxin"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="password" id="txtRePwd" required="required" pattern="\w{6,16}" placeholder="请再次填写登录密码"/>
                        </div>              
                    </div>
                </div>
                <div class="btn-row">                    
                    <a href="javascript:;" id="btnRegister" class="weui-btn weui-btn_plain-primary">注册</a>
                    <p class="tip_links">
                        <i class="iconfont icon-tishi"></i> 已经有账号，点击<a href="login.aspx">去登录</a>
                    </p>
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