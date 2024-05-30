USE Seminarsystem


-- INSERTION PROCEDURES --
CREATE PROCEDURE sp_add_user (@firstname VARCHAR(MAX), @lname VARCHAR(MAX), @email VARCHAR(MAX), @contact BIGINT, @utype VARCHAR(MAX), @username VARCHAR(MAX), @pass VARCHAR(MAX))
AS
INSERT into [dbo].[users] VALUES (@firstname, @lname, @email, @contact, @utype)
INSERT into [dbo].[logins] VALUES ((SELECT top 1 user_id from users ORDER BY user_id DESC) , @username, @pass)
GO

CREATE PROCEDURE sp_add_topic (@name VARCHAR(MAX))
AS
INSERT into [dbo].[topics] VALUES (@name)
GO

CREATE PROCEDURE sp_add_speaker (@available bit, @user_id int, @topic_id int)
AS
INSERT into [dbo].[speakers] VALUES (@user_id, @available, @topic_id)
GO

CREATE PROCEDURE sp_add_speaker_topic (@user_id int, @topic_id int)
AS
INSERT into [dbo].[stopics] VALUES (@topic_id, @user_id)
GO

CREATE PROCEDURE sp_add_speaker_requests (@user_id int, @topic_id int, @req_status VARCHAR(MAX))
AS
INSERT into [dbo].[speaker_requests] VALUES (@user_id, @topic_id, @req_status)
GO

CREATE PROCEDURE sp_add_seminar (@sem_name VARCHAR(MAX), @user_id INT, @s_date DATE, @topic_id INT, @active BIT)
AS
INSERT INTO [dbo].[seminars] (sem_name, user_id, s_date, topic_id, active) VALUES (@sem_name, @user_id, @s_date, @topic_id, @active)
GO

CREATE PROCEDURE sp_assign_seminar (@topic_id INT, @user_id INT, @seminar_id INT, @agreed BIT)
AS
INSERT INTO [dbo].[assigned_seminars] (topic_id, user_id, seminar_id, agreed) VALUES (@topic_id, @user_id, @seminar_id, @agreed)
GO

CREATE PROCEDURE sp_add_attendance (@sem_id INT, @user_id INT, @status VARCHAR(MAX))
AS
INSERT INTO [dbo].[attendance] (sem_id, user_id, status) VALUES (@sem_id, @user_id, @status)
GO

CREATE PROCEDURE sp_add_feedback (@user_id INT, @sem_id INT, @feedback_text VARCHAR(MAX))
AS
INSERT INTO [dbo].[FEEDBACK] (user_id, sem_id, feedback_text) VALUES (@user_id, @sem_id, @feedback_text)
GO

CREATE PROCEDURE sp_add_organizer_request (@user_id INT, @topic_id INT, @seminar_name VARCHAR(MAX), @s_date DATE, @req_status VARCHAR(MAX))
AS
INSERT INTO [dbo].[organizer_requests] (user_id, topic_id, seminar_name, s_date, req_status) VALUES (@user_id, @topic_id, @seminar_name, @s_date, @req_status)
GO

CREATE PROCEDURE sp_add_rejected_seminar (@user_id INT, @sem_id INT)
AS
INSERT INTO [dbo].[rejected] (user_id, sem_id) VALUES (@user_id, @sem_id)
GO


-- Delete Procedures --
CREATE PROCEDURE sp_delete_user (@user_id INT)
AS
BEGIN
    DELETE FROM [dbo].[users] WHERE user_id = @user_id;
END
GO

CREATE PROCEDURE sp_delete_topic (@topic_id INT)
AS
BEGIN
    DELETE FROM [dbo].[topics] WHERE topic_id = @topic_id;
END
GO

CREATE PROCEDURE sp_delete_seminar (@seminar_id INT)
AS
BEGIN
    DELETE FROM [dbo].[seminars] WHERE seminar_id = @seminar_id;
END
GO

CREATE PROCEDURE sp_delete_speaker (@user_id INT)
AS
BEGIN
    DELETE FROM [dbo].[speakers] WHERE user_id = @user_id;
END
GO

CREATE PROCEDURE sp_delete_speaker_request (@req_id INT)
AS
BEGIN

    DELETE FROM [dbo].[speaker_requests] WHERE req_id = @req_id;
END
GO

CREATE PROCEDURE sp_delete_organizer_request (@req_id INT)
AS
BEGIN
    DELETE FROM [dbo].[organizer_requests] WHERE req_id = @req_id;
END
GO


-- SELECT PROCEDURES --
CREATE PROCEDURE sp_get_all_users
AS
BEGIN
	SELECT * from users
END
GO

CREATE PROCEDURE sp_get_rejected_records_with_seminar_details
AS
BEGIN
    SELECT r.user_id, r.sem_id, s.sem_name, s.s_date, s.topic_id, s.active
    FROM rejected r
    INNER JOIN seminars s ON r.sem_id = s.seminar_id;
END

