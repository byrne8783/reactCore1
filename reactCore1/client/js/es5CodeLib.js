var ES5Lib = (function () {
    function ES5Lib() {
        this.textMessage = "Data from getData function in ES5Lib.js!!";
    }
    ES5Lib.prototype.getData = function () {
        return this.textMessage;
    };
    return ES5Lib;
}());
export default ES5Lib;

//getText = function () {
//    return "Data from getText function in lib.js!";
//};


