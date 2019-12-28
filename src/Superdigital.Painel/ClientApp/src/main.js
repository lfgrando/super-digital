"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var app_module_1 = require("./app/app.module");
var environment_1 = require("./environments/environment");
var apiUrl = {
    base_dev: "http://localhost:5000/api",
    base_prod: ""
};
function getBaseUrl() {
    if (environment_1.environment.production) {
        return apiUrl.base_prod;
    }
    else {
        return apiUrl.base_dev;
    }
}
exports.getBaseUrl = getBaseUrl;
exports.providers = [
    { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
];
if (environment_1.environment.production) {
    core_1.enableProdMode();
}
platform_browser_dynamic_1.platformBrowserDynamic(exports.providers).bootstrapModule(app_module_1.AppModule)
    .catch(function (err) { return console.log(err); });
//# sourceMappingURL=main.js.map