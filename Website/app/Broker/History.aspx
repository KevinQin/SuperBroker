<%@ Page Language="C#" AutoEventWireup="true" CodeFile="History.aspx.cs" Inherits="app_Broker_History" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>备案历史</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/swiper/swiper-3.4.0.min.css" />
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server"></form>
    <div class="container">
        <div class="page history-page navbar js_show">
            <div class="page__bd" style="height: 100%;">
                <div class="weui-tab" id="hisTab">
                    <div class="weui-navbar">  
                        <div class="weui-navbar__item weui-bar__item_on">
                            全部
                        </div>
                        <div class="weui-navbar__item">
                            已报备
                        </div>
                        <div class="weui-navbar__item">
                            已带客
                        </div>
                        <div class="weui-navbar__item">
                            已返佣
                        </div>                       
                    </div>
                    <div class="weui-tab__panel" style="padding-top:2.3rem;">
                        
                    </div>
                </div>
            </div>
        </div>
     </div>
</body>
<script src="//r.edmp.cc/jquery/jquery-2.2.3.min.js"></script>
<script src="//r.edmp.cc/jquerycookie/jquery.cookie.js"></script>
<script src="//res.wx.qq.com/open/js/jweixin-1.2.0.js"></script>
<script src="//r.edmp.cc/weui/weui.min.js"></script>
<script src="//r.edmp.cc/swiper/swiper-3.4.0.jquery.min.js"></script>
<script src="//r.edmp.cc/seascape/dateclass.js"></script>
<script src="//r.edmp.cc/seascape/common.js"></script>
<script src="../script/broker.js"></script>
<script src="../script/WeChat.js"></script>
</html>
