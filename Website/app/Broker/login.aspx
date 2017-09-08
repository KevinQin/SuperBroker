<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="app_index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>经纪人登录</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="frmLogin" runat="server">
    <div class="container">
        <div class="page register-page js_show">
            <div class="page__bd" >
                <div class="logo-row">
                    <img src="../images/logo.png" />
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-jingjiren1"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="number" id="txtAccount" required="required" pattern="[0-9]{5,11}" placeholder="请填写经纪人账号或手机"/>
                        </div>
                    </div>                   
                    <div class="weui-cell">
                        <div class="weui-cell__hd"><label class="weui-label"><i class="iconfont icon-kouling"></i></label></div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="password" id="txtPwd" required="required" pattern="\w{6,16}" placeholder="请填写经纪人密码"/>
                        </div>              
                    </div>
                </div>
                <div class="btn-row">
                    <a href="javascript:;" id="btnLogin" class="weui-btn weui-btn_plain-primary">登录</a>
                    <p class="tip_links">
                        <i class="iconfont icon-tishi"></i> 没有账号，点击<a href="register.aspx">立即注册</a>加入
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
