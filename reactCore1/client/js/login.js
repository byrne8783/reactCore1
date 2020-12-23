import ServerGeneral from './ServerGeneral';
var Login = /** @class */ (function () {
    function Login(modalTag) {
        var _this = this;
        this.modalTag = modalTag;
        this.dialog = $("#" + modalTag);
        this.dialog.on('show.bs.modal', function (evt) { return _this.initLogin(evt); });
        var btn = this.dialog.find("#submitLogin");
        btn.click(function (evt) { return _this.submitLogin(evt); });
        this.userId = this.dialog.find('#userId1');
        this.userId.off("change").on("change", function (evt) { return _this.clearDialog(evt); });
    }
    Login.prototype.clearDialog = function (evt) {
        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("").hide();
    };
    Login.prototype.initLogin = function (evt) {
        this.loadingIcon = this.dialog.find('span#iconLoadingSignin');
        this.responseElem = this.dialog.find("#happyIn");
        this.loadingIcon.addClass('d-none');
        this.clearDialog(evt);
    };
    Login.prototype.submitLogin = function (evt) {
        var _this = this;
        if ((this.userId.val() != "") && ($("#userId2").val() != "")) {
            var data = JSON.stringify({
                userId: $("#userId1").val(),
                password: $("#userId2").val()
            });
            var serv = new ServerGeneral();
            var submitButton = $(evt.target);
            var lIcon = submitButton.find('span#iconLoadingSignin');
            var ui = void 0;
            ui = function () {
                if (lIcon) {
                    lIcon.removeClass('d-none');
                }
            };
            serv.postInTime('User/Login', data, ui).then(function (result) {
                if (result.hasValue && !result.error) {
                    lIcon.addClass('d-none');
                    _this.responseElem.removeClass('invalid-feedback').addClass('valid-feedback').show().text("Welcome back.");
                    _this.userId.addClass("is-valid");
                    // sort out any cookies etc you need here then redirect to the returnUrl and / or header location
                    var currentUrl = $("#" + "requestUrlItem");
                    window.location.href = currentUrl.attr('returnUrl');
                }
                else {
                    _this.responseElem.text('I got code "' + (result.data.id || '') +
                        '" and error message : " ' + (result.error.message || '')) + '"';
                    _this.responseElem.removeClass('valid-feedback').addClass('invalid-feedback').show();
                    lIcon.addClass('d-none');
                    _this.userId.addClass('is-invalid');
                }
            });
        }
    };
    return Login;
}());
export default Login;
//# sourceMappingURL=login.js.map