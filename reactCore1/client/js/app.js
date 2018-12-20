//$ = require('jquery');  don't need to require it as it is included by the plugin
import JQ5Lib from './jq5Codelib';
import ES5Lib from './es5Codelib';
import ES6Lib from './es6CodeLib';
import 'bootstrap/dist/css/bootstrap.min.css'; //not altogether clear to me how this finds bootstrap
import 'bootstrap'; //I'de love to have done this with the plugin too.  Who knows?
import TSLib from '../js/tsCodeLib';
$(document).ready(function () {
    $("body").tooltip({ selector: '[data-toggle=tooltip]' });
});
var myES5Object = new ES5Lib();
document.getElementById("fillthis").innerHTML = myES5Object.getData();
var myJQ5Object = new JQ5Lib();
$('#fillthiswithjquery').html(myJQ5Object.getData());
var myES6Object = new ES6Lib();
$('#fillthiswithes6lib').html(myES6Object.getData());
var myTSObject = new TSLib();
$('#fillThisWithTSlib').html(myTSObject.getData());
//# sourceMappingURL=app.js.map