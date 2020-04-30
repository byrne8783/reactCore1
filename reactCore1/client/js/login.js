import ServerGeneral from './ServerGeneral';
var Login = /** @class */ (function () {
    function Login(modalTag) {
        var _this = this;
        this.modalTag = modalTag;
        this.dialog = $("#" + modalTag);
        this.dialog.on('show.bs.modal', function (evt) { return _this.showLogin(evt); });
        var btn = this.dialog.find("#submitLogin");
        btn.click(function (evt) { return _this.submitLogin(evt); });
    }
    Login.prototype.initialise = function (evt) {
        this.loadingIcon = this.dialog.find('span#iconLoadingSignin');
        this.userId = this.dialog.find('#userId1');
        this.responseElem = this.dialog.find("#happyIn");
        this.loadingIcon.addClass('d-none');
        this.userId.removeClass(['is-valid', 'is-invalid']);
        this.responseElem.text("");
    };
    Login.prototype.showLogin = function (evt) {
        this.initialise(evt);
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
                    var replyData = result.data.data;
                    lIcon.addClass('d-none');
                    _this.responseElem.text("Welcome back " + replyData.name);
                    _this.userId.addClass("is-valid");
                    // sort out any cookies etc you need here then redirect to the returnUrl and / or header location
                    var currentUrl = $("#" + "requestUrlItem");
                    window.location.href = currentUrl.attr('returnUrl');
                }
                else {
                    _this.responseElem.text('I got ' + (result.hasValue || '') +
                        ' with ' + (result.data.id || '') +
                        ' and error message : ' + (result.error.message || ''));
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