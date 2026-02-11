namespace Pika.Configuration;

public class AuthSettings
{
    public string AngularAppUrl { get; set; } = "https://app.publish.tr/admin";
    public DemoUserSettings DemoUser { get; set; } = new();

    public class DemoUserSettings
    {
        public string Username { get; set; } = "admin@publish.tr";
        public string Password { get; set; } = "ChangeMe123!";
        public string[] Roles { get; set; } = ["Admin"];
    }
}
