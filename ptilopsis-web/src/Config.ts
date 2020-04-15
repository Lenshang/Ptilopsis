  
let Config:any={}

Config.env=process.env.REACT_APP_ENV;

if(Config.env=="development"){
    Config.host="http://127.0.0.1:6505"
}
else if(Config.env=="build"){
    Config.host=""
}
export default Config;