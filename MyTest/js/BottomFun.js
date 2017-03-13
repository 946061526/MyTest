var Gobal = new Object();
function GetJPData(domain, action, para, callfun) {
    if (domain == "")
        domain = _Domain;
    $.getJSON(domain + "/JPData?action=" + action + "&" + para + "&fun=?", callfun);
}

function loadImgFun(n) {
    var PicBox = $("#loadingPicBlock");
    if (PicBox.length > 0) {
        var OldSrc = "src2";
        Gobal.LoadImg = PicBox.find("img[" + OldSrc + "]");
        var scrollTop = function () { return $(window).scrollTop() };
        var NowHeight = function () { return $(window).height() + scrollTop() + 50 };
        var loadPic = function () {
            Gobal.LoadImg.each(function (i) {
                if ($(this).offset().top <= NowHeight()) {
                    var Src = $(this).attr(OldSrc);
                    if (Src) {
                        $(this).attr("src", Src).removeAttr(OldSrc).show();
                    }
                }
            });
        };
        var nowScrollTop = 0;
        var oldScrollTop = -100;
        var CheckFun = function () {
            nowScrollTop = scrollTop();
            if (nowScrollTop - oldScrollTop > 50) {
                oldScrollTop = nowScrollTop;
                loadPic();
            }
        };
        if (n == 0) {
            $(window).bind("scroll", CheckFun);
        }
        CheckFun();
    }
}

//滚动加载数据
var _IsLoading = false;
function scrollForLoadData(getDataFunc) {
    $(window).scroll(function () {
        var _TotalHeight = $(document).height();
        var _WinHeight = $(window).height();
        var _CurHeight = $(window).scrollTop() + _WinHeight;
        if (_TotalHeight - _CurHeight <= _WinHeight * 4) {
            if (!_IsLoading && getDataFunc) {
                _IsLoading = true;
                getDataFunc();
            }
        }
    });
}

//清除左右空格
String.prototype.trim = function () { return this.replace(/(\s*$)|(^\s*)/g, ''); };
//清除所有空格
String.prototype.trims = function () { return this.replace(/\s/g, ''); };

var _Domain, passportUrl;
(function () {
    var _IsHttps = $("#hidIsHttps").val() == "1" ? true : false;
    _Domain = _IsHttps ? "https://json.1yyg.com" : "http://json.1yyg.com";
    //站点样式及脚本所在域名Url
    Gobal.Skin = _IsHttps ? "https://skin.1yyg.net" : "http://skin.1yyg.net";
    //短信站点
    passportUrl = _IsHttps ? 'https://passport.1yyg.com' : 'http://passport.1yyg.com';
    //需要异步加载图片对象
    Gobal.LoadImg = null;

    //延时加载页面图片
    loadImgFun(0);
    var loadPageJS = function () {

        /*加载页面的脚本*/
        var setSrc = function (obj) {
            var D = new Date();
            obj.attr('src', obj.attr('data') + '?v=' + GetVerNum()).removeAttr('id').removeAttr('data');
        }
        var _ThePageJS = $("#pageJS", 'head');
        if (_ThePageJS.length > 0) {
            setSrc(_ThePageJS);
        } else {
            _ThePageJS = $("#pageJS", 'body');
            if (_ThePageJS.length > 0) { setSrc(_ThePageJS); }
        };
    }
    loadPageJS();
})()