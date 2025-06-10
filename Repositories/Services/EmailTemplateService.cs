using EcoPowerHub.Models;

namespace EcoPowerHub.Repositories.Services
{
    public class EmailTemplateService
    {
        private readonly string _templatePath;

        public EmailTemplateService(string templatePath)
        {
            _templatePath = templatePath;
        }

        public string RenderWelcomeEmail(string UserName, string Email, string Role)
        {

            //     var fullTemplatePath = File.ReadAllText("/mnt/MyData/Courses/Projects/GrdPrj/Back-end/Templates/WelcomeEmailTemplate.html");

            //     Console.WriteLine($"Looking for template at: {fullTemplatePath}");

            //     if (!File.Exists(fullTemplatePath))
            //     {
            //         throw new FileNotFoundException("Email template file not found.", fullTemplatePath);
            //     }
            //     var template = File.ReadAllText(fullTemplatePath); 
            // template = template.Replace("{UserName}", userName);

            // template = template.Replace("{Email}", email);
            // template = template.Replace("{Role}", role);

            return $@"<!DOCTYPE html>
<html >
<head>
    <meta charset='UTF-8>
    <meta name='viewport content='width=device-width, initial-scale=1.0>
    <title>Welcome to EcoPowerHub</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        h1 {{
            color: #4CAF50;
        }}
        ul {{
            list-style-type: none;
            padding: 0;
        }}
        li {{
            margin: 10px 0;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 0.9em;
            color: #888;
        }}
    </style>
</head>
<body>
    <div class='container>
        <h1>Welcome, {UserName}!</h1>
        <p>Thank you for registering with us. We're excited to have you on board.</p>
        <p>Your account has been successfully created with the following details:</p>
        <ul>
            <li><strong>Username:</strong>'{UserName}'</li>
            <li><strong>Email:</strong>'{Email}</li>
            <li><strong>Role:</strong>'{Role}</li>
        </ul>
        <p>If you have any questions, feel free to contact us.</p>
            <div class='footer'>
                Eco Power Hub, 123 Green Energy Street, Sustainable City, Earth <br>
                <a href='http://157.175.182.159'>Privacy Policy</a> | 
                <a href='http://157.175.182.159'>Terms & Conditions</a>
            </div>
    </div>
</body>
</html>";
        }

        public string ResetPasswordEmail(string email, string resetLink)
        {
            return $@"
    <html>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Reset Your Password</title>
        <style>
                body
                {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4; 
                margin: 0; padding: 0;
                }}
            .container
                {{
                width: 100%; 
                max-width: 500px;
                margin: 20px auto;
                background-color: #ffffff;
                padding: 20px; 
                border-radius: 8px; 
                box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
                text-align: center;
                }}
            .logo 
                {{
                margin-bottom: 20px;
                }}
            .title 
                {{ 
                font-size: 22px;
                font-weight: bold;
                color: #333;
                }}

            .message 
                {{
                font-size: 16px;
                color: #555;
                margin: 15px 0; 
                }}
            .btn 
                {{
                display: inline-block;
                padding: 12px 20px;
                background-color: #f0f0f0;
                color: #f0f0f0;
                text-decoration: none;
                font-size: 16px;
                font-weight: bold;
                border-radius: 6px;
                margin: 20px 0;
                }}
            .btn:hover 
                {{
                background-color: #f0f0f0;
                }}
            .info 
                {{ 
                font-size: 14px;
                color: #666;
                margin-top: 10px;
                }}
            .footer
                {{
                margin-top: 20px; 
                font-size: 12px;
                color: #999;
                }}
        </style>
    </head>
    <body>
        <div class='container'>
            
            <div class='title'>We Got Your Request</div>
            <div class='message'>You can now reset your password!</div>
            <a href='{resetLink}' class='btn' aria-label='Reset your password'>Reset Password</a>
            <div class='info'>
                Just so you know: You have 24 hours to pick your password. After that, you'll have to ask for a new one.
            </div>
            <div class='info'>
                Didn’t ask for a new password? You can ignore this email.
            </div>
            <div class='footer'>
                Eco Power Hub, 123 Green Energy Street, Sustainable City, Earth <br>
                <a href='http://157.175.182.159'>Privacy Policy</a> | 
                <a href='http://157.175.182.159'>Terms & Conditions</a>
            </div>
        </div>
    </body>
    </html>";
        }

    public string OrderConfirmationEmail(Order order)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Order Confirmation</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 500px;
            margin: auto;
            background-color: #fff;
            padding: 20px;
            border-radius: 8px;
            text-align: center;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        h2 {{
            color: #333;
        }}
        p {{
            color: #555;
        }}
        a.button {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #28a745;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}
        .footer {{
            font-size: 14px;
            color: #888;
            margin-top: 30px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Your Order Has Been Confirmed!</h2>
        <p>Thank you for choosing Eco Power Hub 🌞</p>
        <p>Order ID: <strong>{order.Id}</strong></p>
        <p>Order Date: {order.OrderDate.ToString("yyyy-MM-dd HH:mm")}</p>
        <p>Total Price: <strong>{order.Price} EGP</strong></p>
        <a href='http://157.175.182.159/cart'/{order.Id} class='button'>View Order</a>
        <p class='footer'>If you didn’t place this order, please contact support immediately.</p>
        <p>Thank you for choosing Eco Power Hub 🌞</p>
    </div>
</body>
</html>
";
        }

        public string PackagePurchaseConfirmationEmail(Package pkg, Order order)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>تم استلام طلبك بنجاح، سوف يتم التواصل معك من قبل مختص</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0; padding: 20px;
        }}
        .container {{
            max-width: 600px; margin: auto;
            background-color: #fff; padding: 20px;
            border-radius: 8px; text-align: center;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        h2 {{ color: #4CAF50; }}
        p {{ color: #555; }}
        .footer {{ margin-top: 30px; font-size: 12px; color: #888; }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>تم استلام طلبك بنجاح، سوف يتم التواصل معك من قبل مختص!</h2>
        <p>اسم الحزمة: <strong>{pkg.Name}</strong></p>
        <p>رقم الطلب: <strong>{order.Id}</strong></p>
        <p>المبلغ المدفوع: <strong>{order.Price} EGP</strong></p>
        <p>تاريخ الطلب: {order.OrderDate:yyyy-MM-dd HH:mm}</p>
        <div class='footer'>
            شكراً لاختيارك EcoPowerHub 🌞<br/>
            إذا لم تقم بهذا الطلب، الرجاء التواصل مع الدعم فوراً.
        </div>
    </div>
</body>
</html>";
        }
    }
}
    
