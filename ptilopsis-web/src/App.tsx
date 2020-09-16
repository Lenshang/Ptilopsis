import React from 'react';
import logo from './logo.svg';
import Main from './Pages/Main';
import Login from './Pages/Login';
import 'antd/dist/antd.css';
import Http from './Utils/Http';
import ExLoading from './components/ExLoading';
//import './App.css';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import { AxiosError, AxiosRequestConfig } from 'axios';
import { Console } from 'console';
import { request } from 'http';

function App() {
  Http.onError=(err: AxiosError)=>{
    console.log(err);
    if(err?.response?.status==401){
      window.location.href="/login"
    }
    return err.response;
  }
  Http.beforeRequest=(request:AxiosRequestConfig)=>{
    var bearerToken=localStorage.getItem("token");
    if(bearerToken){
      if(!request.headers){
        request.headers={}
      }
      request.headers["Authorization"]="Bearer "+bearerToken;
    }
    return request;
  }
  return (
    <BrowserRouter>
      {/* <Main></Main> */}
      <Switch>
        <Route key={0} path="/login" exact component={Login}></Route>
        <Route key={1} path="/" component={Main} />
      </Switch>
      <ExLoading/>
    </BrowserRouter>
  );
}

export default App;
