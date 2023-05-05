import requests

class steamworks_wrapper:

    @classmethod
    def get_app_id(instance, game_name):
        url = 'https://api.steampowered.com/ISteamApps/GetAppList/v2/'

        response = requests.get(url)
        response.raise_for_status()
        app_list = response.json()
        for app in app_list['applist']['apps']:
            if game_name.lower() == app['name'].lower():
                return app['appid']
        raise Exception(f"Game {game_name} not found")

    @classmethod
    def get_game_player_count(instance, game_name):
        app_id = instance.get_app_id(game_name)
        url = f'https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={app_id}' 
        response = requests.get(url)
        response.raise_for_status()
        return response.json()['response']['player_count']