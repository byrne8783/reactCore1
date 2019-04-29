import serverSite from './app';
import ServerGeneral,{ ResponseGeneral, ResponseDataGeneral } from './ServerGeneral'
export default class Login {

    private dialog: JQuery;//btn: JQuery;
    private userId: JQuery; responseElem: JQuery;
    private loadingIcon: JQuery; 

    constructor(public modalTag: string) {
        this.dialog = $(`#${modalTag}`);
        this.dialog.on('show.bs.modal', (evt:JQuery.Event) => this.showLogin(evt));
        const btn = this.dialog.find(`#submitLogin`);
        btn.click((evt: JQuery.Event) => this.submitLogin(evt))
    }
    private initialise(evt): void {         // initialise the dialog
        this.loadingIcon = this.dialog.find('span#iconLoadingSignin');
        this.userId = this.dialog.find('#userId1');
        this.responseElem = this.dialog.find(`#happyOut`);
        this.loadingIcon.addClass('d-none');
        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("");
    }

    public showLogin(evt: JQuery.Event): void {
        this.initialise(evt);
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
                    let replyData = result.data.data;
                    lIcon.addClass('d-none');
                    this.responseElem.text(`Welcome back ${replyData.name}`)
                    this.userId.addClass("is-valid");
                }
                else {
                    this.responseElem.text('I got ' + (result.hasValue || '') +
                        ' with ' + (result.data.id || '') + 
                        ' and error message : ' + (result.error.message || ''));
                    lIcon.addClass('d-none');
                   this.userId.addClass('is-invalid')
                }
            });

        }

    }

}