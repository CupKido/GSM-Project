import requests


#############################################
# Usage Instructions:                       #
# 1. Set the client_id and client_secret    #
#    using the set_access_data method       #
# 2. Use the other functons however you'd   #
#    like                                   #
#############################################

class twitch_wrapper:
    access_token = None
    initialized = False

    @classmethod
    def set_access_data(instance, client_id, client_secret):
        instance.client_id = client_id
        instance.client_secret = client_secret
        instance.initialized = True

    @classmethod
    def get_access_token(instance):
        if instance.access_token is not None:
            return instance.access_token
        url = "https://id.twitch.tv/oauth2/token"

        params = {
            "client_id": instance.client_id,
            "client_secret": instance.client_secret,
            "grant_type": "client_credentials",
            "scope": ("analytics:read:extensions analytics:read:games bits:read channel:edit:commercial "
                    "channel:manage:broadcast channel:manage:extensions channel:manage:polls channel:manage:predictions "
                    "channel:manage:redemptions channel:manage:schedule channel:manage:videos channel:read:editors "
                    "channel:read:goals channel:read:hype_train channel:read:polls channel:read:predictions "
                    "channel:read:redemptions channel:read:stream_key channel:read:subscriptions clips:edit moderation:read "
                    "moderator:manage:banned_users moderator:read:blocked_terms moderator:manage:blocked_terms "
                    "moderator:manage:automod moderator:read:automod_settings moderator:manage:automod_settings "
                    "moderator:read:chat_settings moderator:manage:chat_settings user:edit user:edit:follows "
                    "user:manage:blocked_users user:read:blocked_users user:read:broadcast user:read:email user:read:follows "
                    "user:read:subscriptions channel:moderate chat:edit chat:read whispers:read whispers:edit")
        }

        response = requests.post(url, params=params)
        responseData = response.json()
        access_token = responseData["access_token"]
        return access_token

    @classmethod
    def get_game_id(instance, game_name):
        url = f"https://api.twitch.tv/helix/games?name={game_name}"
        headers = {"Client-ID": instance.client_id, "Authorization": f"Bearer {instance.get_access_token()}"}
        response = requests.get(url, headers=headers)

        # if access token is expired, try to get a new one
        if response.status_code == 401:
            instance.access_token = None
            headers = {"Client-ID": instance.client_id, "Authorization": f"Bearer {instance.get_access_token()}"}
            response = requests.get(url, headers=headers)

        response.raise_for_status()  # raise an exception if the request fails
        games_data = response.json()["data"]
        if len(games_data) == 0:
            raise Exception(f"Game {game_name} not found")
        game_id = games_data[0]["id"]
        return game_id
    
    @classmethod
    def get_games_info_by_name(instance, game_name):
        return instance.get_games_info_by_id(instance.get_game_id(game_name))
    
    @classmethod
    def get_games_info_by_id(instance, game_id, game_name=None):
        # function variables
        page = 1
        # limit of pages to get
        limit = 25
        viewer_count = 0
        streamers_per_page = 100 # 1 is minimum, 100 is maximum
        base_url = 'https://api.twitch.tv/helix/streams'
        headers = {"Client-ID": instance.client_id, "Authorization": f"Bearer {instance.get_access_token()}"}
        url = base_url + f"?game_id={game_id}&first={streamers_per_page}"
        
        
        # get first page
        response = requests.get(url, headers=headers)
        # if access token is expired, try to get a new one
        if response.status_code == 401:
            instance.access_token = None
            headers = {"Client-ID": instance.client_id, "Authorization": f"Bearer {instance.get_access_token()}"}
            response = requests.get(url, headers=headers, timeout=60)


        response.raise_for_status()
        stream = response.json()
        stream_data = stream["data"]

        # if data and pagination are empty, game not found
        if not stream_data and not stream["pagination"]:
            raise Exception(f"Game with id {game_id} not found")
        first_ten_streamers = stream_data[:10]
        # if game_name is not provided, get it from the first streamer
        if game_name is None:
            game_name = stream_data[0]["game_name"]
        # get viewer count for the first page
        viewer_count += sum([stream["viewer_count"] for stream in stream_data])

        # if pagination and cursor are in the response, get the next page and count the viewers
        while "pagination" in stream.keys() and "cursor" in stream["pagination"].keys() and page <= limit:
            after = stream["pagination"]["cursor"]
            url = base_url + f"?game_id={game_id}&first={streamers_per_page}&after={after}"
            response = requests.get(url, headers=headers, timeout=60)
            response.raise_for_status()
            stream = response.json()
            stream_data = stream["data"]
            viewer_count += sum([stream["viewer_count"] for stream in stream_data])
            page += 1

        return {"game_name": game_name, "viewer_count": viewer_count, 'game_id': game_id, 'top_streamers': first_ten_streamers}
    
