using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace seminar.Utilities
{
    internal class SpeakerDataAccess
    {
        private readonly string connectionString;

        public SpeakerDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }


        // Browse Only Assigned Seminars
        public List<AssignedSeminarDetails> GetAssignedSeminarsForUser(int userId)
        {
            List<AssignedSeminarDetails> assignedSeminars = new List<AssignedSeminarDetails>();

            // Retrieve assigned seminars for the user from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT s.*, a.*
            FROM seminars s
            INNER JOIN assigned_seminars a ON s.seminar_id = a.seminar_id
            WHERE a.user_id = @UserId
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                try
                {
                    // Execute the query
                    SqlDataReader reader = command.ExecuteReader();

                    // Read the assigned seminar details and add them to the list
                    while (reader.Read())
                    {
                        Seminar seminar = new Seminar
                        {
                            SeminarId = Convert.ToInt32(reader["seminar_id"]),
                            SemName = reader["sem_name"].ToString() ?? ""
                            // Populate other seminar properties as needed
                        };

                        AssignedSeminar assignedSeminar = new AssignedSeminar
                        {
                            TopicId = Convert.ToInt32(reader["topic_id"]),
                            UserId = Convert.ToInt32(reader["user_id"]),
                            SeminarId = Convert.ToInt32(reader["seminar_id"]),
                            Agreed = reader["agreed"].ToString(),
                            // Populate other assigned seminar properties as needed
                        };

                        AssignedSeminarDetails seminarDetails = new AssignedSeminarDetails(seminar, assignedSeminar);
                        assignedSeminars.Add(seminarDetails);
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving assigned seminars: " + ex.Message);
                }
            }

            return assignedSeminars;
        }

        public bool EditOwnSeminar(int seminarId, string newSeminarName, DateTime newDate, string newTopic)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                UPDATE seminars 
                SET sem_name = @NewSeminarName, 
                    s_date = @NewDate, 
                    topic_id = (SELECT topic_id FROM topics WHERE topic_name = @NewTopic)
                WHERE seminar_id = @SeminarId
                AND user_id = @SpeakerUserId
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewSeminarName", newSeminarName);
                command.Parameters.AddWithValue("@NewDate", newDate);
                command.Parameters.AddWithValue("@NewTopic", newTopic);
                command.Parameters.AddWithValue("@SeminarId", seminarId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                // Check if any rows were affected by the update
                if (rowsAffected > 0)
                {
                    return true; // Edit successful
                }
                else
                {
                    return false; // Edit failed (seminar ID not found or no changes made)
                }
            }
        }

        public List<User> GetAttendeesForOwnSingleSeminar(int seminarId, int speakerUserId)
        {
            List<User> attendees = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"
                    SELECT u.user_id, u.fname, u.lname, u.email, u.contact_no, u.utype
                    FROM users u
                    INNER JOIN attendance a ON u.user_id = a.user_id
                    INNER JOIN seminars s ON a.sem_id = s.seminar_id
                    WHERE s.user_id = @SpeakerUserId AND a.sem_id = @SeminarId
                ", connection);

                command.Parameters.AddWithValue("@SpeakerUserId", speakerUserId);
                command.Parameters.AddWithValue("@SeminarId", seminarId);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User attendee = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FirstName = reader["fname"].ToString() ?? "",
                        LastName = reader["lname"].ToString() ?? "",
                        Email = reader["email"].ToString() ?? "",
                        ContactNo = Convert.ToInt64(reader["contact_no"]),
                        UType = reader["utype"].ToString() ?? ""
                    };
                    attendees.Add(attendee);
                }
            }

            return attendees;
        }

        public List<userAttendance> GetAllOwnSeminarAttendees(int speakerId, string keyword = null)
        {
            List<userAttendance> speakerAttendees = new List<userAttendance>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT a.user_id, u.fname, u.lname, u.email, u.contact_no, u.utype, a.sem_id, a.status, s.sem_name
                FROM attendance a
                INNER JOIN users u ON a.user_id = u.user_id
                INNER JOIN seminars s ON a.sem_id = s.seminar_id
                WHERE 
                ";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += "( u.contact_no LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR u.email LIKE @Keyword OR u.utype LIKE @Keyword OR a.status LIKE @Keyword OR s.sem_name LIKE @Keyword) AND ";
                }

                query += @"a.sem_id IN (
                    SELECT s.seminar_id
                    FROM seminars s
                    WHERE s.user_id = @SpeakerId
                )";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpeakerId", speakerId);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Create User object for each attendee and populate its properties
                    User attendee = new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        FirstName = reader["fname"].ToString() ?? "",
                        LastName = reader["lname"].ToString() ?? "",
                        Email = reader["email"].ToString() ?? "",
                        ContactNo = Convert.ToInt64(reader["contact_no"]),
                        UType = reader["utype"].ToString() ?? ""
                    };

                    // Create Attendance object and populate its properties
                    Attendance attendance = new Attendance
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SeminarId = Convert.ToInt32(reader["sem_id"]),
                        Status = reader["status"].ToString() ?? ""
                    };

                    Seminar seminar = new Seminar
                    {
                        SemName = reader["sem_name"].ToString(),
                    };
                    // Create userAttendance object and add to list
                    userAttendance userAttendance = new userAttendance(attendance, attendee, seminar);
                    speakerAttendees.Add(userAttendance);
                }
            }

            return speakerAttendees;
        }


    }
}
