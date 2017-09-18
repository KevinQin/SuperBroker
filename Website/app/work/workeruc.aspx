<%@ Page Language="C#" AutoEventWireup="true" CodeFile="workeruc.aspx.cs" Inherits="app_work_workeruc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>我的</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="../fonts/iconfont.css" />
    <link href="../style/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server"></form>
    <div class="container">
        <div class="page uc-page tabbar js_show">
            <div class="page__bd" style="height:100%;" >
                <div class="weui-tab">
                    <div class="weui-tab__panel">                       
                        <div class="panel__hd" style="background-color:#e64340; height:8rem;padding-top:2rem;padding-bottom:1rem;">
                            <div class="photo">
                                <img src="../images/def.gif" />
                            </div>
                            <h4>-</h4>
                            <h5>-</h5>                            
                        </div> 
                        <div class="weui-cells">
                            <a class="weui-cell weui-cell_access" href="Fee.aspx">
                                <div class="weui-cell__hd"><i class="iconfont icon-jieban"></i></div>
                                <div class="weui-cell__bd">
                                    <p>员工管理</p>
                                </div>
                                <div class="weui-cell__ft"></div>
                            </a>                            
                            <a class="weui-cell weui-cell_access" href="notify.aspx">
                                <div class="weui-cell__hd"><i class="iconfont icon-wodefankui"></i></div>
                                <div class="weui-cell__bd">
                                    <p>通知消息</p>
                                </div>
                                <div class="weui-cell__ft"></div>
                            </a>
                        </div>
                            <div class="weui-cells">
                            <a class="weui-cell weui-cell_access" href="Help.html">
                                <div class="weui-cell__hd"><i class="iconfont icon-bangzhu"></i></div>
                                <div class="weui-cell__bd">
                                    <p>帮助中心</p>
                                </div>
                                <div class="weui-cell__ft"></div>
                            </a>
                            <a class="weui-cell weui-cell_access" href="Service.html">
                                <div class="weui-cell__hd"><i class="iconfont icon-kefu"></i></div>
                                <div class="weui-cell__bd">
                                    <p>联系客服</p>
                                </div>
                                <div class="weui-cell__ft"></div>
                            </a>
                            <a class="weui-cell weui-cell_access" href="ChangePwd.aspx">
                                <div class="weui-cell__hd"><i class="iconfont icon-suo"></i></div>
                                <div class="weui-cell__bd">
                                    <p>修改密码</p>
                                </div>
                                <div class="weui-cell__ft"></div>
                            </a>
                            <a class="weui-cell weui-cell_access" href="javascript:;">
                                <div class="weui-cell__hd"><i class="iconfont icon-tuichu"></i></div>
                                <div class="weui-cell__bd">
                                    <p>退出系统</p>
                                </div>
                                <div class="weui-cell__ft"></div>
                            </a>
                        </div>
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
<script src="//r.edmp.cc/seascape/common.js"></script>
<script src="../script/worker.js"></script>
<script src="../script/WeChat.js"></script>
</html>
