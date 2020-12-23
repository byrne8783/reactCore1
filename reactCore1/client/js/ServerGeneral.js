import axios from 'axios';
var ServerGeneral = /** @class */ (function () {
    function ServerGeneral() {
        this._serverSite = axios.create({
            baseURL: window.location.protocol + "//" + window.location.host + "/",
            timeout: 20000,
            headers: {
                'X-Custom-Header': 'foobar',
                'Content-Type': 'application/json'
            }
        });
        this._serverSite.interceptors.response.use(function (r) {
            var x = r;
            return r;
        }, function (e) {
            var x = e;
            return Promise.reject(e);
        });
    }
    ServerGeneral.prototype.isAxiosError = function (obj) {
        return typeof obj.message === "string" && 'name' in obj && typeof obj.name === "string";
    };
    ServerGeneral.prototype.isAxiosResponse = function (obj) {
        return typeof obj.status === "number" && typeof obj.statusText === "string" && obj.hasOwnProperty('data');
    };
    ServerGeneral.prototype.isResponseDataGeneral = function (obj) {
        return typeof obj.id === "string" && obj.hasOwnProperty('data');
    };
    ServerGeneral.prototype.delay = function (milliS) {
        var reply = {
            hasValue: true, error: null, headers: null, data: { id: 'Delay.Timer', data: "$(milliS)" }, status: 0, raw: null
        };
        return new Promise(function (resolve) { return setTimeout(resolve, milliS, reply); });
    };
    ServerGeneral.prototype.post = function (route, data) {
        var _this = this;
        return this._serverSite.post(route, data)
            .then(function (response) {
            var reply = {
                hasValue: false, error: null, headers: null, data: { id: 'message', data: {} }, status: 0, raw: null
            };
            reply.raw = response;
            var axError = response;
            if (_this.isAxiosResponse(response)) {
                if (_this.isResponseDataGeneral(response.data)) {
                    reply.data = response.data;
                }
                else {
                    reply.data.id = "";
                    reply.data.data = response.data;
                }
                reply.status = response.status;
                reply.headers = _this.setReplyHeaders(response);
                if (response.status >= 400) {
                    reply.error = new Error("Status " + response.status + " received : " + response.statusText);
                }
            }
            else {
                if (_this.isAxiosError(axError)) {
                    reply.data.id = "message";
                    reply.data.data = axError.message;
                    if (axError.hasOwnProperty('response')) {
                        reply.status = axError.response.status;
                        reply.headers = _this.setReplyHeaders(axError.response);
                    }
                    else {
                        reply.status = -1;
                    }
                    reply.error = new Error("Error code " + axError.code + " : " + axError.message);
                }
            }
            reply.hasValue = true;
            return reply;
        })
            .catch(function (err) {
            var replyData = { id: 'message', data: {} };
            var reply = {
                hasValue: false, error: null, headers: null, data: replyData, status: -1, raw: err
            };
            if (_this.isAxiosError(err)) {
                reply.data.data = err.message;
                if (err.hasOwnProperty('response') && !(err.response == null)) {
                    reply.headers = _this.setReplyHeaders(err.response);
                    if (err.response.hasOwnProperty('status') && !(err.response == null)) {
                        reply.status = err.response.status || -1;
                    }
                }
                var errCode = '';
                if ('code' in err) {
                    errCode = "Error code " + (err.code || errCode) + " : ";
                }
                reply.error = new Error(errCode + " " + err.message);
                reply.data.id = errCode;
            }
            else {
                try {
                    reply.data.data = err.toString();
                }
                finally {
                    reply.error = new Error('An unknown error has occurred');
                }
            }
            reply.hasValue = true;
            return reply;
        });
    };
    ServerGeneral.prototype.postInTime = function (route, data, ui) {
        var activity = [this.post(route, data), this.delay(100)];
        return Promise.race(activity).then(function (firstUp) {
            // show the loading bar https://stackoverflow.com/questions/46376432/understanding-promise-race-usage
            // https://blog.jcore.com/2016/12/18/promise-me-you-wont-use-promise-race/
            // https://stackoverflow.com/questions/42341331/es6-promise-all-progress
            // https://blog.sessionstack.com/how-javascript-works-event-loop-and-the-rise-of-async-programming-5-ways-to-better-coding-with-2f077c4438b5
            if (firstUp.hasValue && firstUp.data.id === 'Delay.Timer') {
                try {
                    ui();
                }
                catch (_a) { }
                finally { }
                ; // The timer has resolved,
                return activity[0]; // stick with the real thing
            }
            else {
                return firstUp; // the real thing has resolved.  Thats our man
            }
        });
    };
    ServerGeneral.prototype.setReplyHeaders = function (incoming) {
        var result = {
            location: '', raw: null
        };
        if (incoming !== null && incoming !== undefined && incoming.hasOwnProperty('headers')) {
            result.raw = incoming.headers;
            result.location = incoming.headers.location || result.location;
        }
        return result;
    };
    return ServerGeneral;
}());
export default ServerGeneral;
//# sourceMappingURL=ServerGeneral.js.map