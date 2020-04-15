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