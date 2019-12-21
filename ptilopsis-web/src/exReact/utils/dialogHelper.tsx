import { Modal } from 'antd';
export default class DialogHelper {
    static defaultAlertTitle:string="Alert";
    static show(msg: any, title?: string, type?: string, onCancel?: Function, onOk?: Function) {
        if (!title&&type=="alert") {
            title=DialogHelper.defaultAlertTitle;
        }
        else if(!title){
            title = type;
        }
        var modal: any = null;
        switch (type) {
            default:
                modal = Modal.info({
                    title: title,
                    content: msg
                });
                break;
            case "info":
                modal = Modal.info({
                    title: title,
                    content: msg,
                });
                break;
            case "success":
                modal = Modal.success({
                    title: title,
                    content: msg,
                });
                break;
            case "error":
                modal = Modal.error({
                    title: title,
                    content: msg,
                });
                break;
            case "warning":
                modal = Modal.warning({
                    title: title,
                    content: msg,
                });
                break;
            case "confirm":
                modal = Modal.confirm({
                    title: title,
                    content: msg,
                });
                break;
        }

        if (onCancel) {
            modal.update({
                onCancel: () => {
                    onCancel!.call((global as any).App);
                }
            });
        }

        if (onOk) {
            modal.update({
                onOk: () => {
                    onOk!.call((global as any).App);
                }
            });
        }
    }

    static info(msg: any, title?: string) {
        DialogHelper.show(msg, title, "info");
    }

    static alert(msg: any) {
        DialogHelper.show(msg, undefined, "alert");
    }

    static success(msg: any, title?: string) {
        DialogHelper.show(msg, title, "success");
    }

    static error(msg: any, title?: string) {
        DialogHelper.show(msg, title, "error");
    }

    static warning(msg: any, title?: string) {
        DialogHelper.show(msg, title, "warning");
    }

    static confirm(msg: any, title?: string) {
        DialogHelper.show(msg, title, "confirm");
    }
}