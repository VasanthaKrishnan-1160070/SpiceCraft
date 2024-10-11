using SpiceCraft.Server.DTO.Order;

namespace SpiceCraft.Server.Helpers;

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

public class EmailHelper
{
    private readonly string _smtpServer = "smtp.gmail.com"; 
    private readonly int _smtpPort = 587;  
    private readonly string _smtpUsername = "spicecraftindustryproject@gmail.com"; 
    private readonly string _smtpPassword = "yckd sfae hpwb skpt";

    // Method to send an email when a new order is created
    public async Task SendNewOrderConfirmationEmailAsync(string customerEmail, string customerName, UserOrderDetailDTO orderDetails)
    {
        const string subject = "Your SpiceCraft Order Confirmation";
        string body = GenerateNewOrderEmailTemplate(customerName, orderDetails);
        await SendEmailAsync(subject, body, customerEmail, customerName);
    }

    public async Task SendOrderStatusChangeEmailAsync(string customerEmail, string customerName, string orderId, string newStatus)
    {
        const string subject = "Your Order Status is modified";
        string body = GenerateOrderStatusEmailTemplate(customerName, orderId, newStatus);
        await SendEmailAsync(subject, body, customerEmail, customerName);
    }

    public async Task SendAccountUpdateEmailAsync(string customerEmail, string customerName, bool isAccountCreation)
    {
        string subject = isAccountCreation ? "Welcome to SpiceCraft!" : "Your SpiceCraft Account Has Been Updated";
        string body = GenerateAccountUpdateEmailTemplate(customerName, isAccountCreation);
        await SendEmailAsync(subject, body, customerEmail, customerName);
    }

    public async Task SendNewOffersEmailAsync(string customerEmail, string customerName, string offerDetails)
    {
        string subject = "New Exciting Offers from SpiceCraft!";
        string body = GenerateNewOffersEmailTemplate(customerName, offerDetails);
        await SendEmailAsync(subject, body, customerEmail, customerName);
    }

    public async Task SendEmailAsync(string emailSubject, string emailBody, string toEmail, string fromEmail = "spicecraftindustryproject@gmail.com", string emailName = "")
    {
        try
        {
            fromEmail = "spicecraftcustomer@gmail.com";
            toEmail = "spicecraftcustomer@gmail.com";
            var fromAddress = new MailAddress(fromEmail, "SpiceCraft");
            var toAddress = new MailAddress(toEmail, emailName);

            var smtp = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);  // Sending the email asynchronously
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    // Template for New Order Confirmation Email
     private string GenerateNewOrderEmailTemplate(string customerName, UserOrderDetailDTO orderDetails)
    {
        // Calculate subtotal
        decimal subtotal = 0;
        foreach (var item in orderDetails.OrderItems)
        {
            subtotal += item.Price * item.Quantity;
        }

        // Assume you have shipping cost and GST in orderDetails
        decimal shippingCost = orderDetails.ShippingCost;
        decimal gst = orderDetails.GST;

        // Total cost is already in orderDetails.TotalCost

        // HTML table for ordered items with styled header
        var orderItemsTable = new StringBuilder();
        orderItemsTable.Append("<table style='width: 100%; border-collapse: collapse;'>");
        orderItemsTable.Append("<thead>");
        orderItemsTable.Append("<tr style='background-color: #4682B4; color: white;'>");  // Steel blue background with white text
        orderItemsTable.Append("<th style='border: 1px solid #ddd; padding: 8px;'>Item Name</th>");
        orderItemsTable.Append("<th style='border: 1px solid #ddd; padding: 8px;'>Quantity</th>");
        orderItemsTable.Append("<th style='border: 1px solid #ddd; padding: 8px;'>Price</th>");
        orderItemsTable.Append("<th style='border: 1px solid #ddd; padding: 8px;'>Total</th>");
        orderItemsTable.Append("</tr>");
        orderItemsTable.Append("</thead>");
        orderItemsTable.Append("<tbody>");

        foreach (var item in orderDetails.OrderItems)
        {
            orderItemsTable.Append("<tr>");
            orderItemsTable.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{item.ItemName}</td>");
            orderItemsTable.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{item.Quantity}</td>");
            orderItemsTable.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>${item.Price:F2}</td>");
            orderItemsTable.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>${(item.Price * item.Quantity):F2}</td>");
            orderItemsTable.Append("</tr>");
        }

        orderItemsTable.Append("</tbody></table>");

        // Email content with shipping cost, GST, and total cost
        return $@"
        <html>
        <head>
            <style>
                .email-container {{ font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4; }}
                .email-header {{ background-color: #4682B4; color: white; padding: 10px; text-align: center; }}
                .email-content {{ background-color: white; padding: 20px; margin-top: 20px; border-radius: 8px; border: 1px solid #ddd; }}
                .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #888; }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-header'>
                    <h1>SpiceCraft</h1>
                </div>
                <div class='email-content'>
                    <h2>Hi {customerName},</h2>
                    <p>Thank you for your order (Order ID: <strong>{orderDetails.OrderId}</strong>)!</p>
                    <p>Here are the details of your order:</p>

                    <h3>Items Ordered:</h3>
                    {orderItemsTable}

                    <p><strong>Subtotal:</strong> ${subtotal:F2}</p>
                    <p><strong>Shipping Cost:</strong> ${shippingCost:F2}</p>
                    <p><strong>GST:</strong> ${gst:F2}</p>
                    <p><strong>Total Cost:</strong> ${orderDetails.TotalCost:F2}</p>
                    <p>Your order was placed on: {orderDetails.OrderDate}</p>

                    <h3>Shipping Address:</h3>
                    <p>{orderDetails.ShippingAddress.StreetAddress1}<br/>
                    {orderDetails.ShippingAddress.StreetAddress2}<br/>
                    {orderDetails.ShippingAddress.City}, {orderDetails.ShippingAddress.StateOrProvince}, {orderDetails.ShippingAddress.PostalCode}</p>

                    <p>If you have any questions about your order, feel free to contact us.</p>
                </div>
                <div class='footer'>
                    <p>Thank you for choosing SpiceCraft!</p>
                    <p>SpiceCraft Team</p>
                </div>
            </div>
        </body>
        </html>";
    }

    // Template for Order Status Change Email
    private string GenerateOrderStatusEmailTemplate(string customerName, string orderId, string newStatus)
    {
        return $@"
        <html>
        <head>
            <style>
                .email-container {{ font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4; }}
                .email-header {{ background-color: #4682B4; color: white; padding: 10px; text-align: center; }}
                .email-content {{ background-color: white; padding: 20px; margin-top: 20px; border-radius: 8px; border: 1px solid #ddd; }}
                .order-details {{ margin-top: 20px; }}
                .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #888; }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-header'>
                    <h1>SpiceCraft</h1>
                </div>
                <div class='email-content'>
                    <h2>Hi {customerName},</h2>
                    <p>We wanted to let you know that your order (Order ID: <strong>{orderId}</strong>) has been updated to the following status:</p>
                    <p style='font-size: 18px; color: #4682B4;'><strong>{newStatus}</strong></p>
                    <p>You can review your order details by logging into your SpiceCraft account.</p>
                    <p>If you have any questions, feel free to contact our support team.</p>
                </div>
                <div class='footer'>
                    <p>Thank you for choosing SpiceCraft!</p>
                    <p>SpiceCraft Team</p>
                </div>
            </div>
        </body>
        </html>";
    }

    // Template for Account Creation/Update Email
    private string GenerateAccountUpdateEmailTemplate(string customerName, bool isAccountCreation)
    {
        string welcomeText = isAccountCreation ? "Welcome to SpiceCraft!" : "Your SpiceCraft Account Has Been Updated";

        return $@"
        <html>
        <head>
            <style>
                .email-container {{ font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4; }}
                .email-header {{ background-color: #4682B4; color: white; padding: 10px; text-align: center; }}
                .email-content {{ background-color: white; padding: 20px; margin-top: 20px; border-radius: 8px; border: 1px solid #ddd; }}
                .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #888; }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-header'>
                    <h1>SpiceCraft</h1>
                </div>
                <div class='email-content'>
                    <h2>Hi {customerName},</h2>
                    <p>{welcomeText}</p>
                    <p>Thank you for being part of the SpiceCraft family! You can manage your account by logging into our website.</p>
                    <p>If you have any questions, feel free to contact our support team.</p>
                </div>
                <div class='footer'>
                    <p>Thank you for choosing SpiceCraft!</p>
                    <p>SpiceCraft Team</p>
                </div>
            </div>
        </body>
        </html>";
    }

    // Template for New Offers Email
    private string GenerateNewOffersEmailTemplate(string customerName, string offerDetails)
    {
        return $@"
        <html>
        <head>
            <style>
                .email-container {{ font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4; }}
                .email-header {{ background-color: #4682B4; color: white; padding: 10px; text-align: center; }}
                .email-content {{ background-color: white; padding: 20px; margin-top: 20px; border-radius: 8px; border: 1px solid #ddd; }}
                .offer-details {{ margin-top: 20px; }}
                .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #888; }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-header'>
                    <h1>SpiceCraft</h1>
                </div>
                <div class='email-content'>
                    <h2>Hi {customerName},</h2>
                    <p>We're excited to share our latest offers with you:</p>
                    <div class='offer-details'>
                        {offerDetails}
                    </div>
                    <p>Don't miss out! Log in to your account to check out these exciting offers.</p>
                </div>
                <div class='footer'>
                    <p>Thank you for choosing SpiceCraft!</p>
                    <p>SpiceCraft Team</p>
                </div>
            </div>
        </body>
        </html>";
    }
}
