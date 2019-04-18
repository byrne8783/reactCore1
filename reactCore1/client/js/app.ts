import axios, { AxiosResponse, AxiosError ,AxiosInstance} from 'axios'
import JQ5Lib from './jq5Codelib';
import ES5Lib from './es5Codelib';
import ES6Lib from './es6CodeLib';

import 'bootstrap';                             //This runs the global code in Bootstrap's Javascript

import '../css/site.scss';
import TSLib from '../js/tsCodeLib';
import Login from '../js/login';
import { library, dom as faDom, icon as faIcon, findIconDefinition } from '@fortawesome/fontawesome-svg-core/index.es.js';
import { fas } from '@fortawesome/free-solid-svg-icons/index.es.js'
import { faUser } from "@fortawesome/free-solid-svg-icons/faUser.js";
import { faCaretDown,faKey,faSpinner } from '@fortawesome/free-solid-svg-icons/index.es.js'
////const myCheck = faIcon(findIconDefinition({ prefix: 'fas', iconName: 'igloo' }));
//    <span class="caret" > </span>

library.add(faUser,faCaretDown,faKey,faSpinner);
//const iconUser = findIconDefinition({ iconName: 'user', prefix: 'fas' });
//const glasses = findIconDefinition({ iconName: 'glasses', prefix: 'fas' });
////faLibrary.add(faIgloo,faUser);

//const iUser = faIcon(iconUser);
//const iGlasses = faIcon(glasses);

faDom.watch();  //only if i'm dynamically adding  i tags?

import UserGreetings from '../js/components/reactHooks1';

let loginStuff;

interface ResponseGeneral {
    hasValue: boolean;
    error: Error;
    redirectUrl: string;
    data: any;
    status: number;
    raw: AxiosResponse<object>;
}
interface ResponseDataGeneral {
    id: string;
    data: any;
}

class ServerGeneral {
    private readonly _serverSite: AxiosInstance;
    private isAxiosError(obj: any): obj is AxiosError {
        return typeof obj.message === "string" && typeof obj.name === "string";
    }
    private isAxiosResponse(obj: any): obj is AxiosResponse {
        return typeof obj.status === "number" && typeof obj.statusText === "string" && obj.hasOwnProperty('data');
    }
    private isResponseDataGeneral(obj: any): obj is ResponseDataGeneral {
        return typeof obj.id === "string" && obj.hasOwnProperty('data');
    }
    public post(route : string,data:any) : ResponseGeneral {
        const reply: ResponseGeneral = {hasValue:false,error:null,redirectUrl:"",data:null,status:0,raw:null} ;
        this._serverSite.post(route, data)
            .then((response) => {
                reply.raw = response;
                const axError: any = response;
                if (this.isAxiosResponse(response)) {
                    if (this.isResponseDataGeneral(response.data)) {
                        reply.data = response.data as ResponseDataGeneral; 
                    }
                    else {
                        reply.data = response.data;
                    }
                    reply.status = response.status;
                    if (response.status >= 400) {
                        reply.error = new Error('Status ${response.status} received : ${response.statusText}');
                    }
                }
                else {
                    if (this.isAxiosError(axError)) {
                        reply.data = axError.message;
                        reply.status = -1;
                        reply.error = new Error('Error code ${axError.code} : ${axError.message}');
                    }
                }
                reply.hasValue = true;
            })
            .catch((err) => {
                reply.raw = err;
                const axError = <AxiosError>err;
                if (err.hasOwnProperty('code')) {

                }
                reply.error = new Error(err);
                reply.hasValue = true;
            });

        return reply;
    }


    constructor() {
        this._serverSite = axios.create({
            baseURL: window.location.protocol + "//" + window.location.host + "/" + window.location.pathname.split('/')[1],
            timeout: 20000,
            headers: {
                'X-Custom-Header': 'foobar',
                'Content-Type': 'application/json'
            }
        });
        this._serverSite.interceptors.response.use((r: any) => {
            let x: any = r;
            return r;
        },
            (e) => {
                let x: any = e;
                return e;
            });
    }
}



 
const serverSite = axios.create({
    baseURL: window.location.protocol + "//" + window.location.host + "/" + window.location.pathname.split('/')[1],
    timeout: 20000,
    headers: {
        'X-Custom-Header': 'foobar',
        'Content-Type': 'application/json'}
});
export default serverSite;

$(document).ready(function () {
    loginStuff = new Login("submitLogin");
    $("body").tooltip({ selector: '[data-toggle=tooltip]' });
});

let myES5Object = new ES5Lib();
document.getElementById("fillthis").innerHTML = myES5Object.getData();
let myJQ5Object = new JQ5Lib();
$('#fillthiswithjquery').html(myJQ5Object.getData());
let myES6Object = new ES6Lib();
$('#fillthiswithes6lib').html(myES6Object.getData());
let myTSObject = new TSLib();
$('#fillThisWithTSlib').html(myTSObject.getData());
