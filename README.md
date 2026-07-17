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
- 
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


