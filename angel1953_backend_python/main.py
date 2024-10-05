from flask import Flask, request, jsonify
from scrape import Scraper

app = Flask(__name__)

def start_up():
    app.run(debug=True)

@app.route('/scrape', methods=['POST'])
def call_scraper():
    data = request.json
    personal_url = data.get('url', None)
    Scraper(url=personal_url)
    result = {"result": "成功處理數據"}
    return jsonify(result)


if __name__ == '__main__':
    app.run(debug=True)

