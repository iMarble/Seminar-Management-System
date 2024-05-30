using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace seminar.Utilities
{
    internal class GeneralAccess
    {
        private readonly string connectionString;

        public GeneralAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Method to authenticate user login and retrieve user type and ID
        public (bool isAuthenticated, string userType, int userId) AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;
            string userType = "";
            int userId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT u.utype, l.user_id
                FROM logins l
                INNER JOIN users u ON l.user_id = u.user_id
                WHERE l.username = @Username AND l.userpassword = @Password
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isAuthenticated = true;
                    userType = reader["utype"].ToString() ?? "";
                    userId = Convert.ToInt32(reader["user_id"]);
                }
            }

            return (isAuthenticated, userType, userId);
        }

        // Method to Register new user
        public bool RegisterNewUser(string firstName, string lastName, string email, long contactNo, string userType, string username, string password)
        {
            // Check if the email or username already exists in the database
            if (IsEmailOrUsernameExists(email, username))
            {
                // Email or username already exists, return false to indicate registration failure
                return false;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Begin transaction
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert new user record into the database
                    string userInsertQuery = @"
            INSERT INTO users (fname, lname, email, contact_no, utype)
            VALUES (@FirstName, @LastName, @Email, @ContactNo, @UserType);
            SELECT SCOPE_IDENTITY();
            ";

                    SqlCommand userInsertCommand = new SqlCommand(userInsertQuery, connection, transaction);
                    userInsertCommand.Parameters.AddWithValue("@FirstName", firstName);
                    userInsertCommand.Parameters.AddWithValue("@LastName", lastName);
                    userInsertCommand.Parameters.AddWithValue("@Email", email);
                    userInsertCommand.Parameters.AddWithValue("@ContactNo", contactNo);
                    userInsertCommand.Parameters.AddWithValue("@UserType", userType);

                    int userId = Convert.ToInt32(userInsertCommand.ExecuteScalar()); // ExecuteScalar used to get the inserted user's ID

                    // Insert login record into the database
                    string loginInsertQuery = @"
            INSERT INTO logins (user_id, username, userpassword)
            VALUES (@UserId, @Username, @Password);
            ";

                    SqlCommand loginInsertCommand = new SqlCommand(loginInsertQuery, connection, transaction);
                    loginInsertCommand.Parameters.AddWithValue("@UserId", userId);
                    loginInsertCommand.Parameters.AddWithValue("@Username", username);
                    loginInsertCommand.Parameters.AddWithValue("@Password", password);

                    // Execute the commands
                    userInsertCommand.ExecuteNonQuery();
                    loginInsertCommand.ExecuteNonQuery();

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


        //Method to update profile
        public bool EditUserProfile(int userId, string firstName, string lastName, string email, long contactNo)
        {
            // Check if the provided user ID exists in the database
            if (!IsUserIdExists(userId))
            {
                // User ID does not exist, return false to indicate failure
                return false;
            }

            // Update user profile in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE users 
                    SET fname = @FirstName, 
                        lname = @LastName, 
                        email = @Email, 
                        contact_no = @ContactNo 
                    WHERE user_id = @UserId
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@ContactNo", contactNo);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                // Begin transaction
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;


                try
                {
                    // Execute the commands
                    command.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return true; // Profile updated successfully
                }
                catch (Exception ex)
                {
                    // Rollback transaction if any error occurs
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                    return false; // Update failed
                }
            }
        }

        public bool EditUserPassword(int userId, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE logins 
                    SET userpassword = @NewPassword
                    WHERE user_id = @UserId
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@NewPassword", newPassword);

                connection.Open();

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        // Method to check if useridexists
        public bool IsUserIdExists(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM users WHERE user_id = @UserId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

        // Method to check if email or username already exists in the database
        private bool IsEmailOrUsernameExists(string email, string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM users u JOIN logins l ON u.user_id = l.user_id WHERE u.email = @Email OR l.username = @Username
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        // Method to get all seminar details, filter by name, speaker name and date
        public List<AllSeminars> GetAllSeminarDetails(string keyword = null, string date = null, int speaker_id = 0, bool attendee = false, bool isAssigned = false)
        {
            List<AllSeminars> seminarDetailsList = new List<AllSeminars>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT s.seminar_id, s.sem_name, s.user_id, s.s_date, s.topic_id, s.sem_status,
                           u.fname AS speaker_fname, u.lname AS speaker_lname,
                           t.topic_id AS topic_id, t.topic_name
                    FROM seminars s
                    INNER JOIN users u ON s.user_id = u.user_id
                    INNER JOIN topics t ON s.topic_id = t.topic_id
                    ";

                // Add keyword filter to the query if provided
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += "WHERE (s.sem_name LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword OR t.topic_name LIKE @Keyword OR s.sem_status LIKE @Keyword) ";
                }

                // Add date filter to the query if provided
                if (!string.IsNullOrWhiteSpace(date))
                {
                    query += string.IsNullOrWhiteSpace(keyword) ? "WHERE " : "AND ";
                    query += "s.s_date = @Date ";
                }

                if (speaker_id != 0)
                {
                    query += string.IsNullOrWhiteSpace(keyword) ? "WHERE " : "AND ";
                    query += "u.user_id = @SpeakerId ";
                }

                if (attendee && speaker_id != 0)
                {
                    query += string.IsNullOrWhiteSpace(keyword) ? "AND " : "WHERE ";
                    query += "s.sem_status = 'Scheduled' ";
                }
                else if (attendee)
                {
                    query += string.IsNullOrWhiteSpace(keyword) ? "WHERE " : "AND ";
                    query += "s.sem_status = 'Scheduled' ";
                }

                if (isAssigned)
                {
                    query += speaker_id != 0 ? "AND " : "WHERE ";
                    query += "s.sem_status = 'Assigned' ";
                }

                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters if filters are provided
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                if (!string.IsNullOrWhiteSpace(date))
                {
                    command.Parameters.AddWithValue("@Date", date);
                }

                if (speaker_id != 0)
                {
                    command.Parameters.AddWithValue("@SpeakerId", speaker_id);

                }

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Seminar seminar = new Seminar
                    {
                        SeminarId = Convert.ToInt32(reader["seminar_id"]),
                        SemName = reader["sem_name"].ToString() ?? "",
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SDate = Convert.ToDateTime(reader["s_date"]),
                        TopicId = Convert.ToInt32(reader["topic_id"]),
                        Status = reader["sem_status"].ToString()
                    };

                    Speaker speaker = new Speaker
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SpeakerName = reader["speaker_fname"].ToString() ?? ""
                    };

                    Topic topic = new Topic
                    {
                        TopicId = Convert.ToInt32(reader["topic_id"]),
                        TopicName = reader["topic_name"].ToString() ?? ""
                    };

                    AllSeminars seminarDetails = new AllSeminars(seminar, speaker, topic);
                    seminarDetailsList.Add(seminarDetails);
                }
            }

            return seminarDetailsList;
        }


        public List<AllSeminars> GetAttendedSeminarDetails(int user_id = 0)
        {
            List<AllSeminars> seminarDetailsList = new List<AllSeminars>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        s.seminar_id,
                        s.sem_name,
                        s.s_date,
                        s.sem_status,
                        u.user_id,
                        t.topic_id,
                        t.topic_name,
                        u.fname AS speaker_fname
                    FROM 
                        seminars s
                    JOIN 
                        attendance a ON s.seminar_id = a.sem_id
                    JOIN 
                        users u ON a.user_id = u.user_id
                    JOIN
                        topics t ON s.topic_id = t.topic_id
                    WHERE 
                        a.status = 'present'
                    AND
                    	u.user_id = @UserId
                    AND 
                        s.seminar_id NOT IN (SELECT sem_id from FEEDBACK WHERE user_id = @UserID);";

                SqlCommand command = new SqlCommand(query, connection);

                if (user_id != 0)
                {
                    command.Parameters.AddWithValue("@UserId", user_id);
                }

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Seminar seminar = new Seminar
                    {
                        SeminarId = Convert.ToInt32(reader["seminar_id"]),
                        SemName = reader["sem_name"].ToString() ?? "",
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SDate = Convert.ToDateTime(reader["s_date"]),
                        TopicId = Convert.ToInt32(reader["topic_id"]),
                        Status = reader["sem_status"].ToString()
                    };

                    Speaker speaker = new Speaker
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        SpeakerName = reader["speaker_fname"].ToString() ?? ""
                    };

                    Topic topic = new Topic
                    {
                        TopicId = Convert.ToInt32(reader["topic_id"]),
                        TopicName = reader["topic_name"].ToString() ?? ""
                    };

                    AllSeminars seminarDetails = new AllSeminars(seminar, speaker, topic);
                    seminarDetailsList.Add(seminarDetails);
                }
            }

            return seminarDetailsList;
        }

        // Method to submit feedback by User
        public List<SeminarFeedback> GetUserFeedbacks(bool isUser = false, int userId = 0, bool filter = false, string keyword = null)
        {
            List<SeminarFeedback> userFeedbacks = new List<SeminarFeedback>();

            // Retrieve user's feedbacks from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "";
                if (!isUser)
                {
                    query = @"
                                SELECT 
                        f.feedback_text, 
                        u.fname, u.lname,
                        s.seminar_id,
                        s.sem_name,
                        s.s_date
                    FROM 
                        FEEDBACK f
                    JOIN 
                        users u ON f.user_id = u.user_id
                    JOIN 
                        seminars s ON f.sem_id = s.seminar_id
                            ";
                    if (filter)
                    {
                        query += " WHERE s.sem_name LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword";
                    }
                }
                else if (isUser)
                {
                    query = @"SELECT 
                        f.feedback_text, 
                        u.fname, u.lname,
                        s.seminar_id,
                        s.sem_name,
                        s.s_date
                    FROM 
                        FEEDBACK f
                    JOIN 
                        users u ON f.user_id = u.user_id
                    JOIN 
                        seminars s ON f.sem_id = s.seminar_id
                    WHERE
                        u.user_id = @UserId";

                    if (filter)
                    {
                        query += " AND (s.sem_name LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword)";
                    }
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (isUser)
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                }

                if (filter)
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                }

                connection.Open();

                try
                {
                    // Execute the query
                    SqlDataReader reader = command.ExecuteReader();

                    // Read the seminar and feedback details and add them to the list
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            FirstName = reader["fname"].ToString(),
                            LastName = reader["lname"].ToString()
                        };
                        Seminar seminar = new Seminar
                        {
                            SeminarId = Convert.ToInt32(reader["seminar_id"]),
                            SemName = reader["sem_name"].ToString() ?? "",
                            SDate = DateTime.Parse(reader["s_date"].ToString())
                            // Populate other seminar properties as needed
                        };

                        Feedback feedback = new Feedback
                        {
                            FeedbackText = reader["feedback_text"].ToString() ?? ""
                        };

                        SeminarFeedback seminarFeedback = new SeminarFeedback(seminar, feedback, user);
                        userFeedbacks.Add(seminarFeedback);
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving user feedbacks: " + ex.Message);
                }
            }

            return userFeedbacks;
        }

        // Method to get a users requests
        public List<UserRequest> GetUserRequests(bool isUser = false, int userId = 0, bool filter = false, string keyword = null)
        {
            List<UserRequest> userRequests = new List<UserRequest>();

            // Retrieve user requests from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "";
                if (!isUser)
                {

                    query = @"SELECT sr.req_id, u.*, sr.req_status
                                    FROM speaker_requests sr
                                    JOIN users u ON sr.user_id = u.user_id
                                    ";
                    if (filter)
                    {
                        query += " WHERE sr.req_status LIKE @Keyword OR CONCAT(u.fname, ' ', u.lname) LIKE @Keyword";
                    }

                }
                else if (isUser)
                {
                    query = @"SELECT sr.req_id, u.*, sr.req_status
                                    FROM speaker_requests sr
                                    JOIN users u ON sr.user_id = u.user_id
                                    where u.user_id = @UserId";
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (isUser)
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                }

                if (filter)
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                }

                connection.Open();

                try
                {
                    // Execute the query
                    SqlDataReader reader = command.ExecuteReader();

                    // Read the user request details and add them to the list
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            FirstName = reader["fname"].ToString() ?? "",
                            LastName = reader["lname"].ToString() ?? ""
                            // Populate other user properties as needed
                        };

                        SpeakerRequest speakerRequest = new SpeakerRequest
                        {
                            ReqId = Convert.ToInt32(reader["req_id"]),
                            ReqStatus = reader["req_status"].ToString() ?? "",
                            // Populate other speaker request properties as needed
                        };

                        UserRequest userRequest = new UserRequest(user, speakerRequest);
                        userRequests.Add(userRequest);
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving user requests: " + ex.Message);
                }
            }

            return userRequests;
        }

        // Method to add feedback
        public bool AddFeedback(int userId, int seminarId, string feedbackText)
        {
            // Insert new feedback record into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                INSERT INTO FEEDBACK (user_id, sem_id, feedback_text)
                VALUES (@UserId, @SeminarId, @FeedbackText)
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@SeminarId", seminarId);
                command.Parameters.AddWithValue("@FeedbackText", feedbackText);

                connection.Open();

                try
                {
                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Feedback addition successful
                        return true;
                    }
                    else
                    {
                        // No rows affected, feedback addition failed
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding feedback: " + ex.Message);
                    return false; // Feedback addition failed
                }
            }
        }

        // Method to get user profile details
        public UserLogin GetUserProfile(int userId)
        {
            UserLogin userLogin = null;

            // Retrieve user and login details from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT u.*, l.username, l.userpassword
                FROM users u
                INNER JOIN logins l ON u.user_id = l.user_id
                WHERE u.user_id = @UserId
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                try
                {
                    // Execute the query
                    SqlDataReader reader = command.ExecuteReader();

                    // Check if user exists
                    if (reader.Read())
                    {
                        // Populate user details
                        User user = new User
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            FirstName = reader["fname"].ToString() ?? "",
                            LastName = reader["lname"].ToString() ?? "",
                            Email = reader["email"].ToString() ?? "",
                            ContactNo = Convert.ToInt64(reader["contact_no"]),
                            UType = reader["utype"].ToString() ?? ""
                            // Add more properties as needed
                        };

                        // Populate login details
                        Login login = new Login
                        {
                            Username = reader["username"].ToString() ?? "",
                            UserPassword = reader["userpassword"].ToString() ?? ""
                            // Add more properties as needed
                        };

                        // Create UserLogin object
                        userLogin = new UserLogin(user, login);
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving user profile: " + ex.Message);
                }
            }

            return userLogin;
        }

        public bool AttendSeminar(int userId, int seminarId, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO attendance (sem_id, user_id, status)
                    VALUES (@SeminarId, @UserId, @Status)
                    ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeminarId", seminarId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Status", status);

                connection.Open();

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public bool checkAttendance(int userId, int seminarId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*) from attendance
                    WHERE user_id = @UserId
                    AND sem_id = @SeminarId
                    ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeminarId", seminarId);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

        public bool AcceptAssign(int semId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string ReqQuery = @"
                UPDATE seminars
                SET sem_status = 'Scheduled'
                WHERE
                seminar_id = @SeminarId
            ";


                SqlCommand command = new SqlCommand(ReqQuery, connection);
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

        public bool RejectAssign(int semId, int userId, string reason)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string ReqQuery = @"
                UPDATE seminars
                SET sem_status = 'Rejected'
                WHERE
                seminar_id = @SeminarId
                ";

                string RejQuery = @"
                INSERT into rejected_seminars VALUES (@userId, @SeminarId, @Reason)
                ";

                SqlCommand command = new SqlCommand(ReqQuery, connection);
                command.Parameters.AddWithValue("@SeminarId", semId);
                
                SqlCommand reqcommand = new SqlCommand(RejQuery, connection);
                reqcommand.Parameters.AddWithValue("@SeminarId", semId);
                reqcommand.Parameters.AddWithValue("@userId", userId);
                reqcommand.Parameters.AddWithValue("@Reason", reason);


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
    }
}

