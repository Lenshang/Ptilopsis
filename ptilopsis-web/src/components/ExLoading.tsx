import React from 'react';
import { message } from 'antd';
import {StarOutlined} from '@ant-design/icons';
import './ExLoading.css';
export interface IProps {
}
export interface IState {
    show: boolean;
    icon: boolean;
    loadingMessage: string | React.Component;
}
export default class ExLoading extends React.Component<IProps, IState>{
    static instance?: ExLoading;
    constructor(props: IProps) {
        super(props);
        this.state = {
            show: false,
            icon: false,
            loadingMessage: "加载中..."
        }
    }

    componentWillMount() {
        if (!ExLoading.instance) {
            ExLoading.instance = this;
        }
        else {
            throw new Error("该组件只能渲染一次!");
        }
    }

    componentWillUnmount() {
        ExLoading.instance = undefined;
    }

    render() {
        if (this.state.show) {
            return (
                <div id="exloading" className="ant-modal-mask">
                    <div className="mask">
                        <div className="mask-loading">
                            {this.state.icon ? (
                                <StarOutlined spin style={{ fontSize: 25 }}/>
                            ) : (null)}
                            <span style={{ marginLeft: 20 }}>{this.state.loadingMessage}</span>
                        </div>
                    </div>
                </div>)
        }
        return (null);
    }

    static show = (showIcon: boolean = false, message: string | React.Component = "Loading...") => {
        if (ExLoading.instance) {
            ExLoading.instance!.setState({
                show: true,
                icon: showIcon,
                loadingMessage: message
            });

            return () => {
                ExLoading.instance!.setState({
                    show: false,
                    icon: false,
                    loadingMessage: "加载中..."
                });
            };
        }
        return () => { };
    }

    static hide = () => {
        if (ExLoading.instance) {
            return () => {
                ExLoading.instance!.setState({
                    show: false,
                    icon: false
                });
            };
        }
    }

    static info = (msg: any) => {
        if (ExLoading.instance) {
            var hide = message.info(msg, 0);
            ExLoading.instance!.setState({
                show: true,
                icon: false
            });

            return () => {
                hide();
                ExLoading.instance!.setState({
                    show: false,
                    icon: false,
                    loadingMessage: "加载中..."
                });
            };
        }
        return () => { };
    }

    static error = (msg: any) => {
        if (ExLoading.instance) {
            var hide = message.error(msg, 0);
            ExLoading.instance!.setState({
                show: true,
                icon: false
            });

            return () => {
                hide();
                ExLoading.instance!.setState({
                    show: false,
                    icon: false,
                    loadingMessage: "加载中..."
                });
            };
        }
        return () => { };
    }
}