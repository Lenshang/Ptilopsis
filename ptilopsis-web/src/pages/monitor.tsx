import React from 'react';
import { Button, Row, Col, Statistic } from 'antd'
import DialogHelper from '../exReact/utils/dialogHelper'

import './monitor.css'
interface IProps {
}
interface IState {
}
export default class Monitor extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);
    }
    clickme = () => {
        DialogHelper.info((<Button onClick={this.clickme2}>再点我试试？</Button>), "点击按钮");
    }
    clickme2 = () => {
        DialogHelper.show("你点击了按钮2", "你点击了按钮2！", "confirm", () => {
            console.log("你取消了！");
        }, () => {
            console.log("你确定了！");
        });
    }
    render() {
        return (
            <div id="monitor">
                {/**图表展示层 */}
                <Row>
                    <Col span={24}>

                    </Col>
                </Row>
                {/**运行中的任务展示层 */}
                <Row gutter={[16, 16]}>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                    <Col span={6}>
                        <div className="col"></div>
                    </Col>
                </Row>
                <Button onClick={this.clickme}>点我试试？</Button>
            </div>
        );
    }
}