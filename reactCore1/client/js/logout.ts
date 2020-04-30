import serverSite from './app';
import ServerGeneral,{ ResponseGeneral, ResponseDataGeneral } from './ServerGeneral'
export default class Logout {
    private dialog: JQuery;//btn: JQuery;
    private userId: JQuery; responseElem: JQuery;
    private loadingIcon: JQuery;

    constructor(public modalTag: string) {
        this.dialog = $(`#${modalTag}`);
        this.dialog.on('show.bs.modal', (evt: JQuery.Event) => this.showLogout(evt));
        const btn = this.dialog.find(`#submitLogout`);
        btn.click((evt: JQuery.Event) => this.submitLogout(evt))
    }
    private initialise(evt): void {         // initialise the logout dialog
        this.loadingIcon = this.dialog.find('span#iconLoadingSignout');
//        this.userId = this.dialog.find('#userId1');
        this.responseElem = this.dialog.find(`#happyOut`);
        this.loadingIcon.addClass('d-none');
//        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("");
    }

    public showLogout(evt: JQuery.Event): void {
        this.initialise(evt);
    }

    public submitLogout(evt): void {

    }

}