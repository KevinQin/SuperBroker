//微信验证成功后的回调
if (typeof(wx) != 'undefined') {
    wx.ready(wechatConfigSuccess);

    wx.error(function (res) {         
        console.debug("WeChat Error：" + res.errMsg);       
    });
}

function initWeChat(config) {
    if (config != null) { WxConfigInfo = config;}
    wx.config({
        debug: false,
        appId: WxConfigInfo.appId,
        timestamp: WxConfigInfo.timestamp,
        nonceStr: WxConfigInfo.nonceStr,
        signature: WxConfigInfo.signature,
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage',
                    'onMenuShareQQ', 'onMenuShareWeibo', 'hideMenuItems',
                    'showMenuItems', 'hideAllNonBaseMenuItem', 'showAllNonBaseMenuItem',
                    'translateVoice', 'startRecord', 'stopRecord', 'onRecordEnd',
                    'playVoice', 'pauseVoice', 'stopVoice', 'uploadVoice',
                    'downloadVoice', 'chooseImage', 'previewImage', 'uploadImage',
                    'downloadImage', 'getNetworkType', 'openLocation',
                    'getLocation', 'hideOptionMenu', 'showOptionMenu',
                    'closeWindow', 'scanQRCode', 'chooseWXPay', 'openProductSpecificView',
                    'addCard', 'chooseCard', 'openCard']
    });
}

function preShare(id) {
    var obj = {
        title: '山西出行酒店预订上线啦，欢迎预订！',
        desp: '山西出行在线酒店上线啦，欢迎预订！',
        link: "http://hjhk.edmp.cc/app/hotel/share.html",
        logo: "http://hjhk.edmp.cc/img/logo.jpg",
        success: function () { },
        cancel: function () { }
    };
    initWeChatShare(obj);
}  

function wechatConfigSuccess() {
    /*if (PageName == "conformpay")
    {
        setTimeout(pay.preDopay, 50);
    } */
    if (PageName == "index") {
        setTimeout(IndexPage.getLocation, 50);
        setTimeout(preShare, 50);
    }
}

function initWeChatShare(obj) {
    wx.onMenuShareTimeline({
        title:obj.title,
        link: obj.link,
        imgUrl: obj.logo,
        success: obj.success,
        cancel: obj.cancel
    });

    wx.onMenuShareAppMessage({
        title: obj.title,
        desc: obj.desp,
        link: obj.link,
        imgUrl: obj.logo,
        type: '',
        dataUrl: '',
        success: obj.success,
        cancel: obj.cancel
    });
}

function imgBrower(currImg, imgs)
{    
    wx.previewImage({
        current: currImg, //当前显示图片的http链接
        urls: imgs // 需要预览的图片http链接列表
    });
}

function WxDoPay(sucCallBack, failCallBack) {
    wx.chooseWXPay({
        timestamp: WxPayConfigInfo.timestamp,
        nonceStr: WxPayConfigInfo.nonce,
        package: WxPayConfigInfo.package,
        signType: 'MD5',
        paySign: WxPayConfigInfo.signature,
        appid: WxPayConfigInfo.appid,
        success: function (res) {
            if (res.errMsg == "chooseWXPay:ok") {                
                sucCallBack();
            }
            else {
                failCallBack();
            }
        },
        cancel: function (res) {
            if (res.errMsg == "chooseWXPay:cancel") {
                failCallBack();
            }
        }
    });   
}


//Wechat调用
function chooseImage(res) {
    var localIds = res.localIds;
    ReportPage.picUrl = localIds;
    $(".input:eq(2) span").text("已选你喜欢的照片");
}

var serverIds = [];
var upoverCallBack = null;
function uploadImage(callback) {
    upoverCallBack = callback;
    updateImageToWeChat();
}

function updateImageToWeChat() {
    try {
        var localId = ReportPage.picUrl.toString();
        //alert(localId);
        wx.uploadImage({
            localId: localId,
            isShowProgressTips: 1,
            success: function (res) {
                var serverId = res.serverId;
                //alert(serverId);
                upoverCallBack(serverId);
            }
        });
    } catch (ex) {
       // alert(ex);
    }
   
}