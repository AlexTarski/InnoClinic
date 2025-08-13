interface AppConfig {
	authUrl: string;
	profilesUrl: string;
	clientUiUrl: string;
}

interface Window {
	appConfig: AppConfig;
}