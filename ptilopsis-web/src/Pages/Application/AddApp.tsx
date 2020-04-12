import React from 'react';
import { Form, Input, Row, Col, Button, Upload } from 'antd';
import { UploadOutlined} from '@ant-design/icons';
import { RcFile } from 'antd/lib/upload/interface';
import ExLoading from '../../components/ExLoading';
import Http from '../../Utils/Http';
interface IProps {

}
interface IState {
    selectFile?:any
}

export default class AddApp extends React.Component<IProps, IState>{
    constructor(props: IProps) {
        super(props);
        this.state={
            selectFile:undefined
        }
    }
    onSubmit =async (values: any) => {
        ExLoading.show(true);
        const formData = new FormData();
        formData.append('file', this.state.selectFile);

        formData.append('name',values.name);
        formData.append('runCmd',values.runCmd);


        console.log(formData.getAll('file'));
        let response =await Http.post("/api/application/add",formData)

        ExLoading.hide();
        
    }

    customUploader_beforeUpload=(file:RcFile)=>{
        this.setState({
            selectFile:file
        });
        return false;
    }
    render() {
        const mainCol = {
            lg: 16,
            sm: 24
        }
        const formItemLayout = {
            labelCol: {
                xs: {
                    span: 24,
                },
                sm: {
                    span: 8,
                },
            },
            wrapperCol: {
                xs: {
                    span: 24,
                },
                sm: {
                    span: 16,
                },
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
                    offset: 8,
                },
            },
        };
        return (
            <div>
                <Row style={{ backgroundColor: "white", padding: 25 }}>
                    <Col lg={4}></Col>
                    <Col {...mainCol}>
                        <Form onFinish={this.onSubmit}>

                            <Form.Item
                                label="应用名称"
                                name="name"
                                rules={[{ required: true, message: '请输入应用名称' }]}
                                {...formItemLayout}
                            >
                                <Input />
                            </Form.Item>

                            <Form.Item
                                label="运行指令"
                                name="runCmd"
                                rules={[{ required: true, message: '请输入运行指令！' }]}
                                {...formItemLayout}
                            >
                                <Input />
                            </Form.Item>

                            <Form.Item
                                label="程序压缩包"
                                name="file"
                                rules={[{ required: false }]}
                                {...formItemLayout}
                            >
                                <Upload
                                    beforeUpload={this.customUploader_beforeUpload} 
                                    fileList={this.state.selectFile?[this.state.selectFile]:[]}
                                    onRemove={()=>{this.setState({selectFile:undefined})}}>
                                    <Button>
                                        <UploadOutlined /> Click to Upload
                                    </Button>
                                </Upload>,
                            </Form.Item>

                            <Form.Item {...tailFormItemLayout}>
                                <Button type="primary" htmlType="submit">
                                    提交
                                </Button>
                            </Form.Item>

                        </Form>
                    </Col>
                    <Col lg={4}></Col>
                </Row>
            </div>)
    }
}