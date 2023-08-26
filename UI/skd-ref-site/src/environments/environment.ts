// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  // baseUrl: 'https://testapi.sketchdaily.net/ReferenceSite/', // TO DO - should rename this
  baseUrl: 'https://localhost:7224/ReferenceSite/',

  imageUrl: 'https://files.sketchdaily.net/references',
  // imageUrl: 'http://localhost:15285',
  auth0RedirectUri: 'http://localhost:4200/callback/en'
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
