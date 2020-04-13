import React from 'react';
import { Form, Input, Row, Col, Button, Upload, message } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import { RcFile } from 'antd/lib/upload/interface';
import ExLoading from '../../components/ExLoading';
import Http from '../../Utils/Http';
import { Redirect, RouteComponentProps } from 'react-router-dom';
import { FormInstance } from 'antd/lib/form';

const { TextArea } = Input;
interface IParam {
    id: string
}
interface IProps extends RouteComponentProps<IParam> {

}
interface IState {
    selectFile?: any,
    selectId?: string,
    /**1=add 2=update 3=view */
    pageType?: number,
    redirect: boolean
}

export default class AddApp extends React.Component<IProps, IState>{
    formInstance:FormInstance | null
    constructor(props: IProps) {
        super(props);
        this.state = {
            selectFile: undefined,
            redirect: false,
            pageType: 1
        }
        this.formInstance=null;
    }

    componentDidMount() {
        let _state:any={}
        if (this.props.match.params.id) {
            _state.selectId= this.props.match.params.id
        }

        if (this.props.location.pathname.startsWith("/app/appmanager/update/")) {
            _state.pageType = 2;
        }
        else if (this.props.location.pathname.startsWith("/app/appmanager/")) {
            _state.pageType = 3;
        }
        else {
            _state.pageType = 1;
        }

        if(_state.pageType!=1){
            this.getAppData(_state.selectId);
        }
        console.log(this.props.location);

        this.setState(_state);
    }

    getAppData=async (id:string)=>{
        ExLoading.show(true);
        let response = await Http.get("/api/application/get?id="+id)
        ExLoading.hide();
        if (!response?.data.success) {
            message.error('获得APP信息失败!', 10);
        }
        else {
            let _data=response?.data.data;
            if(this.formInstance){
                this.formInstance.setFieldsValue({
                    name:_data.Name,
                    runCmd:_data.DefaultRunCmd,
                    description:_data.Description
                })
            }
            // this.setState({
            //     defaultData: {
            //         name:_data.Name,
            //         runCmd:_data.DefaultRunCmd,
            //         description:_data.Description
            //     }
            // })
        }
    }

    onSubmit = async (values: any) => {
        ExLoading.show(true);
        const formData = new FormData();
        formData.append('file', this.state.selectFile);

        formData.append('name', values.name);
        formData.append('runCmd', values.runCmd);
        formData.append('description', values.description);
        let response = await Http.post("/api/application/add", formData)
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

    customUploader_beforeUpload = (file: RcFile) => {
        this.setState({
            selectFile: file
        });
        return false;
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

        const getButton=(pageType?:number)=>{
            if(pageType==3){
                return (<Button onClick={e=>this.setState({redirect:true})}>返回</Button>);
            }
            if(pageType==2){
                return (<>
                    <Button type="primary" style={{marginRight:10}} htmlType="submit">提交</Button>
                    <Button onClick={e=>this.setState({redirect:true})}>返回</Button>
                </>);
            }
            else{
                return (<Button type="primary" htmlType="submit">提交</Button>);
            }
        }
        return (
            <div style={{ backgroundColor: "white", padding: 25 }}>
                {this.state.redirect ? (<Redirect to="/app/appmanager" />) : null}
                <Form ref={(instance)=>{this.formInstance=instance}} onFinish={this.onSubmit}>
                    <Form.Item
                        label="应用名称"
                        name="name"
                        rules={[{ required: true, message: '请输入应用名称' }]}
                        {...formItemLayout}
                    >
                        <Input disabled={this.state.pageType != 1} />
                    </Form.Item>

                    <Form.Item
                        label="运行指令"
                        name="runCmd"
                        rules={[{ required: true, message: '请输入运行指令！' }]}
                        {...formItemLayout}
                    >
                        <Input disabled={this.state.pageType == 3} />
                    </Form.Item>

                    <Form.Item
                        label="说明"
                        name="description"
                        rules={[{ required: false }]}
                        {...formItemLayout}
                    >
                        <TextArea disabled={this.state.pageType == 3} rows={4} />
                    </Form.Item>

                    {this.state.pageType == 3 ? (null) : (
                        <Form.Item
                            label="程序压缩包"
                            name="file"
                            rules={[{ required: false }]}
                            {...formItemLayout}>
                            <Upload
                                beforeUpload={this.customUploader_beforeUpload}
                                fileList={this.state.selectFile ? [this.state.selectFile] : []}
                                onRemove={() => { this.setState({ selectFile: undefined }) }}>
                                <Button>
                                    <UploadOutlined /> Click to Upload
                                 </Button>
                            </Upload>
                        </Form.Item>
                    )}

                    <Form.Item {...tailFormItemLayout}>
                        {getButton(this.state.pageType)}
                    </Form.Item>
                </Form>
            </div>)
    }
}