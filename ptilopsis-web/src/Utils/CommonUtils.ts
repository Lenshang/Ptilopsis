export function getSearchByName(search:string,name: string) {
    search = search.substr(1);
    if (typeof name === 'undefined') return search;
    var searchArr: Array<any> = search.split('&');
    for (var i = 0; i < searchArr.length; i++) {
        var searchStr = searchArr[i];
        searchArr[i] = searchStr.split('=');
        if (searchArr[i][0] == name) {
            return searchStr.replace(name + '=', '');
        }
    }
    return '';
}

export function sleep(mSeconds:number){
    return new Promise((resolve: any, reject: any) => {
        setInterval(() => {
            resolve();
        }, mSeconds);
    });
}
function getzf(num:any) {
    if(parseInt(num) < 10) {
    　　num = '0' + num;
    }
    　　return num;
}

export function getMyDate(str:any) {
    　　var oDate = new Date(str),
    　　oYear = oDate.getFullYear(),
    　　oMonth = oDate.getMonth() + 1,
    　　oDay = oDate.getDate(),
    　　oHour = oDate.getHours(),
    　　oMin = oDate.getMinutes(),
    　　oSen = oDate.getSeconds(),
    　　oTime = oYear + '-' + getzf(oMonth) + '-' + getzf(oDay) + ' ' + getzf(oHour) + ':' + getzf(oMin) + ':' + getzf(oSen); //最后拼接时间
    　　return oTime;
};

export function getTaskState(str:string){
    let map:{[key:string]:string}={
        "0":"准备中",
        "1":"运行",
        "2":"停止"
    }
    return map[str];
}