function GetGameCodeName(strGameCode)
{
	switch (strGameCode)
	{
		case "CQSSC": return "重庆时时彩";
		case "DLT": return "大乐透";
		case "FC3D": return "福彩3D";
		case "GD11X5": return "广东11选5";
		case "GDKLSF": return "广东快乐十分";
		case "GXKLSF": return "广西快乐十分";
		case "JSKS": return "江苏快3";
		case "JX11X5": return "江西11选5";
		case "JXSSC": return "江西时时彩";
		case "PL3": return "排列三";
		case "QLC": return "七乐彩";
		case "QXC": return "七星彩";
		case "SD11X5": return "山东11选5";
		case "SDQYH": return "山东群英会";
		case "SSQ": return "双色球";
		case "CQ11X5": return "重庆11选5";
		case "CQKLSF": return "重庆快乐十分";
		case "DF6J1": return "东方6+1";
		case "HBK3": return "湖北快3";
		case "HC1": return "好彩一";
		case "HD15X5": return "华东15选5";
		case "HNKLSF": return "湖南快乐十分";
		case "JLK3": return "新快3";
		case "JSK3": return "江苏快3";
		case "LN11X5": return "辽宁11选5";
		case "PL5": return "排列五";
		case "BJDC": return "北京单场";
		case "JCZQ": return "竞彩足球";
		case "JCLQ": return "竞彩篮球";
		case "CTZQ": return "传统足球";
		default: return '';
	}
}
// 购彩方式
function GetBuyTypeName(buyType)
{
    switch (buyType) {
        case 1:
            return "自购";
        case 2:
            return "追号";
        case 3:
        default:
            return "自购";
    }
}

//分析url
function parseURL(url) {
    var a = document.createElement('a');
    a.href = url;
    return {
        source: url,
        protocol: a.protocol.replace(':', ''),
        host: a.hostname,
        port: a.port,
        query: a.search,
        params: (function () {
            var ret = {},
                seg = a.search.replace(/^\?/, '').split('&'),
                len = seg.length, i = 0, s;
            for (; i < len; i++) {
                if (!seg[i]) { continue; }
                s = seg[i].split('=');
                ret[s[0]] = s[1];
            }
            return ret;

        })(),
        file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
        hash: a.hash.replace('#', ''),
        path: a.pathname.replace(/^([^\/])/, '/$1'),
        relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
        segments: a.pathname.replace(/^\//, '').split('/')
    };
}

//替换myUrl中的同名参数值 
function replaceUrlParams(myUrl, newParams) {
    if (newParams.length == 0) {
        myUrl.params = {};
    }
    else {
        for (var x in newParams) {
            var hasInMyUrlParams = false;
            for (var y in myUrl.params) {
                if (x.toLowerCase() == y.toLowerCase()) {
                    myUrl.params[y] = newParams[x];
                    hasInMyUrlParams = true;
                    break;
                }
            }
            //原来没有的参数则追加 
            if (!hasInMyUrlParams) {
                myUrl.params[x] = newParams[x];
            }
        }
    }
    var _result = myUrl.protocol + "://" + myUrl.host + ":" + myUrl.port + myUrl.path + "?";

    for (var p in myUrl.params) {
        _result += (p + "=" + myUrl.params[p] + "&");
    }

    if (_result.substr(_result.length - 1) == "&") {
        _result = _result.substr(0, _result.length - 1);
    }

    if (myUrl.hash != "") {
        _result += "#" + myUrl.hash;
    }
    return _result;
}

// {key: value, key2, value}
function publicPagingList(obj) {
    //获取url
    var url = location.href;
    //替换或追加url
    var myURL = parseURL(url);
    var _newUrl = replaceUrlParams(myURL, obj);
    //window.location.href = encodeURIComponent(_newUrl);
    window.location.href = _newUrl;
}

function PageIndexChange(obj, max) {
    var val = obj.value;
    if (val > max)
        obj.value = max;
    else if (val < 1)
        obj.value = 1;
}
$("#btn_ToPage").click(function () {
    var pageIndex = $("#pageIndex").val();
    publicPagingList({ pageIndex: pageIndex });
});

$("#pageSizeList").change(function () {
    var pageSize = $(this).val()
    publicPagingList({ pageSize: pageSize, pageIndex: 1 });
})

