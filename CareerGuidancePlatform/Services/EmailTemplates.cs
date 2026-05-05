namespace CareerGuidancePlatform.Services
{
    public static class EmailTemplates
    {
        private static string Wrap(string color, string body) =>
            "<div style='font-family:Inter,sans-serif;background:#0a0a0f;color:#f5f0ff;padding:2rem;border-radius:12px;max-width:560px;margin:auto'>" +
            body +
            "<p style='margin-top:1.5rem;color:#555;font-size:0.8rem'>Career Guidance Platform</p></div>";

        public static string SessionBooked(string mentorName, string userName, string topic, string sessionType, string scheduledAt)
        {
            var body =
                "<h2 style='color:#ff2d6b'>&#128197; New Session Booked!</h2>" +
                "<p>Hi <strong>" + mentorName + "</strong>,</p>" +
                "<p><strong>" + userName + "</strong> has booked a session with you.</p>" +
                "<table style='width:100%;border-collapse:collapse;margin:1rem 0'>" +
                "<tr><td style='padding:6px;color:#888'>Topic</td><td style='padding:6px'>" + topic + "</td></tr>" +
                "<tr><td style='padding:6px;color:#888'>Type</td><td style='padding:6px'>" + sessionType + "</td></tr>" +
                "<tr><td style='padding:6px;color:#888'>Scheduled</td><td style='padding:6px'>" + scheduledAt + "</td></tr>" +
                "</table>" +
                "<a href='#' style='display:inline-block;background:linear-gradient(135deg,#ff2d6b,#ff6b35);color:white;padding:0.75rem 1.5rem;border-radius:8px;text-decoration:none;font-weight:600'>View in Dashboard</a>";
            return Wrap("#ff2d6b", body);
        }

        public static string SessionStatusChanged(string userName, string mentorName, string status, string chatUrl)
        {
            var (emoji, color) = status switch
            {
                "Confirmed" => ("&#9989;", "#06d6a0"),
                "Cancelled" => ("&#10060;", "#ff3b30"),
                "Completed" => ("&#127942;", "#ffd166"),
                _           => ("&#128203;", "#ff6b35")
            };
            var body =
                "<h2 style='color:" + color + "'>" + emoji + " Session " + status + "!</h2>" +
                "<p>Hi <strong>" + userName + "</strong>,</p>" +
                "<p>Your session with <strong>" + mentorName + "</strong> has been <strong>" + status.ToLower() + "</strong>.</p>" +
                "<p style='color:#888'>Check your notifications or visit the platform for full details.</p>" +
                "<a href='" + chatUrl + "' style='display:inline-block;background:linear-gradient(135deg,#ff2d6b,#ff6b35);color:white;padding:0.75rem 1.5rem;border-radius:8px;text-decoration:none;font-weight:600'>View Mentor Chat</a>";
            return Wrap(color, body);
        }
    }
}
