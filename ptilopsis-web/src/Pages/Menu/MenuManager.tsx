import React from 'react';
import {HomeOutlined,BlockOutlined} from '@ant-design/icons';
import { AntdIconProps } from '@ant-design/icons/lib/components/AntdIcon';
import menuData from './MenuData';
/**
 * 菜单项模型
 */
export interface MenuObj {
    /**
     * 主键KEY
     */
    key: string,
    /**
     * 名称(用于在菜单中显示)
     */
    name: string;
    /**
     * 全称(用于在标题中显示)
     */
    fullName: string;
    /**
     * 虚拟路由路径
     */
    path: string;
    /**
     * 图标类型
     */
    icon: React.ForwardRefExoticComponent<AntdIconProps & React.RefAttributes<HTMLSpanElement>>;
    /**
     * 显示的组件
     */
    component: React.ComponentType<any>;
    /**
     * 是否可用
     */
    available?:boolean;
}

/**
 * 菜单组模型
 */
export interface MenuGroup{
    /**
     * 主键KEY
     */
    key: string,
    /**
     * 名称(用于在菜单中显示)
     */
    name: string;
    /**
     * 全称(用于在标题中显示)
     */
    fullName: string;
    /**
     * 图标类型
     */
    icon: React.ForwardRefExoticComponent<AntdIconProps & React.RefAttributes<HTMLSpanElement>>;
    /**
     * 菜单项
     */
    menuItems:Array<MenuObj>;
}

/**
 * Menu统一管理类
 * Feature:根据用户权限选择性返回菜单
 */
class MenuManager{
    /**
     * 构造函数
     */
    constructor(){

    }

    /**
     * 自定义Menu列表。修改MENU请修改这里
     */
    menuList: Array<MenuGroup|MenuObj> = menuData;

    /**
     * 其他子页面面包屑路由名称映射
     */
    otherPageBreadMap:({ [key: string]: string; })={
    }

    /**
     * 获得Menu列表
     * Feature:根据用户组选择性返回Menu列表
     */
    getMenuList(){
        return this.menuList
    }

    /**
     * 根据otherPageBreadMap中的映射,返回对应面包屑
     * @param routeString Map映射路径
     */
    getBreadName(routeString:string){
        return this.otherPageBreadMap[routeString];
    }

    /**
     * 获得所有Menu列表flatmap
     */
    getAllMenu(){
        //var r:Array<MenuObj>=(this.getMenuList().map(item=>item.menuItems) as any).flat(2);
        
        let r:Array<MenuObj>=[];
        this.menuList.forEach(item=>{
            if(!(item as any).menuItems){
                r.push(item as MenuObj);
            }
            else{
                (item as MenuGroup).menuItems.forEach(m=>{
                    r.push(m);
                });
            }

        });
        return r;
    }

    /**
     * 根据路径URL获得menu对象
     * @param path 路径string
     */
    getMenuByPath(path: string): MenuObj | null {
        let r: MenuObj | null = null;
        this.getAllMenu().some(item => {
            if (path.indexOf(item.path)==0) {
                r = item;
                return true;
            }
        });

        return r;
    }

    /**
     * 根据menu对象获得所在的menuGroup
     * @param menu menu对象
     */
    getMenuGroupByMenu(menu?:MenuObj):MenuGroup|null{
        let r:MenuGroup|null=null;
        if(menu){
            this.getMenuList().some(item=>{
                if((item as any).menuItems){
                    if((item as MenuGroup).menuItems.some(subItem=>subItem.key==menu.key)){
                        r=(item as MenuGroup);
                        return true;
                    }
                }
            });
        }
        return r
    }

    //#region 单例模式
    static instance:MenuManager|undefined=undefined;
    static getInstance(){
        if(!this.instance){
            this.instance=new MenuManager();
        }
        return this.instance;
    }
    //#endregion
}

export default MenuManager;