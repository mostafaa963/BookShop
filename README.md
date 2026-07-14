# BookShop
# BookShop API

A RESTful Web API for an online bookstore built with **ASP.NET Core**. The project follows clean software design principles and provides secure authentication, shopping cart management, favorites, coupons, and order processing.

---

# Features

## Authentication & Authorization

* User Registration
* User Login
* JWT Authentication
* Refresh Token
* Logout
* Change Password
* Forgot Password
* Reset Password
* Email Confirmation
* Reconfirm Email
* Role-Based Authorization

---

## Book Management

* Create Book
* Update Book
* Delete Book
* Get Book By Id
* Get All Books
* Search
* Sorting
* Pagination

---

## Author Management

* Create Author
* Update Author
* Delete Author
* Get Author By Id
* Get All Authors

---

## Favorite

* Add Book To Favorites
* Get Favorite Books
* Remove Favorite Book

---

## Shopping Cart

* Create Cart Automatically
* Add Item To Cart
* Remove Cart Item
* Remove Cart
* Increase Quantity
* Decrease Quantity
* Get Cart Details
* Calculate Total Price
* Calculate Total Items

---

## Coupon

* Create Coupon
* Update Coupon
* Delete Coupon
* Get Coupon By Id
* Get All Coupons
* Search
* Sorting
* Pagination
* Coupon Usage Tracking

---

## Order *(In Progress)*

* Create Order
* Apply Coupon
* Order Items
* Order History
* Cancel Order
* Order Status
* Stock Validation

---

# Technologies

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* FluentValidation
* Repository Pattern
* Unit of Work Pattern
* Specification Pattern
* Dependency Injection
* SMTP Email Service

---

# Architecture

The project is organized using a layered architecture.

```text
Presentation
│
├── Controllers
│
BLL
│
├── Services
├── Interfaces
├── DTOs
├── Validators
│
DAL
│
├── Entities
├── Configurations
├── Repositories
├── UnitOfWork
├── Specifications
│
Database
```

---

# Design Patterns

* Repository Pattern
* Unit of Work
* Specification Pattern
* Dependency Injection

---

# Security

* JWT Access Token
* Refresh Token Rotation
* Password Hashing
* Email Confirmation
* Password Reset
* Role-Based Authorization

---

# Validation

The project uses **FluentValidation** for request validation.

Examples:

* Required Fields
* String Length
* Email Validation
* Password Validation
* Quantity Validation
* Coupon Validation

---

# API Response Format

```json
{
  "success": true,
  "statusCode": 200,
  "message": "Success",
  "data": {}
}
```

---

# Project Structure

```text
BookShop.API
BookShop.BLL
BookShop.DAL
```

---

# Future Improvements

* Payment Integration (Stripe / Paymob)
* Shipping Module
* Product Reviews
* Wishlist Notifications
* Dashboard
* Sales Reports
* Docker Support
* Redis Caching
* Logging
* Unit Testing
* Integration Testing
* CI/CD Pipeline

---
