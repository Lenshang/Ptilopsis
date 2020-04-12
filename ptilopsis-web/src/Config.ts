  
let Config:any={}

Config.env=process.env.REACT_APP_ENV;
Config.fakeApi=false;
if(Config.env=="dev"){
    Config.fakeApi=true;
}

Config.host="http://127.0.0.1:6505"
export default Config;