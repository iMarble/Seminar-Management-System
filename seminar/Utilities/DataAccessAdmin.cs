using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace seminar.Utilities
{
    public class DataAccessAdmin
    {
        private readonly string connectionString;

        public DataAccessAdmin(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<User> GetAllUsers(bool speaker = false, bool attendee = false, string keyword = null)
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"Select * FROM users u WHERE";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += "(u.contact_no LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR email LIKE @Keyword OR Utype LIKE @Keyword) AND ";
                }

                if (attendee)
                {
                    query += " u.utype = 'Attendee'";
                }

                else
                {
                    query += " u.utype = 'Speaker'";
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FirstName = reader["fname"].ToString() ?? "",
                        LastName = reader["lname"].ToString() ?? "",
                        Email = reader["email"].ToString() ?? "",
                        ContactNo = Convert.ToInt64(reader["contact_no"]),
                        UType = reader["Utype"].ToString() ?? ""
                    };
                    users.Add(user);
                }
            }

            return users;
        }


        // Methods Related to Seminars For admins

        // View method is in general access

        public bool AddSeminar(string seminarName, DateTime date, string topicName, int userId, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO seminars (sem_name, s_date, topic_id, user_id, sem_status) VALUES (@SeminarName, @Date, (SELECT topic_id FROM topics WHERE topic_name = @TopicName), @UserId, @status)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeminarName", seminarName);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@TopicName", topicName);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@status", status);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                // Check if any rows were affected by the insert
                if (rowsAffected > 0)
                {
                    return true; // Insert successful
                }
                else
                {
                    return false; // Insert failed
                }
            }
        }

        public bool EditSeminar(int seminarId, string newSeminarName, DateTime newDate, string newTopic, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                UPDATE seminars 
                SET sem_name = @NewSeminarName, 
                    s_date = @NewDate, 
                    sem_status = @Status,
                    topic_id = (SELECT topic_id FROM topics WHERE topic_name = @NewTopic)
                WHERE seminar_id = @SeminarId
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewSeminarName", newSeminarName);
                command.Parameters.AddWithValue("@NewDate", newDate);
                command.Parameters.AddWithValue("@NewTopic", newTopic);
                command.Parameters.AddWithValue("@SeminarId", seminarId);
                command.Parameters.AddWithValue("@Status", status);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                // Check if any rows were affected by the update
                if (rowsAffected > 0)
                {
                    return true; // Edit successful
                }
                else
                {
                    return false; // Edit failed (seminar ID not found)
                }
            }
        }

        public bool DeleteSeminar(int seminarId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM seminars WHERE seminar_id = @SeminarId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeminarId", seminarId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                // Check if any rows were affected by the delete
                if (rowsAffected > 0)
                {
                    return true; // Deletion successful
                }
                else
                {
                    return false; // Deletion failed (seminar ID not found)
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM users WHERE user_id = @UserId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                // Check if any rows were affected by the delete
                if (rowsAffected > 0)
                {
                    return true; // Deletion successful
                }
                else
                {
                    return false; // Deletion failed (seminar ID not found)
                }
            }
        }

        public List<userAttendance> GetAttendance(int userId = 0, int speakerId = 0, string keyword = null)
        {
            List<userAttendance> userAttendanceList = new List<userAttendance>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT a.*, u.*, s.*
                FROM attendance a
                INNER JOIN users u ON a.user_id = u.user_id
                INNER JOIN seminars s on a.sem_id = s.seminar_id
                ";

                if (keyword != null && userId != 0)
                {
                    query += "WHERE (u.contact_no LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR email LIKE @Keyword OR Utype LIKE @Keyword OR a.status LIKE @Keyword OR s.sem_name LIKE @Keyword) AND u.user_id = @UserId";
                }


                if (userId != 0 && keyword == null)
                {
                    query += "WHERE u.user_id = @UserId";
                }

                if (keyword != null && speakerId != 0)
                {
                    query += "WHERE (u.contact_no LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR email LIKE @Keyword OR Utype LIKE @Keyword OR a.status LIKE @Keyword OR s.sem_name LIKE @Keyword) AND s.user_id = @SpeakerId";
                }

                if (speakerId != 0 && keyword == null)
                {
                    query += "WHERE s.user_id = @SpeakerId";
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (userId != 0)
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                }

                if (speakerId != 0)
                {
                    command.Parameters.AddWithValue("@SpeakerId", speakerId);
                }

                if (keyword != null)
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Create Attendance object
                    Attendance attendance = new Attendance
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SeminarId = Convert.ToInt32(reader["sem_id"]),
                        Status = reader["status"].ToString() ?? ""
                        // Populate other properties as needed
                    };

                    // Create User object and populate its properties
                    User user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FirstName = reader["fname"].ToString() ?? "",
                        LastName = reader["lname"].ToString() ?? "",
                        ContactNo = Convert.ToInt64(reader["contact_no"]),
                        Email = reader["email"].ToString() ?? string.Empty,

                        // Populate other properties as needed
                    };

                    Seminar seminar = new Seminar
                    {
                        SemName = reader["sem_name"].ToString(),
                    };
                    // Create userAttendance object and add to list
                    userAttendance userAttendance = new userAttendance(attendance, user, seminar);
                    userAttendanceList.Add(userAttendance);
                }
            }

            return userAttendanceList;
        }

        public List<Topic> GetAllTopics()
        {
            List<Topic> topics = new List<Topic>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT topic_id, topic_name FROM topics";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Topic topic = new Topic();
                        topic.TopicId = Convert.ToInt32(reader["topic_id"]);
                        topic.TopicName = reader["topic_name"].ToString();
                        topics.Add(topic);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving topics: " + ex.Message);
                }
            }

            return topics;
        }

        public List<userAttendance> GetAttendance(string keyword, int seminarId = 0)
        {
            List<userAttendance> userAttendanceList = new List<userAttendance>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT a.*, u.*, s.*
                FROM attendance a
                INNER JOIN users u ON a.user_id = u.user_id
                INNER JOIN seminars s on a.sem_id = s.seminar_id
                WHERE
                ";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += "(u.contact_no LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR email LIKE @Keyword OR Utype LIKE @Keyword OR a.status LIKE @Keyword OR s.sem_name LIKE @Keyword)";
                }

                if (seminarId != 0)
                {
                    query += !string.IsNullOrWhiteSpace(keyword) ? "AND" : " ";
                    query += " s.seminar_id = @SeminarId";
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                if (seminarId != 0)
                {
                    command.Parameters.AddWithValue("@SeminarId", seminarId);
                }

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Create Attendance object
                    Attendance attendance = new Attendance
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SeminarId = Convert.ToInt32(reader["sem_id"]),
                        Status = reader["status"].ToString() ?? ""
                        // Populate other properties as needed
                    };

                    // Create User object and populate its properties
                    User user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FirstName = reader["fname"].ToString() ?? "",
                        LastName = reader["lname"].ToString() ?? "",
                        ContactNo = Convert.ToInt64(reader["contact_no"]),
                        Email = reader["email"].ToString() ?? string.Empty,

                        // Populate other properties as needed
                    };

                    Seminar seminar = new Seminar
                    {
                        SemName = reader["sem_name"].ToString(),
                    };
                    // Create userAttendance object and add to list
                    userAttendance userAttendance = new userAttendance(attendance, user, seminar);
                    userAttendanceList.Add(userAttendance);
                }
            }

            return userAttendanceList;
        }

        public List<userAttendance> GetSeminarAttendance(int seminarId, string keyword = null)
        {
            List<userAttendance> userAttendanceList = new List<userAttendance>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT u.user_id, u.fname, u.lname, u.email, u.contact_no, u.utype, a.status, a.sem_id, s.sem_name
                FROM users u
                INNER JOIN attendance a ON u.user_id = a.user_id
                INNER JOIN seminars s on a.sem_id = s.seminar_id
                WHERE
                ";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += "( u.contact_no LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR email LIKE @Keyword OR Utype LIKE @Keyword OR a.status LIKE @Keyword OR s.sem_name LIKE @Keyword) AND ";
                }

                query += "a.sem_id = @SeminarId";

                SqlCommand command = new SqlCommand(query, connection);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                command.Parameters.AddWithValue("@SeminarId", seminarId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Create Attendance object
                    Attendance attendance = new Attendance
                    {
                        SeminarId = Convert.ToInt32(reader["sem_id"]),
                        Status = reader["status"].ToString() ?? ""
                    };

                    // Create User object and populate its properties
                    User user = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FirstName = reader["fname"].ToString() ?? "",
                        LastName = reader["lname"].ToString() ?? "",
                        Email = reader["email"].ToString() ?? "",
                        ContactNo = Convert.ToInt64(reader["contact_no"]),
                    };

                    Seminar seminar = new Seminar
                    {
                        SemName = reader["sem_name"].ToString(),
                    };
                    // Create userAttendance object and add to list
                    userAttendance userAttendance = new userAttendance(attendance, user, seminar);
                    userAttendanceList.Add(userAttendance);
                }
            }

            return userAttendanceList;
        }

        public List<User> GetAllSpeakers()
        {
            List<User> speakers = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT user_id, fname, lname, email, contact_no, utype
            FROM users
            WHERE utype = 'speaker'
        ";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        User speaker = new User
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            FirstName = reader["fname"].ToString(),
                            LastName = reader["lname"].ToString(),
                            Email = reader["email"].ToString(),
                            ContactNo = Convert.ToInt64(reader["contact_no"]),
                            UType = reader["utype"].ToString()
                        };

                        speakers.Add(speaker);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving speakers: " + ex.Message);
                }
            }

            return speakers;
        }

        public bool ApproveSpeakerRequest(int userId, int reqId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                UPDATE users 
                SET utype = 'Speaker'
                WHERE user_id = @UserId
            ";

                string ReqQuery = @"
                UPDATE speaker_requests
                SET req_status = 'Approved'
                WHERE req_id = @ReqID
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                SqlCommand reqcommand = new SqlCommand(ReqQuery, connection);
                reqcommand.Parameters.AddWithValue("@ReqId", reqId);


                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                reqcommand.Transaction = transaction;

                // Check if any rows were affected by the update
                try
                {
                    // Execute the commands
                    command.ExecuteNonQuery();
                    reqcommand.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return true; // Registration successful
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                    return false; // Registration failed
                }
            }
        }


        public bool RejectSpeakerRequest(int reqId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string ReqQuery = @"
                UPDATE speaker_requests
                SET req_status = 'Rejected'
                WHERE req_id = @ReqID
            ";

                SqlCommand reqcommand = new SqlCommand(ReqQuery, connection);
                reqcommand.Parameters.AddWithValue("@ReqId", reqId);


                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                reqcommand.Transaction = transaction;

                // Check if any rows were affected by the update
                try
                {
                    // Execute the commands
                    reqcommand.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return true; // Registration successful
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                    return false; // Registration failed
                }
            }
        }

        public bool MarkPresent(int userId, int semId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string ReqQuery = @"
                UPDATE attendance
                SET status = 'Present'
                WHERE user_id = @UserID
                AND
                sem_id = @SeminarId
            ";


                SqlCommand command = new SqlCommand(ReqQuery, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@SeminarId", semId);


                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                // Check if any rows were affected by the update
                try
                {
                    // Execute the commands
                    command.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return true; // Registration successful
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                    return false; // Registration failed
                }
            }
        }

        public bool MarkAbsent(int userId, int semId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string ReqQuery = @"
                UPDATE attendance
                SET status = 'Absent'
                WHERE user_id = @UserID
                AND
                sem_id = @SeminarId
            ";


                SqlCommand command = new SqlCommand(ReqQuery, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@SeminarId", semId);


                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                // Check if any rows were affected by the update
                try
                {
                    // Execute the commands
                    command.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return true; // Registration successful
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                    return false; // Registration failed
                }
            }
        }

        public bool AddRequest(int userId) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string ReqQuery = @"
                Insert Into speaker_requests
                VALUES (@UserId, 'pending')
            ";


                SqlCommand command = new SqlCommand(ReqQuery, connection);
                command.Parameters.AddWithValue("@UserId", userId);


                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                // Check if any rows were affected by the update
                try
                {
                    // Execute the commands
                    command.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return true; // Registration successful
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                    return false; // Registration failed
                }
            }
        }
    }

}
