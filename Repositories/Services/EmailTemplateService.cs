namespace EcoPowerHub.Repositories.Services
{
    public class EmailTemplateService
    {
         private readonly string _templatePath;

    public EmailTemplateService(string templatePath)
    {
        _templatePath = templatePath;
    }

    public string RenderWelcomeEmail(string userName, string email, string role)
    {
        var template = File.ReadAllText(@"F:\EcoPowerHub\Templates\WelcomeEmailTemplate.html");
        template = template.Replace("{UserName}", userName);
        template = template.Replace("{Email}", email);
        template = template.Replace("{Role}", role);

        return template;
    }
    }
}
