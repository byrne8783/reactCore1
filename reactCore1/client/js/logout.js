import ServerGeneral from './ServerGeneral';
var Logout = /** @class */ (function () {
    function Logout(modalTag) {
        var _this = this;
        this.modalTag = modalTag;
        this.dialog = $("#" + modalTag);
        this.dialog.on('show.bs.modal', function (evt) { return _this.showLogout(evt); });
        this.submitButton = this.dialog.find("#submitLogout");
        this.submitButton.click(function (evt) { return _this.submitLogout(evt); });
        this.uiShow = function (e) {
            if (e != null) {
                e.removeClass('d-none');
            }
        };
        this.uiHide = function (e) {
            if (e != null) {
                e.addClass('d-none');
            }
        };
    }
    Logout.prototype.initialise = function (evt) {
        this.loadingIcon = this.dialog.find('span#iconLoadingSignout');
        this.responseElem = this.dialog.find("#happyOut");
        this.uiHide(this.loadingIcon);
        this.responseElem.text("").hide();
    };
    Logout.prototype.processLogout = function (response) {
        this.uiHide(this.loadingIcon);
        var currentUrlElem = $("#" + "requestUrlItem");
        var returnUrl = currentUrlElem.attr('returnUrl');
        window.location.href = returnUrl;
    };
    Logout.prototype.showLogout = function (evt) {
        this.initialise(evt);
    };
    Logout.prototype.submitLogout = function (evt) {
        var _this = this;
        var serv = new ServerGeneral();
        var eventTarget = $(evt.target);
        var loadingIconShow;
        loadingIconShow = function () {
            _this.uiShow(_this.loadingIcon);
        };
        serv.postInTime('User/Logout', null, loadingIconShow).then(function (result) {
            if (result.hasValue && !result.error) {
                _this.responseElem.removeClass('invalid-feedback').addClass('valid-feedback').show().text("Goodbye. Come back soon. ");
                _this.processLogout(result);
            }
            else {
                _this.uiHide(_this.loadingIcon);
                _this.responseElem.removeClass('valid-feedback').addClass('invalid-feedback').show().text('I got code "' + (result.data.id || '') +
                    '" and error message : " ' + (result.error.message || '')) + '"';
            }
        });
    };
    return Logout;
}());
export default Logout;
//# sourceMappingURL=logout.js.map