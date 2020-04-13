import React from 'react';
import { Layout, Menu } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined} from '@ant-design/icons';
import { Route, Link, withRouter, RouteComponentProps, Redirect, Switch } from 'react-router-dom';
import ExLoading from '../components/ExLoading';
import "./Main.css";
import MenuManager, { MenuObj, MenuGroup } from './Menu/MenuManager';

const { Header, Sider, Content } = Layout;
const { SubMenu } = Menu;

interface IProps extends RouteComponentProps {

}
interface IState {
    menu_collapsed: boolean;
    currentPage?: MenuObj;//当前页面
}
export default class Main extends React.Component<IProps, IState> {
    menuManager: MenuManager;
    constructor(props: IProps) {
        super(props);
        this.state = {
            menu_collapsed: false,
            currentPage: undefined,
        }
        this.menuManager = new MenuManager();
    }
    componentDidMount() {

    }

    componentWillMount() {
        //监听history,更新currentPage变量
        this.props.history.listen(location => {
            var obj: MenuObj | null = this.menuManager.getMenuByPath(location.pathname);
            if (obj) {
                this.setState({
                    currentPage: obj as MenuObj
                });
            }
            console.log(location.pathname);
        });

        //第一次访问时给currentPage变量赋值
        var obj: MenuObj | null = this.menuManager.getMenuByPath(this.props.location.pathname);
        if (obj) {
            this.setState({
                currentPage: obj as MenuObj
            });
        }
    }
    render() {
        /**
         * 触发菜单收缩
         * @param iscollapsed 是否收缩菜单
         */
        let menu_toggle = (iscollapsed: boolean) => {
            this.setState({
                menu_collapsed: iscollapsed,
            });
        };

        /**
         * 获得菜单
         * @param type 类型
         */
        let getMenuList = (type: string) => {
            if (type == "menu") {
                var r: Array<any> = this.menuManager.getMenuList().map(item => {
                    if ((item as any).menuItems) {
                        return (
                            <SubMenu key={item.key} title={
                                <span>
                                    {/* <Icon type={item.iconType} /> */}
                                    <item.icon></item.icon>
                                    {!this.state.menu_collapsed ? item.name : ""}
                                </span>
                            }>
                                {(item as MenuGroup).menuItems.map(m => {
                                    return (
                                        <Menu.Item key={m.key}>
                                            <m.icon></m.icon>
                                            <span>{m.name}</span>
                                            <Link to={m.path} />
                                        </Menu.Item>
                                    );
                                })}
                            </SubMenu>
                        );
                    }
                    else {
                        var _item = item as MenuObj;
                        return (
                            <Menu.Item key={_item.key}>
                                <item.icon></item.icon>
                                <span>{_item.name}</span>
                                <Link to={_item.path} />
                            </Menu.Item>
                        );
                    }
                })
                return r;
            }
            else {
                return (<Switch>
                    {this.menuManager.getAllMenu().map(item => {
                        return (<Route key={item.key} path={item.path} component={item.component} />);
                    })}
                    {/* <Route key="center" path="/" component={Center} /> */}
                    <Redirect exact from="/" to="/home"></Redirect>
                    <Route component={() => (
                        <Content>
                            <h1>页面没有找到</h1>
                        </Content>
                    )} />
                </Switch>)
            }
        }

        /**
         * 获得当前默认选中的菜单页面
         */
        let getDefaultSelectMenus = () => {
            let r = [];
            if (this.state.currentPage) {
                r.push(this.state.currentPage.key);
            }
            return r;
        }
        /**
         * 获得当前默认展开的页面菜单
         */
        let getDefaultOpenKeyMenu=()=>{
            let r = [];
            if (this.state.currentPage) {
                let group = this.menuManager.getMenuGroupByMenu(this.state.currentPage);
                if (group) {
                    r.push(group.key);
                }
            }
            return r;
        }
        return (
            <Layout id="main-page">
                <Sider width="256px" breakpoint="md" className="main-sider" onBreakpoint={(broken: any) => {
                    menu_toggle(broken);
                }} trigger={null} collapsible collapsed={this.state.menu_collapsed}>
                    <div className="logo" />
                    <Menu theme="dark" mode="inline" selectedKeys={getDefaultSelectMenus()} defaultOpenKeys={getDefaultOpenKeyMenu()}>
                        {getMenuList("menu")}
                    </Menu>
                </Sider>
                <Layout className="site-layout">
                    <Header className="site-layout-background" style={{ padding: 0 }}>
                        <div style={{ display: "flex" }}>
                            {React.createElement(this.state.menu_collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
                                className: 'trigger',
                                onClick: () => menu_toggle(!this.state.menu_collapsed),
                                style:{marginTop:3}
                            })}
                            <h1>{this.state.currentPage ? this.state.currentPage.fullName : ""}</h1>
                        </div>
                    </Header>
                    <Content
                        style={{
                            margin: '24px 16px',
                            minHeight: 280,
                        }}>
                        {getMenuList("route")}
                    </Content>
                </Layout>
                <ExLoading/>
            </Layout>
        )
    }
}