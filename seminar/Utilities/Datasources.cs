using System.Collections.Generic;
using System.Linq;

namespace seminar.Utilities
{
    internal class Datasources
    {
        public List<object> SeminarsDataSource(List<AllSeminars> seminarDetailsList)
        {
            object seminarsDataSource = seminarDetailsList.Select(seminar =>
            new
            {
                seminar.Aseminar.SeminarId,
                SeminarName = seminar.Aseminar.SemName,
                seminar.Aspeaker.SpeakerName,
                Date = seminar.Aseminar.SDate,
                Topic = seminar.Atopic.TopicName,
                seminar.Aseminar.Status
            }).ToList<object>();

            return (List<object>)seminarsDataSource;
        }

        public List<object> UserRequestsDataSource(List<UserRequest> userRequestsList)
        {
            object userRequestsDataSource = userRequestsList.Select(request =>
            new
            {
                request.RUser.UserId,
                UserName = request.RUser.FirstName + " " + request.RUser.LastName,
                request.RSpeakerRequest.ReqId,
                request.RSpeakerRequest.ReqStatus
                // Add other properties as needed
            }).ToList<object>();

            return (List<object>)userRequestsDataSource;
        }

        public List<object> UserFeedbacksDataSource(List<SeminarFeedback> userFeedbacksList)
        {
            object userFeedbacksDataSource = userFeedbacksList.Select(feedback =>
            new
            {
                UserName = feedback.User.FirstName + " " + feedback.User.LastName,
                SeminarId = feedback.SSeminar.SeminarId,
                SeminarName = feedback.SSeminar.SemName,
                SeminarDate = feedback.SSeminar.SDate,
                FeedbackText = feedback.SFeedback.FeedbackText
                // Add other properties as needed
            }).ToList<object>();

            return (List<object>)userFeedbacksDataSource;
        }

        public List<object> AttendanceDataSource(List<userAttendance> userAttendanceList)
        {
            object attendanceDataSource = userAttendanceList.Select(attendance =>
                new
                {
                    attendance.AUser.UserId,
                    attendance.AUser.FirstName,
                    attendance.AUser.LastName,
                    attendance.AUser.Email,
                    attendance.AUser.ContactNo,
                    attendance.AAttendance.SeminarId,
                    attendance.AAttendance.Status,
                    attendance.Aseminar.SemName
                }).ToList<object>();

            return (List<object>)attendanceDataSource;
        }

        public List<object> AllUsersDataSource(List<User> users)
        {
            object AllUsersDataSource = users.Select(user =>
                new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.ContactNo,
                    user.UType,

                }).ToList<object>();

            return (List<object>)AllUsersDataSource;
        }

        public List<object> AllSpeakersDataSource(List<speakerUsers> speakers)
        {
            object allSpeakersDataSource = speakers.Select(speaker =>
                new
                {
                    speaker.speakerUser.UserId,
                    speaker.speakerUser.FirstName,
                    speaker.speakerUser.LastName,
                    speaker.speakerUser.Email,
                    speaker.speakerUser.ContactNo,
                    speaker.speakerUser.UType,
                    speaker.speakerSpeaker.SAvailability,
                }).ToList<object>();

            return (List<object>)allSpeakersDataSource;
        }

    }
}
