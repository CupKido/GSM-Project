from flask import Flask, jsonify
import constants
import random
from steamworks_wrapper import steamworks_wrapper
from twitch_wrapper import twitch_wrapper
from dotenv import dotenv_values
import argparse
config = dotenv_values('.env')

app = Flask(__name__)

games = {}
PLAYERS_COUNT = 'PlayersCount'
CPU_USAGE = 'CPUUsage'
MAX_CPU = 'MaxCPU'
RAM_USAGE = 'RAMUsage'
RAM_SIZE = 'RAMSize'
GAME_NAME = 'GameName'
SOURCE = 'Source'
TOP_SCORE = 'TopScore'
TEMPERATURE = 'Temperature'
twitch_wrapper.set_access_data(config['TWITCH_CLIENT_ID'], config['TWITCH_API_SECRET'])

@app.route('/GetData/<game_name>', methods=['GET'])
def get_data(game_name):
    # Replace this with your actual data retrieval logic
    if game_name not in games.keys():
        games[game_name] = {}
        games[game_name][MAX_CPU] = random.choice(constants.CPU_GHz_options)
        games[game_name][RAM_SIZE] = random.choice(constants.RAM_size_options)
        games[game_name][GAME_NAME] = game_name
    get_data_estimate(game_name)
    return jsonify(games[game_name])

def get_data_estimate(game_name):
    try:
        players_count = steamworks_wrapper.get_game_player_count(game_name)
        games[game_name][PLAYERS_COUNT] = players_count
        games[game_name][SOURCE] = 'Steam'
    except:
        players_count = None
    if players_count is None or players_count == 0:
        try:
            info = twitch_wrapper.get_games_info_by_name(game_name)
            if 'viewer_count' in info.keys():
                players_count = info['viewer_count'] * 70
                if players_count > constants.MAX_PLAYES_COUNT:
                    players_count = info['viewer_count'] * 3
                games[game_name][SOURCE] = 'Twitch based estimate'
        except Exception as e:
            print(e)
            if PLAYERS_COUNT in games[game_name].keys():
                players_count = games[game_name][PLAYERS_COUNT] + (100 * (1 if random.randint(0, 1) else -1))
            else:
                players_count = random.randint(0, constants.MAX_PLAYES_COUNT)
            games[game_name][SOURCE] = 'Random'
    print(games[game_name][SOURCE])
    print(players_count)
    if players_count > constants.MAX_PLAYES_COUNT:
        players_count = constants.MAX_PLAYES_COUNT

    games[game_name][PLAYERS_COUNT] = players_count
    # add to CPU usage
    cpu_usage = (games[game_name][PLAYERS_COUNT] / constants.MAX_PLAYES_COUNT) * games[game_name][MAX_CPU]
    if cpu_usage > games[game_name][MAX_CPU]:
        cpu_usage = games[game_name][MAX_CPU]


    # add to RAM usage
    ram_usage = (games[game_name][PLAYERS_COUNT] / constants.MAX_PLAYES_COUNT) * games[game_name][RAM_SIZE]
    if ram_usage > games[game_name][RAM_SIZE]:
        ram_usage = games[game_name][RAM_SIZE]

    temperature = (games[game_name][PLAYERS_COUNT] / constants.MAX_PLAYES_COUNT) * constants.max_temp
    if temperature > constants.max_temp:
        temperature = constants.max_temp
    temperature += 30
    games[game_name][CPU_USAGE] = cpu_usage
    games[game_name][RAM_USAGE] = ram_usage
    games[game_name][TEMPERATURE] = temperature

    if TOP_SCORE in games[game_name].keys():
        if random.randint(0, 30) == 0:
            games[game_name][TOP_SCORE] = games[game_name][TOP_SCORE] + random.randint(0, 100)
    else:
        games[game_name][TOP_SCORE] = random.randint(0, constants.max_top_score)

    return players_count, cpu_usage, ram_usage, games[game_name][TOP_SCORE], temperature

if __name__ == '__main__':
    port = 5000
    parser = argparse.ArgumentParser(description='API server for GSM project in the lev academic center')
    parser.add_argument('-p','--port', type=int, help='what port to use')
    args = parser.parse_args()
    if args.port is not None:
        port = args.port
    app.run(port=port, host='0.0.0.0')