import React, { Props } from 'react';
import { Layout, Menu, Icon, Modal } from 'antd';
import { Route, Link, withRouter, RouteComponentProps } from 'react-router-dom';

import ExInput from '../exReact/component/exInput';
import ExTextArea from '../exReact/component/exTextArea'
import If from '../exReact/component/exIf'

import Monitor from './monitor'
import TaskList from './taskList'
import AppList from './appList'
import SettingPage from './settingPage'

import './mainPage.css';
import MenuItem from 'antd/lib/menu/MenuItem';

const { Header, Sider, Content } = Layout;
interface IProps extends RouteComponentProps {

}
interface IState {
    components: any;
    params: IParams;//用于主页面参数
    collapsed?: boolean;//用于展示侧边栏
    currentPage: MenuObj;//当前页面
}

interface IParams {
    userName: string;
}

interface MenuObj {
    /**
     * 主键KEY
     */
    key: string,
    /**
     * 名称(用于在菜单中显示)
     */
    name: string;
    /**
     * 全程(用于在标题中显示)
     */
    fullName: string;
    /**
     * 虚拟路由路径
     */
    path: string;
    /**
     * 图标类型
     */
    iconType: string;
    /**
     * 显示的组件
     */
    component: React.ComponentType<any>;
}

class MainPage extends React.Component<IProps, IState> {

    menuList: Array<MenuObj> = [
        {
            key: "1",
            name: "Monitor",
            fullName: "Monitor",
            path: "/",
            iconType: "user",
            component: Monitor
        },
        {
            key: "2",
            name: "TaskList",
            fullName: "Task List",
            path: "/tasklist",
            iconType: "upload",
            component: TaskList
        },
        {
            key: "3",
            name: "AppList",
            fullName: "Application List",
            path: "/applist",
            iconType: "video-camera",
            component: AppList
        },
        {
            key: "4",
            name: "Settings",
            fullName: "Settings",
            path: "/settings",
            iconType: "video-camera",
            component: SettingPage
        }
    ];

    /**
     * 构造函数
     * @param props 
     */
    constructor(props: IProps) {
        super(props);
        this.state = {
            params: {
                userName: ""
            },
            components: {},
            collapsed: false,
            currentPage: this.menuList[0]
        };
        (global as any).App = this;
    }

    //#region Data
    getMenuList = (type: string) => {
        if (type == "menu") {
            return this.menuList.map(item => {
                return (
                    <Menu.Item key={item.key}>
                        <Icon type={item.iconType} />
                        <span>{item.name}</span>
                        <Link to={item.path} />
                    </Menu.Item>
                );
            });
        }
        else {
            return this.menuList.map(item => {
                return (
                    <Route exact path={item.path} component={item.component} />
                );
            });
        }
    }
    //#endregion

    //#region Methods
    getMenuByPath(path: string): MenuObj | null {
        let r: MenuObj | null = null;
        this.menuList.some(item => {
            if (item.path == path) {
                r = item;
                return true;
            }
        });

        return r;
    }
    //#endregion

    //#region Event
    /**
     * 页面开始渲染时
     */
    componentWillMount() {
        this.props.history.listen(location => {
            var obj: MenuObj | null = this.getMenuByPath(location.pathname);
            if (obj) {
                this.setState({
                    currentPage: obj as MenuObj
                });
            }
            console.log(location.pathname);
        });

        var obj: MenuObj | null = this.getMenuByPath(this.props.location.pathname);
        if (obj) {
            this.setState({
                currentPage: obj as MenuObj
            });
        }
    }

    /**
     * 当点击侧边栏收缩时
     */
    toggle = () => {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    }
    //#endregion

    render() {
        let $scope: any = this.state;
        let $params: any = this.state.params;
        return (
            <Layout id="mainPage">
                <Sider className="sider" trigger={null} collapsible collapsed={$scope.collapsed}>
                    <div className="logo" >
                        {!$scope.collapsed ? <div className="logofont">Ptilopsis</div> : <div></div>}
                    </div>
                    <Menu theme="dark" mode="inline" defaultSelectedKeys={[this.state.currentPage.key]}>
                        {this.getMenuList("menu")}
                    </Menu>
                </Sider>
                <Layout>
                    <Header style={{ background: '#fff', padding: 0 }}>
                        <div style={{ display: "flex" }}>
                            <Icon className="trigger" type={$scope.collapsed ? 'menu-unfold' : 'menu-fold'} onClick={this.toggle} />
                            <h1>{this.state.currentPage.fullName}</h1>
                        </div>
                    </Header>
                    <Content style={{ margin: '24px 16px', padding: 24, background: '#fff' }}>
                        {this.getMenuList("route")}
                    </Content>
                </Layout>
            </Layout>
        );
    }
}

export default withRouter(MainPage);