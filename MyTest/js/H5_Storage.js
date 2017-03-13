var H5 = H5 || {};//全站命名空间
H5.StorageObj = {

    Comm: {
        HistoryCount: 15,
        SearchPatrn: new RegExp("[`~@#$^&*=|_{}()':\\[\\].+￥%<>/~！#￥…（）—|{}【】‘：']"),
        $proSearchBox: $('#divSearch'),
        $searchInput: $proSearchBox.find('input'),
        divHistory: $(".history"),
        //if (SearchKey != '')
        //        $searchInput.val(SearchKey);
    },
    Utils: {

    },
};