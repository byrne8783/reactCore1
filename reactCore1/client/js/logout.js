var Logout = /** @class */ (function () {
    function Logout(modalTag) {
        var _this = this;
        this.modalTag = modalTag;
        this.dialog = $("#" + modalTag);
        this.dialog.on('show.bs.modal', function (evt) { return _this.showLogout(evt); });
        var btn = this.dialog.find("#submitLogout");
        btn.click(function (evt) { return _this.submitLogout(evt); });
    }
    Logout.prototype.initialise = function (evt) {
        this.loadingIcon = this.dialog.find('span#iconLoadingSignout');
        //        this.userId = this.dialog.find('#userId1');
        this.responseElem = this.dialog.find("#happyOut");
        this.loadingIcon.addClass('d-none');
        //        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("");
    };
    Logout.prototype.showLogout = function (evt) {
        this.initialise(evt);
    };
    Logout.prototype.submitLogout = function (evt) {
    };
    return Logout;
}());
export default Logout;
//# sourceMappingURL=logout.js.map