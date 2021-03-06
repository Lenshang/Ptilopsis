import React from 'react';
import { Card, Row, Col, message } from 'antd';
import { DownOutlined } from '@ant-design/icons';
import { ColumnsType, TablePaginationConfig } from 'antd/lib/table/interface';
import { PaginationConfig } from 'antd/lib/pagination';
import Http from '../../Utils/Http';
import {getMyDate,getTaskState} from '../../Utils/CommonUtils';
import { Link } from 'react-router-dom';
import ExLoading from '../../components/ExLoading';

interface IProps {

}
interface IState {
    Datas: Array<any>
}

export default class RunningTask extends React.Component<IProps, IState>{
    constructor(props: IProps) {
        super(props);
        this.state = {
            Datas: []
        }
    }
    componentDidMount() {
        this.getDatas();
    }

    getDatas = async () => {
        let datas: Array<any> = [];
        let response: any = await Http.get("/api/task/getrunning");
        response = response.data;
        if (response.success) {
            datas = response.data;
        }
        else {
            message.error('Task数据获取失败!', 10);
        }

        this.setState({
            Datas: datas
        });
    }

    killTask=async (id:String)=>{
        ExLoading.show(true);
        let response: any = await Http.get("/api/task/kill",{"id":id});

        response=response.data;
        if(response.success){
            message.info("成功结束进程",10);
        }
        else{
            message.info("成功进程失败",10);
        }
        await this.getDatas();
        ExLoading.hide();
    }
    render(){
        const TaskComponents = () => {

            return this.state.Datas.map((item: any) => {
                return (
                    <Col key={item._id} xs={24} sm={24} lg={12} xl={8} xxl={6}>
                        <Card hoverable key={item._id} title={item.TaskName} actions={[
                            (<div>状态:{getTaskState(item.TaskState)}</div>),
                            item.TaskState=="1"?(
                            <div onClick={()=>{this.killTask(item._id)}}>强制结束</div>)
                            :(<div>立即启动</div>),
                            (<Link to={'/task/log?taskid='+item._id}>日志</Link>),
                        ]}>
                            <div>上次执行:{getMyDate(item.LastRunDate)}</div>
                            <div>下次执行:{getMyDate(item.NextRunDate)}</div>
                        </Card>
                    </Col>
                )
            });
        }
        return (
            <div id="app-manager">
                <Row gutter={[16, 24]}>
                    {TaskComponents()}
                </Row>
            </div>
        )
    }
}