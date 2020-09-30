import serverSite from './app';
import ServerGeneral,{ ResponseGeneral, ResponseDataGeneral } from './ServerGeneral'
export default class Login {

    private dialog: JQuery;//btn: JQuery;
    private userId: JQuery; private responseElem: JQuery;
    private loadingIcon: JQuery; 

    constructor(public modalTag: string) {
        this.dialog = $(`#${modalTag}`);
        this.dialog.on('show.bs.modal', (evt:JQuery.Event) => this.initLogin(evt));
        const btn = this.dialog.find(`#submitLogin`);
        btn.click((evt: JQuery.Event) => this.submitLogin(evt))
        this.userId = this.dialog.find('#userId1');
        this.userId.off("change").on("change", (evt: JQuery.Event) => this.clearDialog(evt));
    }

    private clearDialog(evt): void {
        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("").hide();
    }

    public initLogin(evt: JQuery.Event): void {
        this.loadingIcon = this.dialog.find('span#iconLoadingSignin');
        this.responseElem = this.dialog.find(`#happyIn`);
        this.loadingIcon.addClass('d-none');
        this.clearDialog(evt);
    }

    public submitLogin(evt): void { 
        if ((this.userId.val() != "") && ($("#userId2").val() != "")) {
            const data = JSON.stringify({
                userId: $("#userId1").val(),
                password: $("#userId2").val()
            });
            const serv: ServerGeneral = new ServerGeneral();
            var submitButton = $(evt.target as HTMLButtonElement);
            var lIcon = submitButton.find('span#iconLoadingSignin');
            type uiIndication = () => void;
            let ui: uiIndication;
            ui = () => {
                if (lIcon) {
                    lIcon.removeClass('d-none');
                }
            }
            serv.postInTime('User/Login', data,ui).then((result) => {
                if (result.hasValue && !result.error) {
                    lIcon.addClass('d-none');
                    this.responseElem.removeClass('invalid-feedback').addClass('valid-feedback').show().text(`Welcome back.`);
                    this.userId.addClass("is-valid");
                    // sort out any cookies etc you need here then redirect to the returnUrl and / or header location
                    const currentUrl = $(`#${"requestUrlItem"}`);
                    window.location.href = currentUrl.attr('returnUrl');
                }
                else {
                    this.responseElem.text('I got code "' + (result.data.id || '') + 
                        '" and error message : " ' + (result.error.message || '')) +  '"';
                    this.responseElem.removeClass('valid-feedback').addClass('invalid-feedback').show();
                    lIcon.addClass('d-none');
                   this.userId.addClass('is-invalid')
                }
            });

        }

    }

}
