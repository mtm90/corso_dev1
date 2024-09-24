# Movie Manager MVC Application

This project is a Movie Manager application built using the MVC (Model-View-Controller) design pattern in C#. The goal is to simulate a realistic scenario for managing movie bookings, handling user accounts, and creating relationships between movies, users, and bookings. 

## Table of Contents

- [Project Overview](#project-overview)
- [Technologies](#technologies)
- [Design Overview](#design-overview)
  - [Classes](#classes)
  - [Database Structure](#database-structure)
- [Setup Instructions](#setup-instructions)
- [Next Steps](#next-steps)

## Project Overview

The Movie Manager is designed to:
- Manage users who book movies.
- Maintain a collection of movies available for booking.
- Create a database that manages relationships between users, movies, and bookings.

In the future, the application will handle functionalities like:
- Movie search
- Booking creation, modification, and cancellation
- Authentication and user account management

## Technologies

- C#
- SQLite (or SQL Server)

## Design Overview

### Classes

The application will start with the following classes:

1. **User**  
   Represents a user who can book movies. Each user will have a unique ID and related attributes such as username, email, and password.

   | Attribute  | Type    | Description        |
   |------------|---------|--------------------|
   | UserId     | int     | Primary Key         |
   | Username   | string  | User's name         |
   | Email      | string  | User's email address|
   | Password   | string  | User's password     |

2. **Movie**  
   Represents a movie that users can book. Each movie will have a unique ID and related attributes such as title, genre, and duration.

   | Attribute  | Type    | Description        |
   |------------|---------|--------------------|
   | MovieId    | int     | Primary Key         |
   | Title      | string  | Movie title         |
   | Genre      | string  | Movie genre         |
   | Duration   | int     | Movie duration (min)|

3. **Booking**  
   Represents a booking of a movie by a user. The booking will store the relationship between users and movies along with booking-specific data.

   | Attribute  | Type    | Description                  |
   |------------|---------|------------------------------|
   | BookingId  | int     | Primary Key                  |
   | UserId     | int     | Foreign Key (References User)|
   | MovieId    | int     | Foreign Key (References Movie)|
   | BookingDate| DateTime| Date of booking              |

### Database Structure

The database will include three tables:
- `Users`: Stores information about users.
- `Movies`: Stores information about movies.
- `Bookings`: Stores information about bookings, linking users and movies.

Each table will be connected by foreign key constraints. The `Bookings` table will have foreign keys referencing both the `Users` and `Movies` tables.

#### Relationships:
- **One-to-Many**: 
  - One user can book many movies (represented in the `Bookings` table).
  - A movie can have many bookings from different users.