import serverSite from './app';
import ServerGeneral,{ ResponseGeneral, ResponseDataGeneral } from './ServerGeneral'
export default class Login {

    private dialog: JQuery;btn: JQuery;
    private userId: JQuery; responseElem: JQuery; loadingIcon: JQuery;
    constructor(public modalTag: string) {
        this.dialog = $(`#${modalTag}`);
        this.dialog.on('show.bs.modal', (evt) => this.showLogin(evt));
        this.btn = this.dialog.find(`#submitLogin`);
        this.btn.click((evt) => this.submitLogin(evt))
        this.loadingIcon = this.btn.children(".iconLoadingSignIn");
        this.userId = this.dialog.find("#userId1");
        this.responseElem = this.dialog.find(`#happyOut`);
    }

    public showLogin(evt: JQuery.Event): void {         // initialise the dialog
        this.loadingIcon.addClass('d-none');
        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("");
    }

    public submitLogin(evt: JQuery.Event): void { 
        if ((this.userId.val() != "") && ($("#userId2").val() != "")) {
            this.loadingIcon.removeClass('d-none');
            const data = JSON.stringify({
                userId: $("#userId1").val(),
                password: $("#userId2").val()
            });
            const serv: ServerGeneral = new ServerGeneral();
            serv.post('User/Login', data).then((result) => {
                if (result.hasValue && !result.error) {
                    let replyData = result.data.data;
                    this.responseElem.text(`Welcome back ${replyData.name}`)
                    this.userId.addClass("is-valid");
                }
                else {
                    this.responseElem.text('OK I got ' + (result.hasValue || '') +
                        ' with ' + (result.data.id || '') + 
                        ' and error message : ' + (result.error.message || ''));
                    this.userId.addClass('is-invalid')
                }
                this.loadingIcon.addClass('d-none');
            });

        }

    }


}