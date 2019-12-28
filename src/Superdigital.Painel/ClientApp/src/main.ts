import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

var apiUrl = {
  base_dev: "http://localhost:5000/api",
  base_prod: ""
}

export function getBaseUrl() {
  if (environment.production) {
    return apiUrl.base_prod;
  } else {
    return apiUrl.base_dev;
  }
}

export const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
