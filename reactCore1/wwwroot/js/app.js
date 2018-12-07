//$ = require('jquery');  don't need to require it as it is included by the plugin

require('./lib');

import ES6Lib from './es6CodeLib';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../css/site.css';


document.getElementById("fillthis").innerHTML = getText();
$('#fillthiswithjquery').html('Filled by Jquery!');
let myES6Object = new ES6Lib();
$('#fillthiswithes6lib').html(myES6Object.getData());
