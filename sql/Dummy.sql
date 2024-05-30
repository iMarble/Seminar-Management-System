-- Users
INSERT INTO users (fname, lname, email, contact_no, utype)
VALUES ('John', 'Doe', 'john.doe@example.com', 1234567890, 'participant'),
       ('Jane', 'Smith', 'jane.smith@example.com', 9876543210, 'speaker'),
       ('Michael', 'Johnson', 'michael.johnson@example.com', 4567891230, 'participant');

-- Logins
INSERT INTO logins (user_id, username, userpassword)
VALUES (1, 'johndoe', 'password123'),
       (2, 'janesmith', 'pass456'),
       (3, 'michaelj', 'mike789');

-- Speakers
INSERT INTO speakers (user_id, savailability)
VALUES (2, 'available'),
       (3, 'unavailable');

-- Topics
INSERT INTO topics (topic_id, topic_name)
VALUES (1, 'Artificial Intelligence'),
       (2, 'Blockchain'),
       (3, 'Data Science');

-- Seminars
INSERT INTO seminars (seminar_id, sem_name, user_id, s_date, topic_id, sem_status)
VALUES (1, 'Introduction to AI', 2, '2024-05-15', 1, 'scheduled'),
       (2, 'Blockchain Basics', 3, '2024-06-10', 2, 'scheduled');

-- Assigned Seminars
INSERT INTO assigned_seminars (topic_id, user_id, seminar_id, agreed)
VALUES (1, 1, 1, 'yes'),
       (2, 1, 2, 'yes');

-- Rejected Seminars
INSERT INTO rejected_seminars (user_id, sem_id)
VALUES (3, 2);

-- Attendance
INSERT INTO attendance (sem_id, user_id, status)
VALUES (1, 1, 'present'),
       (1, 2, 'present');

-- Feedback
INSERT INTO FEEDBACK (user_id, sem_id, feedback_text)
VALUES (1, 1, 'Great seminar! Very informative.');

-- Speaker Requests
INSERT INTO speaker_requests (user_id, req_status)
VALUES (3, 'pending');

-- Organizer Requests
INSERT INTO organizer_requests (user_id, topic_id, seminar_name, s_date, req_status)
VALUES (1, 3, 'Advanced Data Science Workshop', '2024-07-20', 'pending');

-------------------------------

-- Users
INSERT INTO users (fname, lname, email, contact_no, utype)
VALUES ('Alice', 'Johnson', 'alice.johnson@example.com', 5551234567, 'participant'),
       ('Bob', 'Williams', 'bob.williams@example.com', 5559876543, 'speaker'),
       ('Emily', 'Brown', 'emily.brown@example.com', 5554567890, 'participant');

-- Logins
INSERT INTO logins (user_id, username, userpassword)
VALUES (4, 'alicej', 'pass123'),
       (5, 'bobw', 'password456'),
       (6, 'emilyb', 'pass789');

-- Speakers
INSERT INTO speakers (user_id, savailability)
VALUES (5, 'unavailable'),
       (6, 'available');

-- Topics
INSERT INTO topics (topic_id, topic_name)
VALUES (4, 'Cybersecurity'),
       (5, 'Machine Learning'),
       (6, 'Cloud Computing');

-- Seminars
INSERT INTO seminars (seminar_id, sem_name, user_id, s_date, topic_id, sem_status)
VALUES (3, 'Cybersecurity Basics', 5, '2024-07-01', 4, 'scheduled'),
       (4, 'Introduction to Machine Learning', 6, '2024-08-10', 5, 'scheduled');

-- Assigned Seminars
INSERT INTO assigned_seminars (topic_id, user_id, seminar_id, agreed)
VALUES (3, 4, 3, 'yes'),
       (4, 4, 4, 'yes');

-- Rejected Seminars
INSERT INTO rejected_seminars (user_id, sem_id)
VALUES (6, 4);

-- Attendance
INSERT INTO attendance (sem_id, user_id, status)
VALUES (3, 4, 'present'),
       (3, 5, 'present');

-- Feedback
INSERT INTO FEEDBACK (user_id, sem_id, feedback_text)
VALUES (4, 3, 'Excellent seminar! Covered all the basics.');

-- Speaker Requests
INSERT INTO speaker_requests (user_id, req_status)
VALUES (6, 'pending');

-- Organizer Requests
INSERT INTO organizer_requests (user_id, topic_id, seminar_name, s_date, req_status)
VALUES (4, 6, 'Cloud Computing Workshop', '2024-09-15', 'pending');
