export default class TSLib {
    textMessage: string;
    constructor() {
        this.textMessage = "Data from TS class TSLib!";
    }

    getData() {
        return this.textMessage;
    }
}