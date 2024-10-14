import mysql.connector
from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
import logging
from models import Models, Base

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

HOST = '163.17.135.231'
DATABASE = 'angel1953'
USERNAME = '1411031026'
PASSWORD = '123456'

# MySQL connection string
connection_string = f'mysql+mysqlconnector://{USERNAME}:{PASSWORD}@{HOST}/{DATABASE}'

engine = create_engine(connection_string)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

def init_db():
    Base.metadata.create_all(bind=engine)
    logger.info("Database initialized")

def test_db_connection():
    try:
        with engine.connect() as connection:
            logger.info("Database connection successful")
        return True
    except Exception as e:
        logger.error(f"Database connection failed: {str(e)}")
        return False

if __name__ == "__main__":
    test_db_connection()
    # 只在需要時取消下面這行的註釋
    # init_db()