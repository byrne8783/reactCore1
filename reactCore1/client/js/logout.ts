import serverSite from './app';
import ServerGeneral,{ ResponseGeneral, ResponseDataGeneral } from './ServerGeneral'
export default class Logout {
    private dialog: JQuery; private submitButton: JQuery; 
    private loadingIcon: JQuery; private responseElem: JQuery;
    private uiShow: (elem: JQuery) => void; private uiHide: (elem: JQuery) => void;

    constructor(public modalTag: string) {
        this.dialog = $(`#${modalTag}`);
        this.dialog.on('show.bs.modal', (evt: JQuery.Event) => this.showLogout(evt));
        this.submitButton = this.dialog.find(`#submitLogout`);
        this.submitButton.click((evt: JQuery.Event) => this.submitLogout(evt));
        this.uiShow = (e) => {
            if (e!= null) {
                e.removeClass('d-none');
            }
        }
        this.uiHide = (e) => {
            if (e != null) {
                e.addClass('d-none');
            }
        }

    }

    private initialise(evt): void {         // initialise the logout dialog
        this.loadingIcon = this.dialog.find('span#iconLoadingSignout');
        this.responseElem = this.dialog.find(`#happyOut`);
        this.uiHide(this.loadingIcon);
        this.responseElem.text("").hide();

    }

    private processLogout(response: ResponseGeneral): void {         // complete the logout dialog
        this.uiHide(this.loadingIcon);
        const currentUrlElem = $(`#${"requestUrlItem"}`);
        const returnUrl = currentUrlElem.attr('returnUrl');
        window.location.href = returnUrl;

    }

    public showLogout(evt: JQuery.Event): void {
        this.initialise(evt);

    }

    public submitLogout(evt): void {
        const serv: ServerGeneral = new ServerGeneral();
        const eventTarget = $(evt.target as HTMLButtonElement);
        type uiIndication = () => void;
        let loadingIconShow: uiIndication;
        loadingIconShow = () => {
            this.uiShow(this.loadingIcon) ;
        }
        serv.postInTime('User/Logout', null, loadingIconShow).then((result) => {
            if (result.hasValue && !result.error) {
                this.responseElem.removeClass('invalid-feedback').addClass('valid-feedback').show().text(`Goodbye. Come back soon. `);
                this.processLogout(result);
            }
            else {
                this.uiHide(this.loadingIcon);
                this.responseElem.removeClass('valid-feedback').addClass('invalid-feedback').show().text('I got code "' + (result.data.id || '') +
                    '" and error message : " ' + (result.error.message || '')) + '"';                
            }
        });

    }

}