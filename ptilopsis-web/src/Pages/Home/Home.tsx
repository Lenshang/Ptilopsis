import React from 'react';
import Http from '../../Utils/Http';
interface IProps {

}
interface IState {
    message:string
}

export default class Home extends React.Component<IProps, IState>{
    constructor(props:IProps){
        super(props);
        this.state={
            message:""
        }
    }
    componentDidMount(){
        this.getHomeMessage();
    }
    async getHomeMessage(){
        let response: any = await Http.get("/api/home/get");
        response = response.data;
        if (response.success) {
            this.setState({message:response.data})
        }
    }
    render(){
        return (<div>{this.state.message}</div>)
    }
}