from database import engine, SessionLocal
from models import Models, Base
import logging
import csv
from main import get_recently_completed_job

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

def get_db_session():
    db = SessionLocal()
    try:
        return db
    finally:
        db.close()

# 測試
def query_books():
    with get_db_session() as db:
        books = db.query(Models.Book).all()
        return [
            {
                'BookId': book.BookId,
                'BookName': book.BookName,
                'Author': book.Author,
                'PublicDate': book.PublicDate.isoformat() if book.PublicDate else None
            } for book in books
        ]

# def insert_bullying_data(postfile, likefile):
#     with open(postfile, 'r', newline='', encoding='utf-8') as file:
#         with open(likefile, 'r', newline='', encoding='utf-8') as like:
#                 csv_reader = csv.DictReader(file)
#                 headers = csv_reader.fieldnames
#                 for row in csv_reader:
#                     if row[headers[-1]] is 1:
#                         if get_bullyinger(row):
#                             insert_bullyinger(row)
#                         insert_post(row)
        

# def insert_bullyinger(row):
#     db = get_db_session()
#     time = get_recently_completed_job()
#     new_bullyinger = Models.Bullyinger(
#         BullyingerId = row['id'],
#         Bullyinger = row['name'],
#         FBurl = row['url'],
#         State = 0,
#         FirstDate = time['completion_time'],
#         Account = query_account(row)
#     )
#     db.add(new_bullyinger)
#     db.commit()
#     db.refresh(new_bullyinger)
#     logger.info(f"Created new bullyinger: {new_bullyinger['Bullyinger']}")

# def insert_post(data,key):
#     new_post = Models.BullyingerPost(
#         BullyingerId = data['id'],
#         PostInfo = data['content'],
#         PostTime = data['time'],
#         Posturl =  data['post_url'],
        
#     )

# def get_bullyinger(row):
#     with get_db_session() as db:
#         try:
#             bullyinger = db.query(Models.Bullyinger).filter(Models.Bullyinger.BullyingerId == row['id'])
#             if bullyinger:
#                 return True
#         except Exception as e:
#             logger.error(f"Error in add_scrape_job: {str(e)}")
#             return False

# def query_account(row):
#     with get_db_session() as db:
#         Members = db.query(Models.Member).all()
#         for Member in Members:
#             if Member['FBurl'] == row['url']:
#                 return Member['Account']
#             else:
#                 return None
            
