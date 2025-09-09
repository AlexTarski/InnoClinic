interface AppConfig {
	authUrl: string;
	profilesUrl: string;
	officesUrl: string;
	employeeUiUrl: string;
}

interface Window {
	appConfig: AppConfig;
}