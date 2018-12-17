//$ = require('jquery');  don't need to require it as it is included by the plugin

//require('./lib');  don't need to require it as it is imported???????

import ES5Lib from './es5Codelib';
import ES6Lib from './es6CodeLib';
import 'bootstrap/dist/css/bootstrap.min.css';  //not altogether clear to me how this finds bootstrap
import * as styles from '../css/site.scss';
import TSLib from '../ts/tsCodeLib';

let myES5Object = new ES5Lib();
document.getElementById("fillthis").innerHTML = myES5Object.getData();
//document.getElementById("fillthis").innerHTML = getText();
$('#fillthiswithjquery').html('Filled by Jquery!');
let myES6Object = new ES6Lib();
$('#fillthiswithes6lib').html(myES6Object.getData());
let myTSObject = new TSLib();
$('#fillThisWithTSlib').html(myTSObject.getData());
