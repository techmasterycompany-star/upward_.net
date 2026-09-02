# Upwork - Freelance Job Marketplace Platform

A full-stack freelance job marketplace platform built with **ASP.NET
Core Web API** and **Clean Architecture**. The platform connects
**Candidates** and **Employers**, while **Admins** manage and moderate
the platform.

------------------------------------------------------------------------

## Tech Stack

-   **Backend:** ASP.NET Core Web API, C#
-   **Architecture:** Clean Architecture
-   **ORM:** Entity Framework Core
-   **Database:** SQL Server
-   **Authentication:** ASP.NET Core Identity + JWT Bearer
-   **Authorization:** Role-Based Authorization
-   **Payments:** Stripe
-   **Storage:** Cloudinary
-   **API Testing:** Postman
-   **Documentation:** Swagger / OpenAPI

------------------------------------------------------------------------

## Architecture

The project follows **Clean Architecture**, separating business logic
from API, infrastructure, and external integrations.

``` text
UpWard
│
├── UpWard.API
│   ├── Controllers
│   ├── Helpers
│   └── ...
│
├── UpWard.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   └── ...
│
├── UpWard.Domain
│   ├── Entities
│   ├── Enums
│   └── ...
│
└── UpWard.Infrastructure
    ├── Persistence
    ├── Repositories
    ├── Stripe
    ├── Storage
    └── ...
```

------------------------------------------------------------------------

# Features

## Authentication & Authorization

-   Employer and Candidate registration
-   Email verification using a verification token or email verification
    link
-   User login with JWT authentication
-   Password recovery and reset using a time-limited reset token
-   JWT Bearer authentication
-   Role-based authorization for Candidate, Employer, and Admin
-   User logout
-   Protected API endpoints

------------------------------------------------------------------------

## Candidate Module

### Job Search & Discovery

-   Job search and filtering
-   Pagination
-   Salary range filtering and validation
-   View approved job details
-   Job view tracking
-   Duplicate view prevention
-   Saved searches
-   Create, update, list, and delete saved searches

### Job Applications

-   Apply to jobs with resume upload
-   Apply using an existing profile resume
-   Duplicate application prevention
-   Application tracking
-   View individual applications
-   Application cancellation
-   Application status restrictions
-   Resume validation and cloud storage
-   Application notifications for candidates and employers

### Candidate Profile & Resume

-   Create and manage candidate profiles
-   View and update profile information
-   Upload and replace resumes
-   Automatically remove the previous resume from storage
-   Prevent duplicate profiles

### Skills Management

-   Add, update, and remove skills
-   Track years of experience
-   Bulk skill upload using CSV input
-   Skill deduplication
-   Full skills-list replacement
-   Skill validation

### Job Comments

-   Public comment viewing
-   Create comments
-   Edit own comments
-   Delete own comments
-   Comment ownership validation
-   Comment content validation
-   Approved-job validation

------------------------------------------------------------------------

# Employer Module

The Employer module provides functionality for managing company
profiles, job postings, applications, candidate interactions, and job
analytics.

### Employer Profile

-   Create, view, update, and delete company profiles
-   View employer profiles
-   Upload and manage company logo

### Job Management

-   Create job postings
-   View employer's jobs
-   View job details
-   Update job postings
-   Delete job postings
-   Close job postings

### Application Management

-   View applications received for employer jobs
-   View applications for a specific job
-   View individual applications
-   Accept applications
-   Reject applications

### Analytics & Candidate Search

-   Employer analytics dashboard
-   Job analytics
-   Candidate analytics/search

------------------------------------------------------------------------

# Admin Module

The Admin module provides platform-level management, approval, and
moderation capabilities.

### User Management

-   View all users
-   View user details
-   Suspend users
-   Activate users
-   Delete users

### Job Management

-   View all jobs
-   View pending jobs
-   View job details
-   Approve jobs
-   Reject jobs

### Category Management

-   Create categories
-   View categories
-   Update categories
-   Delete categories

### Technology Management

-   Create technologies
-   View technologies
-   Update technologies
-   Delete technologies

### Comment Moderation

-   View comments
-   Hide comments
-   Restore comments
-   Delete comments

### Platform Monitoring

-   Admin dashboard
-   Platform statistics

------------------------------------------------------------------------

# Payment & Subscription

The platform supports employer subscriptions through **Stripe**.

### Features

-   Monthly and yearly subscription plans
-   Stripe checkout session creation
-   Subscription tracking
-   Payment tracking
-   Stripe webhook processing
-   Subscription activation after successful payment
-   Payment status updates
-   Transaction tracking

### Payment Flow

``` text
Employer
   ↓
Create Checkout Session
   ↓
SubscriptionService
   ↓
StripeService
   ↓
Stripe Checkout
   ↓
Stripe Webhook
   ↓
Validate Webhook Signature
   ↓
Update Subscription & Payment
   ↓
Subscription = Active
Payment = Completed
```

Subscription and payment records are initially stored as **Pending**. A
verified Stripe webhook is used to update them to their completed/active
states.

------------------------------------------------------------------------

# Notifications

The platform provides in-app notifications for Candidates, Employers,
and Admins.

### Features

-   View all notifications
-   View unread notifications
-   Get unread notification count
-   Mark an individual notification as read
-   Mark all notifications as read
-   Notification ownership validation
-   Optimized bulk read updates

### Notification Types

**Candidate** - Application submitted - Application accepted -
Application rejected - Application status changed

**Employer** - New application received - Job approved - Job rejected -
Job deadline approaching - Job expired

**Admin** - New job pending approval

**Payment** - Payment completed - Payment failed - Payment refunded -
Subscription renewed - Subscription cancelled

------------------------------------------------------------------------

# API Design & Backend Practices

-   Clean Architecture with separation of responsibilities
-   Layered API, Application, Domain, and Infrastructure projects
-   DTO-based API requests and responses
-   Repository and service abstractions
-   Role-based authorization
-   Resource ownership checks
-   Input validation
-   Consistent error handling
-   `AsNoTracking()` for read-only queries where applicable
-   Idempotent operations where appropriate
-   Storage abstraction through `IStorageService`
-   Cloud-based resume and file storage

------------------------------------------------------------------------

# Main User Roles

| Role | Main Capabilities |
|------|-------------------|
| **Candidate** | Search jobs, apply, manage profile, resumes, skills, comments, and notifications |
| **Employer** | Manage company profile, post and manage jobs, review applications, view analytics, and manage subscriptions |
| **Admin** | Manage users, jobs, categories, technologies, comments, dashboard, and statistics |


------------------------------------------------------------------------

# Security

The platform implements:

-   JWT Bearer authentication
-   Role-based authorization
-   Protected endpoints
-   Resource ownership validation
-   Email verification
-   Password reset tokens
-   Stripe webhook signature validation
-   Request validation
-   Secure file-storage abstraction

------------------------------------------------------------------------

# Project Status

The backend covers the main platform workflows:

-   Authentication & authorization
-   Candidate job discovery and applications
-   Candidate profiles, resumes, and skills
-   Employer profiles and job management
-   Application review
-   Job comments
-   Admin management and moderation
-   Notifications
-   Employer subscriptions and payments
-   Employer analytics and candidate search

The Clean Architecture structure allows additional features and
integrations to be developed while keeping business logic separated from
infrastructure and external services.

## API Documentation

- Swagger: available when running the API in development mode
- Postman documentation: https://documenter.getpostman.com/view/46257067/2sBYAuSrHd

## Deployment

Live application: https://upwork-api.runasp.net/
