import React, { ChangeEvent } from 'react';
import { Input } from 'antd';
import { InputProps } from 'antd/lib/input/Input';
import ObjectHelper from '../utils/objectHelper'
import ExTextArea from './exTextArea';
const { TextArea } = Input;
interface IProps extends InputProps {
    refName?:string;
    bind?:string;
    creator:any;
}
interface IState{
    value:string;
}
class ExInput extends React.Component<IProps,IState> {
    static ExTextArea: typeof ExTextArea;
    constructor(props: IProps) {
        super(props);
        this.state={
            value:""
        }
    }
    handleChange(e:ChangeEvent<HTMLInputElement>) {
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
            <Input value={this.state.value} onChange={this.handleChange.bind(this)} {...this.props}></Input>
        )
    }
}

export default ExInput;