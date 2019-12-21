export default class HttpClient{
    /**
     * URL访问时的前缀
     */
    host:string;
    headers:any;
    /**
     * 初始化一个Http辅助类
     * @param host Url访问时的前缀
     */
    constructor(host:string=""){
        this.host=host;
        this.headers={}
    }

    async Get(url:string){
        return await fetch(url);
    }
    
}