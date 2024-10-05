from flask import Flask, request, jsonify
from apscheduler.schedulers.background import BackgroundScheduler
from apscheduler.jobstores.memory import MemoryJobStore
from apscheduler.executors.pool import ThreadPoolExecutor
from scrape import FacebookScraper
import uuid
import threading
import asyncio
from datetime import datetime, timedelta
import logging
import queue
import database
import queryTest

app = Flask(__name__)

# 配置日誌
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

# 創建一個線程安全的字典來存儲任務狀態和信息
job_info = {}
job_info_lock = threading.Lock()

# 配置 APScheduler
jobstores = {'default': MemoryJobStore()}
executors = {'default': ThreadPoolExecutor(20)}
job_defaults = {'coalesce': False, 'max_instances': 3}

scheduler = BackgroundScheduler(jobstores=jobstores, executors=executors, job_defaults=job_defaults)

# 全局變量，用於存儲定期任務的 job_id
periodic_job_id = None

# 創建一個隊列來管理立即執行的任務
immediate_task_queue = queue.Queue()

def format_time(time):
    if time is None:
        return None
    return time.strftime("%Y-%m-%d %H:%M:%S")

def update_job_info(job_id, status, url, job_type, next_run_time=None):
    with job_info_lock:
        job_info[job_id] = {
            "status": status,
            "url": url,
            "type": job_type,
            "next_run_time": format_time(next_run_time),
            "update_time": format_time(datetime.now())
        }
    logger.info(f"Updated job {job_id} info: {job_info[job_id]}")

    # 如果任務完成或失敗，安排在一定時間後清理
    if status in ["completed", "failed"]:
        scheduler.add_job(clean_up_job, 'date', run_date=datetime.now() + timedelta(hours=24), args=[job_id])

def clean_up_job(job_id):
    with job_info_lock:
        if job_id in job_info:
            del job_info[job_id]
            logger.info(f"Cleaned up completed job {job_id}")

async def scrape_task(job_id, url):
    update_job_info(job_id, "running", url, "immediate")
    try:
        scraper = FacebookScraper('hao104078.2@gmail.com', '!QAZ@WSX3edc', url)
        await scraper.scrape()
        update_job_info(job_id, "completed", url, "immediate")
    except Exception as e:
        logger.error(f"Error in scrape_task: {str(e)}")
        update_job_info(job_id, f"failed: {str(e)}", url, "immediate")

def run_scrape_task(job_id, url):
    asyncio.run(scrape_task(job_id, url))

def process_immediate_tasks():
    while True:
        job_id, url = immediate_task_queue.get()
        logger.info(f"Processing immediate task {job_id}")
        run_scrape_task(job_id, url)
        immediate_task_queue.task_done()

# 啟動處理立即任務的線程
immediate_task_thread = threading.Thread(target=process_immediate_tasks, daemon=True)
immediate_task_thread.start()

def add_scrape_job(url, job_type='one-time', interval=None, next_run_time=None):
    global periodic_job_id
    job_id = str(uuid.uuid4())
    try:
        if job_type == 'periodic':
            if periodic_job_id:
                # 如果已經存在定期任務，先移除它
                scheduler.remove_job(periodic_job_id)
                del job_info[periodic_job_id]
                logger.info(f"Removed existing periodic job {periodic_job_id}")
            
            if next_run_time is None:
                next_run_time = datetime.now() + timedelta(hours=interval)
            scheduler.add_job(run_scrape_task, 'interval', hours=interval, 
                              args=[job_id, url], id=job_id,
                              next_run_time=next_run_time)
            periodic_job_id = job_id
            update_job_info(job_id, "scheduled", url, "periodic", next_run_time)
            logger.info(f"Added new periodic job {job_id} with interval {interval} hours, next run at {format_time(next_run_time)}")
        elif next_run_time:
            scheduler.add_job(run_scrape_task, 'date', run_date=next_run_time,
                              args=[job_id, url], id=job_id)
            update_job_info(job_id, "scheduled", url, "one-time", next_run_time)
            logger.info(f"Added one-time job {job_id} scheduled at {format_time(next_run_time)}")
        else:
            immediate_task_queue.put((job_id, url))
            update_job_info(job_id, "queued", url, "immediate")
            logger.info(f"Added immediate one-time job {job_id} to queue")
        
        return job_id
    except Exception as e:
        logger.error(f"Error in add_scrape_job: {str(e)}")
        raise

def get_recently_completed_job():
    with job_info_lock:
        completed_jobs = [
            (job_id, info) for job_id, info in job_info.items()
            if info['status'] == 'completed'
        ]
        if not completed_jobs:
            return None
        most_recent_job = max(completed_jobs, key=lambda x: datetime.strptime(x[1]['update_time'], "%Y-%m-%d %H:%M:%S"))
        return {
            "job_id": most_recent_job[0],
            "url": most_recent_job[1]['url'],
            "completion_time": most_recent_job[1]['update_time']
        }


@app.route('/scrape', methods=['POST'])
def call_scraper():
    try:
        data = request.json
        personal_url = data.get('url', None)

        # 執行一次性任務
        one_time_job_id = add_scrape_job(personal_url)
        logger.info(f"Added one-time job with id: {one_time_job_id}")

        # 重置定期任務
        new_periodic_job_id = add_scrape_job(personal_url, job_type='periodic', interval=12)

        return jsonify({
            "message": "爬蟲任務已安排執行，定期任務已重置",
            "one_time_job_id": one_time_job_id,
            "new_periodic_job_id": new_periodic_job_id
        }), 202
    except Exception as e:
        logger.error(f"Error in call_scraper: {str(e)}")
        return jsonify({"error": "Internal server error", "details": str(e)}), 500

@app.route('/queryBook', methods=['GET'])
def get_books():
    books = queryTest.query_books()
    return jsonify(books)


@app.route('/job_status/<job_id>', methods=['GET'])
def get_job_status(job_id):
    with job_info_lock:
        info = job_info.get(job_id, {"status": "not_found"})
    return jsonify({"job_id": job_id, **info})

def get_task_sort_key(task):
    status = task['status']
    task_type = task['type']
    next_run_time = task.get('next_run_time')

    # 定義狀態優先級
    status_priority = {
        'running': 0,
        'queued': 1,
        'scheduled': 2,
        'failed': 4,
        'completed': 3
    }

    # 獲取狀態優先級，如果狀態不在預定義列表中，給予最低優先級
    status_value = status_priority.get(status, 5)

    # 解析下次執行時間
    if next_run_time:
        try:
            next_run_datetime = datetime.strptime(next_run_time, "%Y-%m-%d %H:%M:%S")
        except ValueError:
            next_run_datetime = datetime.max
    else:
        next_run_datetime = datetime.max if task_type != 'immediate' else datetime.min

    # 返回排序鍵
    return (status_value, next_run_datetime)

@app.route('/job_status', methods=['GET'])
def get_all_job_status():
    with job_info_lock:
        tasks = [{"job_id": job_id, **info} for job_id, info in job_info.items()]
    
    # 根據狀態和執行時間排序任務
    sorted_tasks = sorted(tasks, key=get_task_sort_key)
    
    return jsonify({"tasks": sorted_tasks})


@app.route('/schedule_scrape', methods=['POST'])
def schedule_scrape():
    data = request.json
    personal_url = data.get('url', None)
    schedule_time_str = data.get('schedule_time', None)
    
    if not schedule_time_str:
        return jsonify({"error": "排程時間不能為空"}), 400

    try:
        schedule_time = datetime.strptime(schedule_time_str, "%Y-%m-%d %H:%M:%S")
        if schedule_time <= datetime.now():
            return jsonify({"error": "排程時間必須是未來的時間"}), 400
    except ValueError:
        return jsonify({
            "error": "無效的時間格式",
            "message": "請使用 'YYYY-MM-DD HH:M  S' 格式，例如 '2023-12-31 23:59:59'"
        }), 400

    job_id = add_scrape_job(personal_url, next_run_time=schedule_time)

    return jsonify({
        "message": "爬蟲任務已排程",
        "job_id": job_id,
        "scheduled_time": format_time(schedule_time)
    }), 202

def start_periodic_scrape(url=None):
    global periodic_job_id
    periodic_job_id = add_scrape_job(url, job_type='periodic', interval=12)
    logger.info(f"Started initial periodic scrape task with id: {periodic_job_id}")

def start_up():
    try:
        scheduler.start()
        start_periodic_scrape()
        # 添加定期清理任務
        scheduler.add_job(clean_up_old_tasks, 'interval', hours=24)
        logger.info("Scheduler started, initial periodic task set, and cleanup task scheduled")
        
        # 測試數據庫連接
        if database.test_db_connection():
            logger.info("Database connection test passed")
        else:
            logger.error("Database connection test failed")
        
        app.run(debug=True)
    except Exception as e:
        logger.error(f"Error in start_up: {str(e)}")


def clean_up_old_tasks():
    current_time = datetime.now()
    with job_info_lock:
        for job_id, info in list(job_info.items()):
            if info['status'] in ["completed", "failed"]:
                update_time = datetime.strptime(info['update_time'], "%Y-%m-%d %H:%M:%S")
                if current_time - update_time > timedelta(days=7):
                    del job_info[job_id]
                    logger.info(f"Cleaned up old job {job_id}")

if __name__ == '__main__':
    start_up()