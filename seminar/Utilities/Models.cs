using System;

namespace seminar.Utilities
{

    // Base classes
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public long ContactNo { get; set; }
        public string UType { get; set; } = "";
    }

    public class Login
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string UserPassword { get; set; } = "";
    }

    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; } = "";
    }

    public class Speaker
    {
        public int UserId { get; set; }
        public string SpeakerName { get; set; } = "";
        public string SAvailability { get; set; }
    }

    public class Seminar
    {
        public int SeminarId { get; set; }
        public string SemName { get; set; } = "";
        public int UserId { get; set; }
        public DateTime SDate { get; set; }
        public int TopicId { get; set; }
        public string Status { get; set; }
    }

    public class AssignedSeminar
    {
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int SeminarId { get; set; }
        public string Agreed { get; set; }
    }

    public class RejectedWithSeminarDetails
    {
        public int UserId { get; set; }
        public int SeminarId { get; set; }
        public string SeminarName { get; set; } = "";
        public DateTime SeminarDate { get; set; }
        public int TopicId { get; set; }
        public string Status { get; set; }
    }

    public class Attendance
    {
        public int SeminarId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = "";
    }

    public class Feedback
    {
        public int UserId { get; set; }
        public int SeminarId { get; set; }
        public string FeedbackText { get; set; } = "";
    }

    public class SpeakerRequest
    {
        public int ReqId { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public string ReqStatus { get; set; } = "";
    }

    public class OrganizerRequest
    {
        public int ReqId { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public string SeminarName { get; set; } = "";
        public DateTime SDate { get; set; }
        public string ReqStatus { get; set; } = "";
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Composite classes
    public class UserLogin
    {
        public UserLogin(User luser, Login lLogin)
        {
            Luser = luser;
            LLogin = lLogin;
        }

        public User Luser { get; set; }
        public Login LLogin { get; set; }
    }

    public class AllSeminars
    {
        public AllSeminars(Seminar aseminar, Speaker aspeaker, Topic atopic)
        {
            Aseminar = aseminar;
            Aspeaker = aspeaker;
            Atopic = atopic;
        }

        public Seminar Aseminar { get; set; }
        public Speaker Aspeaker { get; set; }
        public Topic Atopic { get; set; }
    }

    public class userAttendance
    {
        public userAttendance(Attendance aAttendance, User aUser, Seminar aseminar)
        {
            AAttendance = aAttendance;
            AUser = aUser;
            Aseminar = aseminar;
        }

        public Attendance AAttendance { get; set; }
        public User AUser { get; set; }
        public Seminar Aseminar { get; set; }
    }

    public class SeminarFeedback
    {
        public SeminarFeedback(Seminar sSeminar, Feedback sFeedback, User user)
        {
            SSeminar = sSeminar;
            SFeedback = sFeedback;
            User = user;
        }

        public Seminar SSeminar { get; set; }
        public Feedback SFeedback { get; set; }
        public User User { get; set; }
    }

    public class UserRequest
    {
        public UserRequest(User rUser, SpeakerRequest rSpeakerRequest)
        {
            RUser = rUser;
            RSpeakerRequest = rSpeakerRequest;
        }

        public User RUser { get; set; }
        public SpeakerRequest RSpeakerRequest { get; set; }
    }

    public class AssignedSeminarDetails
    {
        public AssignedSeminarDetails(Seminar seminarAssigned, AssignedSeminar aSSeminar)
        {
            SeminarAssigned = seminarAssigned;
            ASSeminar = aSSeminar;
        }

        public Seminar SeminarAssigned { get; set; }
        public AssignedSeminar ASSeminar { get; set; }

    }

    public class speakerUsers
    {
        public speakerUsers(User speakerUser, Speaker speakerSpeaker)
        {
            this.speakerUser = speakerUser;
            this.speakerSpeaker = speakerSpeaker;
        }

        public User speakerUser { get; set; }
        public Speaker speakerSpeaker { get; set; }
    }
}

