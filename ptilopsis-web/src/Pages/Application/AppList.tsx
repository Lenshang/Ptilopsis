import React from 'react';
import { Card, Row, Col, message } from 'antd';
import ExLoading from '../../components/ExLoading';
import Http from '../../Utils/Http';
import { Route, Link } from 'react-router-dom';
import AddApp from './AddApp';
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
    componentDidMount() {
        this.getApps();
    }

    getApps = async () => {
        let datas: Array<any> = [];
        let response: any = await Http.get("/api/application/getall");
        response = response.data;
        if (response.success) {
            datas = response.data;
        }
        else {
            message.error('App数据获取失败!', 10);
        }

        this.setState({
            AppDatas: datas
        });
    }

    render() {
        const AppComponents = () => {

            return this.state.AppDatas.map((item: any) => {
                return (
                    <Col key={item._id} xs={24} sm={24} lg={12} xl={8} xxl={6}>
                        <Card hoverable key={item._id} title={item.Name} actions={[
                            (<Link to={'/task/add-task?appId='+item._id+'&appName='+item.Name}>启动</Link>),
                            (<Link to={'/app/appmanager/'+item._id}>查看</Link>),
                            (<Link to={'/app/appmanager/update/'+item._id}>修改</Link>),
                            (<div>删除(TODO)</div>),
                        ]}>
                            {item.Description}
                        </Card>
                    </Col>
                )
            });
        }
        return (
            <>
                <Route exact key={3} path="/app/appmanager/:id" component={AddApp} />
                <Route exact key={2} path="/app/appmanager/update/:id" component={AddApp} />
                <Route exact key={1} path="/app/appmanager">
                    {props => (
                        <div id="app-manager" style={props.match ? {} : { display: 'none' }}>
                            <Row gutter={[16, 24]}>
                                {AppComponents()}
                            </Row>
                        </div>
                    )}
                </Route>
            </>)
    }
}