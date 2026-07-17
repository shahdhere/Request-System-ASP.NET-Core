# Request System - ASP.NET Core MVC

## Overview

Request System is a web-based Innovation Request Management System developed using ASP.NET Core MVC and Entity Framework Core.
The system enables individuals and companies to submit innovation requests digitally,
securely authenticate using email OTP verification, upload supporting documents, and track the status of their requests.

## Features

### Authentication
- Passwordless authentication using One-Time Password (OTP)
- Email verification via SMTP
- Secure Cookie Authentication
- Session Management
- OTP expiration and resend functionality

### User Management
- Automatic account creation after OTP verification
- Support for Individual and Company users
- Profile creation and management
- Returning users are recognized automatically

### Request Management
- Create new innovation requests
- Support for Individual and Company request types
- Automatic generation of unique Request Numbers
- View all submitted requests
- View request details
- Track request status
- Display required actions for each request

### Attachment Management
- Upload supporting documents
- Store attachments securely in SQL Server
- Download uploaded files

### Database
- SQL Server
- Entity Framework Core
- Relational database design
- Efficient data management

### Validation
- Server-side validation
- Form validation
- Secure request handling

##  Screenshots

### Welcome Page
The main landing page of the system.

![Welcome Page](Screenshots/WelcomePage.png)

### Email Login
Users enter their email to receive a verification code.

![Login](Screenshots/Login.png)

### OTP Verification
Users verify their identity using the received verification code.

![OTP Verification](Screenshots/ANewVerificationCodeHasBeenSentToYourEmail.png)

### Individual or Company Registration
Users can choose between Individual and Company registration.

![Individual or Company](Screenshots/IndividualOrCompany.png)

### Create Request
Users can submit a new innovation request.

![Create Request](Screenshots/CreateRequest.png)

### My Requests
Users can view and track their submitted requests.

![My Requests](Screenshots/MyRequest.png)
## Built With

- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core
- SQL Server
- Razor Views
- HTML5
- CSS3
- Bootstrap
- JavaScript
- 
## Main Functionalities

- Passwordless login using Email OTP
- Automatic user registration
- Individual & Company profiles
- Innovation request submission
- Attachment upload & download
- Request tracking
- Secure authentication using Cookies
- SQL Server integration

---

## Future Improvements

- Admin Dashboard
- Email notifications for request status updates
- Search and filtering
- User profile editing
- Role-based authorization
- API integration
- Dashboard analytics


