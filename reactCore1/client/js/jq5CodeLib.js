var JQ5Lib = (function () {
    function JQ5Lib() {
        this.textMessage = "Filled by jQuery from getData function in jq5Lib.js!";
    }
    JQ5Lib.prototype.getData = function () {
        return this.textMessage;
    };
    return JQ5Lib;
}());
export default JQ5Lib;
