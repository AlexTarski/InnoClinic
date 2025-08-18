interface AppConfig {
	authUrl: string;
	profilesUrl: string;
	employeeUiUrl: string;
}

interface Window {
	appConfig: AppConfig;
}