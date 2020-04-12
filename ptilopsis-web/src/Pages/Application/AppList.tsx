import React from 'react';
import { Card, Row, Col, message } from 'antd';
import ExLoading from '../../components/ExLoading';
import Http from '../../Utils/Http';
interface IProps {

}
interface IState {
    AppDatas: Array<any>
}

export default class AppList extends React.Component<IProps, IState>{
    constructor(props: IProps) {
        super(props);
        this.state = {
            AppDatas: []
        }
    }
    componentDidMount(){
        this.getApps();
    }

    getApps = async () => {
        let datas: Array<any> = [];
        let response: any = await Http.get("/api/application/getall");
        response=response.data;
        if (response.success) {
            response.data.forEach((item: any) => {
                datas.push({
                    name: item.Name
                })
            });
        }
        else {
            message.error('App数据获取失败!', 10);
        }

        this.setState({
            AppDatas:datas
        });
    }

    render() {
        const AppComponents = () => {

            return this.state.AppDatas.map((item: any) => {
                return (
                    <Col span={6}>
                        <Card title={item.name} actions={[
                            (<div>查看</div>),
                            (<div>修改</div>),
                            (<div>删除</div>),
                        ]}>
                            {item.name}
                        </Card>
                    </Col>
                )
            });
        }
        return (
            <div>
                <Row gutter={[16, 24]}>
                    {AppComponents()}
                </Row>
            </div>)
    }
}