import axios, {AxiosResponse, AxiosError } from 'axios';
import serverSite from './app';
export default class Login {

    private id: string;
    private btn: JQuery;
    baseUrl: string;
    public rsp = {
        HasValue: <boolean> false,
        Error: <Error> null,
        RedirectUrl : <string> null,
        Data: <any> void 0,
        status: <number> null,
        raw: <AxiosResponse<object>> null
    };
    //public rsp: {
    //    HasValue: boolean;
    //    Error: Error;
    //    RedirectUrl: string;
    //    Data: any;
    //    status: number;
    //    Raw: AxiosResponse<object>;
    //    } ;
    private rspMessage: any;
    constructor(public tagId: string) {
        this.id = "#" + tagId;
        this.btn = $(this.id);
        this.btn.click((evt) => this.submitLogin(evt))
        this.baseUrl = serverSite.defaults.baseURL;
        this.rspMessage = null
        this.rsp.HasValue = false;
    }
    private isAxiosError(obj: any): obj is AxiosError {
        return typeof obj.message === "string" && typeof obj.name === "string";
    }
    private isAxiosResponse(obj: any): obj is AxiosResponse {
        var x: AxiosResponse;
        return typeof obj.status === "number" && typeof obj.statusText === "string" && obj.hasOwnProperty('data');
    }

    public  submitLogin(evt: JQuery.Event): void { //, ...args: any[]
        //Where the '...args' can be anything you like that will give you the number and types of parameters you want.
        if (($("#userId1").val() != "") && ($("#userId2").val() != "")) {
            this.ajaxStart();
            const data = JSON.stringify({
                userId: $("#userId1").val(),
                password: $("#userId2").val()
            });
            //const url = this.baseUrl + '/Home/Login';
            //const ajaxHheaders = {
            //    'Content-Type': 'application/json',
            //};
            //const request1 =  axios.post(url, data, {
            //    headers: ajaxHheaders
            //})
            serverSite.interceptors.response.use((r:any) => {
                let x: any = r;
                return r;
            },
                (e) => {
                    let x: any = e;
                    return e;
                });
            this.rsp.HasValue = false;
            serverSite.post('User/Login',data)
                .then((response) => {
                    this.rsp.raw = response;
                    this.rsp.Data = null;
                    this.rsp.Error = null;
                    this.rsp.status = 0;
                    this.rsp.RedirectUrl = "";
                    const axError : any = response;
                    if (this.isAxiosResponse(response)) {
                        this.rsp.Data = response.data;
                        this.rsp.status = response.status;
                        if (response.status >= 400) {
                            this.rsp.Error = new Error('Status ${response.status} received : ${response.statusText}');
                        }
                    }
                    else {
                        if (this.isAxiosError(axError)) {
                            this.rsp.Data = axError.message;
                            this.rsp.status = -1;
                            this.rsp.Error = new Error('Error code ${axError.code} : ${axError.message}');
                        }
                    }
                    this.rsp.HasValue = true;
                })
                .catch((err) => {
                    this.rsp.raw = err;
                    const axError = <AxiosError>err;
                    if ( err.hasOwnProperty('code') ) {

                    }
                    this.rsp.Error = new Error(err);
                    this.rsp.HasValue = true;
                });
// process the rsp here 
            const wtf = this.rsp;
        }
        this.ajaxStop();
    }

    private ajaxStart() {
        this.btn.show();
    }
    private ajaxStop() {
        this.btn.hide();
    }



}