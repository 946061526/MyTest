// JavaScript Document
(function($) {
	//显示对话框
    $.PageDialog = function(msg, options){
		//默认参数值
        var settings = {
            W: 255,		//对话框宽，像素
			H: 45,		//对话框高，像素
			obj: null,	//初始位置对象
			oL: 0,		//相对对象左侧偏移
			oT: 0,		//相对对象上侧偏移
			autoClose: true,	//是否自动关闭
			autoTime: 1000,	//自动关闭的延时时间
			ready: function(){},//初始显示前执行
			submit: function () { },//关闭窗口后执行
			isTop: true//是否像上偏移
        };
        var defSetting = {
			obj: null,
			oL: 0,
			oT: 0,
			autoClose: true,
			autoTime: 2000,
			ready: function(){},
			submit: function(){}
        };
		//extending options
		options = options || defSetting;
		$.extend(settings, options);
		var _AC = settings.autoClose;
		//屏蔽触屏拉动
		var _DisTouch = function(obj){
		    var el = obj.get(0);
				el.addEventListener('touchstart', onTouchStart, false);
				function onTouchStart(e) {
					if (e.touches.length === 1) {												
						el.addEventListener('touchmove', onTouchMove, false);
						el.addEventListener('touchend', onTouchEnd, false);
					}
				}
				
				function onTouchMove(e) {
					e.preventDefault();
				}
				
				function onTouchEnd(e) {
					//e.preventDefault();//启用的话在ios上会屏蔽子元素的操作
					el.removeEventListener('touchmove', onTouchMove, false);
					el.removeEventListener('touchend', onTouchEnd, false);					
				}
			}
		//检测是否存在框架,不存在创建
		var _DialogBG=$("#pageDialogBG");
		if (!_AC) {
		    if (_DialogBG.length == 0) {
		        _DialogBG = $('<div id="pageDialogBG" class="pageDialogBG"></div>');
		        _DialogBG.appendTo('body');
		    } else {
		        _DialogBG = $("#pageDialogBG");
		    }
		    _DialogBG.css('height', $(document).height() > $(window).height() ? $(document).height() : $(window).height());
			_DisTouch(_DialogBG);
		}
		var _Dialog=$("#pageDialog");
		if(_Dialog.length == 0){
			_Dialog = $('<div id="pageDialog" class="pageDialog" />');
			_Dialog.appendTo('body');
			if(!_AC){
				_DisTouch(_Dialog);
			}
		}
		var winObj=$(window);
		if (typeof(settings.obj)!="undefined"&&settings.obj != null) {
			if(settings.obj.length < 1){settings.obj = null;}
		}
        //处理内容和弹窗宽度不一致问题
		var removeHTMLTag = function (str) {
		    str = str.replace(/<\/?[^>]*>/g, ''); //去除HTML tag
		    str = str.replace(/[ | ]*\n/g, '\n'); //去除行尾空白
		    str = str.replace(/ /ig, '');//去掉 
		    return str;
		}
        //(lth20160606去掉，系统自动宽度会导致无法确定弹窗宽度，从而导致弹窗位置计算不准确)
		//var _width = settings.W;
		//if (_width == 255 && settings.H == 45) {
		//    msg = msg.replace(/！/ig, '');
		//    msg = msg.replace(/!/ig, '');
		//    var _padding = 10;
		//    if (msg.indexOf("<s></s>") > -1) {
		//        _padding = 50;
		//    }
		//    var _rMsg = removeHTMLTag(msg);
		//    if (_rMsg.length > 0) {
		//        _width = 20 * _rMsg.length + _padding;
		//        if (_width >= ($(window).width() - 20)) {
		//            _width = $(window).width() - 20;
		//        }
		//    }
		//    settings.W = _width;
		//}
	   
		//设置弹窗大小
		_Dialog.css({
		    width: settings.W + 'px',
		    height: settings.H + 'px'
		})
		//填充内容
		_Dialog.html(msg);

		//确定显示位置
		var positionPrompt = function () {
		   
			var vl,vt,pos;
			if(settings.obj != null){
				var objOffset=settings.obj.offset();
				vl = objOffset.left+settings.oL;
				vt = objOffset.top+settings.obj.height()+settings.oT;
				pos = 'absolute';
			}else{
				vl = (winObj.width()-settings.W)/2;
				vt = (winObj.height() - settings.H) / 2;
			    //弹窗距离顶部高度减小一半
				if (settings.isTop) {
				    vt = vt - vt / 2;
				}
				pos = 'fixed';
			}
		    _Dialog.css({
		        position: pos,
		        left: vl,
		        top: vt
		    });

		}
		
		positionPrompt();

		winObj.resize(positionPrompt);
		
	
		//winObj.scroll(resizePrompt);
		
		//关闭弹窗事件
		var cancelDialog = function(){
			//winObj.unbind('resize');
			//这里会影响滚动加载图片
		    //winObj.unbind('scroll');
		  
		    if (_AC) {
		        _Dialog.fadeOut("fast");
		        _DialogBG.hide();
			}else{
				_Dialog.hide();
				_DialogBG.hide();
			}
		}
		var closeDialog = function () {
		    settings.submit();
			cancelDialog();
		}
		//显示
		if (_AC) {
		    _Dialog.show();
		    //_Dialog.fadeIn("fast");
		}else{
		    //当不是自动关闭则显示背景
		    _Dialog.show();
		    _DialogBG.show();
		}
		//初始后执行事件
		_Dialog.ready = settings.ready();
		//定时关闭弹出层
		if (_AC) {
			window.setTimeout(closeDialog, settings.autoTime);
		}
		//关闭事件接口
		this.close = function () {
			closeDialog();
		}
		//取消关闭事件接口，不调用submit事件
		this.cancel = function () {
			cancelDialog();
		}
	};
	
	$.PageDialog.ok = function( str, closefun ) {
	    $.PageDialog('<div class="Prompt"><s></s>' + str + '</div>', { autoTime: 500, submit: typeof (closefun) == "function" ? closefun:function () { } });
	};
	$.PageDialog.fail = function( str,_obj,_oT,_oL,closefun ) {
	    $.PageDialog('<div class="Prompt">' + str + '</div>', { obj: _obj, oT: _oT, oL: _oL, autoTime: 1000, submit: typeof (closefun) == "function" ? closefun:function () { } });
	};
	$.PageDialog.fail2 = function (str, width, height, closefun) {
	    $.PageDialog('<div class="Prompt">' + str + '</div>', { W: typeof (width) == "number" ? width : 255, H: typeof (height) == "number" ? height : 45, autoTime: 2000, submit: typeof (closefun) == "function" ? closefun : function () { } });
	};
	var iii=0;
	$.PageDialog.confirm = function( str, okFun, height ) {
		var _Dialog = null;
		var _Str = '<div class="clearfix m-round u-tipsEject"><div class="u-tips-txt">'+str+'</div><div class="u-Btn"><div class="u-Btn-li"><a href="javascript:;" id="btnMsgCancel" class="z-CloseBtn">取消</a></div><div class="u-Btn-li"><a id="btnMsgOK" href="javascript:;" class="z-DefineBtn">确定</a></div></div></div>';
		var _ReadyFun = function(){
			$("#btnMsgCancel").click(function(){ _Dialog.cancel(); });
			$("#btnMsgOK").click(function(){ _Dialog.close();});
		}
		iii++;
		_Dialog = new $.PageDialog(_Str, { H: typeof (height) == "number" ? height:45, autoClose: false, ready: _ReadyFun, submit: okFun });
	};
	$.PageDialog.fail1 = function (str, _obj, closefun) {
	    var _oT = getTop(_obj);
	    var _oL = getLeft(_obj);
	    $.PageDialog('<div class="Prompt">' + str + '</div>', { obj: _obj, oT: _oT, oL: _oL, autoTime: 1000, submit: typeof (closefun) == "function" ?closefun:function () { } });
	};
	var getLeft = function (obj) {
	    var _width = $(obj).width() - 255;
	    var _tmp = _width > 0 ? _width : _width * -1;
	    var _Left = _tmp / 2;
	    return _Left;
	}
	var getTop = function (obj) {

	    return ($(obj).height() * 2 + 20) * -1;

	}
	
})(jQuery);