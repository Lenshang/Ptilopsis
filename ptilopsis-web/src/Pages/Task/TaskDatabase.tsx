import React from 'react';
import { Table, Tag, Menu, Dropdown,message } from 'antd';
import { DownOutlined } from '@ant-design/icons';
import { ColumnsType, TablePaginationConfig } from 'antd/lib/table/interface';
import { PaginationConfig } from 'antd/lib/pagination';
import Http from '../../Utils/Http';
import { Link } from 'react-router-dom';
import ExLoading from '../../components/ExLoading';

interface IProps {

}
interface IState {
    data: Array<any>;
    pagination: TablePaginationConfig;
    tableLoading: boolean;
}
export default class TaskDatabase extends React.Component<IProps, IState>{
    constructor(props: IProps) {
        super(props);
        this.state = {
            pagination: { current: 1, pageSize: 10 },
            data: [],
            tableLoading: false
        }
    }
    componentDidMount() {
        this.getData();
    }
    getData = async () => {
        this.setState({ tableLoading: true })
        const pager = { ...this.state.pagination };
        pager.total = 20;
        let r: Array<any> = [];
        let params: any = {}
        if (pager.current && pager.pageSize) {
            params.skip = (pager.current - 1) * pager.pageSize;
            params.take = pager.pageSize;
        }
        let response = await Http.get('/api/task/getall', params);

        if (response?.data.success) {
            r = response.data.data.Array;
            pager.total = response.data.data.Total;
        }

        this.setState({
            data: r,
            pagination: pager,
            tableLoading: false
        })
    }
    handleTableChange = (pagination: PaginationConfig, filters: Record<string, React.ReactText[] | null>) => {
        const pager = { ...this.state.pagination };
        pager.current = pagination.current;

        this.setState({ pagination: pager }, () => {
            this.getData();
        })
    }
    enableTask=async (id:string)=>{
        ExLoading.show(true,"操作中");
        let response: any = await Http.get("/api/task/enable",{"id":id});
        response=response.data;
        if(response.success){
            message.info("操作成功");
            await this.getData()
        }
        else{
            message.error("操作失败");
        }
        ExLoading.hide();
    }
    disableTask=async (id:string)=>{
        ExLoading.show(true,"操作中");
        let response: any = await Http.get("/api/task/disable",{"id":id});
        response=response.data;
        if(response.success){
            message.info("操作成功");
            await this.getData()
        }
        else{
            message.error("操作失败");
        }
        ExLoading.hide();
    }
    deleteTask=async (id:string)=>{
        ExLoading.show(true,"操作中");
        let response: any = await Http.get("/api/task/delete",{"id":id});
        response=response.data;
        if(response.success){
            message.info("操作成功");
            await this.getData()
        }
        else{
            message.error("操作失败:"+response.message);
        }
        ExLoading.hide();
    }
    render() {
        const { pagination, data } = this.state;
        const ActionMenu = (record: any) => {
            return (
                <Menu>
                    <Menu.Item>
                        <Link to={'/task/add-task?id='+record._id+'&pageType=3'}>查看</Link>
                    </Menu.Item>
                    <Menu.Item>
                        <Link to={'/task/log?taskid='+record._id}>日志</Link>
                    </Menu.Item>
                    <Menu.Item>
                        <Link to={'/task/add-task?id='+record._id+'&pageType=2'}>编辑</Link>
                    </Menu.Item>
                    <Menu.Item>
                        {record.Enable?(<a onClick={()=>{this.disableTask(record._id)}}>禁用</a>):(<a onClick={()=>{this.enableTask(record._id)}}>启用</a>)}
                    </Menu.Item>
                    <Menu.Item>
                        <a onClick={()=>{this.deleteTask(record._id)}}>删除</a>
                    </Menu.Item>
                </Menu>
            );
        };
        const columns: ColumnsType<any> = [
            {
                title: 'TaskName',
                dataIndex: 'TaskName',
                fixed: 'left',
                key: 'col0'
            },
            {
                title: 'RunCmd',
                dataIndex: 'RunCmd',
                key: 'col1'
            },
            {
                title: 'RunArgs',
                dataIndex: 'RunArgs',
                key: 'col2'
            },
            {
                title: 'ApplicationId',
                dataIndex: 'ApplicationId',
                key: 'col3'
            },
            {
                title: 'CreateDate',
                dataIndex: 'CreateDate',
                key: 'col4',
                render: data => {
                    return (new Date(data)).toLocaleString()
                }
            },
            {
                title: 'LastRunDate',
                dataIndex: 'LastRunDate',
                key: 'col5',
                render: data => {
                    return (new Date(data)).toLocaleString()
                }
            },
            {
                title: 'Enable',
                dataIndex: 'Enable',
                key: 'col6',
                render: data => {
                    if(data){
                        return (<div style={{color:"darkgreen"}}>Enable</div>);
                    }
                    return (<div style={{color:"red"}}>Disable</div>)
                }
            },
            {
                title: 'Action',
                key: 'col7',
                width: 100,
                render: (text, record) => (
                    <Dropdown overlay={ActionMenu(record)}>
                        <a className="ant-dropdown-link" onClick={e => e.preventDefault()}>
                            操作 <DownOutlined />
                        </a>
                    </Dropdown>
                ),
            }
        ]
        return (
            <div style={{ backgroundColor: "white", padding: 10 }}>
                <Table
                    columns={columns}
                    dataSource={data}
                    pagination={pagination}
                    rowKey={(record) => record._id}
                    onChange={this.handleTableChange}
                    loading={this.state.tableLoading}
                    scroll={{ x: 1300 }} />
            </div>
        )
    }
}


