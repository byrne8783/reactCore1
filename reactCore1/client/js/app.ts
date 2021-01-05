import axios, { AxiosResponse, AxiosError, AxiosInstance } from 'axios';
import JQ5Lib from './jq5Codelib';
import ES5Lib from './es5Codelib';
import ES6Lib from './es6CodeLib';

import 'bootstrap';                             //This runs the global code in Bootstrap's Javascript

import '../css/site.scss';
import TSLib from '../js/tsCodeLib';
import Login from '../js/login';
import Logout from '../js/logout';
import { library, dom as faDom, icon as faIcon, findIconDefinition } from '@fortawesome/fontawesome-svg-core/index.es.js';
import { fas } from '@fortawesome/free-solid-svg-icons/index.es.js';
import { faUser } from "@fortawesome/free-solid-svg-icons/faUser.js";
import { faCaretDown,faKey,faSpinner } from '@fortawesome/free-solid-svg-icons/index.es.js'

library.add(faUser,faCaretDown,faKey,faSpinner);

faDom.watch();  //only if i'm dynamically adding  i tags?

import UserGreetings from '../js/components/reactHooks1';
import { ResponseGeneral } from './ServerGeneral';





 
const serverSite = axios.create({
    baseURL: window.location.protocol + "//" + window.location.host + "/" + window.location.pathname.split('/')[1],
    timeout: 20000,
    headers: {
        'X-Custom-Header': 'foobar',
        'Content-Type': 'application/json'}
});
export default serverSite;

let loginStuff;
let logoutStuff;
$(document).ready(function () {
    loginStuff = new Login("signinModal");
    logoutStuff = new Logout("signoutModal");
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
