import React, { ChangeEvent } from 'react';
import { Input } from 'antd';
import ObjectHelper from '../utils/objectHelper'
import { TextAreaProps } from 'antd/lib/input/TextArea';
const { TextArea } = Input;
interface IProps extends TextAreaProps {
    refName?:string;
    bind?:string;
    creator:any;
}
interface IState{
    value:string;
}
class ExTextArea extends React.Component<IProps,IState> {
    constructor(props: IProps) {
        super(props);
        this.state={
            value:""
        }
    }
    handleChange(e:ChangeEvent<HTMLTextAreaElement>) {
        this.setState({
            value: e.target.value
        });

        if (this.props.bind && this.props.creator) {
            var obj:{}=ObjectHelper.createCollection(this.props.bind!,e.target.value);
            this.props.creator.setState(obj)
        }

        this.props.onChange?.call(this,e);
    }

    componentWillMount() {
        if (this.props.refName && this.props.creator) {
            var obj:{}=ObjectHelper.createCollection(this.props.refName!,this);
            this.props.creator.setState(obj)
        }
    }

    getValue() {
        return this.state.value;
    }

    setValue(value:string) {
        this.setState({
            value: value
        });
    }

    render() {
        return (
            <TextArea value={this.state.value} onChange={this.handleChange.bind(this)} {...this.props}></TextArea>
        )
    }
}

export default ExTextArea;