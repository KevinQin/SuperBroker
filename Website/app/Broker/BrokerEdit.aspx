<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BrokerEdit.aspx.cs" Inherits="app_Broker_BrokerEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <title>我的资料</title>
    <link rel="stylesheet" href="//r.edmp.cc/weui/weui.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/animate/animate.min.css"/>
    <link rel="stylesheet" href="//r.edmp.cc/swiper/swiper-3.4.0.min.css" />
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
                            <input type="hidden" id="txtBrokerId" value="0" />
                            <span id="txtName">-</span>
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
                            <input class="weui-input" type="number" id="txtMobile" required="required" pattern="[0-9]{11}" placeholder="请输入手机号码"/>
                        </div>
                    </div> 
                    <div class="weui-cell">                        
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="number" id="txtTel" placeholder="填写您的备用电话"/>
                        </div>
                    </div> 
                    <div class="weui-cell">                       
                        <div class="weui-cell__bd">
                            <input type="hidden" id="txtCity" value="" />
                            <span id="txtCity_" class="gray" data-msg="选择您所在的城市">选择您所在的城市</span>
                        </div>              
                    </div>                   
                    <div class="weui-cell">                        
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" id="txtAddress"  placeholder="填写您的详细地址"/>
                        </div>              
                    </div>
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input type="hidden" id="txtTrade" value="" />
                            <span id="txtTrade_"  class="gray"  data-msg="选择您所属的行业">选择您所属的行业</span>
                        </div>              
                    </div>
                    <div class="weui-cell">                        
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" id="txtCompany"  placeholder="填写所在的单位名称"/>
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
<script src="//r.edmp.cc/swiper/swiper-3.4.0.jquery.min.js"></script>
<script src="//r.edmp.cc/seascape/dateclass.js"></script>
<script src="//r.edmp.cc/seascape/common.js"></script>
<script src="../script/citydata.js"></script>
<script src="../script/broker.js"></script>
<script src="../script/WeChat.js"></script>
</html>
