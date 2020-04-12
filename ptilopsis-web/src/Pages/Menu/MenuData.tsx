import React from 'react';
import { MenuObj, MenuGroup } from './MenuManager';
import { HomeOutlined, BlockOutlined,CloudServerOutlined,BuildOutlined,AppstoreOutlined } from '@ant-design/icons';

import AppList from '../Application/AppList';
import Home from '../Home/Home';
import AddApp from '../Application/AddApp';

let menuData: Array<MenuGroup | MenuObj> = [
    {
        key: "home",
        name: "首页",
        fullName: "首页",
        path: "/home",
        icon: HomeOutlined,
        component: Home
    },
    {
        key: "1",
        name: "应用",
        fullName: "应用",
        icon: AppstoreOutlined,
        menuItems: [
            {
                key: "1-1",
                name: "应用管理",
                fullName: "应用管理",
                path: "/app/appmanager",
                icon: AppstoreOutlined,
                component: AppList
            },
            {
                key: "1-2",
                name: "上传应用",
                fullName: "上传应用",
                path: "/app/uploadapp",
                icon: AppstoreOutlined,
                component: AddApp
            },
        ]
    }, {
        key: "2",
        name: "任务",
        fullName: "任务",
        icon: BlockOutlined,
        menuItems: [
            {
                key: "2-1",
                name: "运行任务管理",
                fullName: "运行任务管理",
                path: "/task/taskrunningmanager",
                icon: BuildOutlined,
                component: () => <h1>运行任务管理</h1>
            },
            {
                key: "2-2",
                name: "任务数据库管理",
                fullName: "任务数据库管理",
                path: "/task/taskmanager",
                icon: CloudServerOutlined,
                component: () => <h1>任务管理</h1>
            },
            {
                key: "2-3",
                name: "添加任务",
                fullName: "添加任务",
                path: "/task/add-task",
                icon: BlockOutlined,
                component: () => <h1>添加任务</h1>
            },
        ]
    },
    {
        key: "3",
        name: "系统",
        fullName: "系统",
        icon: BlockOutlined,
        menuItems: [
            {
                key: "3-1",
                name: "全局设置",
                fullName: "全局设置",
                path: "/system/allsetting",
                icon: BlockOutlined,
                component: () => <h1>全局设置</h1>
            },
            {
                key: "3-2",
                name: "关于",
                fullName: "关于",
                path: "/system/about",
                icon: BlockOutlined,
                component: () => (
                <div>
                    <h1>版权所有</h1>
                    <h2>Lenshang 2020</h2>
                </div>
                )
            },
        ]
    },
];

export default menuData;