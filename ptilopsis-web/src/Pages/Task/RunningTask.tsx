import React from 'react';
import { Table, Tag, Menu, Dropdown } from 'antd';
import { DownOutlined } from '@ant-design/icons';
import { ColumnsType, TablePaginationConfig } from 'antd/lib/table/interface';
import { PaginationConfig } from 'antd/lib/pagination';
import Http from '../../Utils/Http';
import { Link } from 'react-router-dom';

interface IProps {

}
interface IState {

}

export default class RunningTask extends React.Component<IProps, IState>{
    constructor(prop:IProps){
        super(prop);
    }
    render(){
        return (<div>Running Task</div>)
    }
}