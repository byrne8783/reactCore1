import axios, { AxiosResponse, AxiosError, AxiosInstance } from 'axios';

interface ResponseDataGeneral {
    id: string;
    data: any;
}

interface ResponseGeneral {
    hasValue: boolean;
    error: Error;
    redirectUrl: string;
    data: ResponseDataGeneral;
    status: number;
    raw: AxiosResponse<object>;
}

export { ResponseGeneral, ResponseDataGeneral };

export default class ServerGeneral {
    private readonly _serverSite: AxiosInstance;

    private isAxiosError(obj: any): obj is AxiosError {
        return typeof obj.message === "string" && typeof obj.name === "string";
    }
    private isAxiosResponse(obj: any): obj is AxiosResponse {
        return typeof obj.status === "number" && typeof obj.statusText === "string" && obj.hasOwnProperty('data');
    }
    private isResponseDataGeneral(obj: any): obj is ResponseDataGeneral {
        return typeof obj.id === "string" && obj.hasOwnProperty('data');
    }

    private delay(milliS: number) :Promise<ResponseGeneral> {
        const reply: ResponseGeneral = {
            hasValue: true, error: null, redirectUrl: "", data: { id: 'Delay.Timer', data: `$(milliS)` }, status: 0, raw: null };
        return new Promise(resolve => setTimeout(resolve, milliS,reply));
    }

    public post(route : string,data:any) : Promise<ResponseGeneral> {
        return this._serverSite.post(route, data)
            .then((response) => {
                const reply: ResponseGeneral = { hasValue: false, error: null, redirectUrl: "", data: null, status: 0, raw: null };
                reply.raw = response;
                const axError: any = response;
                if (this.isAxiosResponse(response)) {
                    if (this.isResponseDataGeneral(response.data)) {
                        reply.data = response.data as ResponseDataGeneral; 
                    }
                    else {
                        reply.data.id = ""
                        reply.data.data = response.data;
                    }
                    reply.status = response.status;
                    if (response.status >= 400) {
                        reply.error = new Error(`Status ${response.status} received : ${response.statusText}`);
                    }
                }
                else {
                    if (this.isAxiosError(axError)) {
                        reply.data.id = "message";
                        reply.data.data = axError.message;
                        reply.status = -1;
                        reply.error = new Error(`Error code ${axError.code} : ${axError.message}`);
                    }
                }
                reply.hasValue = true;
                return reply;
            })
            .catch((err) => {
                const replyData: ResponseDataGeneral = { id: 'message', data: {}};
                const reply: ResponseGeneral = {
                    hasValue: false, error: null, redirectUrl: "", data: replyData, status: -1, raw: err };
                if (this.isAxiosError(err)) {
                    reply.data.data = err.message;
                    reply.error = new Error(`Error code ${err.code} : ${err.message}`);
                }
                else {
                    try {
                        reply.data.data = err.toString();
                    }
                    finally {
                        reply.error = new Error('An unknown error has occurred')
                    }
                }
                reply.hasValue = true;
                return reply;
            });
    }

    public postInTime(route: string, data: any,ui?:()=>void): Promise<ResponseGeneral> {
        const activity = [this.post(route, data), this.delay(100)];
        return Promise.race(activity).then((firstUp:ResponseGeneral) => {
             // show the loading bar https://stackoverflow.com/questions/46376432/understanding-promise-race-usage
            // https://blog.jcore.com/2016/12/18/promise-me-you-wont-use-promise-race/
			// https://stackoverflow.com/questions/42341331/es6-promise-all-progress
            // https://blog.sessionstack.com/how-javascript-works-event-loop-and-the-rise-of-async-programming-5-ways-to-better-coding-with-2f077c4438b5
            if (firstUp.hasValue && firstUp.data.id === 'Delay.Timer') {
                try { ui(); } catch { } finally { };    // The timer has resolved,
                return activity[0];                     // stick with the real thing
            }
            else {
                return firstUp;                               // the real thing has resolved.  Thats our man
            }
        });

    }

    constructor() {
        this._serverSite = axios.create({
            baseURL: window.location.protocol + "//" + window.location.host + "/" + window.location.pathname.split('/')[1],
            timeout: 20000,
            headers: {
                'X-Custom-Header': 'foobar',
                'Content-Type': 'application/json'
            }
        });
        this._serverSite.interceptors.response.use((r: any) => {
            let x: any = r;
            return r;
        },
            (e) => {
                let x: any = e;
                return e;
            });
    }
}


