import React from 'react';
import { Form, Input, Row, Col, Button, Upload, message } from 'antd';
import ExLoading from '../../components/ExLoading';
import Http from '../../Utils/Http';
import { Redirect, RouteComponentProps } from 'react-router-dom';
import { FormInstance } from 'antd/lib/form';
const { TextArea } = Input;
interface IParam {

}
interface IProps extends RouteComponentProps<IParam> {

}
interface IState {
    appId?:string,
    /**1=add 2=update 3=view*/
    pageType?: number,
    redirect: boolean
}

export default class AddTask extends React.Component<IProps, IState>{
    formInstance: FormInstance | null
    constructor(props: IProps) {
        super(props);
        this.state = {
            redirect: false,
            pageType: 1
        }
        this.formInstance = null;
    }
    componentDidMount() {
        let _state: any = {}
        let _appId=this.getSearchByName("appId")
        let _appName=decodeURI(this.getSearchByName("appName"))
        if (_appId) {
            _state.appId = _appId;
            if(this.formInstance){
                this.formInstance.setFieldsValue({
                    appId:_appId
                })
            }
        }
        
        if(_appName){
            if(this.formInstance){
                this.formInstance.setFieldsValue({
                    name:"快速启动-"+_appName
                })
            }
        }

        this.setState(_state);
    }

    getSearchByName(name: string) {
        var search = this.props.location.search;
        search = search.substr(1);
        if (typeof name === 'undefined') return search;
        var searchArr: Array<any> = search.split('&');
        for (var i = 0; i < searchArr.length; i++) {
            var searchStr = searchArr[i];
            searchArr[i] = searchStr.split('=');
            if (searchArr[i][0] == name) {
                return searchStr.replace(name + '=', '');
            }
        }
        return '';
    }

    onSubmit = async (values: any) => {
        ExLoading.show(true);
        let response = await Http.post("/api/task/add", values)
        ExLoading.hide();
        // const formData = new FormData();
        // formData.append('file', this.state.selectFile);

        // formData.append('name', values.name);
        // formData.append('runCmd', values.runCmd);
        // formData.append('description', values.description);
        // let response = await Http.post("/api/application/add", formData)
        ExLoading.hide();
        if (!response?.data.success) {
            message.error('App添加失败!', 10);
        }
        else {
            this.setState({
                redirect: true
            })
        }
    }

    render() {
        const formItemLayout = {
            labelCol: {
                xs: {
                    span: 24,
                },
                sm: {
                    span: 5,
                },
                lg: {
                    span: 8,
                },
            },
            wrapperCol: {
                xs: {
                    span: 24,
                },
                sm: {
                    span: 20,
                },
                lg: {
                    span: 10,
                },
                xl: {
                    span: 8
                },
                xxl: {
                    span: 6
                }
            },
        };
        const tailFormItemLayout = {
            wrapperCol: {
                xs: {
                    span: 24,
                    offset: 0,
                },
                sm: {
                    span: 16,
                    offset: 5,
                },
                lg: {
                    span: 16,
                    offset: 8,
                }
            },
        };
        const getButton = (pageType?: number) => {
            if (pageType == 3) {
                return (<Button onClick={e => this.setState({ redirect: true })}>返回</Button>);
            }
            if (pageType == 2) {
                return (<>
                    <Button type="primary" style={{ marginRight: 10 }} htmlType="submit">提交</Button>
                    <Button onClick={e => this.setState({ redirect: true })}>返回</Button>
                </>);
            }
            else {
                return (<Button type="primary" htmlType="submit">提交</Button>);
            }
        }
        return (
            <div style={{ backgroundColor: "white", padding: 25 }}>
                {this.state.redirect ? (<Redirect to="/task/taskmanager" />) : null}
                <Form ref={(instance) => { this.formInstance = instance }} onFinish={this.onSubmit}>
                    <Form.Item
                        label="应用ID"
                        name="appId"
                        rules={[{ required: true, message: '请输入任务对应应用ID!' }]}
                        {...formItemLayout}
                    >
                        <Input disabled={this.state.pageType != 1||(!!this.state.appId)} />
                    </Form.Item>
                    <Form.Item
                        label="任务名称"
                        name="name"
                        rules={[{ required: true, message: '请输入任务名称' }]}
                        {...formItemLayout}
                    >
                        <Input disabled={this.state.pageType != 1} />
                    </Form.Item>

                    <Form.Item
                        label="启动参数"
                        name="runArgs"
                        rules={[{ required: false }]}
                        {...formItemLayout}
                    >
                        <Input disabled={this.state.pageType == 3} />
                    </Form.Item>

                    <Form.Item
                        label="任务计划"
                        name="schedule"
                        rules={[{ required: false }]}
                        {...formItemLayout}
                    >
                        <Input disabled={this.state.pageType == 3} />
                        (例:* * * * * 留空则立即执行)
                    </Form.Item>

                    <Form.Item {...tailFormItemLayout}>
                        {getButton(this.state.pageType)}
                    </Form.Item>
                </Form>
            </div>)
    }
}