import React from 'react';

interface IProps {
}
interface IState {
}
export default class SettingPage extends React.Component<IProps, IState> {
    constructor(props: IProps){
        super(props);
    }
    render(){
        return (
        <div>
            Settings Page!
        </div>
        );
    }
}