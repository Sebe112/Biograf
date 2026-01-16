# Biografweb

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.2.0.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Troubleshooting local TypeScript errors

If your editor shows `Cannot find module '@angular/core'` (or other `@angular/*` imports) even though the project builds, ensure the workspace TypeScript version is used:

1. Run `npm ci` to restore dependencies (this project targets TypeScript `~5.7.2`).
2. In VS Code, use the **Select TypeScript Version** command and pick **Use Workspace Version**, or rely on the provided `.vscode/settings.json` which pins `typescript.tsdk` to `./node_modules/typescript/lib`.
3. Reload the window. The diagnostics should disappear once the editor loads the workspace compiler.

These steps are safe on Windows and macOS and keep editor diagnostics aligned with the Angular CLI build.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
