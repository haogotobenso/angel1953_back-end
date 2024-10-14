# coding: utf-8
from sqlalchemy import Column, DateTime, ForeignKey, Integer, NCHAR, String, Unicode, text
from sqlalchemy.dialects.mssql import IMAGE, TINYINT
from sqlalchemy.orm import relationship
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()
metadata = Base.metadata

class Models:
    class Book(Base):
        __tablename__ = 'Book'

        BookId = Column(Integer, primary_key=True, autoincrement=True)
        BookName = Column(Unicode(100), nullable=False)
        Author = Column(Unicode(50))
        PublicDate = Column(DateTime)


    class Clas(Base):
        __tablename__ = 'Class'

        ClassId = Column(Integer, primary_key=True, autoincrement=True)
        Class = Column(Unicode(100), nullable=False)


    class CrawlerLink(Base):
        __tablename__ = 'CrawlerLink'

        LinkId = Column(Integer, primary_key=True, autoincrement=True)
        LinkName = Column(Unicode(200), nullable=False)
        FBLink = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'), nullable=False)


    class ExternalLink(Base):
        __tablename__ = 'ExternalLinks'

        LinkId = Column(Integer, primary_key=True, autoincrement=True)
        Title = Column(Unicode(200))
        Link = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'))
        LinkTime = Column(DateTime)


    class Question(Base):
        __tablename__ = 'Question'

        QuestionId = Column(Integer, primary_key=True, autoincrement=True)
        Question = Column(Unicode)
        Answer = Column(Unicode)
        Option1 = Column(Unicode)
        Option2 = Column(Unicode)
        Option3 = Column(Unicode)


    class School(Base):
        __tablename__ = 'School'

        SchoolId = Column(Integer, primary_key=True)
        School = Column(Unicode(50), nullable=False)


    class VideoLink(Base):
        __tablename__ = 'VideoLink'

        VideoId = Column(Integer, primary_key=True, autoincrement=True)
        VideoName = Column(Unicode(200), nullable=False)
        VideoImg = Column(IMAGE)
        VideoLink = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'), nullable=False)
        LinkClick = Column(Integer, server_default=text("((0))"))


    class Member(Base):
        __tablename__ = 'Member'

        Account = Column(String(20, 'Chinese_Taiwan_Stroke_CI_AS'), primary_key=True)
        Password = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'), nullable=False)
        Name = Column(Unicode(20), nullable=False)
        Email = Column(String(200, 'Chinese_Taiwan_Stroke_CI_AS'), nullable=False)
        FBurl = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'))
        AuthCode = Column(NCHAR(10))
        IsTeacher = Column(TINYINT, nullable=False)
        SchoolId = Column(ForeignKey('School.SchoolId'))
        TeacherImg = Column(IMAGE)
        ClassId = Column(ForeignKey('Class.ClassId'))
        StudentId = Column(String(50, 'Chinese_Taiwan_Stroke_CI_AS'))

        Clas = relationship('Clas')
        School = relationship('School')


    class Bullyinger(Base):
        __tablename__ = 'Bullyinger'

        BullyingerId = Column(String(100, 'Chinese_Taiwan_Stroke_CI_AS'), primary_key=True)
        Bullyinger = Column(Unicode(100), nullable=False)
        FBurl = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'), nullable=False)
        State = Column(TINYINT)
        FirstDate = Column(DateTime)
        BullyingerPoint = Column(Integer, nullable=False, server_default=text("((0))"))
        Account = Column(ForeignKey('Member.Account'))

        Member = relationship('Member')


    class Recovery(Base):
        __tablename__ = 'Recovery'

        RecoveryId = Column(Integer, primary_key=True, autoincrement=True)
        Time = Column(DateTime, nullable=False)
        Account = Column(ForeignKey('Member.Account'))
        Correct = Column(Integer, nullable=False)

        Member = relationship('Member')


    class BullyingerPost(Base):
        __tablename__ = 'BullyingerPost'

        BPId = Column(Integer, primary_key=True, autoincrement=True)
        BullyingerId = Column(ForeignKey('Bullyinger.BullyingerId'))
        PostInfo = Column(Unicode, nullable=False)
        PostTime = Column(DateTime)
        Posturl = Column(String(collation='Chinese_Taiwan_Stroke_CI_AS'))
        KeyWord = Column(Unicode(100))

        Bullyinger = relationship('Bullyinger')


    class RecoveryRecord(Base):
        __tablename__ = 'RecoveryRecord'

        RecordId = Column(Integer, primary_key=True, autoincrement=True)
        RecoveryId = Column(ForeignKey('Recovery.RecoveryId'))
        QuestionId = Column(ForeignKey('Question.QuestionId'))
        UserAnswer = Column(Unicode, nullable=False)

        Question = relationship('Question')
        Recovery = relationship('Recovery')
