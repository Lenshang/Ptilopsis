import React from 'react';
import { Card, Form, Input, Button,message } from 'antd';
import { Redirect } from 'react-router-dom';
import Http from '../Utils/Http';
import ExLoading from '../components/ExLoading';
interface IProps {

}
interface IState {
    redirect:Boolean;
}
export default class extends React.Component<IProps, IState> {
    constructor(props: IProps){
        super(props);
        this.state={
            redirect:false
        }
        this.onFinish = this.onFinish.bind(this);
    }
    async onFinish(values: any) {
        //console.log('Success:', values);
        ExLoading.show(true,"登陆中");
        let response = await Http.post("/api/login/login",{userName:values["username"],password:values["password"]});
        response=response.data;
        if(response?.success){
            ExLoading.hide();
            message.info("登录成功");
            localStorage.setItem("token",response.data.token);
            this.setState({redirect:true});
        }
        else{
            ExLoading.hide();
            message.error(response?.message);
        }
    };

    onFinishFailed(errorInfo: any) {
        //console.log('Failed:', errorInfo);
    };
    render() {
        let boxStyle: React.CSSProperties = {
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            width: "100%",
            height: "100vh",
            backgroundColor: "#eeeeee"
        }
        const layout = {
            labelCol: { span: 6 },
            wrapperCol: { span: 18 },
        };
        const tailLayout = {
            wrapperCol: { offset: 6, span: 18 },
        };

        let loginForm = () => {
            return (
                <Form
                    {...layout}
                    name="basic"
                    initialValues={{ remember: true }}
                    onFinish={this.onFinish}
                    onFinishFailed={this.onFinishFailed}
                >
                    <Form.Item
                        label="Username"
                        name="username"
                        rules={[{ required: true, message: 'Please input your username!' }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        label="Password"
                        name="password"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input.Password />
                    </Form.Item>

                    <Form.Item {...tailLayout}>
                        <Button type="primary" htmlType="submit">
                            Login
                        </Button>
                    </Form.Item>
                </Form>
            );
        }
        return (
            <div style={boxStyle}>
                {this.state.redirect?(<Redirect to="/"></Redirect>):(null)}
                <Card title={(
                    <>
                        <div style={{ fontSize: "24px" }}>PTILOPSIS</div>
                        <div style={{ fontWeight: "normal", fontSize: "14px" }}>Lightweight Task Schedule Admin UI</div>
                    </>
                )} bordered={false} style={{ marginTop: "-30vh", width: "450px" }} hoverable>
                    {loginForm()}
                </Card>
            </div>
        );
    }
}

