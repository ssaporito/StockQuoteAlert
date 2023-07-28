using Moq;
using MailService.MailSender;
using Microsoft.Extensions.Options;
using MimeKit;
using Common.Dtos.Mail;
using MailService.Smtp;

namespace MailApiTest
{
    public class SendMailTest
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public async Task SendMail_SendsEmail_WithCorrectParameters(string subject, string body, string senderName, string senderEmail, string recipientName, string recipientEmail)
        {
            // Arrange
            var smtpSettings = new SmtpSettings { Server = "localhost", Port = 587, Username = "user", Password = "pass" };
            var smtpClientMock = new Mock<ISmtpClient>();

            var sender = new MailSender(smtpClientMock.Object, Options.Create(smtpSettings));
            EmailMessage email = new(subject, body, senderName, senderEmail, recipientName, recipientEmail);

            // Act
            await sender.SendMail(email);

            // Assert
            smtpClientMock.Verify(client => client.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            smtpClientMock.Verify(client => client.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            smtpClientMock.Verify(client => client.SendAsync(It.Is<MimeMessage>(message =>
                message.Subject == email.Subject &&
                ((TextPart)message.Body).Text == email.Body)), Times.Once);
            smtpClientMock.Verify(client => client.DisconnectAsync(It.IsAny<bool>()), Times.Once);
        }        

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { "Test Subject", "Test Body", "Test Sender", "testsender@gmail.com", "Test Recipient", "testrecipient@gmail.com" },
            };

    }
}