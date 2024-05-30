--CREATE DATABASE Seminarsystem;

USE [Seminarsystem];

CREATE table users (
user_id INT PRIMARY KEY IDENTITY(1,1),
fname VARCHAR(MAX) NOT NULL,
lname VARCHAR(MAX) NOT NULL,
email VARCHAR(MAX),
contact_no BIGINT,
utype VARCHAR(255),
)

CREATE table logins (
user_id INT,
username VARCHAR(MAX) NOT NULL,
userpassword VARCHAR(MAX) NOT NULL,
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
)

CREATE table speakers (
user_id INT,
savailability VARCHAR(MAX),
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
)

CREATE TABLE topics (
topic_id int PRIMARY KEY,
topic_name VARCHAR(MAX)
)

CREATE table seminars (
seminar_id int primary key,
sem_name VARCHAR(MAX) NOT NULL,
user_id INT,
s_date DATE NOT NULL,
topic_id int,
sem_status VARCHAR(MAX),
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
FOREIGN KEY (topic_id) REFERENCES topics(topic_id) ON DELETE NO ACTION
)

CREATE table assigned_seminars (
topic_id int,
user_id INT,
seminar_id int,
agreed VARCHAR(MAX)
FOREIGN KEY (topic_id) REFERENCES topics(topic_id) ON DELETE NO ACTION,
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE NO ACTION,
FOREIGN KEY (seminar_id) REFERENCES seminars(seminar_id) ON DELETE CASCADE 
)

CREATE table rejected_seminars (
user_id INT,
sem_id int,
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE NO ACTION,
FOREIGN KEY (sem_id) REFERENCES seminars(seminar_id) ON DELETE CASCADE,
)

CREATE table attendance (
sem_id int,
user_id INT,
status VARCHAR(MAX) NOT NULL,
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE NO ACTION,
FOREIGN KEY (sem_id) REFERENCES seminars(seminar_id) ON DELETE CASCADE
)

CREATE table FEEDBACK (
user_id INT,
sem_id int,
feedback_text VARCHAR(MAX),
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE NO ACTION,
FOREIGN KEY (sem_id) REFERENCES seminars(seminar_id) ON DELETE CASCADE
)

Create table speaker_requests (
req_id int PRIMARY KEY IDENTITY (1,1),
user_id INT,
req_status VARCHAR(MAX) NOT NULL,
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
)

Create table organizer_requests (
req_id int PRIMARY KEY IDENTITY (1,1),
user_id INT,
topic_id int,
seminar_name VARCHAR(MAX),
s_date DATE NOT NULL,
req_status VARCHAR(MAX) NOT NULL,
FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
FOREIGN KEY (topic_id) REFERENCES topics(topic_id) ON DELETE NO ACTION
)