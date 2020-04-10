import React from 'react';

interface IProps {

}
interface IState {

}

export default class AppList extends React.Component<IProps, IState>{
    constructor(props:IProps){
        super(props);
    }

    render(){
        return (<div>Application List</div>)
    }
}