/*
    新版验证码的通用部分的代码
    ZHAOs@2016年8月30日19:56:26
*/
var vcCodeUtil = {
    getCharException: "请求异常，请重试",
    reqVcCodeToomany: "请求验证码太频繁，请稍后再试",
    reqVcCodeException: "请求异常，请稍后重试",
    //vcCanvasClickMiss: "点击位置不正确，请重试",
    vcCanvasClickMiss: "",
    vcCodePassed: "验证通过",
    //-----------------------------------------------------------------------
    canvasWidth: 0,
    canvasHeight: 0,
    containerWidth: 0,
    btnWidth: 0,

    isAdvancedVcCode: false,
    isVcCodeValidated: false,
    isDragEnabled: true,
    //------------------------
    $dialogObj: null,

    $dragBtn: null,
    $dragBtnLeft: null,
    $tips: null,
    $selectedChar: null,
    $vcCanvas: null,
    $dragBtnContainer: null,
    $canvasContainer: null,
    $vcCodeRefresh: null,
    //--
    key: null,
    //--
    canvasClickRightCallback: null,
    canvasClickTooManyCallback:null,

    dialogReadyFun: function (option) {
        var that = this;
        var defaultOption = {
            canvasWidth: 260,
            canvasHeight: 138
        };
        option = $.extend(defaultOption, option);

        that.$dialogObj = option.$dialogObj;
        that.key = option.key;
        that.canvasWidth = option.canvasWidth;
        that.canvasHeight = option.canvasHeight;
        that.canvasClickRightCallback = option.canvasClickRightCallback;
        that.canvasClickTooManyCallback = option.canvasClickTooManyCallback;
        that.isAdvancedVcCode = option.isAdvancedVcCode;
        that.isVcCodeValidated = option.isVcCodeValidated;
        that.isDragEnabled = option.isDragEnabled;

        that.$dragBtn = that.$dialogObj.find("#dragBtn");
        that.$dragBtnLeft = that.$dialogObj.find("#dragBtnLeft");
        that.$tips = that.$dialogObj.find("#vcCodeTips");
        that.$selectedChar = that.$dialogObj.find("#selectedChar");
        that.$vcCanvas = that.$dialogObj.find("#vcCanvas");
        that.$dragBtnContainer = that.$dialogObj.find("#dragBtnContainer");
        that.$canvasContainer = that.$dialogObj.find("#canvasContainer");
        that.$vcCodeRefresh = that.$dialogObj.find("#vcCodeRefresh");

        that.containerWidth = that.$dragBtnContainer.outerWidth();
        that.btnWidth = that.$dragBtn.outerWidth();

        that.$dragBtn.draggable({
            containment: '#dragBtnContainer',
            start: function () {
                if (that.isDragEnabled === false) {
                    return false;
                }
                that.$tips.text("");
            },
            drag: function (event, ui) {
                var dis = ui.position.left;
                that.$dragBtnLeft.css("width", dis + "px");
            },
            stop: function (event, ui) {
                var dis = ui.position.left;
                if (dis < that.containerWidth - that.btnWidth) {
                    that.$dragBtn.animate({ "left": "0" });
                    that.$dragBtnLeft.animate({ "width": "0" });
                }
                else {

                    alert('滑到右侧了');

                    GetJPData("http://localhost:9938/VcCode/BLL/API.ashx", 'getVcChar', "key=" + that.key, function (data) {
                        if (data.state == 1) {
                            //达到请求次数上限
                            //that.resetVcCode();
                            //that.$tips.text(that.reqVcCodeToomany);
                            if (!!that.canvasClickTooManyCallback) {
                                that.canvasClickTooManyCallback();
                            }
                            return false;
                        } else if (data.state == 0) {
                            that.$vcCodeRefresh.show();

                            var text = data.vcChar;
                            that.$selectedChar.text(text);
                            that.$canvasContainer.addClass("canvas-showloading");
                            that.$dragBtnContainer.children(".vc-slide-text").hide();
                            that.$dragBtnContainer.children(".vc-slideBtnLeft").find("span").show();
                            that.$dragBtn.css({ "float": "left", "left": dis + "px" });
                            that.$vcCanvas.attr("src", "http://localhost:9938/VcCode/GetVcImg.html?" + that.getVcImgParam(data.str)).load(function () {
                                that.$vcCanvas.show();
                            });
                            that.isDragEnabled = false;
                            that.isVcCodeValidated = false;
                        }
                    });
                }
            }
        });
        that.toolEvtReg();
    },
    canvasClickActive:true, //为了防止对验证码的多次点击，为true时点击有效，否则点击无效
    toolEvtReg: function () {
        var that = this;

        //刷新验证码
        var _IsRefresh = null;
        that.$vcCodeRefresh.click(function () {
            if (_IsRefresh != null) { return; }
            _IsRefresh = setTimeout(function () { _IsRefresh = null; }, 200);
            that.showVcCode();
            that.$tips.text("");
        });

        that.$vcCanvas.click(function (e) {

            if (that.canvasClickActive === false) {
                return false;
            }

            that.canvasClickActive = false;
            var pPageX = that.$vcCanvas.offset().left;
            var pPageY = that.$vcCanvas.offset().top;
            var offsetX = e.pageX - pPageX;
            var offsetY = e.pageY - pPageY;

            //去比较点击位置
            GetJPData("http://localhost:9938/VcCode/BLL/API.ashx", 'VcCompare', "x=" + offsetX + "&y=" + offsetY, function (data) {

                that.canvasClickActive = true;

                //点击位置不对
                if (data.state == 1) {
                    if (data.num == 1) {
                        that.$tips.text(that.vcCanvasClickMiss);
                        that.showVcCode();
                        return false;
                    } else {
                        //请求次数超限
                        //that.resetVcCode();
                        //that.$tips.text(that.reqVcCodeToomany);

                        if (!!that.canvasClickTooManyCallback) {
                            that.canvasClickTooManyCallback();
                        }

                        return false;
                    }
                }
                else if (data.state == 0) {
                    that.$tips.text(that.vcCodePassed);
                    that.isVcCodeValidated = true;
                    if (!!that.canvasClickRightCallback) {
                        that.canvasClickRightCallback(data.str);
                    }
                }
            });
        });
    },
    resetVcCode: function () {
        var that = this;
        that.$dragBtnLeft.css("width", "0");
        that.$dragBtn.animate({ "left": "0" });
        that.$dragBtnContainer.children(".vc-slide-text").show();
        that.$dragBtnContainer.children(".vc-slideBtnLeft").find("span").hide();
        that.$canvasContainer.removeClass("canvas-showloading");
    },
    showVcCode: function () {
        var that = this;
        that.$vcCanvas.hide();
        //前台无法获取当前用户名，因此用户名采用后台获取的方法
        GetJPData("http://localhost:9938/VcCode/BLL/API.ashx", 'getVcChar', "key=" + that.key, function (data) {
            if (data.state == 1) {
                //达到请求次数上限
                //that.resetVcCode();
                //that.$tips.text(that.reqVcCodeToomany);
                if (!!that.canvasClickTooManyCallback) {
                    that.canvasClickTooManyCallback();
                }
                return false;
            }
            else if (data.state == 0) {
                var text = data.vcChar;
                that.$selectedChar.text(text);
                that.$canvasContainer.addClass("canvas-showloading");
                that.$dragBtnContainer.children(".vc-slide-text").hide();
                that.$dragBtnContainer.children(".vc-slideBtnLeft").find("span").show();

                that.$vcCanvas
                    .attr("src", "http://localhost:9938/VcCode/GetVcImg.html?" + that.getVcImgParam(data.str))
                    .load(function () {
                    that.$vcCanvas.show();
                });

                that.isDragEnabled = false;
                that.isVcCodeValidated = false;
            }
        });
    },
    getVcImgParam: function (vcCharCookieKey) {
        var that = this;
        var result = "width=" + that.canvasWidth +
            "&height=" + that.canvasHeight +
            "&vcCharCookieKey=" + vcCharCookieKey;
        return result;
    }
};