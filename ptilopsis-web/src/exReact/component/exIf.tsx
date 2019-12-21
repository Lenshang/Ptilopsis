import React, { ChangeEvent } from 'react';
import ObjectHelper from '../utils/objectHelper';
interface IProps {
    refName?:string;
    bind?:boolean;
    creator?:any;
}
interface IState{

}
class ExIf extends React.Component<IProps,IState>{
    constructor(props: IProps){
        super(props);
    }
    componentWillMount() {
        if (this.props.refName && this.props.creator) {
            var obj:{}=ObjectHelper.createCollection(this.props.refName!,this);
            this.props.creator.setState(obj)
        }
        console.log(this.props.children);
    }
    render(){
        if(this.props.bind){
            return (
                this.props.children
            );
        }
        else{
            return(<></>);
        }
    }
}

export default ExIf;