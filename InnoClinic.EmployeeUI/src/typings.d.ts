interface AppConfig {
	authUrl: string;
	profilesUrl: string;
	officesUrl: string;
	employeeUiUrl: string;
	filesUrl: string;
}

interface Window {
	appConfig: AppConfig;
}