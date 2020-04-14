import React from 'react';
import logo from './logo.svg';
import Main from './Pages/Main'
import 'antd/dist/antd.css';
//import './App.css';
import { BrowserRouter, Switch, Route } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <Main></Main>
      {/* <Switch>
        <Route key={1} path="/" component={Main} />
      </Switch> */}
    </BrowserRouter>
  );
}

export default App;
