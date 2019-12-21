/**
 * Object帮助类
 */
export default class ObjectHelper{
    /**
     * 根据jsonPath自动解析创建collection
     * @param exp 表达式
     * @param value 赋的值
     * @param _col 原有对象
     */
    static createCollection(exp:string,value:any,_col:any=null){
        var _r={};
        if(_col){
            _r=_col;
        }
        var exps=exp.split(".");
        var currentSubObj:any=null;
        for(let i:number=0;i<exps.length;i++){
            var name=exps[i];
            if(i==0){
                currentSubObj=_r;
            }

            if(!currentSubObj[name]){
                currentSubObj[name]={};
            }

            if(i==exps.length-1){
                currentSubObj[name]=value;
            }
            currentSubObj=currentSubObj[name];
        }
        return _r;
    }
}