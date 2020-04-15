import React from 'react';
import Http from '../../Utils/Http';
import { Link, RouteComponentProps } from 'react-router-dom';
import { FolderOpenOutlined, FileTextOutlined } from '@ant-design/icons';
import { Row, Col, Card, message, Modal } from 'antd';
import { getSearchByName, sleep } from '../../Utils/CommonUtils';
import ExLoading from '../../components/ExLoading';
interface IProps extends RouteComponentProps {

}
interface IState {
    isTaskPage: boolean;
    taskId: string;
    data: Array<any>;
}

export default class Log extends React.Component<IProps, IState>{
    constructor(props: IProps) {
        super(props);
        this.state = {
            isTaskPage: false,
            taskId: "",
            data: []
        }
    }
    componentDidMount() {
        let taskId = getSearchByName(this.props.location.search, "taskid");
        let _state = { ...this.state };
        if (taskId) {
            _state.isTaskPage = true;
            _state.taskId = taskId;
        }
        else {
            _state.isTaskPage = false;
        }

        this.setState(_state, () => this.getData());
    }
    getData = async () => {
        if (this.state.isTaskPage) {
            var response = await Http.get("/api/log/getlogs", { taskid: this.state.taskId });
            if (response?.data.success) {
                this.setState({ data: response?.data.data });
            }
            else {
                message.error('获得日志列表失败!', 10)
            }
        }
        else {
            var response = await Http.get("/api/log/getall");
            if (response?.data.success) {
                this.setState({ data: response?.data.data });
            }
            else {
                message.error('获得日志列表失败!', 10)
            }
        }
    }
    openFile = async (fileName: string) => {
        ExLoading.show(true);
        var response = await Http.get("/api/log/getdetail", { taskid: this.state.taskId, filename: fileName });
        await sleep(500);
        ExLoading.hide();
        if (response?.data.success) {
            let _data = response?.data.data as string;
            let _dataArr = _data.split(/\r\n/);
            Modal.info({
                content: 
                    <div>
                        <h3>{fileName}</h3>
                        <div style={{padding:20,overflowX:"auto",overflowY:"auto"}}>
                            <div style={{minWidth:500,minHeight:500}}>{_dataArr.map((item:any,index:number) => (<p key={index}>{item}</p>))}</div>
                        </div>
                    </div>,
                width: "80%"
            })
        }
        else {
            message.error('打开日志失败!', 10)
        }
    }
    render() {
        const createComponent = (data: any) => {
            let goTask = (_data: any) => {
                let url = '/task/log?taskid=' + data.name;
                this.setState({
                    isTaskPage: true,
                    taskId: _data.name
                }, () => this.getData())
            }
            if (data.isDir) {
                return (
                    <Col key={data.name} xs={12} sm={8} lg={8} xl={6} xxl={4}>
                        <Card hoverable title={
                            <div style={{ textAlign: "center" }}><FolderOpenOutlined style={{ fontSize: 80 }} /></div>
                        } actions={[
                            (<a onClick={() => goTask(data)}>打开</a>),
                        ]}>
                            <div style={{ wordBreak: "break-all" }}>{data.name}</div>
                        </Card>
                    </Col>
                )
            }
            else {
                return (
                    <Col key={data.name} xs={12} sm={8} lg={8} xl={6} xxl={4}>
                        <Card hoverable title={
                            <div style={{ textAlign: "center" }}><FileTextOutlined style={{ fontSize: 80 }} /></div>
                        } actions={[
                            (<a onClick={() => this.openFile(data.name)}>打开</a>),
                        ]}>
                            <div style={{ wordBreak: "break-all" }}>{data.name}</div>
                        </Card>
                    </Col>
                );
            }
        }
        const getLogComponents = () => {
            let r = this.state.data.map(item => createComponent(item));
            return r;
        }
        return (
            <div>
                <h1>日志浏览器 {this.state.isTaskPage ? (<a onClick={() => this.setState({ isTaskPage: false }, () => this.getData())} style={{ marginLeft: 20 }}>返回</a>) : (null)}</h1>
                <Row gutter={[16, 24]}>
                    {getLogComponents()}
                </Row>
            </div>)
    }
}